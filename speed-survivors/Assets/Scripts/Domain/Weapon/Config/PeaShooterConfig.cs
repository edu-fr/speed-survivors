using Domain.Interface.General;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;

namespace Domain.Weapon.Config
{
	public class PeaShooterConfig : IWeaponConfig
	{
		public WeaponType WeaponType => WeaponType.PeaShooter;
		public IStats<WeaponStatType> Stats { get; } = new WeaponStats(
			5,
			20,
			1,
			0.4f,
			2f,
			0f);
	}
}