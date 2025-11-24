using System;
using System.Collections.Generic;
using Controller.Weapon;
using Domain.Interface.Player;
using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerWeaponArsenalHandler
	{
		private IPlayer Player { get; set; }
		private IList<WeaponInstance> ActiveWeaponInstances { get; set; }

		public PlayerWeaponArsenalHandler(IPlayer player)
		{
			Player = player;
			CreateAndSetupWeaponArsenal();
		}

		public void Tick(Vector3 playerPosition)
		{
			foreach (var weaponInstance in ActiveWeaponInstances)
			{
				weaponInstance.Tick(playerPosition);
			}
		}

		private void CreateAndSetupWeaponArsenal()
		{
			if (Player == null)
				throw new InvalidOperationException("Player reference is null in PlayerWeaponArsenalHandler.");

			ActiveWeaponInstances = new List<WeaponInstance>();

			foreach (var weaponConfig in Player.Arsenal.ActiveWeapons)
			{
				AddWeaponInstance(weaponConfig);
			}

			Player.Arsenal.SubscribeToWeaponAdded(AddWeaponInstance);
		}

		private void AddWeaponInstance(IWeaponConfig weaponConfig)
		{
			ActiveWeaponInstances.Add(WeaponHelper.CreateWeaponInstanceByType(weaponConfig));
		}

		public void OnDestroy()
		{
			if (Player == null)
				throw new InvalidOperationException("Player reference is null in OnDestroy from PlayerWeaponArsenalHandler.");

			Player.Arsenal.UnsubscribeFromWeaponAdded(AddWeaponInstance);
		}
	}
}