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

		public void Initialize(Vector3 position, ILoot loot)
		{
			Loot = loot;
			IsMagnetized = false;
			AnimationTimer = 0f;
			StartPosition = position;
			IsAnimationFinished = false;

			var randomCircle = Random.insideUnitCircle * 1.5f;
			TargetPosition = position + new Vector3(randomCircle.x, 0, randomCircle.y);

			Transform.position = position;
			Transform.localScale = Vector3.one;
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