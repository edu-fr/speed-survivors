using System;
using Controller.General;
using UnityEngine;

namespace Controller.CustomCamera
{
	[Serializable]
	public class FollowingCameraHandler : Initializable
	{
		private const float SmoothTime = 0.2f;

		[field: SerializeField]
		private Transform Transform { get; set; }

		[field: SerializeField]
		private Vector3 Offset { get; set; }

		private Transform Target { get; set; }
		private Vector3 _currentVelocity;

		public void Init(Transform target)
		{
			EnsureStillNotInitialized();

			Target = target;

			Initialized = true;
		}

		public void LateTick()
		{
			CheckInit();

			FollowTargetZ();
		}

		public void SnapToTarget()
		{
			CheckInit();

			if (Target == null)
				return;

			Transform.position = Target.position + Offset;
			_currentVelocity = Vector3.zero;
		}

		private void FollowTargetZ()
		{
			var desiredPosition = Target.position + Offset;
			Transform.position = Vector3.SmoothDamp(
				Transform.position,
				new Vector3(Transform.position.x, Transform.position.y, desiredPosition.z),
				ref _currentVelocity,
				SmoothTime
			);
		}
	}
}