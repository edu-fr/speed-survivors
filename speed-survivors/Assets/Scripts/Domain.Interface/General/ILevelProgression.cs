using Domain.Interface.Config;

namespace Domain.Interface.General
{
	public interface ILevelProgression
	{
		int CurrentLevel { get; }
		int CurrentExperience { get; }
		int ExperienceRequiredForNextLevel { get; }
		IGrowthConfig GrowthConfig { get; }
		void AddExperience(int amount);
	}
}