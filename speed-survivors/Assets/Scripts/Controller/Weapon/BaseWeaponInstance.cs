using System;
using Controller.General;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;
using Domain.Interface.Weapon.Strategy;

namespace Controller.Weapon
{
	public abstract class BaseWeaponInstance : InitializableMono
	{
		public abstract IWeaponConfig Config { get; }
		protected abstract IWeaponStrategy Strategy { get; }
		private float CurrentCooldown { get; set; }
		private Random Rand { get; set; }

		public virtual void Init()
		{
			EnsureStillNotInit();
			CurrentCooldown = 0f;
			Rand = new Random();
			Initialized = true;
		}

		public void Tick(float deltaTime, bool shouldShoot, float emitterSpeed, int weaponLevel, float critChance)
		{
			CheckInit();

			CurrentCooldown -= deltaTime;
			if (CurrentCooldown > 0f)
				return;

			if (!shouldShoot)
				return;

			var isCritical = Rand.NextDouble() < critChance;
			PerformAttack(emitterSpeed, weaponLevel, isCritical);
			CurrentCooldown = Config.GetStat(WeaponStatType.FireCooldown, weaponLevel);
		}

		protected abstract void PerformAttack(float emitterSpeed, int weaponLevel, bool isCritical);
	}
}