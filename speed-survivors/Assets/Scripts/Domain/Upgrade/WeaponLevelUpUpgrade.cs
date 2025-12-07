using Domain.Interface.Player;
using Domain.Interface.Upgrade;
using Domain.Interface.Weapon.Base;

namespace Domain.Upgrade
{
	public class WeaponLevelUpUpgrade : BaseUpgrade
	{
		public const int WeaponMaxLevel = 5;
		public override UpgradeType Type => UpgradeType.WeaponLevelUp;
		public override string Title => $"Upgrade the {WeaponType.ToString()} to level {Level}";
		public override string Description => $"Upgrade {WeaponType.ToString()} to your current arsenal.";
		private WeaponType WeaponType { get; }
		private int Level { get; }

		public WeaponLevelUpUpgrade(WeaponType type, int level)
		{
			WeaponType = type;
			Level = level;
		}

		public override bool Eligible(IPlayer player)
		{
			return player.Arsenal.HasWeapon(WeaponType) &&
			       Level < WeaponMaxLevel &&
			       Level == player.Arsenal.GetWeaponLevel(WeaponType) + 1;
		}

		public override void Apply(IPlayer player)
		{
			player.Arsenal.UpdateWeaponLevel(WeaponType, Level);
		}
	}
}