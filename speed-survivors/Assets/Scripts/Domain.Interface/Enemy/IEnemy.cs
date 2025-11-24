
namespace Domain.Interface.Enemy
{
	public interface IEnemy
	{
		public float MaxHP { get; }
		public float MoveSpeed { get; }
		public float Damage { get; }
		public float CurrentHP { get;  }
	}
}