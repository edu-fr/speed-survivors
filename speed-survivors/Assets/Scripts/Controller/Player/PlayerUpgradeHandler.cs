using Domain.Interface.Player;

namespace Controller.Player
{
	public class PlayerUpgradeHandler
	{
		private IPlayer PlayerDomainRef { get; set; }

		public PlayerUpgradeHandler(IPlayer playerDomainRef)
		{
			PlayerDomainRef = playerDomainRef;
		}
	}
}