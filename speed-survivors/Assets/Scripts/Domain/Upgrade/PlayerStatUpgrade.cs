using Domain.Interface.General;
using Domain.Interface.Player;
using Domain.Interface.Upgrade;

namespace Domain.Upgrade
{
	public class PlayerStatUpgrade : BaseUpgrade
	{
		public override UpgradeType Type => UpgradeType.PlayerStats;
		public override string Title => $"Increase {StatType}";
		public override string Description => $"Upgrade {StatType} by {Amount}.";
		private StatType StatType { get; }
		private float Amount { get; }

		public PlayerStatUpgrade(StatType statType, float amount)
		{
			StatType = statType;
			Amount = amount;
		}

		public override bool Eligible(IPlayer player)
		{
			return true;
		}

		public override void Apply(IPlayer player)
		{
			player.Stats.IncreaseStat(StatType, Amount);
		}
	}
}