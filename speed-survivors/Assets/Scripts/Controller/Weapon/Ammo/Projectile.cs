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
		private float CurrentDistanceTraveled { get; set; }
		private float LimitDistanceTraveled { get; set; }
		private Vector3 StartPosition { get; set; }
		private float ProjectileSpeed { get; set; }
		private float EmitterSpeed { get; set; }
		private float Damage { get; set; }
		private Vector3 Direction { get; set; }
		private Vector3 MaxPositionOffset { get; set; }
		private bool ParabolicMovement { get; set; }
		private float AreaOfEffectRadius { get; set; }
		private float CachedMeshBoundsRadius { get; set; } = -1f;
		private float SpawnTimer { get; set; }
		private float ElapsedTime { get; set; }

		private static readonly RaycastHit[] RaycastResults = new RaycastHit[1];
		private static readonly Collider[] AreaOfEffectResults = new Collider[100];

		public void Init(Projectile prefab,
			float damage,
			float projectileSpeed,
			float emitterSpeed,
			float limitDistanceTraveled,
			Vector3 direction,
			Vector3 maxOffset,
			float areaOfEffect,
			bool parabolicMovement,
			float spawnDelay)
		{
			EnsureStillNotInit();

			Prefab = prefab;
			CurrentDistanceTraveled = 0f;
			LimitDistanceTraveled = Mathf.Max(0.0001f, limitDistanceTraveled);
			StartPosition = transform.position;
			Direction = direction.normalized;
			ProjectileSpeed = projectileSpeed;
			EmitterSpeed = emitterSpeed;
			Damage = damage;
			AreaOfEffectRadius = areaOfEffect;
			ParabolicMovement = parabolicMovement;
			MaxPositionOffset = maxOffset;
			ElapsedTime = 0f;

			if (CachedMeshBoundsRadius <= 0f)
			{
				var extents = MeshRenderer.bounds.extents;
				CachedMeshBoundsRadius = Mathf.Max(extents.x, Mathf.Max(extents.y, extents.z));
			}

			SpawnTimer = spawnDelay;
			gameObject.SetActive(false);

			Initialized = true;
		}

		public bool Tick(float deltaTime)
		{
			CheckInit();

			if (SpawnTimer > 0f)
			{
				SpawnTimer -= deltaTime;

				if (SpawnTimer > 0f)
					return true;

				gameObject.SetActive(true);
			}

			ElapsedTime += deltaTime;

			var previousPosition = transform.position;
			var projectileStep = ProjectileSpeed * deltaTime;
			var emitterStep = EmitterSpeed * deltaTime;
			var totalBodyStep = projectileStep + emitterStep;

			var totalProjectileDistance = ElapsedTime * ProjectileSpeed;
			var totalEmitterContribution = ElapsedTime * EmitterSpeed;

			var movementAlongDirection = totalProjectileDistance + totalEmitterContribution;
			var clampedProgress = Mathf.Clamp01(totalProjectileDistance / LimitDistanceTraveled);

			if (ParabolicMovement)
			{
				var basePosition = StartPosition + Direction * movementAlongDirection;
				var heightOffset = Mathf.Sin(clampedProgress * Mathf.PI) * MaxPositionOffset.y;
				basePosition.y = StartPosition.y + heightOffset + Direction.y * movementAlongDirection;
				transform.position = basePosition;
			}
			else
			{
				var nextLinearPos = previousPosition + Direction * totalBodyStep;
				transform.position = nextLinearPos;
			}

			var hits = Physics.SphereCastNonAlloc(
				previousPosition,
				CachedMeshBoundsRadius,
				Direction,
				RaycastResults,
				totalBodyStep,
				DamageableHurtBoxLayer.value
			);

			if (hits > 0)
			{
				var hitInfo = RaycastResults[0];
				if (HandleHit(hitInfo.collider))
				{
					return false;
				}
			}

			CurrentDistanceTraveled = totalProjectileDistance;
			if (CurrentDistanceTraveled >= LimitDistanceTraveled)
			{
				if (AreaOfEffectRadius > 0f)
					ApplyAreaOfEffect();

				return false;
			}

			return true;
		}

		private bool HandleHit(Collider hitCollider)
		{
			if (AreaOfEffectRadius > 0f)
			{
				ApplyAreaOfEffect();
				return true;
			}

			if (!hitCollider.TryGetComponent<EnemyHitboxRelay>(out var relay))
				return false;

			return relay.EnemyController.TakeHit(Damage);
		}

		private void ApplyAreaOfEffect()
		{
			if (HitVfx != null)
				Instantiate(HitVfx, transform.position, Quaternion.identity); // TODO: Use pool

			var count = Physics.OverlapSphereNonAlloc(transform.position,
				AreaOfEffectRadius,
				AreaOfEffectResults,
				DamageableHurtBoxLayer.value
			);

			for (var i = 0; i < count; i++)
			{
				if (!AreaOfEffectResults[i].TryGetComponent<EnemyHitboxRelay>(out var relay))
					continue;

				relay.EnemyController.TakeHit(Damage);
			}
		}

		public void OnDespawn()
		{
			Initialized = false;
		}
	}
}