namespace Domain.Interface.World.Objects
{
	public interface IBreakableObject
	{
		float MaxHp { get; }
		float CurrentHp { get; }
		void TakeDamage(float amount);
		bool IsAlive();
	}
}