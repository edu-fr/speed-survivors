using Controller.Interface.Weapon.Strategy;
using Controller.Weapon.Ammo;
using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Controller.Weapon
{
	public abstract class WeaponInstance : MonoBehaviour
	{
		/// <summary>
		/// Serialize field for the projectile prefab associated with this weapon.
		/// </summary>
		protected abstract Projectile ProjectilePrefab { get; set; }

		public abstract IWeaponConfig Config { get; }
		protected abstract IWeaponStrategy Strategy { get; }
		public float CurrentCooldown { get; set; }

		public virtual void Tick(float deltaTime, Vector3 origin)
		{
			CurrentCooldown -= deltaTime;
			if (CurrentCooldown <= 0)
			{
				Fire();
				CurrentCooldown = Config.BaseCooldown;
			}
		}

		private void Fire()
		{
			// Strategy.Execute(origin, Config); ?
			ProjectileManager.Instance.SpawnProjectile(
				ProjectilePrefab,
				transform.position,
				Vector3.forward,
				Config.ProjectileSpeed,
				Config.BaseDamage);
		}
	}
}