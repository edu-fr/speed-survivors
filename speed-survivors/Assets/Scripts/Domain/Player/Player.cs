using System.Collections.Generic;
using Domain.Interface.Player;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;
using Domain.Weapon.Base;
using Domain.Weapon.Config;

namespace Domain.Player
{
	public class Player : IPlayer
	{
		public float MaxHP { get; private set; }
		public float LateralLateralMoveSpeed { get; private set; }
		public float ForwardMoveSpeed { get; private set; }
		public float BaseDamage { get; private set; }
		public float CurrentHP { get; private set; }
		public IWeaponArsenal Arsenal { get; private set; }

		public Player(float maxHP, float lateralMoveSpeed, float forwardMoveSpeed, float baseDamage)
		{
			MaxHP = maxHP;
			LateralLateralMoveSpeed = lateralMoveSpeed;
			ForwardMoveSpeed = forwardMoveSpeed;
			BaseDamage = baseDamage;
			CurrentHP = maxHP;
			Arsenal = new WeaponArsenal();
		}

		public Player() // Debug default player
		{
			MaxHP = 100f;
			LateralLateralMoveSpeed = 10f;
			ForwardMoveSpeed = 2f;
			BaseDamage = 5f;
			CurrentHP = MaxHP;
			Arsenal = new WeaponArsenal(new List<IWeaponConfig>() { new PeaShooterConfig() });
		}
	}
}