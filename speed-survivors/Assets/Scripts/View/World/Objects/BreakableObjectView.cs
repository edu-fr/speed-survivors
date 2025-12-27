using UnityEngine;
using View.General;

namespace View.World.Objects
{
	public class BreakableObjectView : MonoBehaviour
	{
		[field: SerializeField]
		public Collider Collider { get; private set; }

		[field: SerializeField]
		private HitFeedbackView HitFeedbackView { get; set; }

		public void PlayHitFeedback()
		{
			HitFeedbackView.PlayHitFeedback();
		}
	}
}