using System.Numerics;

namespace Domain.Interface.Weapon
{
	public interface IWeaponStrategy
	{
		void Execute(Vector3 origin, IWeaponConfig config);
	}
}