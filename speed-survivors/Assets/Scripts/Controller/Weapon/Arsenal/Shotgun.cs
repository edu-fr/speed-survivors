using Controller.Interface.Weapon.Strategy;
using Controller.Weapon.Ammo;
using Controller.Weapon.Strategy;
using Domain.Interface.Weapon.Config;
using Domain.Weapon.Config;
using UnityEngine;

namespace Controller.Weapon.Arsenal
{
	public class Shotgun : WeaponInstance
	{
		[field: SerializeField]
		protected override Projectile ProjectilePrefab { get; set; }
		public override IWeaponConfig Config { get; } = new ShotgunConfig();
		protected override IWeaponStrategy Strategy { get; } = new PeaShooterStrategy();
	}
}