using System.Collections.Generic;
using Domain.Config;
using Domain.Interface.Config;
using UnityEngine;

namespace Data.ScriptableObjects.Generator
{
	[CreateAssetMenu(fileName = "GrowthConfigGenerator", menuName = "Configs/Generate Growth Config", order = 0)]
	public class GrowthConfigGeneratorSO : ScriptableObject
	{
		[field: Header("Settings")]
		[field: SerializeField]
		private GrowthStrategy GrowthType { get; set; } = GrowthStrategy.Polynomial;

		[field: SerializeField]
		private int BaseValue { get; set; } = 10;

		[field: SerializeField]
		private float GrowthFactor { get; set; } = 2.0f;

		[field: SerializeField]
		private float PolynomialPower { get; set; } = 2.0f;

		[field: Header("Editor Preview")]
		[field: SerializeField]
		private int PreviewGoalCount { get; set; } = 10;

		[field: SerializeField]
		private List<string> EditorPreviewList { get; set; }

		public IGrowthConfig ToDomain()
		{
			return new GrowthConfig(GrowthType, BaseValue, GrowthFactor, PolynomialPower);
		}

		private void OnValidate()
		{
			UpdateEditorPreview();
		}

		private void UpdateEditorPreview()
		{
			EditorPreviewList = new List<string>();
			var calculator = ToDomain();

			for (var i = 1; i <= PreviewGoalCount; i++)
			{
				var valueNeeded = calculator.CalculateRequiredValue(i + 1);
				var valuePrevious = calculator.CalculateRequiredValue(i);
				var delta = valueNeeded - valuePrevious;

				EditorPreviewList.Add($"Goal {i} -> {i + 1}: {valueNeeded} Value (Delta: {delta})");
			}
		}
	}
}