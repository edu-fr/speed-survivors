using System;
using Domain.Interface.Config;

namespace Domain.Config
{
	public class GrowthConfig : IGrowthConfig
	{
		public GrowthStrategy Strategy { get; private set; }
		public int BaseValue { get; private set; }
		public float GrowthFactor { get; private set; }
		public float PolynomialPower { get; private set; }

		public GrowthConfig()
		{
			Strategy = GrowthStrategy.Linear;
			BaseValue = 10;
			GrowthFactor = 2.0f;
			PolynomialPower = 2.0f;
		}

		public GrowthConfig(GrowthStrategy strategy, int baseValue, float growthFactor, float polyPower)
		{
			Strategy = strategy;
			BaseValue = baseValue;
			GrowthFactor = growthFactor;
			PolynomialPower = polyPower;
		}

		public int CalculateRequiredValue(int targetGoal)
		{
			if (targetGoal <= 1)
				return 0;

			var x = targetGoal - 1;

			return Strategy switch
			{
				GrowthStrategy.Linear => (int)(BaseValue + x * GrowthFactor),
				GrowthStrategy.Exponential => (int)(BaseValue * Math.Pow(GrowthFactor, x)),
				GrowthStrategy.Polynomial => (int)(BaseValue * Math.Pow(targetGoal, PolynomialPower) / 2),
				_ => BaseValue
			};
		}
	}
}