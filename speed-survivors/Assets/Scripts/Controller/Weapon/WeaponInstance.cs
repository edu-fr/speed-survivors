using Controller.Interface.Weapon.Strategy;
using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Controller.Weapon
{
	public abstract class WeaponInstance
	{
		protected IWeaponConfig Config { get; }
		protected abstract IWeaponStrategy Strategy { get; }

		protected WeaponInstance(IWeaponConfig config)
		{
			Config = config;
		}

		public abstract void Tick(Vector3 origin);
	}
}