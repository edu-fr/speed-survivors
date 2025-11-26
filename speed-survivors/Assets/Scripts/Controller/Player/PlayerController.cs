using Domain.Interface.Player;
using UnityEngine;

namespace Controller.Player
{
	public class PlayerController : MonoBehaviour
	{
		[field: SerializeField]
		private BoxCollider Collider { get; set; }

		[field: SerializeField]
		private PlayerWeaponArsenalHandler WeaponArsenalHandler { get; set; }

		private PlayerInputHandler InputHandler { get; set; }
		private PlayerMovementHandler MovementHandler { get; set; }
		private bool Initialized { get; set; }
		private IPlayer Player { get; set; }

		public void Init(Camera mainCamera, Vector3 startingPos, float xMoveRange)
		{
			Player = new Domain.Player.Player();
			InputHandler = new PlayerInputHandler(mainCamera);
			MovementHandler = new PlayerMovementHandler(Player, transform, xMoveRange, startingPos.x);
			SetupStartingPosition(startingPos, Collider.size.y);
			WeaponArsenalHandler.Init(Player);

			Initialized = true;
		}

		public void Tick(float deltaTime)
		{
			if (!Initialized)
				return;

			// Weapon tick is called here, and the projectiles tick are called on ProjectileManager's Update
			WeaponArsenalHandler.Tick(Time.deltaTime, true);
			HandleInput();
			HandleMovement();
			HandleAutoForwardMovement(deltaTime);
		}

		private void HandleAutoForwardMovement(float deltaTime)
		{
			MovementHandler.UpdateCurrentZTargetPosition(transform.position.z + Player.MoveSpeed * deltaTime);
		}

		private void HandleInput()
		{
			if (!InputHandler.GetTargetInputPosition(out var touchWorldPosition))
				return;

			MovementHandler.UpdateCurrentXTargetPosition(touchWorldPosition.x);
		}

		private void HandleMovement()
		{
			MovementHandler.MovePlayerTowardsCurrentTargetPosition();
		}

		private void SetupStartingPosition(Vector3 startingPos, float playerHeight)
		{
			transform.position = startingPos + new Vector3(0, playerHeight / 2f, 0);
		}

		private void OnDestroy()
		{
			WeaponArsenalHandler.OnDestroy();
		}
	}
}