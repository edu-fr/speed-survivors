using System;
using System.Collections.Generic;
using Domain.General;
using Domain.Interface.Config;
using Domain.Interface.General;
using Domain.Interface.Loot;
using Domain.Interface.Player;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;
using Domain.Weapon.Base;
using Domain.Weapon.Config;

namespace Domain.Player
{
	public class Player : IPlayer
	{
		public float MaxHP { get; private set; }
		public float LateralLateralMoveSpeed { get; private set; }
		public float ForwardMoveSpeed { get; private set; }
		public float BaseDamage { get; private set; }
		public float CurrentHP { get; private set; }
		public IWeaponArsenal Arsenal { get; private set; }
		public float MagnetRadius { get; private set; }
		public ILevelProgression LevelProgression { get; private set; }
		public event Action<(int currentXp, int level, int nextLevelXpDelta)> OnXpCollected;

		public Player(float maxHP, float lateralMoveSpeed, float forwardMoveSpeed, float baseDamage, float magnetRadius,
			IGrowthConfig growthConfig)
		{
			MaxHP = maxHP;
			LateralLateralMoveSpeed = lateralMoveSpeed;
			ForwardMoveSpeed = forwardMoveSpeed;
			BaseDamage = baseDamage;
			CurrentHP = maxHP;
			Arsenal = new WeaponArsenal();
			MagnetRadius = magnetRadius;
			LevelProgression = new LevelProgression(growthConfig);
		}

		public Player(IGrowthConfig growthConfig) // Debug default player
		{
			MaxHP = 100f;
			LateralLateralMoveSpeed = 30f;
			ForwardMoveSpeed = 5f;
			BaseDamage = 5f;
			CurrentHP = MaxHP;
			Arsenal = new WeaponArsenal(new List<IWeaponConfig>() { new PeaShooterConfig() });
			MagnetRadius = 2f;
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