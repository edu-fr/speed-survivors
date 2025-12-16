using System;
using System.Collections.Generic;
using Controller.General;
using Controller.Weapon;
using Controller.Weapon.Ammo;
using Domain.Interface.Player;
using Domain.Interface.Weapon.Base;
using UnityEngine;

namespace Controller.Player
{
	[Serializable]
	public class PlayerWeaponArsenalHandler : Initializable
	{
		/// <summary>
		/// Contains all weapons available at this point in the game
		/// </summary>
		[field: SerializeField]
		private List<BaseWeaponInstance> WeaponInstances { get; set; }

		private Dictionary<WeaponType, BaseWeaponInstance> WeaponInstancesMap { get; set; }
		private IList<BaseWeaponInstance> ActiveWeaponInstances { get; set; }
		private IPlayer PlayerDomainRef { get; set; }
		private ProjectileHandler ProjectileHandler { get; set; }

		public void Init(IPlayer playerDomainRef, ProjectileHandler projectileHandler)
		{
			EnsureStillNotInitialized();

			PlayerDomainRef = playerDomainRef;
			ProjectileHandler = projectileHandler;
			CreateAndSetupWeaponArsenal();

			Initialized = true;
		}

		public void Tick(float deltaTime, bool shouldShoot, float transformCurrentForwardVelocity)
		{
			CheckInit();

			for (var i = 0; i < ActiveWeaponInstances.Count; i++)
			{
				var weaponLevel = PlayerDomainRef.Arsenal.GetWeaponLevel(ActiveWeaponInstances[i].Config.WeaponType);
				ActiveWeaponInstances[i].Tick(deltaTime, shouldShoot, transformCurrentForwardVelocity, weaponLevel);
			}
		}

		private void CreateAndSetupWeaponArsenal()
		{
			SetupWeaponInstancesMap();
			SetupActiveWeaponList();
			PlayerDomainRef.Arsenal.SubscribeToWeaponAdded(AddWeaponInstance);
		}

		private void SetupWeaponInstancesMap()
		{
			WeaponInstancesMap = new Dictionary<WeaponType, BaseWeaponInstance>();
			foreach (var weaponInstance in WeaponInstances)
			{
				WeaponInstancesMap[weaponInstance.Config.WeaponType] = weaponInstance;
			}
		}

		// Necessary because when the Domain was created the ArsenalHandler was not subscribed to its changes yet
		private void SetupActiveWeaponList()
		{
			ActiveWeaponInstances = new List<BaseWeaponInstance>();
			foreach (var weaponType in PlayerDomainRef.Arsenal.StartingWeapons)
			{
				AddWeaponInstance(weaponType);
			}
		}

		private void AddWeaponInstance(WeaponType weaponType)
		{
			if (!WeaponInstancesMap.TryGetValue(weaponType, out var weaponInstance))
				throw new InvalidOperationException(
					$"No WeaponInstance found for WeaponType {weaponType} in WeaponInstancesDict at PlayerWeaponArsenalHandler.");

			if (ActiveWeaponInstances.Contains(weaponInstance))
				throw new InvalidOperationException(
					$"WeaponInstance for WeaponType {weaponType} is already active in PlayerWeaponArsenalHandler.");

			weaponInstance.gameObject.SetActive(true);

			switch (weaponInstance)
			{
				case ProjectileWeaponInstance projectileWeaponInstance:
					projectileWeaponInstance.Init(ProjectileHandler);
					break;
				case AuraWeaponInstance auraWeaponInstance:
					auraWeaponInstance.Init();
					break;
			}

			ActiveWeaponInstances.Add(weaponInstance);

			Debug.Log($"Added {weaponType} to Active Weapons Instances");
		}

		~PlayerWeaponArsenalHandler()
		{
			PlayerDomainRef.Arsenal.UnsubscribeFromWeaponAdded(AddWeaponInstance);
		}
	}
}