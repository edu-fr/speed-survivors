using Domain.Interface.Player;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerMovementHandler
	{
		private IPlayer Player { get; set; }
		private Transform Transform { get; set; }
		private float XMoveRange { get; set; }
		private Vector3 _currentVelocity;
		private float CurrentTargetPositionX { get; set; }
		private float CurrentTargetPositionZ { get; set; }

		public PlayerMovementHandler(IPlayer player, Transform playerTransform, float xMoveRange, float startingPointX)
		{
			Player = player;
			Transform = playerTransform;
			XMoveRange = xMoveRange;
			CurrentTargetPositionX = startingPointX;
			CurrentTargetPositionZ = playerTransform.position.z;
		}

		public void MovePlayerTowardsCurrentTargetPosition()
		{
			var clampedX = Mathf.Clamp(CurrentTargetPositionX, -XMoveRange, XMoveRange);
			var target = new Vector3(clampedX, Transform.position.y, CurrentTargetPositionZ);
			Transform.position = Vector3.SmoothDamp(
				Transform.position, target, ref _currentVelocity, GetPlayerSpeedAsSmoothTime());
		}

		public void UpdateCurrentXTargetPosition(float worldPositionX)
		{
			CurrentTargetPositionX = worldPositionX;
		}

		public void UpdateCurrentZTargetPosition(float worldPositionZ)
		{
			CurrentTargetPositionZ = worldPositionZ;
		}

		private float GetPlayerSpeedAsSmoothTime()
		{
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