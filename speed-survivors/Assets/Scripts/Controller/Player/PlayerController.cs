using System;
using Controller.DebugController;
using Controller.General;
using Controller.Interface;
using Controller.UI;
using Controller.Weapon.Ammo;
using Data.ScriptableObjects.Generator;
using Domain.Interface.General;
using Domain.Interface.Loot;
using Domain.Interface.Player;
using UnityEngine;
using View.Player;

namespace Controller.Player
{
	public class PlayerController : InitializableMono, IHitable
	{
		[field: SerializeField]
		private PlayerView View { get; set; }

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
		private int HpChangedSubscribeCount { get; set; }
		private int StatsUpdateSubscribeCount { get; set; }
		private int DeathSubscribeCount { get; set; }

		public void Init(Camera mainCamera, Vector3 startingPos, float xMoveRange, ProjectileHandler projectileHandler)
		{
			EnsureStillNotInit();

			Player = new Domain.Player.Player(LevelProgressionSO.ToDomain());
			WeaponArsenalHandler.Init(Player, projectileHandler);
			InputHandler = new PlayerInputHandler(mainCamera);
			MovementHandler = new PlayerMovementHandler(Player, transform, xMoveRange, startingPos.x);

			SetupStartingPosition(startingPos);
			View.Setup();

			Initialized = true;
		}

		public void Tick(float deltaTime)
		{
			CheckInit();

			if (!Player.IsAlive)
				return;

			DebugTick();
			WeaponArsenalHandler.Tick(deltaTime, true, Player.Stats.GetStat(StatType.ForwardMoveSpeed));
			HandleInput();
			HandleMovement(deltaTime);
			View.Tick(deltaTime);
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
			CheckInit();

			Player.SubscribeToXpCollected(callback);
			XpCollectedSubscribeCount++;
		}

		public void UnsubscribeToXpCollected(Action<(int currentXp, int level, int nextLevelXpDelta)> callback)
		{
			CheckInit();

			Player.UnsubscribeFromXpCollected(callback);
			XpCollectedSubscribeCount--;
		}

		public void SubscribeToCurrentHpChanged(Action<float, float> handlePlayerHp)
		{
			CheckInit();

			Player.SubscribeToCurrentHpChanged(handlePlayerHp);
			HpChangedSubscribeCount++;
		}

		public void UnsubscribeFromCurrentHpChanged(Action<float, float> handlePlayerHp)
		{
			CheckInit();

			Player.UnsubscribeFromCurrentHpChanged(handlePlayerHp);
			HpChangedSubscribeCount--;
		}

		public void SubscribeToStatsUpdate(Action<StatType, float, float> handleStatsUpdate)
		{
			CheckInit();

			Player.SubscribeToStatsUpdate(handleStatsUpdate);
			StatsUpdateSubscribeCount++;
		}

		public void UnsubscribeFromStatsUpdate(Action<StatType, float, float> handleStatsUpdate)
		{
			CheckInit();

			Player.UnsubscribeFromStatsUpdate(handleStatsUpdate);
			StatsUpdateSubscribeCount--;
		}

		public void SubscribeToPlayerDeath(Action onPlayerDeath)
		{
			Player.SubscribeToPlayerDeath(onPlayerDeath);
		}

		public void UnsubscribeFromPlayerDeath(Action onPlayerDeath)
		{
			Player.UnsubscribeFromPlayerDeath(onPlayerDeath);
		}

		public (int currentXp, int level, int nextLevelXpDelta) GetCurrentXpData()
		{
			CheckInit();

			var progress = Player.LevelProgression;
			return (progress.CurrentExperience,
				progress.CurrentLevel,
				progress.ExperienceRequiredForNextLevel - progress.ExperienceRequiredForPrevious);
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

		public bool TakeHit(float damage, bool isCritical)
		{
			CheckInit();

			var alive = Player.TakeDamage(damage);
			View.PlayHitFeedback();

			var positionWithOffset = transform.position + new Vector3(0f, GetHeight() * 0.5f, 0f);
			DamageNumbersManager.Instance.SpawnDamagePopup(positionWithOffset, (int) damage, isCritical);

			return alive;
		}

		private float GetHeight()
		{
			return Collider.size.y;
		}

		private void OnDestroy()
		{
			if (InputHandler != null)
				InputHandler.DisableInput();

			if (XpCollectedSubscribeCount > 0 ||
			    HpChangedSubscribeCount > 0 ||
			    StatsUpdateSubscribeCount > 0 ||
			    DeathSubscribeCount > 0)
				Debug.LogWarning("PlayerController was destroyed but there are still subscriptions to player events");
		}
	}
}