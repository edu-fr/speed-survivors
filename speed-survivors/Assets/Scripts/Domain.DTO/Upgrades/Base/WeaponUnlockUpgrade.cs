using Domain.DTO.Mapper;
using Domain.Interface.Player;
using Domain.Interface.Weapon.Base;
using UnityEngine;

namespace Domain.DTO.Upgrades.Base
{
	[CreateAssetMenu(menuName = "Upgrades/Weapon Unlock")]
	public class WeaponUnlockUpgrade : UpgradeEffectSO
	{
		protected override string Id => WeaponType.ToString();

		[field: SerializeField]
		private WeaponType WeaponType { get; set; }

		public override void Apply(IPlayer player)
		{
			player.Arsenal.AddWeapon(WeaponMapper.IdToConfig(Id));
		}
	}
}