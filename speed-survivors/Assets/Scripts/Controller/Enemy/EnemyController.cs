using System;
using Controller.Interface.General;
using Domain.Enemy;
using Domain.Interface.Enemy;
using UnityEngine;

namespace Controller.Enemy
{
	public class EnemyController : MonoBehaviour, IHitable
	{
		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		private IEnemy Enemy { get; set; }
		private bool StoppedByGettingHit { get; set; }

		private readonly Vector3 _moveDirection = Vector3.back;
		public event Action<EnemyController> OnDeath;

		public void Initialize()
		{
			Enemy = new Zombie();
			StoppedByGettingHit = false;
			OnDeath = null;
		}

		public bool Tick()
		{
			HandleMovement();

			return !Enemy.IsDead();
		}

		private void HandleMovement()
		{
			if (StoppedByGettingHit)
				return;

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

		public bool TakeHit(float damage)
		{
			if (Enemy.IsDead())
				return false;

			Enemy.TakeDamage(damage);
			StoppedByGettingHit = true;
			Invoke(nameof(ResetMovement), 0.3f);

			if (Enemy.IsDead())
				OnDeath?.Invoke(this);

			return true;
		}

		private void ResetMovement()
		{
			StoppedByGettingHit = false;
		}

		public void SubscribeToDeathEvent(Action<EnemyController> callback)
		{
			OnDeath += callback;
		}

		public void UnsubscribeFromDeathEvent(Action<EnemyController> callback)
		{
			OnDeath -= callback;
		}
	}
}