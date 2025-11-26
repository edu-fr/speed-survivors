using System;
using System.Collections.Generic;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;

namespace Domain.Weapon.Base
{
	public class WeaponArsenal : IWeaponArsenal
	{
		public IList<IWeaponConfig> ActiveWeapons { get; private set; }
		private event Action<IWeaponConfig> OnWeaponAdded;

		public WeaponArsenal(IList<IWeaponConfig> startingWeapons = null)
		{
			ActiveWeapons = new List<IWeaponConfig>(startingWeapons ?? new List<IWeaponConfig>());
		}

		public void AddWeapon(IWeaponConfig weaponConfig)
		{
			ActiveWeapons.Add(weaponConfig);
			OnWeaponAdded?.Invoke(weaponConfig);
		}

		public void SubscribeToWeaponAdded(Action<IWeaponConfig> callback)
		{
			OnWeaponAdded += callback;
		}

		public void UnsubscribeFromWeaponAdded(Action<IWeaponConfig> callback)
		{
			OnWeaponAdded -= callback;
		}
	}
}