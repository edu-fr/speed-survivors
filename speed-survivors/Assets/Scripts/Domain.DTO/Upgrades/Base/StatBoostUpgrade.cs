using Domain.Interface.General;
using Domain.Interface.Player;
using UnityEngine;

namespace Domain.DTO.Upgrades.Base
{
	[CreateAssetMenu(menuName = "Upgrades/Stat Boost")]
	public class StatBoostUpgrade : UpgradeEffectSO
	{
		protected override string Id => StatType.ToString();

		[field: SerializeField]
		public StatType StatType { get; private set; }

		[field: SerializeField]
		private float Amount { get; set; }

		public override void Apply(IPlayer player)
		{
			player.Stats.IncreaseStat(StatType, Amount);
		}
	}
}