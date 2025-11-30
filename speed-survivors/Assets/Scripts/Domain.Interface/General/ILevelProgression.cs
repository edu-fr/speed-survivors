using Domain.Interface.Config;

namespace Domain.Interface.General
{
	public interface ILevelProgression
	{
		int CurrentExperience { get; }
		int TotalExperience { get; }
		int CurrentLevel { get; }
		int ExperienceRequiredForPrevious { get; }
		int ExperienceRequiredForNextLevel { get; }
		IGrowthConfig GrowthConfig { get; }
		void AddExperience(int amount);
	}
}