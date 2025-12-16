using Domain.Interface.Weapon.Strategy;
using Domain.Weapon.Strategy.Base;
using Engine;
using UnityEngine;

namespace Domain.Weapon.Strategy.ProjectileStrategies
{
	public class GranadeLauncherStrategy : ProjectileStrategy
	{
		public override ProjectileMovementPattern ProjectileMovementPattern => ProjectileMovementPattern.Parabolic;

		private const float SpreadAngle = 3f;
		private Vector3 HighSpawnOffset { get; } = new(0, 1.5f, 0.5f); // For casting from above the player
		private Range<float> ProjectileHeight { get; } = new(2.5f, 3f);

		public override Vector3 GetProjectileDirection(int projectileIndex, int totalProjectiles)
		{
			if (totalProjectiles <= 1)
				return Vector3.forward;

			var step = SpreadAngle / (totalProjectiles - 1);
			var currentAngle = -SpreadAngle / 2 + (step * projectileIndex);

			var rotation = Quaternion.Euler(0, currentAngle, 0);
			return rotation * Vector3.forward;
		}

		public override Vector3 GetPositionModifier(int projectileIndex, int totalProjectiles)
		{
			return HighSpawnOffset;
		}

		public override Vector3 GetMaxOffsetPosition(int projectileIndex, int totalProjectiles)
		{
			return new Vector3(0, Random.Range(ProjectileHeight.Start, ProjectileHeight.End), 0);
		}

		public override float GetSpawnDelay(int projectileIndex, int totalProjectiles)
		{
			return 0.1f * projectileIndex;
		}
	}
}