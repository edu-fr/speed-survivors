using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Controller.General
{
	public class PoolManager : MonoBehaviour
	{
		public const int DefaultPoolCapacity = 100;
		public const int DefaultPoolMaxSize = 1000;
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

		public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
		{
			var key = prefab.GetInstanceID();

			if (!Pools.ContainsKey(key))
			{
				CreatePool(key, prefab);
			}

			var instance = ((ObjectPool<T>)Pools[key]).Get();
			instance.transform.SetParent(parent);
			instance.transform.SetPositionAndRotation(position, rotation);

			return instance;
		}

		public void Despawn<T>(T prefab, T instance) where T : Component
		{
			var key = prefab.GetInstanceID();
			if (Pools.TryGetValue(key, out var pool))
			{
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
			var pool = new ObjectPool<T>(
				createFunc: () => Instantiate(prefab),
				actionOnGet: component => component.gameObject.SetActive(true),
				actionOnRelease: component => component.gameObject.SetActive(false),
				defaultCapacity: DefaultPoolCapacity,
				maxSize: DefaultPoolMaxSize
			);

			Pools.Add(key, pool);
		}
	}
}