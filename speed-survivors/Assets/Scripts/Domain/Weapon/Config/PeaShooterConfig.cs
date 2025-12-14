using System.Collections.Generic;
using Domain.Interface.Weapon.Base;

namespace Domain.Weapon.Config
{
	public class PeaShooterConfig : BaseWeaponConfig
	{
		public override WeaponType WeaponType => WeaponType.PeaShooter;
		protected override Dictionary<WeaponStatType, float[]> StatsByLevel { get; } = new()
		{
			[WeaponStatType.DamagePerHit] =           new[] { 5f, 7f, 9f, 11f, 13f },
			[WeaponStatType.Range] =                  new[] { 10f, 15f, 20f, 25f, 30f },
			[WeaponStatType.ProjectilesPerShot] =     new[] { 1f, 1f, 1f, 1f, 1f },
			[WeaponStatType.FireRate] =               new[] { 0.4f, 0.5f, 0.6f, 0.7f, 0.8f },
			[WeaponStatType.ProjectileForwardSpeed] = new[] { 5f, 7.5f, 10f, 12.5f, 15f },
			[WeaponStatType.ProjectileLateralSpeed] = new[] { 0f, 0f, 0f, 0f, 0f },
			[WeaponStatType.AreaOfEffectRadius] =     new[] { 0f, 0f, 0f, 0f, 0f },
		};
	}
}