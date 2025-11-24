using Domain.Interface.Weapon;

namespace Controller.Weapon
{
	public class WeaponInstance
	{
		private IWeaponConfig Config { get; set; }

		public WeaponInstance(IWeaponConfig config)
		{
			Config = config;
		}
	}
}