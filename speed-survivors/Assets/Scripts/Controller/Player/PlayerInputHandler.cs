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
		private Plane GroundPlane { get; set; }

		public PlayerInputHandler(Camera camera)
		{
			Camera = camera;
			PlayerInputActions = new PlayerInputActions();
			TouchPressAction = PlayerInputActions.Gameplay.TouchPress;
			TouchPositionAction = PlayerInputActions.Gameplay.TouchPosition;

			GroundPlane = new Plane(Vector3.up, Vector3.zero);

			PlayerInputActions.Gameplay.Enable();
		}

		public bool TryGetTouchWorldPosition(out Vector3 worldPosition)
		{
			if (!IsTouchingScreen())
			{
				worldPosition = Vector3.zero;
				return false;
			}

			return CalculateWorldPositionFromTouch(out worldPosition);
		}

		public void DisableInput()
		{
			PlayerInputActions.Gameplay.Disable();
		}

		private bool IsTouchingScreen()
		{
			return TouchPressAction.IsPressed();
		}

		private bool CalculateWorldPositionFromTouch(out Vector3 worldPosition)
		{
			var screenPosition = TouchPositionAction.ReadValue<Vector2>();
			var ray = Camera.ScreenPointToRay(screenPosition);

			if (GroundPlane.Raycast(ray, out var enterDistance))
			{
				worldPosition = ray.GetPoint(enterDistance);
				return true;
			}

			worldPosition = Vector3.zero;
			return false;
		}
	}
}