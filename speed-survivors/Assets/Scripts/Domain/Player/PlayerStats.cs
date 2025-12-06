using System.Collections.Generic;
using Domain.General;
using Domain.Interface.General;

namespace Domain.Player
{
	public sealed class PlayerStats : BaseStats<StatType>
	{
		protected override Dictionary<StatType, float> StatDict { get; set; }

		public PlayerStats()
		{
			// Placeholder initial stats
			StatDict = new Dictionary<StatType, float>
			{
				{ StatType.Health, 100f },
				{ StatType.ForwardMoveSpeed, 15f },
				{ StatType.LateralMoveSpeed, 10f },
				{ StatType.Damage, 5f },
				{ StatType.MagnetRadius, 1.2f }
			};
		}

	}
}