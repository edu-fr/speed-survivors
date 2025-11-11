using UnityEngine;

namespace Controller.Player
{
	public class PlayerController : MonoBehaviour
	{
		[field: SerializeField]
		private Camera MainCamera { get; set; }

		private PlayerInputHandler InputHandler { get; set; }
		private PlayerMovementHandler MovementHandler { get; set; }

		private void Start()
		{
			InputHandler = new PlayerInputHandler(MainCamera);
			MovementHandler = new PlayerMovementHandler(transform);
		}

		private void Update()
		{
			HandleMovement();
		}

		private void HandleMovement()
		{
			var dirX = InputHandler.GetXTargetPositionBasedOnCurrentInput();
			MovementHandler.UpdatePlayerMovement(dirX);
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
		}
	}
}