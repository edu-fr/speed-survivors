using Controller.Enemy;
using Controller.General;
using Controller.Player;
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

		private PlayerController PlayerController { get; set; }

		private void Start()
		{
			SpawnPlayer();
			WorldManager.Init(PlayerController.transform);
			WorldManager.SpawnInitialWorldSections();
			CameraController.Init(PlayerController.transform);
			CameraController.StartFollowing();
			EnemyManager.StartSpawn();
		}

		private void Update()
		{
			var deltaTime = Time.deltaTime;
			PlayerController.Tick(deltaTime);
			EnemyManager.Tick(deltaTime);
			WorldManager.Tick();
		}

		private void SpawnPlayer()
		{
			PlayerController = Instantiate(PlayerPrefab);
			PlayerController.Init(MainCamera, StartingPoint.position, 5f);
		}
	}
}