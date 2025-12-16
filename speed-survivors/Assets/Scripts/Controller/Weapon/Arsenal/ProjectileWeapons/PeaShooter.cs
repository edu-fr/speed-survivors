using Controller.Weapon.Ammo;
using Domain.Interface.Weapon.Config;
using Domain.Interface.Weapon.Strategy;
using Domain.Weapon.Config;
using Domain.Weapon.Strategy.ProjectileStrategies;
using UnityEngine;

namespace Controller.Weapon.Arsenal.ProjectileWeapons
{
	public class PeaShooter : ProjectileWeaponInstance
	{
		[field: SerializeField]
		protected override Projectile ProjectilePrefab { get; set; }

		public override IWeaponConfig Config => new PeaShooterConfig();
		protected override IWeaponStrategy Strategy => new PeaShooterStrategy();
	}
}