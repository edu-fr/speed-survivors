using System.Collections.Generic;
using Controller.General;
using Controller.UI.Enemy;
using UnityEngine;

namespace Controller.UI
{
	public class DamageNumbersManager : MonoBehaviour
	{
		public static DamageNumbersManager Instance { get; private set; }
		private const float Lifetime = 0.8f;
		private const float FloatSpeed = 3f;
		private const float ScaleUpDuration = 0.1f;
		private Vector2 SpreadX { get; } = new(-1f, 1f);

		[field: SerializeField]
		private DamageNumberController Prefab { get; set; }

		private List<DamageNumberController> _activeControllers;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				_activeControllers = new List<DamageNumberController>(100);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void SpawnDamagePopup(Vector3 position, int amount, bool isCritical)
		{
			var controller = PoolManager.Instance.Spawn(Prefab, position, Quaternion.identity);

			var randomX = Random.Range(SpreadX.x, SpreadX.y);
			var velocity = new Vector3(randomX, FloatSpeed, 0);
			controller.Init(position, amount, isCritical, Lifetime, ScaleUpDuration, velocity, DamageNumberStyle.Parabolic);

			_activeControllers.Add(controller);
		}

		private void Update()
		{
			var dt = Time.deltaTime;

			for (var i = _activeControllers.Count - 1; i >= 0; i--)
			{
				var controller = _activeControllers[i];
				if (!controller.Tick(dt))
				{
					DespawnController(controller, i);
				}
			}
		}

		private void DespawnController(DamageNumberController ctrl, int index)
		{
			int lastIndex = _activeControllers.Count - 1;
			_activeControllers[index] = _activeControllers[lastIndex];
			_activeControllers.RemoveAt(lastIndex);

			PoolManager.Instance.Despawn(Prefab, ctrl);
		}
	}
}