using Domain.DTO;
using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Controller.Mapper
{
	public static class WeaponMapper
	{
		public static UpgradeOptionData ToDTO(IWeaponConfig weapon)
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
	}
}