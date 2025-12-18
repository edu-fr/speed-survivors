using TMPro;
using UnityEngine;

namespace View.UI.Enemy
{
	public class DamageNumberView : MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshPro TextComponent { get; set; }

		[field: SerializeField]
		private float OriginalFontSize { get; set; }

		[field: SerializeField]
		private Color BaseColor { get; set; }

		public void SetupVisuals(int amount, bool isCritical)
		{
			TextComponent.SetText("{0}", amount);

			if (isCritical)
			{
				TextComponent.fontSize = OriginalFontSize * 1.5f;
				TextComponent.fontStyle = FontStyles.Bold;
				BaseColor = Color.red;
			}
			else
			{
				TextComponent.fontSize = OriginalFontSize;
				TextComponent.fontStyle = FontStyles.Normal;
				BaseColor = Color.white;
			}

			TextComponent.color = BaseColor;
			transform.localScale = Vector3.zero; // Starts invisible
		}

		public void UpdateTransform(Vector3 position, Vector3 scale)
		{
			transform.position = position;
			transform.localScale = scale;
		}

		public void UpdateAlpha(float alpha)
		{
			var c = BaseColor;
			c.a = alpha;
			TextComponent.color = c;
		}
	}
}