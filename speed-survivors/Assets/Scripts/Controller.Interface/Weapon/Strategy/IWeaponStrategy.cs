using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Controller.Interface.Weapon.Strategy
{
	public interface IWeaponStrategy
	{
		void Execute(Vector3 origin, IWeaponConfig config);
	}
}