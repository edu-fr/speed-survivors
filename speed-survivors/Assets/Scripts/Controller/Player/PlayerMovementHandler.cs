using Domain.Interface.General;
using Domain.Interface.Player;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerMovementHandler
	{
		private IPlayer Player { get; set; }
		private Transform Transform { get; set; }
		private float XMoveRange { get; set; }
		private float TargetPositionX { get; set; }
		private Vector3 _currentVelocity;

		public PlayerMovementHandler(IPlayer player, Transform playerTransform, float xMoveRange, float startingPointX)
		{
			Player = player;
			Transform = playerTransform;
			XMoveRange = xMoveRange;
			TargetPositionX = startingPointX;
		}

		public void UpdateInputTargetX(float inputWorldPositionX)
		{
			TargetPositionX = ClampTargetPositionToLane(inputWorldPositionX);
		}

		public void TickMovement(float deltaTime)
		{
			var nextX = CalculateNextLateralPosition();
			var nextZ = CalculateNextForwardPosition(deltaTime);

			ApplyNextPosition(nextX, nextZ);
		}

		private float CalculateNextLateralPosition()
		{
			var currentX = Transform.position.x;
			var smoothTime = CalculateSmoothTimeBasedOnSpeed(Player.Stats.GetStat(StatType.LateralMoveSpeed));

			var nextX = Vector3.SmoothDamp(new Vector3(currentX, 0, 0), new Vector3(TargetPositionX, 0, 0),
				ref _currentVelocity, smoothTime).x;

			return nextX;
		}

		private float CalculateNextForwardPosition(float deltaTime)
		{
			return Transform.position.z + (Player.Stats.GetStat(StatType.ForwardMoveSpeed) * deltaTime);
		}

		private void ApplyNextPosition(float x, float z)
		{
			Transform.position = new Vector3(x, Transform.position.y, z);
		}

		private float ClampTargetPositionToLane(float rawX)
		{
			return Mathf.Clamp(rawX, -XMoveRange, XMoveRange);
		}

		/// <summary>
		/// Translates speed to smooth time for SmoothDamp function.
		/// Initial speed value is 10, which translates to 0.2 smooth time.
		/// Speed increases linearly up to 50, which translates to 0.05 smooth time.
		/// Beyond that, smooth time remains constant at 0.05.
		/// </summary>
		private float CalculateSmoothTimeBasedOnSpeed(float speed)
		{
			switch (speed)
			{
				case <= 10f:
					return 0.2f;

				case >= 50f:
					return 0.05f;

				default:
				{
					var t = (speed - 10f) / (50f - 10f);
					return Mathf.Lerp(0.2f, 0.05f, t);
				}
			}
		}
	}
}