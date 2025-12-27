using Domain.Interface.World.Objects;

namespace Domain.World.Objects
{
	public abstract class BaseBreakableObject : IBreakableObject
	{
		public abstract float MaxHp { get; }
		public float CurrentHp { get; private set; }

		public void TakeDamage(float amount)
		{
			var newHp = CurrentHp - amount;
			CurrentHp = newHp < 0f ? 0f : newHp;
		}

		public bool IsAlive()
		{
			return CurrentHp > 0f;
		}
	}
}