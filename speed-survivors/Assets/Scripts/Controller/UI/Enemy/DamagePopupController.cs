using System;
using Controller.General;
using Controller.Interface;
using UnityEngine;
using View.UI.Enemy;

namespace Controller.UI.Enemy
{
	public enum DamageNumberStyle
	{
		Linear,
		Parabolic
	}

	public class DamageNumberController : InitializableMono, ISpawnable
	{
		private const float Gravity = 5f;

		[field: SerializeField]
		private DamageNumberView View { get; set; }

		private float _timer;
		private float _maxLifetime;
		private Vector3 _velocity;
		private Vector3 _currentPos;
		private float _scaleDuration;
		private DamageNumberStyle _currentStyle;

		public void Init(Vector3 startPos,
			int amount,
			bool isCritical,
			float lifetime,
			float scaleDuration,
			Vector3 initialVelocity,
			DamageNumberStyle style)
		{
			EnsureStillNotInit();

			_currentPos = startPos;
			_maxLifetime = lifetime;
			_scaleDuration = scaleDuration;
			_velocity = initialVelocity;
			_currentStyle = style;
			_timer = 0f;

			View.SetupVisuals(amount, isCritical);
			View.UpdateTransform(_currentPos, Vector3.zero);

			Initialized = true;
		}

		public bool Tick(float dt)
		{
			_timer += dt;
			if (_currentStyle == DamageNumberStyle.Parabolic)
			{
				_velocity.y -= Gravity * dt;
			}

			_currentPos += _velocity * dt;

			Vector3 currentScale;
			if (_timer < _scaleDuration)
			{
				var t = _timer / _scaleDuration;
				currentScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.2f, t);
			}
			else
			{
				currentScale = Vector3.Lerp(View.transform.localScale, Vector3.one, dt * 10f);
			}

			View.UpdateTransform(_currentPos, currentScale);

			var shouldStartFade = false;
			switch (_currentStyle)
			{
				case DamageNumberStyle.Linear:
				{
					if (_timer > _maxLifetime * 0.7f)
						shouldStartFade = true;
					break;
				}
				case DamageNumberStyle.Parabolic:
				{
					if (_velocity.y < 0f || _timer > _maxLifetime * 0.7f)
						shouldStartFade = true;
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(_currentStyle.ToString(), "Invalid style for damage number");
			}

			if (shouldStartFade)
			{
				var fadeDuration = _maxLifetime * 0.3f;
				var timeInFade = _timer - (_maxLifetime - fadeDuration);
				var alpha = Mathf.Clamp01(1f - (timeInFade / fadeDuration));
				View.UpdateAlpha(alpha);
			}

			return _timer < _maxLifetime;
		}

		public void OnDespawn()
		{
			Initialized = false;
		}
	}
}