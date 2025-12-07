using Domain.Interface.Player;
using Domain.Interface.Upgrade;
using Domain.Interface.Weapon.Base;

namespace Domain.Upgrade
{
	public class WeaponUnlockUpgrade : BaseUpgrade
	{
		public override UpgradeType Type => UpgradeType.WeaponUnlock;
		public override string Title => $"Unlock {WeaponType.ToString()}";
		public override string Description => $"Add the {WeaponType.ToString()} to your current arsenal.";
		private WeaponType WeaponType { get; }

		public WeaponUnlockUpgrade(WeaponType weaponType)
		{
			WeaponType = weaponType;
		}

		public override bool Eligible(IPlayer player)
		{
			return !player.Arsenal.HasWeapon(WeaponType);
		}

		public override void Apply(IPlayer player)
		{
			player.Arsenal.AddWeapon(WeaponType);
		}
	}
}