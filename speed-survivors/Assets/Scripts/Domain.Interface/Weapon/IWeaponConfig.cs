namespace Domain.Interface.Weapon
{
	public interface IWeaponConfig
	{
		float BaseDamage { get; }
		float Range { get; }
		float BaseCooldown { get; }
		IWeaponStrategy Strategy { get; }
	}
}