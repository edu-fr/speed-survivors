using Domain.Interface.Player;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerController : MonoBehaviour
	{
		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		private PlayerInputHandler InputHandler { get; set; }
		private PlayerMovementHandler MovementHandler { get; set; }
		private bool Initialized { get; set; }
		private IPlayer Player { get; set; }

		public void Init(Camera mainCamera, Vector3 startingPos, Bounds groundBounds)
		{
			Player = new Domain.Player.Player();
			InputHandler = new PlayerInputHandler(mainCamera);
			var playerMovementBounds = GetPlayerMovementBounds(groundBounds, Collider);
			MovementHandler = new PlayerMovementHandler(Player, transform, playerMovementBounds, startingPos.x);

			SetupStartingPosition(startingPos, Collider.size.y);

			Initialized = true;
		}

		private Bounds GetPlayerMovementBounds(Bounds groundBounds, BoxCollider playerCollider)
		{
			return new Bounds
			{
				min = new Vector3(groundBounds.min.x + playerCollider.size.x / 2f, groundBounds.min.y, groundBounds.min.z),
				max = new Vector3(groundBounds.max.x - playerCollider.size.x / 2f, groundBounds.max.y, groundBounds.max.z)
			};
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
			if (!InputHandler.GetTargetInputPosition(out var touchWorldPosition))
				return;

			MovementHandler.UpdateCurrentTargetPosition(touchWorldPosition.x);
		}

		private void HandleMovement()
		{
			MovementHandler.MovePlayerTowardsCurrentTargetPosition();
		}

		private void SetupStartingPosition(Vector3 startingPos, float playerHeight)
		{
			transform.position = startingPos + new Vector3(0, playerHeight / 2f, 0);
		}
	}
}