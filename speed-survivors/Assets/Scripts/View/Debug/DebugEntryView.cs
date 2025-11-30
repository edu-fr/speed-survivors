using TMPro;
using UnityEngine;

namespace View.Debug
{
	public class DebugEntryView : MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshProUGUI LabelText { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI ValueText { get; set; }

		public void Initialize(string label)
		{
			LabelText.text = label;
		}

		public void UpdateValue(object value)
		{
			// O ToString() gera lixo de memória (Garbage)
			ValueText.text = value.ToString();
		}

		public void UpdateColor(Color color)
		{
			ValueText.color = color;
		}
	}
}