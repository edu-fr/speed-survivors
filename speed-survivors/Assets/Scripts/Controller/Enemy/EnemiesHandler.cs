using System;
using System.Collections.Generic;
using Controller.Drop;
using Controller.General;
using Controller.General.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.Enemy
{
	[Serializable]
	public class EnemiesHandler : Initializable
	{
		private const float SpawnCooldown = 1f;

		[field: SerializeField]
		private EnemyController EnemyPrefab { get; set; }

		[field: SerializeField]
		private MeshRenderer SpawnArea { get; set; }

		private List<EnemyController> ActiveEnemies { get; set; }

		private float CurrentTimer { get; set; }
		private bool SpawningActive { get; set; }
		private Transform TransformToFollow { get; set; }

		private Vector3 _currentVelocity;
		private Vector3 Offset { get; set; }
		private const float SmoothTime = 0.2f;
		private bool SpawnerShouldAccompanyTargetZ { get; set; }
		private DropHandler SceneDropHandler { get; set; }

		public void Init(Transform transformToFollow, DropHandler sceneDropHandler)
		{
			EnsureStillNotInitialized();

			TransformToFollow = transformToFollow;
			SceneDropHandler = sceneDropHandler;
			var spawnAreaTransform = SpawnArea.transform;
			Offset = spawnAreaTransform.position - transformToFollow.position; // This make sure that will use the current Scene offset

			Initialized = true;
		}

		public void Tick(float deltaTime)
		{
			CheckInit();

			SpawnLoop(deltaTime);
			ActiveEnemiesLoop();
		}

		public void LateTick()
		{
			CheckInit();

			AccompanyTargetZ();
		}

		public void StartSpawn()
		{
			CheckInit();

			if (SpawningActive)
				throw new InvalidOperationException("Enemy spawn already active");

			ActiveEnemies = new List<EnemyController>();
			SpawningActive = true;
			SpawnerShouldAccompanyTargetZ = true;
		}

		private void SpawnLoop(float deltaTime)
		{
			if (!SpawningActive)
				return;

			CurrentTimer += deltaTime;
			if (CurrentTimer < SpawnCooldown)
				return;

			CurrentTimer = 0f;
			SpawnEnemy();
		}

		private void ActiveEnemiesLoop()
		{
			for (var i = ActiveEnemies.Count - 1; i >= 0; i--)
			{
				if (!ActiveEnemies[i].Tick())
				{
					DespawnEnemy(ActiveEnemies[i], i);
				}
			}
		}

		private void SpawnEnemy()
		{
			var spawnPosition = GetRandomPositionInSpawnArea();
			var enemy = PoolManager.Instance.Spawn(EnemyPrefab, spawnPosition, Quaternion.identity);
			enemy.Init();
			ActiveEnemies.Add(enemy);
			AdjustEnemyHeightOnFloor(enemy, spawnPosition);
		}

		private Vector3 GetRandomPositionInSpawnArea()
		{
			var spawnBounds = SpawnArea.bounds;
			var spawnX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
			var spawnZ = Random.Range(spawnBounds.min.z, spawnBounds.max.z);
			return new Vector3(spawnX, 0, spawnZ);
		}

		private void AdjustEnemyHeightOnFloor(EnemyController enemy, Vector3 spawnPosition)
		{
			enemy.SetPosition(spawnPosition + new Vector3(0, enemy.GetHeight() / 2f, 0));
		}

		private void DespawnEnemy(EnemyController enemyController, int index)
		{
			var lastIndex = ActiveEnemies.Count - 1;
			ActiveEnemies[index] = ActiveEnemies[lastIndex];
			ActiveEnemies.RemoveAt(lastIndex);

			SceneDropHandler.SpawnLootCluster(
				enemyController.transform.position,
				enemyController.GetLoot()
				);

			// Dying animation?
			PoolManager.Instance.Despawn(EnemyPrefab, enemyController);
		}

		private void AccompanyTargetZ()
		{
			if (!SpawnerShouldAccompanyTargetZ)
				return;

			var spawnAreaTransform = SpawnArea.transform;
			var desiredPosition = TransformToFollow.position + Offset;
			spawnAreaTransform.position = Vector3.SmoothDamp(
				spawnAreaTransform.position,
				new Vector3(spawnAreaTransform.position.x, spawnAreaTransform.position.y, desiredPosition.z),
				ref _currentVelocity,
				SmoothTime
			);
		}
	}
}