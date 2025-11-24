using System;
using System.Collections.Generic;
using Domain.Interface.Weapon.Config;

namespace Domain.Interface.Weapon.Base
{
	public interface IWeaponArsenal
	{
		IList<IWeaponConfig> ActiveWeapons { get; }
		void AddWeapon(IWeaponConfig weaponConfig);
		public void SubscribeToWeaponAdded(Action<IWeaponConfig> callback);
		public void UnsubscribeFromWeaponAdded(Action<IWeaponConfig> callback);
	}
}