using System.Collections.Generic;

namespace Domain.Interface.Weapon
{
	public interface IWeaponArsenal
	{
		IList<IWeaponConfig> ActiveWeapons { get; }
		void AddWeapon(IWeaponConfig weaponConfig);
	}
}