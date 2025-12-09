using Controller.General;
using Controller.Player;
using Domain.Interface.Upgrade;
using Domain.Upgrade;
using UnityEngine;
using View.UI.Modal;

namespace Controller.Modal
{
	public class LevelUpModalController : InitializableMono
	{
		[field: SerializeField]
		private LevelUpModalView LevelUpModalView { get; set; }

		private PlayerUpgradeHandler PlayerUpgradeHandler { get; set; }
		private int LastHandledLevel { get; set; } = 1;

		public void Init(PlayerUpgradeHandler playerUpgradeHandler)
		{
			EnsureStillNotInit();

			PlayerUpgradeHandler = playerUpgradeHandler;

			Initialized = true;
		}

		public void HandleExperienceUpdate(int currentLevel)
		{
			CheckInit();

			if (currentLevel > LastHandledLevel)
			{
				LastHandledLevel = currentLevel;
				InitiateLevelUpSequence();
			}
		}

		private void InitiateLevelUpSequence()
		{
			GameManager.Instance.PauseTime(nameof(LevelUpModalController));

			var options =
				UpgradeDictGenerator.GetRandomEligibleUpgrades(PlayerUpgradeHandler.GetPlayerDomainRef(), 3);

			LevelUpModalView.DisplayOptions(options, OnUpgradeSelected);
		}

		private void OnUpgradeSelected(IUpgrade selectedUpgrade)
		{
			CheckInit();

			Debug.Log($"Upgrade Selected: {selectedUpgrade.Title}");

			PlayerUpgradeHandler.HandleUpgrade(selectedUpgrade);
			LevelUpModalView.Hide();

			GameManager.Instance.ResumeTime(nameof(LevelUpModalController));
		}
	}
}