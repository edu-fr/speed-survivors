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
		private event Action<(int currentXp, int level, int nextLevelXpDelta)> OnXpCollected;
		private event Action<float, float> OnCurrentHpChanged;
		private event Action OnPlayerDeath;

		public bool IsAlive => CurrentHP > 0;

		public Player(PlayerStats stats, IGrowthConfig growthConfig)
		{
			Stats = stats;
			CurrentHP = stats.GetStat(StatType.MaxHealth);
			Arsenal = new WeaponArsenal();
			LevelProgression = new LevelProgression(growthConfig);
		}

		public Player(IGrowthConfig growthConfig) // Debug default player
		{
			Stats = new PlayerStats();
			CurrentHP = Stats.GetStat(StatType.MaxHealth);
			Arsenal = new WeaponArsenal(new List<WeaponType>() { WeaponType.IceWind });
			LevelProgression = new LevelProgression(growthConfig);
		}

		public void OnLootCollected(ILoot loot)
		{
			if (!IsAlive)
				return;

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

		public bool TakeDamage(float amount)
		{
			if (!IsAlive)
				return false;

			var oldHp = CurrentHP;
			CurrentHP = MathF.Max(CurrentHP - amount, 0);
			OnCurrentHpChanged?.Invoke(CurrentHP, oldHp - CurrentHP);

			if (CurrentHP == 0)
				OnPlayerDeath?.Invoke();

			return CurrentHP == 0;
		}

		public void Heal(float amount)
		{
			if (!IsAlive)
				return;

			var oldHp = CurrentHP;
			CurrentHP = MathF.Min(CurrentHP + amount, Stats.GetStat(StatType.MaxHealth));
			OnCurrentHpChanged?.Invoke(CurrentHP, CurrentHP - oldHp);
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

		public void SubscribeToCurrentHpChanged(Action<float, float> handlePlayerHpChanged)
		{
			OnCurrentHpChanged += handlePlayerHpChanged;
		}

		public void UnsubscribeFromCurrentHpChanged(Action<float, float> handlePlayerHpChanged)
		{
			OnCurrentHpChanged -= handlePlayerHpChanged;
		}

		public void SubscribeToStatsUpdate(Action<StatType, float, float> handleStatsUpdate)
		{
			Stats.OnStatChanged += handleStatsUpdate;
		}

		public void UnsubscribeFromStatsUpdate(Action<StatType, float, float> handleStatsUpdate)
		{
			Stats.OnStatChanged -= handleStatsUpdate;
		}

		public void SubscribeToPlayerDeath(Action handlePlayerDeath)
		{
			OnPlayerDeath += handlePlayerDeath;
		}

		public void UnsubscribeFromPlayerDeath(Action handlePlayerDeath)
		{
			OnPlayerDeath -= handlePlayerDeath;
		}
	}
}