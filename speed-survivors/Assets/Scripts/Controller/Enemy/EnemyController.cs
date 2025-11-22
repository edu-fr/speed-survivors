using Domain.Enemy;
using UnityEngine;

namespace Controller.Enemy
{
	public class EnemyController : MonoBehaviour
	{
		[field: SerializeField]
		private float MoveSpeed { get; set; } = 2f;

		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		private BaseEnemy Enemy { get; set; }

		private Vector3 _moveDirection = Vector3.back;


		private void FixedUpdate()
		{
			HandleMovement();
		}

		private void HandleMovement()
		{
			transform.Translate(_moveDirection * (MoveSpeed * Time.deltaTime));
		}

		public float GetHeight()
		{
			return Collider.size.y;
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
		}
	}
}