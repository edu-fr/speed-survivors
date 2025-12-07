using System;
using System.Collections.Generic;

namespace Domain.Interface.Weapon.Base
{
	public interface IWeaponArsenal
	{
		IList<WeaponType> StartingWeapons { get; }
		void AddWeapon(WeaponType weaponConfig);
		void UpdateWeaponLevel(WeaponType weaponType, int newLevel);
		bool HasWeapon(WeaponType weaponType);
		int GetWeaponLevel(WeaponType weaponType);
		public void SubscribeToWeaponAdded(Action<WeaponType> callback);
		public void UnsubscribeFromWeaponAdded(Action<WeaponType> callback);
	}
}