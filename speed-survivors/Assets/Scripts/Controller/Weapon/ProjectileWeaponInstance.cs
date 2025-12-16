using Controller.Weapon.Ammo;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Strategy;
using Domain.Weapon.Strategy.Base;

namespace Controller.Weapon
{
	public abstract class ProjectileWeaponInstance : BaseWeaponInstance
	{
		protected abstract Projectile ProjectilePrefab { get; set; }
		private ProjectileStrategy ProjectileStrategy => Strategy as ProjectileStrategy;
		private ProjectileHandler ProjectileHandler { get; set; }

		public void Init(ProjectileHandler handler)
		{
			ProjectileHandler = handler;
			base.Init();
		}

		protected override void PerformAttack(float emitterSpeed, int weaponLevel)
		{
			var projectileCount = (int)Config.GetStat(WeaponStatType.ProjectilesPerShot, weaponLevel);

			for (var i = 0; i < projectileCount; i++)
			{
				var speedMod = Strategy.GetSpeedModifier(i, projectileCount);
				var direction = Strategy.GetProjectileDirection(i, projectileCount);
				var positionOffset = Strategy.GetPositionModifier(i, projectileCount);
				var maxOffset =
					Strategy.GetMaxOffsetPosition(i, projectileCount); // Assumindo que adicionou isso na Strategy
				var delay = Strategy.GetSpawnDelay(i, projectileCount); // Assumindo que adicionou isso

				ProjectileHandler.SpawnProjectile(
					ProjectilePrefab,
					transform.position + positionOffset,
					direction,
					maxOffset,
					Config.GetStat(WeaponStatType.ProjectileForwardSpeed, weaponLevel) * speedMod,
					emitterSpeed,
					Config.GetStat(WeaponStatType.DamagePerHit, weaponLevel),
					Config.GetStat(WeaponStatType.Range, weaponLevel),
					Config.GetStat(WeaponStatType.AreaOfEffectRadius, weaponLevel),
					ProjectileStrategy.ProjectileMovementPattern == ProjectileMovementPattern.Parabolic,
					delay
				);
			}
		}
	}
}