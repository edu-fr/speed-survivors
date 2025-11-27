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

			SetupStartingPosition(startingPos);
			WeaponArsenalHandler.Init(Player);

			Initialized = true;
		}

		public void Tick(float deltaTime)
		{
			if (!Initialized)
				return;

			WeaponArsenalHandler.Tick(deltaTime, true, MovementHandler.CurrentForwardVelocity);

			HandleInput();
			HandleMovement(deltaTime);
		}

		private void HandleInput()
		{
			if (InputHandler.TryGetTouchWorldPosition(out var touchWorldPosition))
			{
				MovementHandler.UpdateInputTargetX(touchWorldPosition.x);
			}
		}

		private void HandleMovement(float deltaTime)
		{
			MovementHandler.TickMovement(deltaTime);
		}

		private void SetupStartingPosition(Vector3 startingPos)
		{
			var heightOffset = Collider != null ? Collider.size.y / 2f : 1f;
			transform.position = startingPos + new Vector3(0, heightOffset, 0);
		}

		private void OnDestroy()
		{
			if (InputHandler != null)
				InputHandler.DisableInput();

			if (WeaponArsenalHandler != null)
				WeaponArsenalHandler.OnDestroy();
		}
	}
}