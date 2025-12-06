using Domain.Interface.General;
using Domain.Interface.Weapon.Base;

namespace Domain.Interface.Weapon.Config
{
	public interface IWeaponConfig
	{
		WeaponType WeaponType { get; }
		IStats<WeaponStatType> Stats { get; }
	}
}