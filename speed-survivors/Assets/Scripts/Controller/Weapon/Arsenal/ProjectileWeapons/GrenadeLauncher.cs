using Domain.Interface.Weapon.Config;
using Domain.Interface.Weapon.Strategy;
using Domain.Weapon.Config;
using Domain.Weapon.Strategy.ProjectileStrategies;
using UnityEngine;

namespace Controller.Weapon.Arsenal.ProjectileWeapons
{
	public class GrenadeLauncher : ProjectileWeaponInstance
	{
		[field: SerializeField]
		protected override Ammo.Projectile ProjectilePrefab { get; set; }

		public override IWeaponConfig Config => new GrenadeLauncherConfig();
		protected override IWeaponStrategy Strategy => new GranadeLauncherStrategy();
	}
}