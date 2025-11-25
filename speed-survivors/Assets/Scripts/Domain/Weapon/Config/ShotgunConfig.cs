using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;

namespace Domain.Weapon.Config
{
	public class ShotgunConfig : IWeaponConfig
	{
		public WeaponType WeaponType => WeaponType.Shotgun;
		public float BaseDamage => 20;
		public float Range => 5;
		public float BaseCooldown => 1.5f;
		public float ProjectileSpeed => 15f;
	}
}