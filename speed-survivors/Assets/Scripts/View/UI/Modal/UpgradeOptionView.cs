using Domain.Interface.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View.UI.Modal
{
	public class UpgradeOptionView : MonoBehaviour
	{
		[field: Header("Interaction")]
		[field: SerializeField]
		private Button CardButton { get; set; }

		[field: Header("Content")]
		[field: SerializeField]
		private Image IconImage { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI TitleText { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI DescriptionText { get; set; }

		public void Initialize(IUpgrade data, UnityAction onSelected)
		{
			TitleText.text = data.Title;
			DescriptionText.text = data.Description;
			IconImage.sprite = null;
			IconImage.gameObject.SetActive(true);
			CardButton.interactable = true;

			SetupInteraction(onSelected);
		}

		private void SetupInteraction(UnityAction onSelected)
		{
			CardButton.onClick.RemoveAllListeners();
			CardButton.onClick.AddListener(() =>
			{
				CardButton.interactable = false;
				onSelected?.Invoke();
			});
		}
	}
}