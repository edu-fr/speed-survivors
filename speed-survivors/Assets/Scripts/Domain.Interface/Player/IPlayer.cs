using System;
using Domain.Interface.General;
using Domain.Interface.Loot;
using Domain.Interface.Weapon.Base;

namespace Domain.Interface.Player
{
	public interface IPlayer
	{
		float CurrentHP { get; }
		IStats<StatType> Stats { get; }
		IWeaponArsenal Arsenal { get; }
		ILevelProgression LevelProgression { get; }
		bool IsAlive { get; }
		bool TakeDamage(float amount);
		void Heal(float amount);
		void OnLootCollected(ILoot loot);
		void SubscribeToXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback);
		void UnsubscribeFromXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback);
		void SubscribeToCurrentHpChanged(Action<float, float> handlePlayerHpChanged);
		void UnsubscribeFromCurrentHpChanged(Action<float, float> handlePlayerHpChanged);
		void SubscribeToStatsUpdate(Action<StatType, float, float> handleStatsUpdate);
		void UnsubscribeFromStatsUpdate(Action<StatType, float, float> handleStatsUpdate);
		void SubscribeToPlayerDeath(Action handlePlayerDeath);
		void UnsubscribeFromPlayerDeath(Action handlePlayerDeath);
	}
}