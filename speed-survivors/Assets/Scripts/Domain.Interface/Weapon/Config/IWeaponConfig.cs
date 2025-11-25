using Domain.Interface.Weapon.Base;

namespace Domain.Interface.Weapon.Config
{
	public interface IWeaponConfig
	{
		WeaponType WeaponType { get; }
		float BaseDamage { get; }
		float Range { get; }
		float BaseCooldown { get; }
		float ProjectileSpeed { get; }
	}
}