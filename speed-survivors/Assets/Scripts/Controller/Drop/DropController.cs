using Domain.Interface.Loot;
using UnityEngine;

namespace Controller.Drop
{
	public class DropController : MonoBehaviour
	{
		public ILoot Loot { get; private set; }

		private const float PopAnimationDuration = 0.5f;
		private Transform CachedTransform { get; set; }
		private bool IsMagnetized { get; set; }
		private Vector3 StartPosition { get; set; }
		private Vector3 TargetPosition { get; set; }

		private float _animationTime;

		private void Awake()
		{
			CachedTransform = transform;
		}

		public void Initialize(Vector3 position, ILoot loot)
		{
			Loot = loot;
			IsMagnetized = false;
			_animationTime = 0f;
			StartPosition = position;

			var randomCircle = Random.insideUnitCircle * 1.5f;
			TargetPosition = position + new Vector3(randomCircle.x, 0, randomCircle.y);

			CachedTransform.position = position;
			CachedTransform.localScale = Vector3.one;
		}

		public bool TickMovementAndCheckCollected(float deltaTime, Vector3 playerPosition, float magnetRadius, float magnetSpeed)
		{
			if (_animationTime < PopAnimationDuration)
			{
				AnimatePop(deltaTime);
				return false;
			}

			var distanceSqr = (playerPosition - CachedTransform.position).sqrMagnitude;

			if (IsMagnetized || distanceSqr < magnetRadius * magnetRadius)
			{
				IsMagnetized = true;
				return MoveTowardsPlayer(deltaTime, playerPosition, magnetSpeed);
			}

			return false;
		}

		// Used on big vacuums
		public void SetMagnetized()
		{
			IsMagnetized = true;
		}

		private void AnimatePop(float deltaTime)
		{
			_animationTime += deltaTime;
			var t = _animationTime / PopAnimationDuration;

			CachedTransform.position = Vector3.Lerp(StartPosition, TargetPosition, t);

			var scale = Mathf.Sin(t * Mathf.PI); // 0 -> 1 -> 0 (curva de pulo)
		}

		private bool MoveTowardsPlayer(float deltaTime, Vector3 playerPos, float speed)
		{
			var step = (speed * 2f) * deltaTime;
			CachedTransform.position = Vector3.MoveTowards(CachedTransform.position, playerPos, step);

			return Vector3.SqrMagnitude(CachedTransform.position - playerPos) < 0.5f;
		}
	}
}