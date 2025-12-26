using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
	public class HpBarView : MonoBehaviour
	{
		[field: SerializeField]
		private Image FillImage { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI CurrentHpText { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI TotalHpText { get; set; }

		private int _cachedCurrentHp = -1;
		private int _cachedTotalHp = -1;

		public void UpdateProgress(int currentHp, int totalHp)
		{
			var percentage = CalculateFillAmount(currentHp, totalHp);
			FillImage.fillAmount = percentage;
		}

		public void UpdateCurrentLabel(int currentHp)
		{
			if (currentHp == _cachedCurrentHp)
				return;

			_cachedCurrentHp = currentHp;
			CurrentHpText.text = currentHp.ToString();
		}

		public void UpdateTotalLabel(int totalHp)
		{
			if (totalHp == _cachedTotalHp)
				return;

			_cachedTotalHp = totalHp;
			TotalHpText.text = totalHp.ToString();
		}

		private float CalculateFillAmount(float current, float max)
		{
			return max <= 0 ? 1f : Mathf.Clamp01(current / max);
		}
	}
}