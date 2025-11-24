using Domain.Interface.Player;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerMovementHandler
	{
		private IPlayer Player { get; set; }
		private Transform Transform { get; set; }
		private Bounds MovementBounds { get; set; }

		private Vector3 _currentVelocity;
		private float CurrentTargetPositionX { get; set; }

		public PlayerMovementHandler(IPlayer player, Transform playerTransform, Bounds movementBounds, float startingPointX)
		{
			Player = player;
			Transform = playerTransform;
			MovementBounds = movementBounds;
			CurrentTargetPositionX = startingPointX;
		}

		public void MovePlayerTowardsCurrentTargetPosition()
		{
			var clampedX  = Mathf.Clamp(CurrentTargetPositionX, MovementBounds.min.x, MovementBounds.max.x);
			var target = new Vector3(clampedX, Transform.position.y, Transform.position.z);
			Transform.position = Vector3.SmoothDamp(Transform.position, target, ref _currentVelocity, GetPlayerSpeedAsSmoothTime());
		}

		public void UpdateCurrentTargetPosition(float worldPositionX)
		{
			CurrentTargetPositionX = worldPositionX;
		}

		private float GetPlayerSpeedAsSmoothTime()
		{
			if (Player == null)
				throw new System.InvalidOperationException("PlayerMovementHandler: Player is null when getting speed as smooth time.");

			return SpeedToSmoothTime(Player.MoveSpeed);
		}

		/// <summary>
		/// Translates speed to smooth time for SmoothDamp function.
		/// Initial speed value is 10, which translates to 0.2 smooth time.
		/// Speed increases linearly up to 50, which translates to 0.05 smooth time.
		/// Beyond that, smooth time remains constant at 0.05.
		/// </summary>
		private float SpeedToSmoothTime(float speed)
		{
			switch (speed)
			{
				case <= 10f:
					return 0.2f;

				case >= 50f:
					return 0.05f;

				default:
				{
					// Linear interpolation between 0.2 and 0.05
					var t = (speed - 10f) / (50f - 10f);
					return Mathf.Lerp(0.2f, 0.05f, t);
				}
			}
		}
	}
}