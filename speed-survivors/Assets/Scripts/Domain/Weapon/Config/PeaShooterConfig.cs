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
			[WeaponStatType.Range] =                  new[] { 20f, 25f, 30f, 35f, 40f },
			[WeaponStatType.ProjectilesPerShot] =     new[] { 1f, 1f, 1f, 1f, 1f },
			[WeaponStatType.FireRate] =               new[] { 0.4f, 0.5f, 0.6f, 0.7f, 0.8f },
			[WeaponStatType.ProjectileForwardSpeed] = new[] { 2f, 2.5f, 3f, 3.5f, 4f },
			[WeaponStatType.ProjectileLateralSpeed] = new[] { 0f, 0f, 0f, 0f, 0f },
			[WeaponStatType.AreaOfEffectRadius] =     new[] { 0f, 0f, 0f, 0f, 0f },
		};
	}
}