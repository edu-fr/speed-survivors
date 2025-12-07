using Domain.Interface.Weapon.Base;

namespace Domain.Interface.Weapon.Config
{
	public interface IWeaponConfig
	{
		WeaponType WeaponType { get; }
		float GetStat(WeaponStatType type, int level);
	}
}