using UnityEngine;

namespace Domain.Interface.Weapon.Strategy
{
	public interface IWeaponStrategy
	{
		/// <summary>
		/// Returns a modifier to be mu
		///
		///
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
	}
}