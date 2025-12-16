using Domain.Interface.Weapon.Config;
using Domain.Interface.Weapon.Strategy;
using Domain.Weapon.Config;
using Domain.Weapon.Strategy.AuraStrategies;

namespace Controller.Weapon.Arsenal.AuraWeapons
{
	public class Flamethrower : AuraWeaponInstance
	{
		public override IWeaponConfig Config => new FlamethrowerConfig();
		protected override IWeaponStrategy Strategy => new FlamethrowerStrategy();
	}
}