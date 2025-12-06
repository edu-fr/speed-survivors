using System;
using System.Collections.Generic;
using Controller.General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.World
{
	[Serializable]
	public class WorldBuildHandler
	{
		private const int InitialSegments = 3;
		private const float DistanceToOldestSegmentNecessaryToDespawnIt = 10f;
		private const float DistanceToSpawnNewSegment = 150f;

		[field: SerializeField]
		private WorldSection[] SectionPrefabs { get; set; }

		public Vector3 DefaultSegmentTransformSize => SectionPrefabs[0].SectionTransformSize;
		private Transform PlayerTransform { get; set; }
		private Queue<(WorldSection prefab, WorldSection instance)> ActiveSections { get; set; }

		private float _currentConnectionZ;

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

			var distanceToCurrent = _currentConnectionZ - PlayerTransform.position.z;
			if (distanceToCurrent < DistanceToSpawnNewSegment)
			{
				SpawnNextSection();
			}

			if (ActiveSections.Count <= 0)
				throw new InvalidOperationException("No active world sections. This should never happen.");

			var oldestSectionSpawned = ActiveSections.Peek();
			var oldestSizeZ = oldestSectionSpawned.prefab.SectionTransformSize.z;
			var oldestMaxZ = oldestSectionSpawned.instance.transform.position.z + (oldestSizeZ / 2f);
			var distanceToOldest = PlayerTransform.position.z - oldestMaxZ;
			if (distanceToOldest > DistanceToOldestSegmentNecessaryToDespawnIt)
			{
				RemoveOldestSection();
			}
		}

		private void SpawnInitialSection()
		{
			var prefab = SectionPrefabs[0];
			var instance = PoolManager.Instance.Spawn(prefab, Vector3.zero, Quaternion.identity);
			ActiveSections = new Queue<(WorldSection prefab, WorldSection instance)>();
			ActiveSections.Enqueue(new(prefab, instance));
			_currentConnectionZ = prefab.SectionTransformSize.z / 2f;
		}

		private void SpawnNextSection()
		{
			var prefab = SectionPrefabs[Random.Range(0, SectionPrefabs.Length)];
			var halfSize = prefab.SectionTransformSize.z / 2f;
			var spawnPos = new Vector3(0, 0, _currentConnectionZ + halfSize);
			var instance = PoolManager.Instance.Spawn(prefab, spawnPos, Quaternion.identity);
			ActiveSections.Enqueue(new(prefab, instance));
			_currentConnectionZ += prefab.SectionTransformSize.z;
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