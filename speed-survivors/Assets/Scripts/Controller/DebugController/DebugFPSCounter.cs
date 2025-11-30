using UnityEngine;

namespace Controller.DebugController
{
	public class DebugFPSCounter : MonoBehaviour
	{
		private void Update()
		{
			var fps = 1.0f / Time.deltaTime;
			var color = fps < 30 ? Color.red : Color.green;

			DebugOverlayManager.Instance.Track("FPS", (int) fps, color);
		}
	}
}