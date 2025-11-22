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
		private EnemySpawner EnemySpawner { get; set; }

		[field: SerializeField]
		private MeshRenderer Ground { get; set; }

		void Start()
		{
			SpawnPlayer();
			EnemySpawner.StartSpawningEnemies();
		}

		private void SpawnPlayer()
		{
			var player = Instantiate(PlayerPrefab);
			player.Init(MainCamera, GetAdjustedStartingPositionOnGround(), Ground.bounds.min.x, Ground.bounds.max.x);
		}

		private Vector3 GetAdjustedStartingPositionOnGround()
		{
			return new Vector3(StartingPoint.position.x, Ground.bounds.max.y, StartingPoint.position.z);
		}
	}
}