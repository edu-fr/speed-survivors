using Controller.Interface.General;
using UnityEngine;

namespace Controller.Weapon.Ammo
{
	public class Projectile : MonoBehaviour
	{
		[field: SerializeField]
		private LayerMask DamageableHurtBoxLayer { get; set; }

		[field: SerializeField]
		private MeshRenderer MeshRenderer { get; set; }

		public Projectile Prefab { get; private set; }
		private float Lifetime { get; set; }
		private float CurrentTimer { get; set; }
		private float Speed { get; set; }
		private float Damage { get; set; }
		private Vector3 Direction { get; set; }

		private static readonly RaycastHit[] RaycastResults = new RaycastHit[1];

		public void Initialize(Projectile prefab, float damage, float speed, float lifetime, Vector3 direction)
		{
			Prefab = prefab;
			Lifetime = lifetime;
			CurrentTimer = lifetime;
			Direction = direction;
			Speed = speed;
			Damage = damage;
		}

		public bool Tick(float deltaTime)
		{
			Lifetime -= deltaTime;

			if (Lifetime <= 0f)
				return false;

			var previousPosition = transform.position;
			var stepDistance = Speed * deltaTime;
			var nextPos = previousPosition + Direction * stepDistance;

			var hits = Physics.SphereCastNonAlloc(
				previousPosition,
				MeshRenderer.bounds.extents.x,
				Direction,
				RaycastResults,
				stepDistance,
				DamageableHurtBoxLayer.value
			);

#if UNITY_EDITOR
			Color debugColor = hits > 0 ? Color.red : Color.green;
			Debug.DrawLine(previousPosition, nextPos, debugColor, 0.5f);
#endif

			if (hits > 0)
			{
				RaycastHit hitInfo = RaycastResults[0];

				HandleHit(hitInfo.collider);
				return false;
			}

			// Se não bateu, move a bala visualmente
			transform.position = nextPos;
			transform.position += Direction * (Speed * deltaTime);

			return true;
		}

		private void HandleHit(Collider hitCollider)
		{
			hitCollider.GetComponentInParent<IHitable>()?.TakeHit(Damage);
		}
	}
}