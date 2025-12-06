using System.Collections.Generic;
using Controller.General;
using UnityEngine;

namespace Controller.Weapon.Ammo
{
	public class ProjectileHandler
	{
		private const float ProjectileLifetime = 2f;
		private List<Projectile> ActiveProjectiles { get; set; }

		public ProjectileHandler()
		{
			ActiveProjectiles = new List<Projectile>();
		}

		public void Tick()
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
			var projectile = PoolManager.Instance.Spawn(prefab, pos, Quaternion.LookRotation(dir));
			projectile.Init(prefab, damage, speed, ProjectileLifetime, dir);
			ActiveProjectiles.Add(projectile);
		}

		private void DespawnProjectile(Projectile projectile, int indexInList)
		{
			var lastIndex = ActiveProjectiles.Count - 1;
			ActiveProjectiles[indexInList] = ActiveProjectiles[lastIndex];
			ActiveProjectiles.RemoveAt(lastIndex);

			PoolManager.Instance.Despawn(projectile.Prefab, projectile);
		}
	}
}