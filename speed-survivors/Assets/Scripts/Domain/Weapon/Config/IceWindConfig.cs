using System.Collections.Generic;
using Domain.Interface.Weapon.Base;
using Domain.Weapon.Strategy.Base;

namespace Domain.Weapon.Config
{
	public class IceWindConfig : BaseWeaponConfig
	{
		public override WeaponType WeaponType => WeaponType.IceWind;
		protected override Dictionary<WeaponStatType, float[]> StatsByLevel { get; } = new()
		{
			[WeaponStatType.DamagePerHit] =           new[] { 1f, 1.1f, 1.5f, 2f, 2.5f },
			[WeaponStatType.Range] =                  new[] { 4f, 4.3f, 4.5f, 5f, 6f },
			[WeaponStatType.ProjectilesPerShot] =     new[] { 0f },
			[WeaponStatType.FireCooldown] =           new[] { AuraStrategy.DefaultDamageActivationCooldown },
			[WeaponStatType.ProjectileForwardSpeed] = new[] { 0f },
			[WeaponStatType.ProjectileLateralSpeed] = new[] { 0f },
			[WeaponStatType.AreaOfEffectRadius] =     new[] { 360f },
		};
	}
}