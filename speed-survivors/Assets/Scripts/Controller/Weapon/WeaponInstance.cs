using Controller.General.Base;
using Controller.Weapon.Ammo;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;
using Domain.Interface.Weapon.Strategy;

namespace Controller.Weapon
{
	public abstract class WeaponInstance : InitializableMono
	{
		/// <summary>
		/// Serialize field for the projectile prefab associated with this weapon.
		/// </summary>
		protected abstract Projectile ProjectilePrefab { get; set; }
		private ProjectileHandler ProjectileHandler { get; set; }
		public abstract IWeaponConfig Config { get; }
		protected abstract IWeaponStrategy Strategy { get; }
		private float CurrentCooldown { get; set; }

		public void Init(ProjectileHandler projectileHandler)
		{
			EnsureStillNotInit();

			ProjectileHandler = projectileHandler;
			CurrentCooldown = 0f;

			Initialized = true;
		}

		public void Tick(float deltaTime, bool shouldShoot, float emitterSpeed, int weaponLevel)
		{
			CheckInit();

			CurrentCooldown -= deltaTime;

			if (CurrentCooldown > 0f)
				return;

			if (shouldShoot)
				Fire(emitterSpeed, weaponLevel);
		}

		private void Fire(float emitterSpeed, int weaponLevel)
		{
			var projectileCount = (int) Config.GetStat(WeaponStatType.ProjectilesPerShot, weaponLevel);
			for (var i = 0; i < projectileCount; i++)
			{
				var projectileSpeedMod = Strategy.GetSpeedModifier(i, projectileCount);
				var projectileDirection = Strategy.GetProjectileDirection(i, projectileCount);
				var projectilePosition = Strategy.GetPositionModifier(i, projectileCount);

				ProjectileHandler.SpawnProjectile(
					ProjectilePrefab,
					transform.position + projectilePosition,
					projectileDirection,
					emitterSpeed * projectileSpeedMod,
					Config.GetStat(WeaponStatType.DamagePerHit, weaponLevel));
			}

			CurrentCooldown = Config.GetStat(WeaponStatType.FireRate, weaponLevel);
		}
	}
}