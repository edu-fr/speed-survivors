using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;

namespace Domain.Weapon.Config
{
	public class PeaShooterConfig : IWeaponConfig
	{
		public WeaponType WeaponType => WeaponType.PeaShooter;
		public float BaseDamage => 5;
		public float Range => 20;
		public float BaseCooldown => 0.004f;
		public float ProjectileSpeed => 3f;
	}
}