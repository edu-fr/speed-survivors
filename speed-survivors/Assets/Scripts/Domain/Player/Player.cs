using Domain.Interface.Player;
using Domain.Interface.Weapon;
using Domain.Weapon;

namespace Domain.Player
{
	public class Player : IPlayer
	{
		public float MaxHP { get; private set; }
		public float MoveSpeed { get; private set; }
		public float BaseDamage { get; private set; }
		public float CurrentHP { get; private set; }
		public IWeaponArsenal Arsenal { get; private set; }

		public Player(float maxHP, float moveSpeed, float baseDamage)
		{
			MaxHP = maxHP;
			MoveSpeed = moveSpeed;
			BaseDamage = baseDamage;
			CurrentHP = maxHP;
			Arsenal = new WeaponArsenal();
		}

		public Player() // Debug default player
		{
			MaxHP = 100f;
			MoveSpeed = 2f;
			BaseDamage = 5f;
			CurrentHP = MaxHP;
			Arsenal = new WeaponArsenal();
		}
	}
}