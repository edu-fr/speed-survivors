using System.Collections.Generic;
using Domain.General;
using Domain.Interface.Weapon.Base;

namespace Domain.Weapon
{
	public sealed class WeaponStats : BaseStats<WeaponStatType>
	{
		protected override Dictionary<WeaponStatType, float> StatDict { get; set; }

		public WeaponStats(float damagePerHit,
			float range,
			float projectilesPerShot,
			float fireRate,
			float projectileForwardSpeed,
			float projectileLateralSpeed)
		{
			StatDict = new Dictionary<WeaponStatType, float>
			{
				{ WeaponStatType.DamagePerHit, damagePerHit },
				{ WeaponStatType.Range, range },
				{ WeaponStatType.ProjectilesPerShot, projectilesPerShot },
				{ WeaponStatType.FireCooldown, fireRate },
				{ WeaponStatType.ProjectileForwardSpeed, projectileForwardSpeed },
				{ WeaponStatType.ProjectileLateralSpeed, projectileLateralSpeed }
			};
		}
	}
}