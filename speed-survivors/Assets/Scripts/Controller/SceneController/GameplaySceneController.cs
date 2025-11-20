using Controller.Player;
using UnityEngine;

namespace Controller.SceneController
{
	public class GameplaySceneController : MonoBehaviour
	{
		[field: SerializeField]
		private Transform StartingPoint { get; set; }

		[field: SerializeField]
		private PlayerController Player { get; set; }

		[field: SerializeField]
		private BoxCollider Ground { get; set; }

		void Start()
		{
			Player.Init(StartingPoint.position, Ground.bounds.min.x, Ground.bounds.max.x);
		}
	}
}