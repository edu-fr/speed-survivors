using UnityEngine;

namespace View.UI.Utils
{
	[RequireComponent(typeof(RectTransform))]
	public class SafeArea : MonoBehaviour
	{
		private RectTransform RectTransform { get; set; }
		private Rect LastSafeArea { get; set; } = new(0, 0, 0, 0);
		private Vector2Int LastScreenSize { get; set; } = new(0, 0);
		private ScreenOrientation LastOrientation { get; set; } = ScreenOrientation.AutoRotation;

		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
			RefreshSafeArea();
		}

		private void Update()
		{
			if (HasScreenChanged())
			{
				RefreshSafeArea();
			}
		}

		private bool HasScreenChanged()
		{
			if (LastOrientation != Screen.orientation)
				return true;

			if (LastScreenSize.x != Screen.width || LastScreenSize.y != Screen.height)
				return true;

			if (LastSafeArea != Screen.safeArea)
				return true;

			return false;
		}

		private void RefreshSafeArea()
		{
			var safeArea = Screen.safeArea;

			LastSafeArea = safeArea;
			LastScreenSize = new Vector2Int(Screen.width, Screen.height);
			LastOrientation = Screen.orientation;

			ApplySafeAreaToRectTransform(safeArea);
		}

		private void ApplySafeAreaToRectTransform(Rect safeArea)
		{
			var anchorMin = safeArea.position;
			var anchorMax = safeArea.position + safeArea.size;

			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			RectTransform.anchorMin = anchorMin;
			RectTransform.anchorMax = anchorMax;

			RectTransform.offsetMin = Vector2.zero;
			RectTransform.offsetMax = Vector2.zero;
		}
	}
}