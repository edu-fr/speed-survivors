using System;
using System.Collections;
using Controller.CustomCamera;
using Controller.Drop;
using Controller.Enemy;
using Controller.General;
using Controller.Player;
using Controller.UI;
using Controller.Weapon.Ammo;
using Controller.World;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		private EnemySpawnHandler EnemySpawnHandler { get; set; }

		[field: SerializeField]
		private WorldObjectsSpawnHandler WorldObjectsSpawnHandler { get; set; }

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
			EnemySpawnHandler.Tick(deltaTime);
			WorldObjectsSpawnHandler.Tick(deltaTime);
			WorldBuildHandler.Tick();
			DropHandler.Tick();
		}

		public void LateUpdate()
		{
			if (!GameplayStarted)
				return;

			FollowingCameraHandler.LateTick();
		}

		private void SetupScene()
		{
			ProjectileHandler = new ProjectileHandler();
			PlayerController = Instantiate(PlayerPrefab);
			PlayerController.Init(MainCamera, StartingPoint.position, WorldBuildHandler.DefaultSegmentTransformSize.x / 2f, ProjectileHandler);
			PlayerController.SubscribeToPlayerDeath(OnPlayerDeath);
			PlayerUpgradeHandler = new PlayerUpgradeHandler(PlayerController.GetPlayerDomainRef());
			UIHandler.Init(PlayerController, PlayerUpgradeHandler);
			DropHandler.Init(PlayerController);

			var playerTransform = PlayerController.transform;
			FollowingCameraHandler.Init(playerTransform);
			EnemySpawnHandler.Init(playerTransform, DropHandler);
			EnemySpawnHandler.OnDespawnedAlive += PlayerController.TakeHit;
			WorldObjectsSpawnHandler.Init(playerTransform, DropHandler);
			WorldBuildHandler.Init(playerTransform);
			WorldBuildHandler.SpawnInitialWorldSections();
		}

		private void StartGameplay()
		{
			if (GameplayStarted)
				throw new InvalidOperationException("Gameplay already started");

			EnemySpawnHandler.StartSpawn();
			WorldObjectsSpawnHandler.StartSpawn();
			GameplayStarted = true;
		}

		private void OnPlayerDeath()
		{
			StartCoroutine(DeathCoroutine());
		}

		private IEnumerator DeathCoroutine()
		{
			GameManager.Instance.SlowTime(nameof(GameplaySceneController) + ".OnPlayerDeath");
			yield return new WaitForSecondsRealtime(.4f);
			UIHandler.ShowTryAgainModal();
			yield return new WaitForSecondsRealtime(1.3f);
			UIHandler.HideTryAgainModal();
			GameManager.Instance.ResumeTime(nameof(GameplaySceneController) + ".OnPlayerDeath");
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void OnDestroy()
		{
			EnemySpawnHandler.OnDespawnedAlive -= PlayerController.TakeHit;
			PlayerController.UnsubscribeFromPlayerDeath(OnPlayerDeath);
		}
	}
}