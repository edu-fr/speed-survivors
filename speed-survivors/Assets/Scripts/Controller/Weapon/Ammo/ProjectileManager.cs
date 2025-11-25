using System.Collections.Generic;
using Controller.General;
using UnityEngine;

namespace Controller.Weapon.Ammo
{
	public class ProjectileManager : MonoBehaviour
	{
		private const float ProjectileLifetime = 4f;

		[field: SerializeField]
		private Transform ProjectilePoolParent { get; set; }

		public static ProjectileManager Instance { get; private set; }
		private List<Projectile> ActiveProjectiles { get; set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				ActiveProjectiles = new List<Projectile>(PoolManager.DefaultPoolMaxSize);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void Update()
		{
			for (int i = ActiveProjectiles.Count - 1; i >= 0; i--)
			{
				if (!ActiveProjectiles[i].Tick(Time.deltaTime))
				{
					DespawnProjectile(ActiveProjectiles[i], i);
				}
			}
		}

		public void SpawnProjectile(Projectile prefab, Vector3 pos, Vector3 dir, float speed, float damage)
		{
			var projectile = PoolManager.Instance.Spawn(prefab, pos, Quaternion.LookRotation(dir), ProjectilePoolParent);
			projectile.Initialize(prefab, damage, speed, ProjectileLifetime, dir);
			ActiveProjectiles.Add(projectile);
		}

		public void DespawnProjectile(Projectile projectile, int indexInList = -1)
		{
			if (indexInList != -1)
			{
				int lastIndex = ActiveProjectiles.Count - 1;
				ActiveProjectiles[indexInList] = ActiveProjectiles[lastIndex];
				ActiveProjectiles.RemoveAt(lastIndex);
			}
			else
			{
				ActiveProjectiles.Remove(projectile);
			}

			PoolManager.Instance.Despawn(projectile.Prefab, projectile);
		}
	}
}