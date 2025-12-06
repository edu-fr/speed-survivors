using Domain.Interface.Player;
using UnityEngine;

namespace Domain.DTO.Upgrades.Base
{
	public abstract class UpgradeEffectSO : ScriptableObject
	{
		/// <summary>
		/// Used in the inheritor class to map to a domain object
		/// </summary>
		protected abstract string Id { get; }

		[field: SerializeField]
		public string Title { get; protected  set; }

		[field: SerializeField]
		public string Description { get; protected  set; }

		[field: SerializeField]
		public Sprite Icon { get; protected  set; }

		public abstract void Apply(IPlayer player);
	}
}