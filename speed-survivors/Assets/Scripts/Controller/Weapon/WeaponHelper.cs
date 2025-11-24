using Domain.Interface.Weapon.Config;
using Domain.Weapon.Config;

namespace Controller.Weapon
{
	public static class WeaponHelper
	{
		public static WeaponInstance CreateWeaponInstanceByType(IWeaponConfig config)
		{
			return config switch
			{
				PeaShooterConfig peaShotConfig => new PeaShooter(peaShotConfig),
				ShotgunConfig shotgunConfig => new Shotgun(shotgunConfig),
				_ => throw new System.ArgumentException("Unsupported weapon config type")
			};
		}
	}
}