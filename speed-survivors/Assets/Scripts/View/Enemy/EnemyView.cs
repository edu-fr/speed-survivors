using UnityEngine;
using View.General;

namespace View.Enemy
{
	public class EnemyView : MonoBehaviour
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