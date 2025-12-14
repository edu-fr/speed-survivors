using System;
using Controller.General;
using Controller.Interface;
using Controller.UI;
using Controller.UI.Enemy;
using Domain.Enemy;
using Domain.Interface.Enemy;
using Domain.Interface.Loot;
using UnityEngine;
using View.Enemy;

namespace Controller.Enemy
{
	public class EnemyController : InitializableMono, IHitable, ISpawnable
	{
		private const float DefaultMovementRecoverTime = .3f;

		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		[field: SerializeField]
		private EnemyView View { get; set; }

		private IEnemy Enemy { get; set; }

		private readonly Vector3 _moveDirection = Vector3.back;
		public event Action<EnemyController> OnDeath;

		private bool _stoppedByGettingHit;
		private float _currentMovementRecoverTime;

		public void Init()
		{
			EnsureStillNotInit();

			Enemy = new Zombie();
			_stoppedByGettingHit = false;
			OnDeath = null;
			View.Setup();

			Initialized = true;
		}

		public bool Tick(float dt)
		{
			CheckInit();

			HandleMovement(dt);
			View.Tick(dt);

			return !Enemy.IsDead();
		}

		private void HandleMovement(float dt)
		{
			if (_stoppedByGettingHit)
			{
				_currentMovementRecoverTime -= dt;
				if (_currentMovementRecoverTime > 0f)
				{
					return;
				}

				_stoppedByGettingHit = false;
			}

			transform.Translate(_moveDirection * (Enemy.MoveSpeed * Time.deltaTime));
		}

		public float GetHeight()
		{
			CheckInit();

			return Collider.size.y;
		}

		public void SetPosition(Vector3 position)
		{
			CheckInit();

			transform.position = position;
		}

		public bool TakeHit(float damage)
		{
			CheckInit();

			if (Enemy.IsDead())
				return false;

			View.PlayHitFeedback();
			Enemy.TakeDamage(damage);
			DamageNumbersManager.Instance.SpawnDamagePopup(transform.position, (int) damage, false);
			_stoppedByGettingHit = true;
			_currentMovementRecoverTime = DefaultMovementRecoverTime;

			if (Enemy.IsDead())
				OnDeath?.Invoke(this);

			return true;
		}

		public void SubscribeToDeathEvent(Action<EnemyController> callback)
		{
			CheckInit();

			OnDeath += callback;
		}

		public void UnsubscribeFromDeathEvent(Action<EnemyController> callback)
		{
			CheckInit();

			OnDeath -= callback;
		}

		public ILoot GetLoot()
		{
			CheckInit();

			return Enemy.Loot;
		}

		public void OnDespawn()
		{
			Initialized = false;
		}
	}
}