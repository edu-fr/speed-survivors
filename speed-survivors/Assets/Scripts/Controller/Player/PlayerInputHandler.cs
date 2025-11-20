using System;
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

		public bool GetTargetInputPos(out float xPos)
		{
			if (!TouchPressAction.IsPressed())
			{
				xPos = 0;
				return false;
			}

			var touchPos = TouchPositionAction.ReadValue<Vector2>();
			var worldPos = Camera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, Camera.transform.position.z * -1));
			xPos = worldPos.x;

			return true;
		}
	}
}