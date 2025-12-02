using Controller.SceneController;
using UnityEngine;

namespace Controller.General
{
	public class GameplayManager : MonoBehaviour
	{
		[field: SerializeField]
		private GameplaySceneController GameplaySceneController { get; set; }

		private void Start()
		{
			GameplaySceneController.SetupScene();
			GameplaySceneController.StartGameplay();
		}

		private void Update()
		{
			GameplaySceneController.Tick();
		}

		private void LateUpdate()
		{
			GameplaySceneController.LateTick();
		}

		public static void PauseTime(string caller)
		{
			Time.timeScale = 0f;
			Debug.Log($"Time Paused by {caller}");
		}

		public static void ResumeTime(string caller)
		{
			Time.timeScale = 1f;
			Debug.Log($"Time unpaused by {caller}");
		}
	}
}