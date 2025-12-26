using Controller.General;
using UnityEngine;
using View.UI.Modal;

namespace Controller.Modal
{
	public class TryAgainModalController : InitializableMono
	{
		[field: SerializeField]
		private TryAgainModalView TryAgainModalView { get; set; }

		public void Init()
		{
			EnsureStillNotInit();

			Initialized = true;
		}

		public void Show()
		{
			CheckInit();
			TryAgainModalView.Show();
		}

		public void Hide()
		{
			CheckInit();
			TryAgainModalView.Hide();
		}
	}
}