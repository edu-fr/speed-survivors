using UnityEngine;

namespace View.General
{
	public class HitFeedbackView : MonoBehaviour
	{
		[field: SerializeField]
		private Renderer TargetRenderer { get; set; }

		private const float FlashDuration = .09f;
		private Color FlashColor { get; } = new(1f, 1f, 1f, 1f);

		private MaterialPropertyBlock PropertyBlock { get; set; }
		private int EmissionColorId { get; set; }

		private float _currentFlashTime;
		private bool _isFlashing;

		public void Setup()
		{
			PropertyBlock ??= new MaterialPropertyBlock();
			TargetRenderer.material.EnableKeyword("_EMISSION");
			EmissionColorId = Shader.PropertyToID("_EmissionColor"); // Default for Standard/URP Lit
			ResetVisuals();
		}

		public void Tick(float dt)
		{
			if (!_isFlashing)
				return;

			_currentFlashTime -= dt;

			if (_currentFlashTime <= 0)
				ResetVisuals();
		}

		public void PlayHitFeedback()
		{
			_currentFlashTime = FlashDuration;

			if (_isFlashing)
				return;

			ApplyColor(FlashColor);
			_isFlashing = true;
		}

		private void ResetVisuals()
		{
			_isFlashing = false;
			ApplyColor(Color.black);
		}

		private void ApplyColor(Color color)
		{
			TargetRenderer.GetPropertyBlock(PropertyBlock);
			PropertyBlock.SetColor(EmissionColorId, color);
			TargetRenderer.SetPropertyBlock(PropertyBlock);
		}
	}
}