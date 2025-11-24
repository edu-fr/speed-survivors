using Domain.Enemy;
using Domain.Interface.Enemy;
using UnityEngine;

namespace Controller.Enemy
{
	public class EnemyController : MonoBehaviour
	{
		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		private IEnemy Enemy { get; set; }

		private readonly Vector3 _moveDirection = Vector3.back;

		private void Awake()
		{
			Enemy = new Zombie();
		}

		private void FixedUpdate()
		{
			HandleMovement();
		}

		private void HandleMovement()
		{
			transform.Translate(_moveDirection * (Enemy.MoveSpeed * Time.deltaTime));
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