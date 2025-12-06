using System;
using Controller.CustomCamera;
using Controller.Drop;
using Controller.Enemy;
using Controller.Player;
using Controller.UI;
using Controller.Weapon.Ammo;
using Controller.World;
using UnityEngine;

namespace Controller.SceneController
{
	public class GameplaySceneController : MonoBehaviour
	{
		[field: SerializeField]
		private Camera MainCamera { get; set; }

		[field: SerializeField]
		private Transform StartingPoint { get; set; }

		[field: SerializeField]
		private PlayerController PlayerPrefab { get; set; }

		[field: SerializeField]
		private EnemiesHandler EnemiesHandler { get; set; }

		[field: SerializeField]
		private DropHandler DropHandler { get; set; }

		[field: SerializeField]
		private WorldBuildHandler WorldBuildHandler { get; set; }

		[field: SerializeField]
		private FollowingCameraHandler FollowingCameraHandler { get; set; }

		[field: SerializeField]
		private GameplayUIHandler UIHandler { get; set; }

		private PlayerUpgradeHandler PlayerUpgradeHandler { get; set; }
		private ProjectileHandler ProjectileHandler { get; set; }

		private PlayerController PlayerController { get; set; }
		private bool GameplayStarted { get; set; }

		private void Start()
		{
			SetupScene();
			StartGameplay();
		}

		public void Update()
		{
			if (!GameplayStarted)
				return;

			var deltaTime = Time.deltaTime;
			PlayerController.Tick(deltaTime);
			ProjectileHandler.Tick();
			EnemiesHandler.Tick(deltaTime);
			WorldBuildHandler.Tick();
			DropHandler.Tick();
		}

		public void LateUpdate()
		{
			if (!GameplayStarted)
				return;

			FollowingCameraHandler.LateTick();
			EnemiesHandler.LateTick();
		}

		private void SetupScene()
		{
			ProjectileHandler = new ProjectileHandler();
			PlayerController = Instantiate(PlayerPrefab);
			PlayerController.Init(MainCamera, StartingPoint.position, WorldBuildHandler.DefaultSegmentTransformSize.x / 2f, ProjectileHandler);
			PlayerUpgradeHandler = new PlayerUpgradeHandler(PlayerController.GetPlayerDomainRef());
			UIHandler.Init(PlayerController, PlayerUpgradeHandler);
			DropHandler.Init(PlayerController);

			var playerTransform = PlayerController.transform;
			FollowingCameraHandler.Init(playerTransform);
			EnemiesHandler.Init(playerTransform, DropHandler);
			WorldBuildHandler.Init(playerTransform);
			WorldBuildHandler.SpawnInitialWorldSections();
		}

		private void StartGameplay()
		{
			if (GameplayStarted)
				throw new InvalidOperationException("Gameplay already started");

			EnemiesHandler.StartSpawn();
			GameplayStarted = true;
		}

	}
}