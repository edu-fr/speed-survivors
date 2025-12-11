using Controller.General;
using Controller.Interface;
using UnityEngine;

namespace Controller.Weapon.Ammo
{
	public class Projectile : InitializableMono, ISpawnable
	{
		[field: SerializeField]
		private LayerMask DamageableHurtBoxLayer { get; set; }

		[field: SerializeField]
		private MeshRenderer MeshRenderer { get; set; }

		[field: SerializeField]
		private GameObject HitVfx { get; set; }

		public Projectile Prefab { get; private set; }
		private float CurrentTimer { get; set; }
		private float TotalLifetime { get; set; }
		private Vector3 StartPosition { get; set; }
		private float Speed { get; set; }
		private float Damage { get; set; }
		private Vector3 Direction { get; set; }
		public Vector3 MaxPositionOffset { get; set; }
		private bool ParabolicMovement { get; set; }
		private float AreaOfEffectRadius { get; set; }

		private static readonly RaycastHit[] RaycastResults = new RaycastHit[1];
		private static readonly Collider[] AreaOfEffectResults = new Collider[100]; // Buffer para explosão

		// TODO: Alterar para que o lifetime da bala seja distancia percorrida, e nao tempo
		public void Init(Projectile prefab,
			float damage,
			float speed,
			float lifetime,
			Vector3 direction,
			Vector3 maxOffset,
			float areaOfEffect,
			bool parabolicMovement)
		{
			EnsureStillNotInit();

			Prefab = prefab;
			CurrentTimer = lifetime;
			TotalLifetime = lifetime;
			StartPosition = transform.position;
			Direction = direction;
			Speed = speed;
			Damage = damage;
			AreaOfEffectRadius = areaOfEffect;
			ParabolicMovement = parabolicMovement;
			MaxPositionOffset = maxOffset;

			Initialized = true;
		}


		public bool Tick(float deltaTime)
		{
			CheckInit();

			CurrentTimer -= deltaTime;

			if (CurrentTimer <= 0f)
			{
				if (AreaOfEffectRadius > 0)
					ApplyAreaOfEffect();

				return false;
			}

			var previousPosition = transform.position;
			var stepDistance = Speed * deltaTime;
			var nextLinearPos = previousPosition + Direction * stepDistance;
			var hits = Physics.SphereCastNonAlloc(
				previousPosition,
				MeshRenderer.bounds.extents.x,
				Direction,
				RaycastResults,
				stepDistance,
				DamageableHurtBoxLayer.value
			);

			// ShowDebugRaycastHit(hits, previousPosition, nextLinearPos);

			if (hits > 0)
			{
				var hitInfo = RaycastResults[0];
				if (HandleHit(hitInfo.collider))
					return false;
			}

			if (ParabolicMovement)
			{
				transform.position += Direction * stepDistance;
				var progress = 1f - CurrentTimer / TotalLifetime;
				// Calculate the height based on progress (Sin)
				var heightOffset = Mathf.Sin(progress * Mathf.PI) * MaxPositionOffset.y;
				var currentPos = transform.position;
				currentPos.y = StartPosition.y + heightOffset + (Direction.y * stepDistance);
				transform.position = currentPos;
			}
			else // Linear y movement
			{
				transform.position = nextLinearPos;
			}

			return true;
		}

		private bool HandleHit(Collider hitCollider)
		{
			if (AreaOfEffectRadius > 0)
			{
				ApplyAreaOfEffect();
				return true; // Destroy projectile after explosion
			}

			var gotRelay = hitCollider.TryGetComponent<EnemyHitboxRelay>(out var relay);
			if (!gotRelay)
				return false;

			return relay.EnemyController.TakeHit(Damage);
		}

		private void ApplyAreaOfEffect()
		{
			if (HitVfx != null)
			{
				// Placeholder
				Instantiate(HitVfx, transform.position, Quaternion.identity);
			}

			var count = Physics.OverlapSphereNonAlloc(transform.position,
				AreaOfEffectRadius,
				AreaOfEffectResults,
				DamageableHurtBoxLayer);

			for (var i = 0; i < count; i++)
			{
				AreaOfEffectResults[i].TryGetComponent<EnemyHitboxRelay>(out var relay);
				relay.EnemyController.TakeHit(Damage);
			}
		}

		public void OnDespawn()
		{
			Initialized = false;
		}
	}
}