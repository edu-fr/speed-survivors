using UnityEngine;

namespace View.UI.Modal
{
	public class TryAgainModalView : MonoBehaviour
	{
		[field: SerializeField]
		private GameObject ContentRoot { get; set; }

		public void Show()
		{
			ContentRoot.SetActive(true);
		}

		public void Hide()
		{
			ContentRoot.SetActive(false);
		}
	}
}