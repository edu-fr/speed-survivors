using Controller.General;
using Controller.Interface;
using UnityEngine;
using View.UI.Enemy;

namespace Controller.UI.Enemy
{
	public class DamageNumberController : InitializableMono, ISpawnable
	{
		[field: SerializeField]
		private DamageNumberView View { get; set; }

		private float _timer;
		private float _maxLifetime;
		private Vector3 _velocity;
		private Vector3 _currentPos;
		private float _scaleDuration;

		public void Init(Vector3 startPos, int amount, bool isCritical, float lifetime, float scaleDuration, Vector3 initialVelocity)
		{
			EnsureStillNotInit();

			_currentPos = startPos;
			_maxLifetime = lifetime;
			_scaleDuration = scaleDuration;
			_velocity = initialVelocity;
			_timer = 0f;

			View.SetupVisuals(amount, isCritical);
			View.UpdateTransform(_currentPos, Vector3.zero);

			Initialized = true;
		}

		public bool Tick(float dt)
		{
			_timer += dt;

			// Simple euler for position
			_currentPos += _velocity * dt;

			// Punch
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

			// Start fade out on 70% of lifetime
			if (_timer > _maxLifetime * 0.7f)
			{
				float fadeDuration = _maxLifetime * 0.3f;
				float timeInFade = _timer - (_maxLifetime * 0.7f);
				float alpha = 1f - (timeInFade / fadeDuration);

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