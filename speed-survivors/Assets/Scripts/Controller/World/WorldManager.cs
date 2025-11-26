using System;
using System.Collections.Generic;
using Controller.General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.World
{
	public class WorldManager : MonoBehaviour
	{
		private const int InitialSegments = 5;
		private const float DespawnDistance = 5f;

		[field: SerializeField]
		private WorldSection[] SectionPrefabs { get; set; }

		[field: SerializeField]
		private Transform WorldParent { get; set; }

		private Transform PlayerTransform { get; set; }
		private Queue<(WorldSection prefab, WorldSection instance)> ActiveSections { get; set; }

		private float _currentConnectionZ = 0f;

		private bool Initialized { get; set; }

		public void Init(Transform playerControllerRef)
		{
			PlayerTransform = playerControllerRef;
			Initialized = true;
		}

		public void SpawnInitialWorldSections()
		{
			CheckInit();
			SpawnInitialSection();

			for (var i = 0; i < InitialSegments - 1; i++)
			{
				SpawnNextSection();
			}
		}

		public void Tick()
		{
			CheckInit();

			if (PlayerTransform.position.z > _currentConnectionZ - 100f)
			{
				SpawnNextSection();
			}

			if (ActiveSections.Count > 0)
			{
				var oldest = ActiveSections.Peek();

				// Check distance from player to the oldest section
				if (PlayerTransform.position.z - oldest.instance.transform.position.z > DespawnDistance)
				{
					RemoveOldestSection();
				}
			}
		}

		private void SpawnInitialSection()
		{
			var prefab = SectionPrefabs[0];
			var instance = PoolManager.Instance.Spawn(prefab, Vector3.zero, Quaternion.identity, WorldParent);
			ActiveSections = new Queue<(WorldSection prefab, WorldSection instance)>();
			ActiveSections.Enqueue(new(prefab, instance));
			_currentConnectionZ = instance.SizeZ / 2f;
		}

		private void SpawnNextSection()
		{
			var prefab = SectionPrefabs[Random.Range(0, SectionPrefabs.Length)];
			var halfSize = prefab.SizeZ / 2f;
			var spawnPos = new Vector3(0, 0, _currentConnectionZ + halfSize);
			var instance = PoolManager.Instance.Spawn(prefab, spawnPos, Quaternion.identity, WorldParent);
			ActiveSections.Enqueue(new(prefab, instance));
			_currentConnectionZ += prefab.SizeZ;
		}

		private void RemoveOldestSection()
		{
			var section = ActiveSections.Dequeue();
			PoolManager.Instance.Despawn(section.prefab, section.instance); // Devolve pro Pool
		}

		private void CheckInit()
		{
			if (!Initialized)
				throw new InvalidOperationException("WorldManager not initialized. Call Init() before using it.");
		}
	}
}