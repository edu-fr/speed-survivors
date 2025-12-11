using Controller.Weapon.Ammo;
using Domain.Interface.Weapon.Config;
using Domain.Interface.Weapon.Strategy;
using Domain.Weapon.Config;
using Domain.Weapon.Strategy;
using UnityEngine;

namespace Controller.Weapon.Arsenal
{
	public class GrenadeLauncher : WeaponInstance
	{
		[field: SerializeField]
		protected override Projectile ProjectilePrefab { get; set; }
		public override IWeaponConfig Config => new GrenadeLauncherConfig();
		protected override IWeaponStrategy Strategy => new GranadeLauncherStrategy();
	}
}