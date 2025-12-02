using System.Collections.Generic;
using Domain.DTO;
using UnityEngine;
using UnityEngine.Events;

namespace View.UI.Modal
{
	public class LevelUpModalView : MonoBehaviour
	{
		[field: Header("References")]
		[field: SerializeField]
		private GameObject ContentRoot { get; set; }

		[field: SerializeField]
		private List<UpgradeOptionView> OptionCards { get; set; }

		public void DisplayOptions(List<UpgradeOptionData> upgrades, UnityAction<UpgradeOptionData> onChosenCallback)
		{
			ConfigureCards(upgrades, onChosenCallback);
			SetVisibility(true);
		}

		public void Hide()
		{
			SetVisibility(false);
		}

		private void SetVisibility(bool isVisible)
		{
			ContentRoot.SetActive(isVisible);
		}

		private void ConfigureCards(List<UpgradeOptionData> upgrades, UnityAction<UpgradeOptionData> onChosenCallback)
		{
			for (var i = 0; i < OptionCards.Count; i++)
			{
				if (i < upgrades.Count)
				{
					var data = upgrades[i];
					OptionCards[i].gameObject.SetActive(true);
					OptionCards[i].Initialize(data, () => onChosenCallback.Invoke(data));
				}
				else
				{
					OptionCards[i].gameObject.SetActive(false);
				}
			}
		}
	}
}