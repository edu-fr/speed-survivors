using System;
using Controller.General;
using Controller.World.Objects;
using Domain.Loot;
using Domain.World.Objects;
using Engine;

namespace Controller.World
{
	[Serializable]
	public class WorldObjectsSpawnHandler : BaseSpawnHandler<BreakableObjectController>
	{
		protected override Range<float> SpawnCooldown { get; set; } = new(3f, 7f);
		protected override float DespawnRange { get; set; } = 2f;
		protected override float SpawnAreaDistanceZ { get; set; } = 45f;

		public override event DespawnedAliveDelegate OnDespawnedAlive;
		protected override void AbstractInitSpawn(BreakableObjectController spawnController)
		{
			spawnController.Init(new BreakableCar());
		}

		protected override bool AbstractUnitTick(BreakableObjectController spawnController, float dt)
		{
			return spawnController.IsAlive();
		}

		protected override void AbstractDrop(BreakableObjectController spawnController)
		{
			SceneDropHandler.SpawnLootCluster(spawnController.transform.position, Loot.Item("Bag", 1));
		}

		protected override void AbstractDespawnAlive(BreakableObjectController spawnController)
		{
		}
	}
}