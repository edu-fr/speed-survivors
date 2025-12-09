using System.Collections.Generic;
using Controller.Interface;
using UnityEngine;
using UnityEngine.Pool;

namespace Controller.General
{
	public class PoolManager : MonoBehaviour
	{
		public const int DefaultPoolCapacity = 1000;
		public const int DefaultPoolMaxSize = 10000;
		public static PoolManager Instance { get; set; }
		private Dictionary<int, object> Pools { get; set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				Pools = new Dictionary<int, object>();
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component, ISpawnable
		{
			var key = prefab.GetInstanceID();

			if (!Pools.ContainsKey(key))
			{
				CreatePool(key, prefab);
			}

			var instance = ((ObjectPool<T>) Pools[key]).Get();
			instance.transform.SetPositionAndRotation(position, rotation);

			return instance;
		}

		public void Despawn<T>(T prefab, T instance) where T : Component, ISpawnable
		{
			var key = prefab.GetInstanceID();
			if (Pools.TryGetValue(key, out var pool))
			{
				instance.OnDespawn();
				((ObjectPool<T>)pool).Release(instance);
			}
			else
			{
				Debug.LogWarning(
					$"No pool found for prefab {prefab.name} when trying to despawn instance {instance.name}.");
				// Fallback
				Destroy(instance);
			}
		}

		private void CreatePool<T>(int key, T prefab) where T : Component
		{
			var holderName = $"{prefab.name}_PoolHolder";
			var holderGameObject = new GameObject(holderName);
			holderGameObject.transform.SetParent(transform);
			holderGameObject.transform.localPosition = Vector3.zero;

			var pool = new ObjectPool<T>(
				createFunc: () =>
				{
					var obj = Instantiate(prefab, holderGameObject.transform);
					obj.gameObject.SetActive(false);
					return obj;
				},
				actionOnGet: component => component.gameObject.SetActive(true),
				actionOnRelease: component =>
				{
					component.gameObject.SetActive(false);
					component.transform.SetParent(holderGameObject.transform);
				},
				defaultCapacity: DefaultPoolCapacity,
				maxSize: DefaultPoolMaxSize
			);

			Pools.Add(key, pool);
		}
	}
}