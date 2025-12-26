using UnityEngine;

namespace Controller.General
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }

		public void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void SlowTime(string caller)
		{
			Time.timeScale = .2f;
			Debug.Log($"Time slowed by {caller}");
		}

		public void PauseTime(string caller)
		{
			Time.timeScale = 0f;
			Debug.Log($"Time Paused by {caller}");
		}

		public void ResumeTime(string caller)
		{
			Time.timeScale = 1f;
			Debug.Log($"Time unpaused by {caller}");
		}
	}
}