using System;
using System.Linq;
using Domain.Interface.Player;
using Domain.Interface.Weapon.Base;
using UnityEngine;

namespace Domain.DTO.Upgrades.Base
{
	[CreateAssetMenu(menuName = "Upgrades/Weapon Upgrade")]
	public class WeaponBoostUpgrade : UpgradeEffectSO
	{
		protected override string Id => WeaponType + "_" + WeaponStatType;

		[field: SerializeField]
		private WeaponType WeaponType { get; set; }

		[field: SerializeField]
		private WeaponStatType WeaponStatType { get; set; }

		[field: SerializeField]
		private float Amount { get; set;}

		public override void Apply(IPlayer player)
		{
			var playerWeapon = player.Arsenal.ActiveWeapons.FirstOrDefault(w => w.WeaponType == WeaponType);
			if (playerWeapon == null)
				throw new InvalidOperationException($"Player does not have weapon of type {WeaponType} to apply boost upgrade.");

			playerWeapon.Stats.IncreaseStat(WeaponStatType, Amount);
		}
	}
}