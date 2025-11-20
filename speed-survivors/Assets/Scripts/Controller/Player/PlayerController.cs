using UnityEngine;

namespace Controller.Player
{
	public class PlayerController : MonoBehaviour
	{
		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		private PlayerInputHandler InputHandler { get; set; }
		private PlayerMovementHandler MovementHandler { get; set; }
		private float CurrentTargetX { get; set; }

		private bool Initialized { get; set; }

		public void Init(Camera mainCamera, Vector3 startingPos, float groundMinBoundX, float groundMaxBoundX)
		{
			InputHandler = new PlayerInputHandler(mainCamera);

			var playerWidth = Collider.size.x;
			var movementMinBoundX = groundMinBoundX + playerWidth/2f;
			var movementMaxBoundX = groundMaxBoundX - playerWidth/2f;
			MovementHandler = new PlayerMovementHandler(transform, movementMinBoundX, movementMaxBoundX);

			SetupStartingPosition(startingPos);

			Initialized = true;
		}

		private void Update()
		{
			if (!Initialized)
				return;

			HandleInput();
		}

		private void FixedUpdate()
		{
			if (!Initialized)
				return;

			HandleMovement();
		}

		private void HandleInput()
		{
			if (InputHandler.GetTargetInputPos(out var xPos))
			{
				CurrentTargetX = xPos;
			}
		}

		private void HandleMovement()
		{
			MovementHandler.UpdatePlayerMovement(CurrentTargetX);
		}

		private void SetupStartingPosition(Vector3 startingPos)
		{
			transform.position = startingPos;
			CurrentTargetX = startingPos.x;
		}
	}
}