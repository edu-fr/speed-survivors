using Domain.Interface.Weapon.Strategy;
using UnityEngine;

namespace Domain.Weapon.Strategy.Base
{
	public abstract class ProjectileStrategy : IWeaponStrategy
	{
		public virtual ProjectileMovementPattern ProjectileMovementPattern => ProjectileMovementPattern.Linear;

		/// <summary>
		/// Base implementation returns 1f
		/// </summary>
		public virtual float GetSpeedModifier(int projectileIndex, int totalProjectiles)
		{
			return 1f;
		}

		/// <summary>
		/// Base implementation returns Vector3.forward
		/// </summary>
		public virtual Vector3 GetProjectileDirection(int projectileIndex, int totalProjectiles)
		{
			return Vector3.forward;
		}

		/// <summary>
		/// Base implementation returns Vector3.one
		/// </summary>
		public virtual Vector3 GetPositionModifier(int projectileIndex, int totalProjectiles)
		{
			return Vector3.zero;
		}

		/// <summary>
		/// Base implementation returns Vector3.zero
		/// </summary>
		public virtual Vector3 GetMaxOffsetPosition(int projectileIndex, int totalProjectiles)
		{
			return Vector3.zero;
		}

		/// <summary>
		/// Base implementation returns 0f
		/// </summary>
		public virtual float GetSpawnDelay(int projectileIndex, int totalProjectiles)
		{
			return 0f;
		}
	}
}