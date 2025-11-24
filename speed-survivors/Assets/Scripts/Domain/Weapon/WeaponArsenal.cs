using System.Collections.Generic;
using System.Diagnostics;
using Domain.Interface.Weapon;

namespace Domain.Weapon
{
	public class WeaponArsenal : IWeaponArsenal
	{
		public IList<IWeaponConfig> ActiveWeapons { get; private set; }

		public WeaponArsenal(IList<IWeaponConfig> startingWeapons = null)
		{
			ActiveWeapons = new List<IWeaponConfig>(startingWeapons ?? new List<IWeaponConfig>());

			Debugger.Log(0, "WeaponArsenal", $"Initialized with {ActiveWeapons.Count} weapons.");
		}

		public void AddWeapon(IWeaponConfig weaponConfig)
		{
			ActiveWeapons.Add(weaponConfig);
		}
	}
}