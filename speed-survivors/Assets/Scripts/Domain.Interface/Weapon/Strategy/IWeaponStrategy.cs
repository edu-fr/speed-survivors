using UnityEngine;

namespace Domain.Interface.Weapon.Strategy
{
	public interface IWeaponStrategy
	{
		/// <summary>
		/// Returns a modifier for the projectile speed based on its index and the total number of projectiles.
		/// Default: 1f
		/// </summary>
		float GetSpeedModifier(int projectileIndex, int totalProjectiles);

		/// <summary>
		/// Returns a direction vector for the projectile based on its index and the total number of projectiles.
		/// Default: Vector3.forward
		/// </summary>
		Vector3 GetProjectileDirection(int projectileIndex, int totalProjectiles);

		/// <summary>
		/// Returns a position modifier vector for the projectile based on its index and the total number of projectiles.
		/// Default: Vector3.zero
		/// </summary>
		Vector3 GetPositionModifier(int projectileIndex, int totalProjectiles);

		/// <summary>
		/// Returns a maximum offset position for the projectile. Used for launch weapons or different movement types
		/// </summary>
		Vector3 GetMaxOffsetPosition(int projectileIndex, int totalProjectiles);

		/// <summary>
		/// Returns a delay between projectile spawns based on its index and the total number of projectiles.
		/// </summary>
		float GetSpawnDelay(int projectileIndex, int totalProjectiles);
	}
}