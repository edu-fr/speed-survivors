using UnityEngine;

namespace Controller.Weapon.Ammo
{
	public class Projectile : MonoBehaviour
	{
		public Projectile Prefab { get; set; }
		private float Lifetime { get; set; }
		private float CurrentTimer { get; set; }
		private float Speed { get; set; }
		private float Damage { get; set; }
		private Vector3 Direction { get; set; }

		public void Initialize(Projectile prefab, float damage, float speed, float lifetime, Vector3 direction)
		{
			Prefab = prefab;
			Lifetime = lifetime;
			CurrentTimer = lifetime;
			Direction = direction;
			Speed = speed;
			Damage = damage;
		}

		public bool Tick(float deltaTime)
		{
			Lifetime -= deltaTime;

			transform.position += (Direction * (Speed * deltaTime));

			return Lifetime > 0f;
		}

	}
}