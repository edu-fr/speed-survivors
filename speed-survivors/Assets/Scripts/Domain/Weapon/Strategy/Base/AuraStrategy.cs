using Domain.Interface.Weapon.Strategy;
using UnityEngine;

namespace Domain.Weapon.Strategy.Base
{
	public class AuraStrategy : IWeaponStrategy
	{
		public ProjectileMovementPattern ProjectileMovementPattern => ProjectileMovementPattern.Aura;

		public float GetSpeedModifier(int projectileIndex, int totalProjectiles)
		{
			throw new System.NotImplementedException();
		}

		public Vector3 GetProjectileDirection(int projectileIndex, int totalProjectiles)
		{
			throw new System.NotImplementedException();
		}

		public Vector3 GetPositionModifier(int projectileIndex, int totalProjectiles)
		{
			throw new System.NotImplementedException();
		}

		public Vector3 GetMaxOffsetPosition(int projectileIndex, int totalProjectiles)
		{
			throw new System.NotImplementedException();
		}
	}
}