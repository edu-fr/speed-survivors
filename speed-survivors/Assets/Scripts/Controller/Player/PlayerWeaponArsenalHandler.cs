using System;
using System.Collections.Generic;
using Controller.Weapon;
using Domain.Interface.Player;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerWeaponArsenalHandler : MonoBehaviour
	{
		[field: SerializeField] private List<WeaponInstance> WeaponInstances { get; set; }

		private IPlayer Player { get; set; }
		private Dictionary<WeaponType, WeaponInstance> WeaponInstancesMap { get; set; }
		private IList<WeaponInstance> ActiveWeaponInstances { get; set; }

		public void Init(IPlayer player)
		{
			Player = player;
			CreateAndSetupWeaponArsenal();
		}

		public void Tick(float deltaTime, bool shouldShoot, float transformCurrentForwardVelocity)
		{
			for (var i = 0; i < ActiveWeaponInstances.Count; i++)
			{
				ActiveWeaponInstances[i].Tick(deltaTime, shouldShoot, transformCurrentForwardVelocity);
			}
		}

		private void CreateAndSetupWeaponArsenal()
		{
			if (Player == null)
				throw new InvalidOperationException("Player reference necessary to setup the PlayerWeaponArsenal");

			SetupWeaponInstancesMap();
			SetupActiveWeaponList();
		}

		private void SetupWeaponInstancesMap()
		{
			WeaponInstancesMap = new Dictionary<WeaponType, WeaponInstance>();
			foreach (var weaponInstance in WeaponInstances)
			{
				WeaponInstancesMap[weaponInstance.Config.WeaponType] = weaponInstance;
			}
		}

		private void SetupActiveWeaponList()
		{
			ActiveWeaponInstances = new List<WeaponInstance>();
			foreach (var weaponConfig in Player.Arsenal.ActiveWeapons)
			{
				AddWeaponInstance(weaponConfig);
			}

			Player.Arsenal.SubscribeToWeaponAdded(AddWeaponInstance);
		}

		private void AddWeaponInstance(IWeaponConfig weaponConfig)
		{
			if (!WeaponInstancesMap.TryGetValue(weaponConfig.WeaponType, out var weaponInstance))
				throw new InvalidOperationException(
					$"No WeaponInstance found for WeaponType {weaponConfig.WeaponType} in WeaponInstancesDict at PlayerWeaponArsenalHandler.");

			if (ActiveWeaponInstances.Contains(weaponInstance))
				throw new InvalidOperationException(
					$"Weapon instance of type {weaponConfig.WeaponType} is already active in PlayerWeaponArsenalHandler.");

			weaponInstance.gameObject.SetActive(true);
			ActiveWeaponInstances.Add(weaponInstance);
		}

		public void OnDestroy()
		{
			if (Player == null)
				throw new InvalidOperationException(
					"Player reference is null in OnDestroy from PlayerWeaponArsenalHandler.");

			Player.Arsenal.UnsubscribeFromWeaponAdded(AddWeaponInstance);
		}
	}
}