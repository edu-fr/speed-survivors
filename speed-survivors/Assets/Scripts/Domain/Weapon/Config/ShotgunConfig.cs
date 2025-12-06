using Domain.Interface.General;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;

namespace Domain.Weapon.Config
{
	public class ShotgunConfig : IWeaponConfig
	{
		public WeaponType WeaponType => WeaponType.Shotgun;

		public IStats<WeaponStatType> Stats { get; } = new WeaponStats(
			1,
			20,
			3,
			0.4f,
			3f,
			.5f);
	}
}