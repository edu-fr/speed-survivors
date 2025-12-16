using Controller.General;
using Controller.Interface;
using Controller.UI;
using Domain.Interface.Weapon.Base;
using UnityEngine;

namespace Controller.Weapon
{
	public abstract class AuraWeaponInstance : BaseWeaponInstance
	{
		private static readonly Collider[] HitBuffer = new Collider[100];

		[field: SerializeField]
		protected LayerMask EnemyLayer { get; set; }

		private float _range;
		private float _angle;

		protected override void PerformAttack(float emitterSpeed, int weaponLevel)
		{
			_range = Config.GetStat(WeaponStatType.Range, weaponLevel);
			_angle = Config.GetStat(WeaponStatType.AreaOfEffectRadius, weaponLevel);
			var damage = Config.GetStat(WeaponStatType.DamagePerHit, weaponLevel);
			var hits = Physics.OverlapSphereNonAlloc(transform.position, _range, HitBuffer, EnemyLayer);

			for (var i = 0; i < hits; i++)
			{
				var targetCollider = HitBuffer[i];
				var targetPosition = targetCollider.transform.position;

				if (!IsTargetInAngle(targetPosition, _angle))
					continue;

				if (!targetCollider.TryGetComponent<EnemyHitboxRelay>(out var relay))
					return;

				relay.EnemyController.TakeHit(damage);
				DamageNumbersManager.Instance.SpawnDamagePopup(targetPosition, (int) damage, true);
			}
		}

		private bool IsTargetInAngle(Vector3 targetPos, float angle)
		{
			if (angle >= 360f)
				return true;

			var directionToTarget = (targetPos - transform.position).normalized;
			var angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

			return angleToTarget < angle / 2f;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(1, 0, 0, 0.3f);
			Gizmos.DrawWireSphere(transform.position, _range);

			if (_angle < 360)
			{
				Gizmos.color = Color.yellow;
				Vector3 rightLimit = Quaternion.Euler(0, _angle / 2, 0) * transform.forward;
				Vector3 leftLimit = Quaternion.Euler(0, -_angle / 2, 0) * transform.forward;

				Gizmos.DrawLine(transform.position, transform.position + rightLimit * _range);
				Gizmos.DrawLine(transform.position, transform.position + leftLimit * _range);
			}
		}
	}
}