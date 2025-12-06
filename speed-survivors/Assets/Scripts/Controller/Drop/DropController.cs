using Controller.General.Base;
using Controller.Interface.General;
using Domain.Interface.Loot;
using Domain.Loot;
using UnityEngine;

namespace Controller.Drop
{
	public class DropController : InitializableMono, ISpawnable
	{
		private ILoot Loot { get; set; }
		public Transform TransformCache { get; private set; }
		public bool IsMagnetized { get; private set; }
		public Vector3 StartPosition { get; private set; }
		public Vector3 TargetPosition { get; private set; }

		// Public variables used for animation by the manager to avoid function overhead
		public float AnimationTimer { get; set; }
		public bool IsAnimationFinished { get; set; }

		public void Init(Vector3 startPos, Vector3 targetPos, ILoot loot)
		{
			EnsureStillNotInit();

			TransformCache = transform;

			StartPosition = startPos;
			TargetPosition = targetPos;
			Loot = loot;

			AnimationTimer = 0;
			IsAnimationFinished = false;
			IsMagnetized = false;

			TransformCache.position = startPos;

			Initialized = true;
		}

		// Used on big vacuums
		public void SetMagnetized()
		{
			CheckInit();

			IsMagnetized = true;
		}

		public ILoot GetLootDataCopy()
		{
			CheckInit();

			return new Loot(Loot.Type, Loot.Amount, Loot.Id);
		}

		public void OnDespawn()
		{
			Initialized = false;
		}
	}
}