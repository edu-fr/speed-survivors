using System;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerInputHandler
	{
		private Camera Camera { get; set; }

		public PlayerInputHandler(Camera mainCamera)
		{
			Camera = mainCamera;
		}

		public float GetXTargetPositionBasedOnCurrentInput()
		{
			if (Input.touchCount <= 0 || !Input.GetMouseButton(0))
			{
				return 0f;
			}

			var touchPosition = GetTouchPosition();
			touchPosition.z = Camera.nearClipPlane + 10f;
			var targetWorldPosition = Camera.ScreenToWorldPoint(touchPosition);

			return targetWorldPosition.x;
		}

		private Vector3 GetTouchPosition()
		{
#if UNITY_EDITOR
			return Input.mousePosition;
#elif UNITY_ANDROID || UNITY_IOS
			return Input.GetTouch(0).position;
#endif
			throw new InvalidOperationException("Touch not supported.");
		}
	}
}