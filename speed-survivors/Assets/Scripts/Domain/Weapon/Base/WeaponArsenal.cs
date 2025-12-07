using System;
using System.Collections.Generic;
using Domain.Interface.Weapon.Base;

namespace Domain.Weapon.Base
{
	public class WeaponArsenal : IWeaponArsenal
	{
		public IList<WeaponType> StartingWeapons { get; }
		private Dictionary<WeaponType, int> ActiveWeapons { get; set; }
		private event Action<WeaponType> OnWeaponAdded;

		public WeaponArsenal(IList<WeaponType> startingWeapons = null)
		{
			ActiveWeapons = new Dictionary<WeaponType, int>();
			if (startingWeapons == null)
				return;

			StartingWeapons = startingWeapons;

			foreach (var weapon in startingWeapons)
			{
				ActiveWeapons[weapon] = 1; // Initialize starting weapons at level 1
			}
		}

		public void AddWeapon(WeaponType weaponType)
		{
			if (!ActiveWeapons.TryAdd(weaponType, 1))
				throw new InvalidOperationException("Weapon already present in player domain arsenal");

			OnWeaponAdded?.Invoke(weaponType);
		}

		public bool HasWeapon(WeaponType weaponType)
		{
			return ActiveWeapons.ContainsKey(weaponType);
		}

		public int GetWeaponLevel(WeaponType weaponType)
		{
			if (!ActiveWeapons.TryGetValue(weaponType, out var level))
				throw new InvalidOperationException("Weapon not present in player domain arsenal");

			return level;
		}

		public void SubscribeToWeaponAdded(Action<WeaponType> callback)
		{
			OnWeaponAdded += callback;
		}

		public void UnsubscribeFromWeaponAdded(Action<WeaponType> callback)
		{
			OnWeaponAdded -= callback;
		}

		public void UpdateWeaponLevel(WeaponType weaponType, int level)
		{
			if (!ActiveWeapons.ContainsKey(weaponType))
				throw new InvalidOperationException("Weapon not present in player domain arsenal");

			ActiveWeapons[weaponType] = level;
		}
	}
}