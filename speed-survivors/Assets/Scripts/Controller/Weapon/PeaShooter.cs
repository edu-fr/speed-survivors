using Controller.Interface.Weapon.Strategy;
using Controller.Weapon.Strategy;
using Domain.Weapon.Config;
using UnityEngine;

namespace Controller.Weapon
{
	public class PeaShooter : WeaponInstance
	{
		protected override IWeaponStrategy Strategy => new PeaShooterStrategy();

		public PeaShooter(PeaShooterConfig config) : base(config)
		{
		}

		public override void Tick(Vector3 origin)
		{
			Strategy.Execute(origin, Config);
		}
	}
}