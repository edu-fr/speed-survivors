using System;
using System.Collections.Generic;
using Controller.General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.Enemy
{
	public class EnemySpawner : MonoBehaviour
	{
		private const float SpawnStartDelay = 1f;
		private const float SpawnInterval = 2f;

		[field: SerializeField]
		private EnemyController EnemyPrefab { get; set; }

		[field: SerializeField]
		private MeshRenderer SpawnArea { get; set; }

		[field: SerializeField]
		private MeshRenderer Floor { get; set; }

		private bool SpawningEnemies { get; set; }

		public void StartSpawningEnemies()
		{
			if (SpawningEnemies)
				throw new InvalidOperationException("EnemySpawner is already spawning enemies");

			SpawningEnemies = true;
			InvokeRepeating(nameof(SpawnEnemy), SpawnStartDelay, SpawnInterval);
			Debug.Log("EnemySpawner started");
		}

		public void StopSpawningEnemies()
		{
			if (!SpawningEnemies)
				throw new InvalidOperationException("EnemySpawner is not currently spawning enemies");

			SpawningEnemies = false;
			CancelInvoke(nameof(SpawnEnemy));
			Debug.Log("EnemySpawner stopped");
		}

		private void SpawnEnemy()
		{
			var spawnPosition = GetRandomPositionInSpawnArea();
			var enemy = PoolManager.Instance.Spawn(EnemyPrefab, spawnPosition, Quaternion.identity, transform);
			enemy.Initialize();
			enemy.SubscribeToDeathEvent(OnEnemyDeath);
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

		private void OnEnemyDeath(EnemyController enemyController)
		{
			Debug.Log($"Enemy {enemyController.name} dead!");

			DespawnEnemy(enemyController);
		}

		private void DespawnEnemy(EnemyController enemyController)
		{
			// Dying animation?
			enemyController.UnsubscribeFromDeathEvent(OnEnemyDeath);
			PoolManager.Instance.Despawn(EnemyPrefab, enemyController);
		}
	}
}