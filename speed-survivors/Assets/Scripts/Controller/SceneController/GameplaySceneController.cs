using System;
using Controller.Drop;
using Controller.Enemy;
using Controller.General;
using Controller.Player;
using Controller.UI;
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
		private EnemyManager EnemyManager { get; set; }

		[field: SerializeField]
		private WorldManager WorldManager { get; set; }

		[field: SerializeField]
		private CameraController CameraController { get; set; }

		[field: SerializeField]
		private GameplayUIController UIController { get; set; }

		private PlayerController PlayerController { get; set; }
		private bool GameplayStarted { get; set; }

		public void SetupScene()
		{
			PlayerController = Instantiate(PlayerPrefab);
			PlayerController.Init(MainCamera, StartingPoint.position, WorldManager.DefaultSegmentTransformSize.x / 2f);
			var playerTransform = PlayerController.transform;
			UIController.Init(PlayerController);
			WorldManager.Init(playerTransform);
			WorldManager.SpawnInitialWorldSections();
			CameraController.Init(playerTransform);
			EnemyManager.Init(playerTransform);
			DropManager.Instance.Init(PlayerController, PlayerController.GetPlayerMagnetRadius());
		}

		public void StartGameplay()
		{
			if (GameplayStarted)
				throw new InvalidOperationException("Gameplay already started");

			EnemyManager.StartSpawn();
			GameplayStarted = true;
		}

		public void Tick()
		{
			var deltaTime = Time.deltaTime;
			PlayerController.Tick(deltaTime);
			EnemyManager.Tick(deltaTime);
			WorldManager.Tick();
		}

		public void LateTick()
		{
			CameraController.LateTick();
		}
	}
}