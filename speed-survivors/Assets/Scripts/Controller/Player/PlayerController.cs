using UnityEngine;

namespace Controller.Player
{
	public class PlayerController : MonoBehaviour
	{
		[field: SerializeField]
		private Camera MainCamera { get; set; }

		private PlayerInputHandler InputHandler { get; set; }
		private PlayerMovementHandler MovementHandler { get; set; }

		private bool Initialized { get; set; }

		public void Init(Vector3 startingPos, float xMoveMinRange, float xMoveMaxRange)
		{
			InputHandler = new PlayerInputHandler(MainCamera);
			MovementHandler = new PlayerMovementHandler(transform, xMoveMinRange, xMoveMaxRange);
			transform.position = startingPos;

			Initialized = true;
		}

		private void Update()
		{
			if (!Initialized)
				return;

			HandleMovement();
		}

		private void HandleMovement()
		{
			if (InputHandler.GetTargetInputPos(out var xPos))
			{
				MovementHandler.UpdatePlayerMovement(xPos);
			}
		}
	}
}