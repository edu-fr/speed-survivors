using Domain.Interface.Player;
using Domain.Interface.Upgrade;

namespace Controller.Player
{
	public class PlayerUpgradeHandler
	{
		private IPlayer PlayerDomainRef { get; set; }

		public PlayerUpgradeHandler(IPlayer playerDomainRef)
		{
			PlayerDomainRef = playerDomainRef;
		}

		public void HandleUpgrade(IUpgrade upgrade)
		{
			upgrade.Apply(PlayerDomainRef);
		}

		public IPlayer GetPlayerDomainRef()
		{
			return PlayerDomainRef;
		}
	}
}