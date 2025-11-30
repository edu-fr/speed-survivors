using Domain.Config;
using Domain.Interface.Config;
using Domain.Interface.General;

namespace Domain.General
{
	public class LevelProgression : ILevelProgression
	{
		public int CurrentExperience { get; private set; }
		public int TotalExperience { get; private set; }
		public int CurrentLevel { get; private set; }
		public int ExperienceRequiredForNextLevel { get; private set; }
		public int ExperienceRequiredForPrevious { get; private set; }
		public IGrowthConfig GrowthConfig { get; private set; }

		public LevelProgression()
		{
			GrowthConfig = new GrowthConfig();
			CurrentLevel = 1;
			TotalExperience = 0;
			CurrentExperience = 0;
			RecalculateRequiredAndCurrentXp();
		}

		public LevelProgression(IGrowthConfig growthConfig)
		{
			GrowthConfig = growthConfig;
			CurrentLevel = 1;
			TotalExperience = 0;
			RecalculateRequiredAndCurrentXp();
		}

		public void AddExperience(int amount)
		{
			TotalExperience += amount;
			CurrentExperience = TotalExperience - ExperienceRequiredForPrevious;
			if (TotalExperience < ExperienceRequiredForNextLevel)
				return;

			CurrentLevel++;
			RecalculateRequiredAndCurrentXp();
		}

		private void RecalculateRequiredAndCurrentXp()
		{
			ExperienceRequiredForPrevious = GrowthConfig.CalculateRequiredValue(CurrentLevel);
			ExperienceRequiredForNextLevel = GrowthConfig.CalculateRequiredValue(CurrentLevel + 1);
			CurrentExperience = TotalExperience - ExperienceRequiredForPrevious;
		}
	}
}