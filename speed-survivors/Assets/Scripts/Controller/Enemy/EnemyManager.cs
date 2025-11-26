using System;
using System.Collections.Generic;
using Controller.General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.Enemy
{
	public class EnemyManager : MonoBehaviour
	{
		private const float SpawnCooldown = 0.1f;

		[field: SerializeField]
		private EnemyController EnemyPrefab { get; set; }

		[field: SerializeField]
		private MeshRenderer SpawnArea { get; set; }

		[field: SerializeField]
		private MeshRenderer Floor { get; set; }

		private List<EnemyController> ActiveEnemies { get; set; }

		private float CurrentTimer { get; set; }
		private bool SpawningActive { get; set; }

		public void Tick(float deltaTime)
		{
			SpawnLoop(deltaTime);
			ActiveEnemiesLoop();
		}

		public void StartSpawn()
		{
			if (SpawningActive)
				throw new InvalidOperationException("Enemy spawn already active");

			ActiveEnemies = new List<EnemyController>();
			SpawningActive = true;
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
			var enemy = PoolManager.Instance.Spawn(EnemyPrefab, spawnPosition, Quaternion.identity, transform);
			enemy.Initialize();
			ActiveEnemies.Add(enemy);
			AdjustEnemyHeightOnFloor(enemy, spawnPosition);
		}

		private Vector3 GetRandomPositionInSpawnArea()
		{
			var spawnBounds = SpawnArea.bounds;
			var spawnX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
			var spawnZ = Random.Range(spawnBounds.min.z, spawnBounds.max.z);
			return new Vector3(spawnX, Floor.bounds.max.y, spawnZ);
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

			// Dying animation?
			PoolManager.Instance.Despawn(EnemyPrefab, enemyController);
		}
	}
}