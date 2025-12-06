using System.Collections.Generic;
using Controller.General;
using Controller.General.Base;
using Controller.Player;
using Domain.DTO;
using Domain.DTO.Mapper;
using Domain.Weapon.Config;
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

			var options = GetRandomUpgradeOptions();

			LevelUpModalView.DisplayOptions(options, OnUpgradeSelected);
		}

		private void OnUpgradeSelected(UpgradeOptionData selectedUpgrade)
		{
			CheckInit();

			Debug.Log($"Upgrade Selected: {selectedUpgrade.Title}");
			LevelUpModalView.Hide();
			GameManager.Instance.ResumeTime(nameof(LevelUpModalController));
		}

		private List<UpgradeOptionData> GetRandomUpgradeOptions()
		{
			// Mock
			return new List<UpgradeOptionData>
			{
				WeaponMapper.ConfigToDTO(new PeaShooterConfig()),
				WeaponMapper.ConfigToDTO(new ShotgunConfig()),
			};
		}
	}
}