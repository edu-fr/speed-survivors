using Domain.Interface.Weapon.Config;
using Domain.Interface.Weapon.Strategy;
using Domain.Weapon.Config;
using Domain.Weapon.Strategy.AuraStrategies;

namespace Controller.Weapon.Arsenal.AuraWeapons
{
	public class IceWind : AuraWeaponInstance
	{
		public override IWeaponConfig Config => new IceWindConfig();
		protected override IWeaponStrategy Strategy => new FlamethrowerStrategy();
	}
}