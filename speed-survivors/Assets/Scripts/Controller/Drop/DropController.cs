using Domain.Interface.Loot;
using UnityEngine;

namespace Controller.Drop
{
	public class DropController : MonoBehaviour
	{
		private ILoot Loot { get; set; }
		public Transform Transform { get; private set; }
		public bool IsMagnetized { get; private set; }
		public Vector3 StartPosition { get; private set; }
		public Vector3 TargetPosition { get; private set; }

		// Public variables used for animation by the manager to avoid function overhead
		public float AnimationTimer { get; set; }
		public bool IsAnimationFinished { get; set; }

		private void Awake()
		{
			Transform = transform;
		}

		public void Initialize(Vector3 startPos, Vector3 targetPos, ILoot loot)
		{
			StartPosition = startPos;
			TargetPosition = targetPos;
			Loot = loot;

			AnimationTimer = 0;
			IsAnimationFinished = false;
			IsMagnetized = false;

			Transform.position = startPos;
		}

		// Used on big vacuums
		public void SetMagnetized()
		{
			IsMagnetized = true;
		}

		public LootType GetLootType()
		{
			return Loot.Type;
		}
	}
}