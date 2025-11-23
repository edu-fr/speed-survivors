using UnityEngine;

namespace Controller.Player
{
	public class PlayerMovementHandler
	{
		// A smaller value means faster, more "instant" following.
		private const float DefaultSmoothTime = 0.2f;

		private Transform Transform { get; }
		private float SmoothTime { get; }

		private float MoveMinRange { get; }
		private float MoveMaxRange { get; }

		private Vector3 _currentVelocity;
		private float CurrentTargetPositionX { get; set; }

		public PlayerMovementHandler(Transform playerTransform, float xMoveMinRange, float xMoveMaxRange, float startingPointX)
		{
			Transform = playerTransform;
			MoveMinRange = xMoveMinRange;
			MoveMaxRange = xMoveMaxRange;
			CurrentTargetPositionX = startingPointX;
			SmoothTime = DefaultSmoothTime;
		}

		public void MovePlayerTowardsCurrentTargetPosition()
		{
			var target = new Vector3(Mathf.Clamp(CurrentTargetPositionX, MoveMinRange, MoveMaxRange), Transform.position.y, Transform.position.z);
			Transform.position = Vector3.SmoothDamp(Transform.position, target, ref _currentVelocity, SmoothTime);
		}

		public void UpdateCurrentTargetPosition(float worldPositionX)
		{
			CurrentTargetPositionX = worldPositionX;
		}
	}
}