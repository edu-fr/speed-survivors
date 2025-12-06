using System;
using Controller.General.Base;
using Controller.Modal;
using Controller.Player;
using UnityEngine;
using View.UI;

namespace Controller.UI
{
	[Serializable]
	public class GameplayUIHandler : Initializable
	{
		[field: SerializeField]
		private ExperienceBarView XpBarView { get; set; }

		[field: SerializeField]
		private LevelUpModalController LevelUpModalController { get; set; }

		private PlayerController PlayerController { get; set; }

		public void Init(PlayerController playerController, PlayerUpgradeHandler playerUpgradeHandler)
		{
			EnsureStillNotInitialized();

			PlayerController = playerController;
			SubscribeToPlayerEvents();
			var initialXpData = playerController.GetCurrentXpData();
			XpBarView.UpdateLevelLabel(initialXpData.level);
			XpBarView.UpdateProgress(initialXpData.currentXp, initialXpData.nextLevelXpDelta);
			LevelUpModalController.Init(playerUpgradeHandler);

			Initialized = true;
		}

		private void SubscribeToPlayerEvents()
		{
			if (PlayerController != null)
			{
				PlayerController.SubscribeToXpCollected(HandleExperienceUpdate);
			}
		}

		private void UnsubscribeFromPlayerEvents()
		{
			if (PlayerController != null)
			{
				PlayerController.UnsubscribeToXpCollected(HandleExperienceUpdate);
			}
		}

		private void HandleExperienceUpdate((int currentXp, int level, int nextLevelXpDelta) xpData)
		{
			CheckInit();

			UpdateVisuals(xpData.currentXp, xpData.level, xpData.nextLevelXpDelta);
			LevelUpModalController.HandleExperienceUpdate(xpData.level);
		}

		private void UpdateVisuals(float xp, int level, float reqXp)
		{
			CheckInit();

			XpBarView.UpdateLevelLabel(level);
			XpBarView.UpdateProgress(xp, reqXp);
		}

		~GameplayUIHandler()
		{
			UnsubscribeFromPlayerEvents();
		}
	}
}