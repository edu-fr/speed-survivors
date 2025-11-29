namespace Domain.Interface.Config
{
	public interface IGrowthConfig
	{
		GrowthStrategy Strategy { get; }
		int BaseValue { get; }
		float GrowthFactor { get; }
		float PolynomialPower { get; }
		int CalculateRequiredValue(int targetGoal);
	}
}