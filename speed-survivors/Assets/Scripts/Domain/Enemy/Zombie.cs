namespace Domain.Enemy
{
	public class Zombie : BaseEnemy
	{
		public override float MaxHP { get; protected set; } = 10f;
		public override float CurrentHP { get; protected set; } = 10f;
		public override float MoveSpeed { get; protected set; } = .5f;
		public override float Damage { get; protected set; } = 10f;
	}
}