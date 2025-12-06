using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Domain.DTO.Mapper
{
	public static class WeaponMapper
	{
		public static UpgradeOptionData ConfigToDTO(IWeaponConfig weapon)
		{
			return new UpgradeOptionData
			{
				UniqueId = weapon.WeaponType.ToString(),
				Title = weapon.WeaponType.ToString(),
				Description = weapon.WeaponType + " Description",
				Icon = null,
				RarityColor = Color.white
			};
		}

		public static IWeaponConfig IdToConfig(string uniqueId)
		{
			return uniqueId switch
			{
				"PeaShooter" => new Domain.Weapon.Config.PeaShooterConfig(),
				"Shotgun" => new Domain.Weapon.Config.ShotgunConfig(),
				_ => throw new System.ArgumentException($"Unknown weapon ID: {uniqueId}")
			};
		}
	}
}