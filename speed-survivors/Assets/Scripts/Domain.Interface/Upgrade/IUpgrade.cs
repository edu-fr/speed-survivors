using Domain.Interface.Player;

namespace Domain.Interface.Upgrade
{
	public interface IUpgrade
	{
		UpgradeType Type { get; }
		string Title { get; }
		string Description { get; }
		void Apply(IPlayer player);
		bool Eligible(IPlayer player);
	}
}