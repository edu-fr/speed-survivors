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
		void OnLootCollected(ILoot loot);
		void SubscribeToXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback);
		void UnsubscribeFromXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback);
	}
}