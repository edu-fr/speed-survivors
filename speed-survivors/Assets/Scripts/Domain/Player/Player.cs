using System;
using System.Collections.Generic;
using Domain.General;
using Domain.Interface.Config;
using Domain.Interface.General;
using Domain.Interface.Loot;
using Domain.Interface.Player;
using Domain.Interface.Weapon.Base;
using Domain.Weapon.Base;

namespace Domain.Player
{
	public class Player : IPlayer
	{
		public float CurrentHP { get; private set; }
		public IStats<StatType> Stats { get; private set; }
		public IWeaponArsenal Arsenal { get; private set; }
		public ILevelProgression LevelProgression { get; private set; }
		public event Action<(int currentXp, int level, int nextLevelXpDelta)> OnXpCollected;

		public Player(PlayerStats stats, IGrowthConfig growthConfig)
		{
			Stats = stats;
			CurrentHP = stats.GetStat(StatType.Health);
			Arsenal = new WeaponArsenal();
			LevelProgression = new LevelProgression(growthConfig);
		}

		public Player(IGrowthConfig growthConfig) // Debug default player
		{
			Stats = new PlayerStats();
			CurrentHP = Stats.GetStat(StatType.Health);
			Arsenal = new WeaponArsenal(new List<WeaponType>() { WeaponType.PeaShooter });
			LevelProgression = new LevelProgression(growthConfig);
		}

		public void OnLootCollected(ILoot loot)
		{
			switch (loot.Type)
			{
				case LootType.XP:
					ProgressXP(loot.Amount);
					break;
				case LootType.Coin:
					// Handle coin collection
					break;
				case LootType.Item:
					// Handle item collection
					break;
			}
		}

		private void ProgressXP(int amount)
		{
			LevelProgression.AddExperience(amount);
			OnXpCollected?.Invoke((
				LevelProgression.CurrentExperience,
				LevelProgression.CurrentLevel,
				LevelProgression.ExperienceRequiredForNextLevel - LevelProgression.ExperienceRequiredForPrevious));
		}

		public void SubscribeToXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback)
		{
			OnXpCollected += callback;
		}

		public void UnsubscribeFromXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback)
		{
			OnXpCollected -= callback;
		}
	}
}