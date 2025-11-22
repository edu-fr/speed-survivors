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

			SetupStartingPosition(startingPos, Collider.size.y);

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
			if (InputHandler.GetTargetInputPosition(out var touchWorldPosition))
			{
				CurrentTargetX = touchWorldPosition.x;
			}
		}

		private void HandleMovement()
		{
			MovementHandler.MovePlayerTowardsPosition(CurrentTargetX);
		}

		private void SetupStartingPosition(Vector3 startingPos, float playerHeight)
		{
			transform.position = startingPos + new Vector3(0, playerHeight / 2f, 0);
			CurrentTargetX = startingPos.x;
		}
	}
}