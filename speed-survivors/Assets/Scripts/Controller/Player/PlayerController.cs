using System;
using Controller.DebugController;
using Data.ScriptableObjects.Generator;
using Domain.Interface.Loot;
using Domain.Interface.Player;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerController : MonoBehaviour
	{
		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		[field: SerializeField]
		private PlayerWeaponArsenalHandler WeaponArsenalHandler { get; set; }

		[field: SerializeField]
		private GrowthConfigGeneratorSO LevelProgressionSO { get; set; }

		private PlayerInputHandler InputHandler { get; set; }
		private PlayerMovementHandler MovementHandler { get; set; }
		private bool Initialized { get; set; }
		private IPlayer Player { get; set; }
		private int XpCollectedSubscribeCount { get; set; }

		public void Init(Camera mainCamera, Vector3 startingPos, float xMoveRange)
		{
			Player = new Domain.Player.Player(LevelProgressionSO.ToDomain());
			InputHandler = new PlayerInputHandler(mainCamera);
			MovementHandler = new PlayerMovementHandler(Player, transform, xMoveRange, startingPos.x);

			SetupStartingPosition(startingPos);
			WeaponArsenalHandler.Init(Player);

			Initialized = true;
		}

		public void Tick(float deltaTime)
		{
			if (!Initialized)
				return;

			DebugTick();

			WeaponArsenalHandler.Tick(deltaTime, true, MovementHandler.CurrentForwardVelocity);

			HandleInput();
			HandleMovement(deltaTime);
		}

		private void DebugTick()
		{
			var debugInstance = DebugOverlayManager.Instance;
			debugInstance.Track("Total XP: ", Player.LevelProgression.TotalExperience);
			debugInstance.Track("Next level total XP: ", Player.LevelProgression.ExperienceRequiredForNextLevel);
		}

		public float GetPlayerMagnetRadius()
		{
			return Player.MagnetRadius;
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
			Player.UnsubscribeFromXpCollected(callback);
			XpCollectedSubscribeCount--;
		}

		public (int currentXp, int level, int nextLevelXpDelta) GetCurrentXpData()
		{
			var progress = Player.LevelProgression;
			return (progress.CurrentExperience,
				progress.CurrentLevel,
				progress.ExperienceRequiredForNextLevel - progress.ExperienceRequiredForPrevious);
		}

		private void OnDestroy()
		{
			if (InputHandler != null)
				InputHandler.DisableInput();

			if (WeaponArsenalHandler != null)
				WeaponArsenalHandler.OnDestroy();

			if (XpCollectedSubscribeCount > 0)
				throw new InvalidOperationException(
					$"PlayerController was destroyed but there are still {XpCollectedSubscribeCount} subscriptions to XP collected events. Make sure to unsubscribe properly to avoid memory leaks.");
		}
	}
}