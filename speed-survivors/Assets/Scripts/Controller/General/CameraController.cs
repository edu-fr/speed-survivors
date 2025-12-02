using System;
using UnityEngine;

namespace Controller.General
{
	public class CameraController : MonoBehaviour
	{
		private const float SmoothTime = 0.2f;

		[field: SerializeField]
		private Camera Camera { get; set; }

		private Vector3 Offset { get; set; }
		private Transform Target { get; set; }
		private Vector3 _currentVelocity;
		private bool Initialized { get; set; }

		public void Init(Transform target)
		{
			Target = target;
			Offset = transform.position - target.position;

			Initialized = true;
		}

		public void LateTick()
		{
			FollowTargetZ();
		}

		public void SnapToTarget()
		{
			if (Target == null)
				return;

			transform.position = Target.position + Offset;
			_currentVelocity = Vector3.zero;
		}

		private void FollowTargetZ()
		{
			CheckInit();

			var desiredPosition = Target.position + Offset;
			transform.position = Vector3.SmoothDamp(
				transform.position,
				new Vector3(transform.position.x, transform.position.y, desiredPosition.z),
				ref _currentVelocity,
				SmoothTime
			);
		}

		private void CheckInit()
		{
			if (!Initialized)
				throw new InvalidOperationException("CameraController not initialized. Call Init() before using it.");
		}
	}
}