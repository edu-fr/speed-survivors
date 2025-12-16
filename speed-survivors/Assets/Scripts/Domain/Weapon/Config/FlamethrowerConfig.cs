using System.Collections.Generic;
using Domain.Interface.Weapon.Base;

namespace Domain.Weapon.Config
{
	public class FlamethrowerConfig : BaseWeaponConfig
	{
		public override WeaponType WeaponType => WeaponType.Flamethrower;
		protected override Dictionary<WeaponStatType, float[]> StatsByLevel { get; } = new()
		{
			[WeaponStatType.DamagePerHit] =           new[] { 3f, 4f, 5f, 6f, 7f },
			[WeaponStatType.Range] =                  new[] { 3f, 3f, 4f, 4f, 5f },
			[WeaponStatType.ProjectilesPerShot] =     new[] { 0f, 0f, 0f, 0f, 0f },
			[WeaponStatType.FireCooldown] =           new[] { .1f, .1f, .1f, .1f, .1f },
			[WeaponStatType.ProjectileForwardSpeed] = new[] { 0f, 0f, 0f, 0f, 0f },
			[WeaponStatType.ProjectileLateralSpeed] = new[] { 0f, 0f, 0f, 0f, 0f },
			[WeaponStatType.AreaOfEffectRadius] =     new[] { 20f, 25f, 30f, 30f, 35f },
		};
	}
}