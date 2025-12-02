using UnityEngine;

namespace Domain.DTO
{
	[System.Serializable]
	public class UpgradeOptionData
	{
		public string Title;
		public string Description;
		public Sprite Icon;
		public Color RarityColor;
		public string UniqueId;
	}
}