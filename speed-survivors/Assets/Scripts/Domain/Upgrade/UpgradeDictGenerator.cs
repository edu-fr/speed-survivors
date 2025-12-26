using System.Collections.Generic;
using Domain.Interface.General;
using Domain.Interface.Player;
using Domain.Interface.Upgrade;
using Domain.Interface.Weapon.Base;

namespace Domain.Upgrade
{
	public static class UpgradeDictGenerator
	{
		private static Dictionary<UpgradeType, List<IUpgrade>> UpgradeDict { get; set; } = new()
		{
			[UpgradeType.WeaponUnlock] = new()
			{
				new WeaponUnlockUpgrade(WeaponType.PeaShooter),
				new WeaponUnlockUpgrade(WeaponType.Shotgun)
			},
			[UpgradeType.WeaponLevelUp] = new()
			{
				new WeaponLevelUpUpgrade(WeaponType.PeaShooter, 2),
				new WeaponLevelUpUpgrade(WeaponType.PeaShooter, 3),
				new WeaponLevelUpUpgrade(WeaponType.Shotgun, 2),
			},

			[UpgradeType.PlayerStats] = new()
			{
				new PlayerStatUpgrade(StatType.MaxHealth, 20f),
				new PlayerStatUpgrade(StatType.ForwardMoveSpeed, 10f),
				new PlayerStatUpgrade(StatType.LateralMoveSpeed, 5f),
				new PlayerStatUpgrade(StatType.Damage, 1f),
				new PlayerStatUpgrade(StatType.MagnetRange, 0.5f),
			}
		};

		public static List<IUpgrade> GetRandomEligibleUpgrades(IPlayer player, int amount)
		{
			var randomUpgradeList = new List<IUpgrade>();
			var upgradeTypes = new List<UpgradeType>(UpgradeDict.Keys);
			var rand = new System.Random();
			var eligibleUpgrades = new List<IUpgrade>();
			foreach (var upgradeType in upgradeTypes)
			{
				foreach (var upgrade in UpgradeDict[upgradeType])
				{
					if (upgrade.Eligible(player))
					{
						eligibleUpgrades.Add(upgrade);
					}
				}
			}

			while (randomUpgradeList.Count < amount)
			{
				var randomUpgrade = eligibleUpgrades[rand.Next(eligibleUpgrades.Count)];
				if (!randomUpgradeList.Contains(randomUpgrade))
				{
					randomUpgradeList.Add(randomUpgrade);
				}
			}

			return randomUpgradeList;
		}
	}
}