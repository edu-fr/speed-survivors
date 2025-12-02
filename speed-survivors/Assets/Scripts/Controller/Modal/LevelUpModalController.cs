using System.Collections.Generic;
using Controller.General;
using Controller.Mapper;
using Domain.DTO;
using Domain.Weapon.Config;
using UnityEngine;
using View.UI.Modal;

namespace Controller.Modal
{
	public class LevelUpModalController : MonoBehaviour
	{
		[field: SerializeField]
		private LevelUpModalView LevelUpModalView { get; set; }

		private int LastHandledLevel { get; set; } = 1;

		public void HandleExperienceUpdate(int currentLevel)
		{
			if (currentLevel > LastHandledLevel)
			{
				LastHandledLevel = currentLevel;
				InitiateLevelUpSequence();
			}
		}

		private void InitiateLevelUpSequence()
		{
			GameplayManager.PauseTime(nameof(LevelUpModalController));

			var options = GetRandomUpgradeOptions();

			LevelUpModalView.DisplayOptions(options, OnUpgradeSelected);
		}

		private void OnUpgradeSelected(UpgradeOptionData selectedUpgrade)
		{
			// Apply here on player?

			Debug.Log($"Upgrade Selected: {selectedUpgrade.Title}");
			LevelUpModalView.Hide();
			GameplayManager.ResumeTime(nameof(LevelUpModalController));
		}

		private List<UpgradeOptionData> GetRandomUpgradeOptions()
		{
			// Mock
			return new List<UpgradeOptionData>
			{
				WeaponMapper.ToDTO(new PeaShooterConfig()),
				WeaponMapper.ToDTO(new ShotgunConfig()),
			};
		}
	}
}