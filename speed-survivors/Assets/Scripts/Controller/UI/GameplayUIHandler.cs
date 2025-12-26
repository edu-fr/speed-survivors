using System;
using Controller.General;
using Controller.Modal;
using Controller.Player;
using Domain.Interface.General;
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
		private HpBarView HpBarView { get; set; }

		[field: SerializeField]
		private LevelUpModalController LevelUpModalController { get; set; }

		[field: SerializeField]
		private TryAgainModalController TryAgainModalController { get; set; }

		private PlayerController PlayerController { get; set; }

		public void Init(PlayerController playerController, PlayerUpgradeHandler playerUpgradeHandler)
		{
			EnsureStillNotInitialized();

			PlayerController = playerController;
			SubscribeToPlayerEvents();
			var initialXpData = playerController.GetCurrentXpData();
			XpBarView.UpdateLevelLabel(initialXpData.level);
			XpBarView.UpdateProgress(initialXpData.currentXp, initialXpData.nextLevelXpDelta);

			var playerDomain = playerController.GetPlayerDomainRef();
			HpBarView.UpdateCurrentLabel((int) playerDomain.CurrentHP);
			HpBarView.UpdateTotalLabel((int) playerDomain.Stats.GetStat(StatType.MaxHealth));
			HpBarView.UpdateProgress((int) playerDomain.CurrentHP, (int) playerDomain.Stats.GetStat(StatType.MaxHealth));

			LevelUpModalController.Init(playerUpgradeHandler);
			TryAgainModalController.Init();

			Initialized = true;
		}

		private void SubscribeToPlayerEvents()
		{
			if (PlayerController != null)
			{
				PlayerController.SubscribeToXpCollected(HandleExperienceUpdate);
				PlayerController.SubscribeToCurrentHpChanged(HandleCurrentHpUpdate);
				PlayerController.SubscribeToStatsUpdate(HandleStatsUpdate);
			}
		}

		private void UnsubscribeFromPlayerEvents()
		{
			if (PlayerController != null)
			{
				PlayerController.UnsubscribeToXpCollected(HandleExperienceUpdate);
				PlayerController.UnsubscribeFromCurrentHpChanged(HandleCurrentHpUpdate);
				PlayerController.UnsubscribeFromStatsUpdate(HandleStatsUpdate);
			}
		}

		private void HandleExperienceUpdate((int currentXp, int level, int nextLevelXpDelta) xpData)
		{
			CheckInit();

			XpBarView.UpdateLevelLabel(xpData.level);
			XpBarView.UpdateProgress(xpData.currentXp, xpData.nextLevelXpDelta);
			LevelUpModalController.HandleExperienceUpdate(xpData.level);
		}

		private void HandleCurrentHpUpdate(float newCurrentHp, float diff)
		{
			CheckInit();

			var maxHp = PlayerController.GetPlayerDomainRef().Stats.GetStat(StatType.MaxHealth);
			HpBarView.UpdateCurrentLabel((int) newCurrentHp);
			HpBarView.UpdateProgress((int) newCurrentHp, (int) maxHp);
		}

		private void HandleStatsUpdate(StatType type, float newStatValue, float diff)
		{
			CheckInit();

			switch (type)
			{
				case StatType.MaxHealth:
					var currentHp = PlayerController.GetPlayerDomainRef().CurrentHP;
					HpBarView.UpdateTotalLabel((int) newStatValue);
					HpBarView.UpdateProgress((int) currentHp, (int) newStatValue);
					break;
			}
		}

		public void ShowTryAgainModal()
		{
			TryAgainModalController.Show();
		}

		public void HideTryAgainModal()
		{
			TryAgainModalController.Hide();
		}

		~GameplayUIHandler()
		{
			UnsubscribeFromPlayerEvents();
		}
	}
}