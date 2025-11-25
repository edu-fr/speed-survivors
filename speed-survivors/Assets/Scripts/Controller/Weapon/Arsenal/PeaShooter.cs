using Controller.Interface.Weapon.Strategy;
using Controller.Weapon.Ammo;
using Controller.Weapon.Strategy;
using Domain.Interface.Weapon.Config;
using Domain.Weapon.Config;
using UnityEngine;

namespace Controller.Weapon.Arsenal
{
	public class PeaShooter : WeaponInstance
	{
		[field: SerializeField]
		protected override Projectile ProjectilePrefab { get; set; }
		public override IWeaponConfig Config => new PeaShooterConfig();
		protected override IWeaponStrategy Strategy => new PeaShooterStrategy();
	}
}