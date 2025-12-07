using System;
using Controller.DebugController;
using Controller.General.Base;
using Controller.Weapon.Ammo;
using Data.ScriptableObjects.Generator;
using Domain.Interface.General;
using Domain.Interface.Loot;
using Domain.Interface.Player;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerController : InitializableMono
	{
		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		[field: SerializeField]
		private PlayerWeaponArsenalHandler WeaponArsenalHandler { get; set; }

		[field: SerializeField]
		private GrowthConfigGeneratorSO LevelProgressionSO { get; set; }

		private IPlayer Player { get; set; }
		private PlayerInputHandler InputHandler { get; set; }
		private PlayerMovementHandler MovementHandler { get; set; }
		private int XpCollectedSubscribeCount { get; set; }

		public void Init(Camera mainCamera, Vector3 startingPos, float xMoveRange, ProjectileHandler projectileHandler)
		{
			EnsureStillNotInit();

			Player = new Domain.Player.Player(LevelProgressionSO.ToDomain());
			WeaponArsenalHandler.Init(Player, projectileHandler);
			InputHandler = new PlayerInputHandler(mainCamera);
			MovementHandler = new PlayerMovementHandler(Player, transform, xMoveRange, startingPos.x);

			SetupStartingPosition(startingPos);

			Initialized = true;
		}

		public void Tick(float deltaTime)
		{
			CheckInit();

			DebugTick();
			WeaponArsenalHandler.Tick(deltaTime, true, Player.Stats.GetStat(StatType.ForwardMoveSpeed));
			HandleInput();
			HandleMovement(deltaTime);
		}

		public IPlayer GetPlayerDomainRef()
		{
			CheckInit();

			return Player;
		}

		private void DebugTick()
		{
			var debugInstance = DebugOverlayManager.Instance;
			debugInstance.Track("Total XP: ", Player.LevelProgression.TotalExperience);
			debugInstance.Track("Next level total XP: ", Player.LevelProgression.ExperienceRequiredForNextLevel);
		}

		private void HandleInput()
		{
			if (InputHandler.TryGetTouchWorldPosition(out var touchWorldPosition))
			{
				MovementHandler.UpdateInputTargetX(touchWorldPosition.x);
			}
		}

		private void HandleMovement(float deltaTime)
		{
			MovementHandler.TickMovement(deltaTime);
		}

		private void SetupStartingPosition(Vector3 startingPos)
		{
			var heightOffset = Collider != null ? Collider.size.y / 2f : 1f;
			transform.position = startingPos + new Vector3(0, heightOffset, 0);
		}

		public void OnLootCollected(ILoot loot)
		{
			Player.OnLootCollected(loot);
		}

		public void SubscribeToXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback)
		{
			Player.SubscribeToXpCollected(callback);
			XpCollectedSubscribeCount++;
		}

		public void UnsubscribeToXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback)
		{
			CheckInit();

			Player.UnsubscribeFromXpCollected(callback);
			XpCollectedSubscribeCount--;
		}

		public (int currentXp, int level, int nextLevelXpDelta) GetCurrentXpData()
		{
			CheckInit();

			var progress = Player.LevelProgression;
			return (progress.CurrentExperience,
				progress.CurrentLevel,
				progress.ExperienceRequiredForNextLevel - progress.ExperienceRequiredForPrevious);
		}

		private void OnDestroy()
		{
			if (InputHandler != null)
				InputHandler.DisableInput();

			if (XpCollectedSubscribeCount > 0)
				throw new InvalidOperationException(
					$"PlayerController was destroyed but there are still {XpCollectedSubscribeCount} subscriptions to XP collected events. Make sure to unsubscribe properly to avoid memory leaks.");
		}

		public float GetMagnetRangeSquared()
		{
			var magnetRange = Player.Stats.GetStat(StatType.MagnetRange);
			return magnetRange * magnetRange;
		}

		public float GetCurrentForwardSpeed()
		{
			return Player.Stats.GetStat(StatType.ForwardMoveSpeed);
		}
	}
}