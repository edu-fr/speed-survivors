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

		public virtual void Init()
		{
			EnsureStillNotInit();
			CurrentCooldown = 0f;
			Initialized = true;
		}

		public void Tick(float deltaTime, bool shouldShoot, float emitterSpeed, int weaponLevel)
		{
			CheckInit();

			CurrentCooldown -= deltaTime;
			if (CurrentCooldown > 0f)
				return;

			if (shouldShoot)
			{
				PerformAttack(emitterSpeed, weaponLevel);
				CurrentCooldown = Config.GetStat(WeaponStatType.FireCooldown, weaponLevel);
			}
		}

		protected abstract void PerformAttack(float emitterSpeed, int weaponLevel);
	}
}