using ProjectInput;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.Player
{
	public class PlayerInputHandler
	{
		private Camera Camera { get; set; }
		private PlayerInputActions PlayerInputActions { get; set; }
		private InputAction TouchPressAction { get; set; }
		private InputAction TouchPositionAction { get; set; }

		public PlayerInputHandler(Camera camera)
		{
			Camera = camera;
			PlayerInputActions = new PlayerInputActions();
			TouchPressAction = PlayerInputActions.Gameplay.TouchPress;
			TouchPositionAction = PlayerInputActions.Gameplay.TouchPosition;
			PlayerInputActions.Gameplay.Enable();
		}

		public bool GetTargetInputPosition(out Vector3 worldPosition)
		{
			if (!TouchPressAction.IsPressed())
			{
				worldPosition = new Vector3(-1, -1, -1);
				return false;
			}

			var touchPos = TouchPositionAction.ReadValue<Vector2>();
			worldPosition = Camera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, Camera.transform.position.z * -1));

			return true;
		}
	}
}