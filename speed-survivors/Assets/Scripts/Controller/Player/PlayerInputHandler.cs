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

		public PlayerInputHandler(Camera mainCamera)
		{
			Camera = mainCamera;
			PlayerInputActions = new PlayerInputActions();
			TouchPressAction = PlayerInputActions.Gameplay.TouchPress;
			TouchPositionAction = PlayerInputActions.Gameplay.TouchPosition;
			PlayerInputActions.Gameplay.Enable();
		}

		public float GetXTargetPositionBasedOnCurrentInput()
		{
			if (!TouchPressAction.IsPressed())
			{
				return 0f;
			}

			Vector2 screenPosition = TouchPositionAction.ReadValue<Vector2>();
			Vector3 screenPositionWithZ = new Vector3(screenPosition.x, screenPosition.y);
			Vector3 worldPosition = Camera.ScreenToWorldPoint(screenPositionWithZ);

			return worldPosition.x;
		}
	}
}