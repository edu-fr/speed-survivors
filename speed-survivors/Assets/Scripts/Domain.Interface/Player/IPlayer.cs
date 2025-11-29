using System;
using Domain.Interface.General;
using Domain.Interface.Loot;
using Domain.Interface.Weapon.Base;

namespace Domain.Interface.Player
{
	public interface IPlayer
	{
		float MaxHP { get; }

		/// <summary>
		/// Starting at 10, and increasing linearly up to 50.
		/// In the player movement handler, translates to SmoothDamp movement from 0.2s up to 0.05s.
		/// </summary>
		float LateralLateralMoveSpeed { get; }

		/// <summary>
		/// Starting at 2, and increasing linearly up to 10.
		/// </summary>
		float ForwardMoveSpeed { get; }

		float BaseDamage { get; }
		float CurrentHP { get; }
		IWeaponArsenal Arsenal { get; }
		float MagnetRadius { get; }
		ILevelProgression LevelProgression { get; }
		void OnLootCollected(ILoot loot);
		void SubscribeToXpCollected(Action<(int xp, int level, int nextLevelXp)> callback);
		void UnsubscribeFromXpCollected(Action<(int xp, int level, int nextLevelXp)> callback);
	}
}