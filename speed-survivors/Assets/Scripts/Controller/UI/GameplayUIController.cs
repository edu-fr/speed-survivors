using Controller.Modal;
using Controller.Player;
using UnityEngine;
using View.UI;

namespace Controller.UI
{
	public class GameplayUIController : MonoBehaviour
	{
		[field: SerializeField]
		private ExperienceBarView XpBarView { get; set; }

		[field: SerializeField]
		private LevelUpModalController LevelUpModalController { get; set; }

		private PlayerController PlayerController { get; set; }

		public void Init(PlayerController playerController)
		{
			PlayerController = playerController;
			SubscribeToPlayerEvents();
			var initialXpData = playerController.GetCurrentXpData();
			XpBarView.UpdateLevelLabel(initialXpData.level);
			XpBarView.UpdateProgress(initialXpData.currentXp, initialXpData.nextLevelXpDelta);
		}

		private void SubscribeToPlayerEvents()
		{
			if (PlayerController != null)
			{
				PlayerController.SubscribeToXpCollected(HandleExperienceUpdate);
			}
		}

		private void UnsubscribeFromPlayerEvents()
		{
			if (PlayerController != null)
			{
				PlayerController.UnsubscribeToXpCollected(HandleExperienceUpdate);
			}
		}

		private void HandleExperienceUpdate((int currentXp, int level, int nextLevelXpDelta) xpData)
		{
			UpdateVisuals(xpData.currentXp, xpData.level, xpData.nextLevelXpDelta);
			LevelUpModalController.HandleExperienceUpdate(xpData.level);
		}

		private void UpdateVisuals(float xp, int level, float reqXp)
		{
			XpBarView.UpdateLevelLabel(level);
			XpBarView.UpdateProgress(xp, reqXp);
		}

		private void OnDestroy()
		{
			UnsubscribeFromPlayerEvents();
		}
	}
}