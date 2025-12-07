using System.Collections.Generic;
using Domain.Interface.Weapon.Base;

namespace Domain.Weapon.Config
{
	public class ShotgunConfig : BaseWeaponConfig
	{
		public override WeaponType WeaponType => WeaponType.Shotgun;
		protected override Dictionary<WeaponStatType, float[]> StatsByLevel { get; } = new()
		{
			[WeaponStatType.DamagePerHit] =           new[] { 1.3f, 1.6f, 2f, 2.5f, 5f },
			[WeaponStatType.Range] =                  new[] { .2f, .2f, .2f, .2f, 0.3f },
			[WeaponStatType.ProjectilesPerShot] =     new[] { 3f, 3f, 3f, 5f, 6f },
			[WeaponStatType.FireRate] =               new[] { 1.2f, 1.1f, 1f, 1f, 0.9f },
			[WeaponStatType.ProjectileForwardSpeed] = new[] { 2f, 2f, 2f, 2, 4f },
			[WeaponStatType.ProjectileLateralSpeed] = new[] { 0f, 0f, 0f, 0f, 0f },
		};
	}
}