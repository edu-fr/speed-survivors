using Domain.Interface.Enemy;

namespace Domain.Enemy
{
	public abstract class BaseEnemy : IEnemy
	{
		public abstract float MaxHP { get; protected set; }
		public abstract float MoveSpeed { get; protected set; }
		public abstract float Damage { get; protected set; }
		public abstract float CurrentHP { get; protected set; }

		public void TakeDamage(float amount)
		{
			CurrentHP -= amount;
			if (CurrentHP < 0)
			{
				CurrentHP = 0;
			}
		}

		public void Heal(float amount)
		{
			CurrentHP += amount;
			if (CurrentHP > MaxHP)
			{
				CurrentHP = MaxHP;
			}
		}

		public bool IsDead()
		{
			return CurrentHP <= 0;
		}
	}
}