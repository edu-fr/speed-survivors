using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
	public class ExperienceBarView : MonoBehaviour
	{
		[field: SerializeField]
		private Image FillImage { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI LevelText { get; set; }

		private int _cachedLevel = -1;

		public void UpdateProgress(float currentXp, float requiredXp)
		{
			var percentage = CalculateFillAmount(currentXp, requiredXp);
			FillImage.fillAmount = percentage;
		}

		public void UpdateLevelLabel(int newLevel)
		{
			if (_cachedLevel == newLevel)
				return;

			_cachedLevel = newLevel;
			LevelText.text = new string($"Lv {_cachedLevel}");
		}

		private float CalculateFillAmount(float current, float max)
		{
			return max <= 0 ? 1f : Mathf.Clamp01(current / max);
		}
	}
}