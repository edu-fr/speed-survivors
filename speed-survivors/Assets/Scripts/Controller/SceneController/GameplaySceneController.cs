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

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
			Player.SetPosition(StartingPoint.position);
		}

		// Update is called once per frame
		void Update()
		{
		}
	}
}