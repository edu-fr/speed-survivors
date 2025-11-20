using UnityEngine;

namespace Controller.Player
{
	public class PlayerMovementHandler
	{
		// A smaller value means faster, more "instant" following.
		private const float DefaultSmoothTime = 0.2f;
		private const float FastSmoothTime = 0.03f;

		private Transform Transform { get; }
		private float SmoothTime { get; }

		private float MoveMinRange { get; }
		private float MoveMaxRange { get; }

		private Vector3 _currentVelocity;

		public PlayerMovementHandler(Transform playerTransform, float xMoveMinRange, float xMoveMaxRange)
		{
			Transform = playerTransform;
			MoveMinRange = xMoveMinRange;
			MoveMaxRange = xMoveMaxRange;

			SmoothTime = DefaultSmoothTime;
		}

		public void UpdatePlayerMovement(float directionX)
		{
			var target = new Vector3(Mathf.Clamp(directionX, MoveMinRange, MoveMaxRange), Transform.position.y, Transform.position.z);
			Transform.position = Vector3.SmoothDamp(Transform.position, target, ref _currentVelocity, SmoothTime);
		}
	}
}