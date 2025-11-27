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
		private bool ShouldFollow { get; set; }
		private Vector3 _currentVelocity;
		private bool Initialized { get; set; }

		public void Init(Transform target)
		{
			Target = target;
			Offset = transform.position - target.position;

			Initialized = true;
		}

		private void LateUpdate()
		{
			FollowTargetZ();
		}

		public void StartFollowing()
		{
			ShouldFollow = true;
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
			if (!ShouldFollow)
				return;

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