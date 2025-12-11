using System.Collections.Generic;
using Domain.Interface.Weapon.Base;

namespace Domain.Weapon.Config
{
	public class GrenadeLauncherConfig : BaseWeaponConfig
	{
		public override WeaponType WeaponType => WeaponType.GrenadeLauncher;
		protected override Dictionary<WeaponStatType, float[]> StatsByLevel { get; } = new()
		{
			[WeaponStatType.DamagePerHit] =           new[] { 15f, 15f, 15f, 20f, 20f },
			[WeaponStatType.Range] =                  new[] { 3f, 3f, 3f, 3f, 3f },
			[WeaponStatType.ProjectilesPerShot] =     new[] { 1f, 1f, 1f, 1f, 1f },
			[WeaponStatType.FireRate] =               new[] { 2f, 2f, 1.8f, 1.8f, 1.5f },
			[WeaponStatType.ProjectileForwardSpeed] = new[] { 5f, 5f, 5f, 5f, 5f },
			[WeaponStatType.ProjectileLateralSpeed] = new[] { 0f, 0f, 0f, 0f, 0f },
			[WeaponStatType.AreaOfEffectRadius] =     new[] { 3f, 3.5f, 4f, 4.5f, 5f },
		};
	}
}