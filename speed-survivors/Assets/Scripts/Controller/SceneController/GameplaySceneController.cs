using Controller.Enemy;
using Controller.Player;
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
		private MeshRenderer Ground { get; set; }

		private void Start()
		{
			SpawnPlayer();
			EnemyManager.StartSpawn();
		}

		private void Update()
		{
			var deltaTime = Time.deltaTime;
			EnemyManager.Tick(deltaTime);
		}

		private void SpawnPlayer()
		{
			var player = Instantiate(PlayerPrefab);
			player.Init(MainCamera, GetAdjustedStartingPositionOnGround(), Ground.bounds);
		}

		private Vector3 GetAdjustedStartingPositionOnGround()
		{
			return new Vector3(StartingPoint.position.x, Ground.bounds.max.y, StartingPoint.position.z);
		}
	}
}