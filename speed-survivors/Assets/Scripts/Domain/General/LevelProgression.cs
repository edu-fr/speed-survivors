using Domain.Config;
using Domain.Interface.Config;
using Domain.Interface.General;

namespace Domain.General
{
	public class LevelProgression : ILevelProgression
	{
		public int CurrentLevel { get; private set; }
		public int CurrentExperience { get; private set; }
		public int ExperienceRequiredForNextLevel { get; private set; }
		public IGrowthConfig GrowthConfig { get; private set; }

		public LevelProgression()
		{
			GrowthConfig = new GrowthConfig();
			CurrentLevel = 1;
			RecalculateNextThreshold();
		}

		public LevelProgression(IGrowthConfig growthConfig)
		{
			GrowthConfig = growthConfig;
			CurrentLevel = 1;
			RecalculateNextThreshold();
		}

		public void AddExperience(int amount)
		{
			CurrentExperience += amount;
			if (CurrentExperience >= ExperienceRequiredForNextLevel)
			{
				CurrentLevel++;
			}
		}

		private void RecalculateNextThreshold()
		{
			ExperienceRequiredForNextLevel = GrowthConfig.CalculateRequiredValue(CurrentLevel + 1);
		}
	}
}