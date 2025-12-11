using Domain.Interface.Weapon.Strategy;
using Domain.Weapon.Strategy.Base;
using UnityEngine;

namespace Domain.Weapon.Strategy
{
	public class GranadeLauncherStrategy : ProjectileStrategy
	{
		public override ProjectileMovementPattern ProjectileMovementPattern => ProjectileMovementPattern.Parabolic;

		private float SpreadAngle { get; set; } = 30f;

		private float RangeVariance { get; set; } = 0f;

		private Vector3 SpawnOffset { get; set; }= new(0, 1.5f, 0.5f); // Cast from above the player

		public override Vector3 GetProjectileDirection(int projectileIndex, int totalProjectiles)
		{
			if (totalProjectiles <= 1)
				return Vector3.forward;

			var step = SpreadAngle / (totalProjectiles - 1);
			var currentAngle = -SpreadAngle / 2 + (step * projectileIndex);

			var rotation = Quaternion.Euler(0, currentAngle, 0);
			return rotation * Vector3.forward;
		}

		public override float GetSpeedModifier(int projectileIndex, int totalProjectiles)
		{
			var variance = Random.Range(1f - RangeVariance, 1f + RangeVariance);
			return variance;
		}

		public override Vector3 GetPositionModifier(int projectileIndex, int totalProjectiles)
		{
			return SpawnOffset;
		}

		public override Vector3 GetMaxOffsetPosition(int projectileIndex, int totalProjectiles)
		{
			return new Vector3(0, 3.5f, 0);
		}
	}
}