using System;
using Controller.General;
using Engine;
using UnityEngine;

namespace Controller.Enemy
{
	[Serializable]
	public class EnemySpawnHandler : BaseSpawnHandler<EnemyController>
	{
		protected override Range<float> SpawnCooldown { get; set; } = new(.3f, 3f);
		protected override float DespawnRange { get; set; } = 1.5f;
		protected override float SpawnAreaDistanceZ { get; set; } = 40f;
		public override event DespawnedAliveDelegate OnDespawnedAlive;

		protected override void AbstractInitSpawn(EnemyController spawnController)
		{
			spawnController.Init();
			AdjustEnemyHeightOnFloor(spawnController);
		}

		private void AdjustEnemyHeightOnFloor(EnemyController spawn)
		{
			spawn.SetPosition(spawn.transform.position + new Vector3(0, spawn.GetHeight() / 2f, 0));
		}

		protected override bool AbstractUnitTick(EnemyController spawnController, float dt)
		{
			return spawnController.Tick(dt);
		}

		protected override void AbstractDrop(EnemyController spawnController)
		{
			SceneDropHandler.SpawnLootCluster(spawnController.transform.position, spawnController.GetLoot());
		}

		protected override void AbstractDespawnAlive(EnemyController spawnController)
		{
			OnDespawnedAlive?.Invoke(spawnController.GetEnemyDomainRef().Damage, true);
		}
	}
}