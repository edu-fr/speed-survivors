using UnityEngine;
using View.General;

namespace View.Player
{
	public class PlayerView : MonoBehaviour
	{
		[field: SerializeField]
		private HitFeedbackView HitFeedbackView { get; set; }

		public void Setup()
		{
			HitFeedbackView.Setup();
		}

		public void Tick(float dt)
		{
			HitFeedbackView.Tick(dt);
		}

		public void PlayHitFeedback()
		{
			HitFeedbackView.PlayHitFeedback();
		}
	}
}