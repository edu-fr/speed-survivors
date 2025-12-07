using Domain.Weapon.Strategy.Base;
using UnityEngine;

namespace Domain.Weapon.Strategy
{
	public class ShotgunStrategy : ProjectileStrategy
	{
		const float SpreadAngle = 5f;
		public override Vector3 GetProjectileDirection(int projectileIndex, int totalProjectiles)
		{
			var startAngle = -SpreadAngle * (totalProjectiles - 1) / 2;
			var angle = startAngle + SpreadAngle * projectileIndex;
			var rotation = Quaternion.Euler(0, angle, 0);
			return rotation * Vector3.forward;
		}
	}
}