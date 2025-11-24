using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;

namespace Domain.Weapon.Config
{
	public class PeaShooterConfig : IWeaponConfig
	{
		public WeaponType WeaponType => WeaponType.PeaShooter;
		public float BaseDamage => 10;
		public float Range => 15;
		public float BaseCooldown => 1f;
	}
}