using System;
using System.Collections.Generic;
using Controller.Drop;
using Controller.Interface;
using Engine;
using UnityEngine;

namespace Controller.General
{
	[Serializable]
	public abstract class BaseSpawnHandler<T> : Initializable where T : InitializableMono, ISpawnable
	{
		[field: SerializeField]
		private T SpawnPrefab { get; set; }

		protected abstract Range<float> SpawnCooldown { get; set; }
		private readonly Range<float> SpawnAreaX = new(-4f, 4f);
		protected abstract float DespawnRange { get; set; }
		protected abstract float SpawnAreaDistanceZ { get; set; }
		private List<T> SpawnedList { get; set; }
		private float CurrentTimer { get; set; }
		private float NextRandomSpawnTime { get; set; }
		private bool SpawningActive { get; set; }
		private Transform ReferenceTransform { get; set; }
		protected DropHandler SceneDropHandler { get; set; }

		public delegate bool DespawnedAliveDelegate(float damage, bool critical);

		public abstract event DespawnedAliveDelegate OnDespawnedAlive;

		public void Init(Transform referenceTransform, DropHandler sceneDropHandler)
		{
			EnsureStillNotInitialized();

			ReferenceTransform = referenceTransform;
			SceneDropHandler = sceneDropHandler;

			Initialized = true;
		}

		public void Tick(float dt)
		{
			CheckInit();

			SpawnLoop(dt);
			ActiveSpawnsLoop(dt);
		}

		public void StartSpawn()
		{
			CheckInit();

			if (SpawningActive)
				throw new InvalidOperationException("Enemy spawn already active");

			SpawnedList = new List<T>();
			SpawningActive = true;
		}

		private void SpawnLoop(float deltaTime)
		{
			if (!SpawningActive)
				return;

			CurrentTimer += deltaTime;
			if (CurrentTimer < NextRandomSpawnTime)
				return;

			CurrentTimer = 0f;
			NextRandomSpawnTime = UnityEngine.Random.Range(SpawnCooldown.Start, SpawnCooldown.End);
			Spawn();
		}

		private void ActiveSpawnsLoop(float dt)
		{
			for (var i = SpawnedList.Count - 1; i >= 0; i--)
			{
				var spawn = SpawnedList[i];
				if (!AbstractUnitTick(spawn, dt))
				{
					Despawn(spawn, i, true);
					return;
				}

				if (IsInDespawnRange(spawn.transform.position.z))
				{
					AbstractDespawnAlive(spawn);
					Despawn(spawn, i, false);
					return;
				}
			}
		}

		private void Spawn()
		{
			var spawn = PoolManager.Instance.Spawn(SpawnPrefab, GetRandomPositionInSpawnArea(), Quaternion.identity);
			AbstractInitSpawn(spawn);
			SpawnedList.Add(spawn);
		}

		private Vector3 GetRandomPositionInSpawnArea()
		{
			var randX = UnityEngine.Random.Range(SpawnAreaX.Start, SpawnAreaX.End);
			return new Vector3(randX, 0, ReferenceTransform.position.z + SpawnAreaDistanceZ);
		}

		private bool IsInDespawnRange(float enemyZ)
		{
			return ReferenceTransform.position.z - enemyZ > DespawnRange;
		}

		private void Despawn(T spawnController, int index, bool dropLoot)
		{
			var lastIndex = SpawnedList.Count - 1;
			SpawnedList[index] = SpawnedList[lastIndex];
			SpawnedList.RemoveAt(lastIndex);

			if (dropLoot)
				AbstractDrop(spawnController);

			// Dying animation?
			PoolManager.Instance.Despawn(SpawnPrefab, spawnController);
		}

		protected abstract void AbstractInitSpawn(T spawnController);
		protected abstract bool AbstractUnitTick(T spawnController, float dt);
		protected abstract void AbstractDrop(T spawnController);
		protected abstract void AbstractDespawnAlive(T spawnController);
	}
}