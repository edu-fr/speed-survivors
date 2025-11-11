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

		void Awake()
		{
			Player.SetPosition(StartingPoint.position);
		}

		void Start()
		{

		}

		void Update()
		{
		}
	}
}