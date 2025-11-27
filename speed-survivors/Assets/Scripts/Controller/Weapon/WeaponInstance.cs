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

		public virtual void Tick(float deltaTime, bool shouldShoot, float transformCurrentVelocity)
		{
			CurrentCooldown -= deltaTime;

			if (CurrentCooldown > 0f)
				return;

			if (shouldShoot)
				Fire(transformCurrentVelocity);
		}

		private void Fire(float transformCurrentVelocity)
		{
			// Strategy.Execute(origin, Config); ?
			ProjectileManager.Instance.SpawnProjectile(
				ProjectilePrefab,
				transform.position,
				Vector3.forward,
				transformCurrentVelocity + Config.ProjectileSpeed,
				Config.BaseDamage);

			CurrentCooldown = Config.BaseCooldown;
		}
	}
}