using Controller.Interface.Weapon.Strategy;
using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Controller.Weapon.Strategy.Base
{
	public class AuraStrategy : IWeaponStrategy
	{
		public void Execute(Vector3 origin, IWeaponConfig config)
		{
			throw new System.NotImplementedException();
		}
	}
}