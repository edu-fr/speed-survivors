using Domain.Interface.Player;
using Domain.Interface.Upgrade;

namespace Domain.Upgrade
{
	public abstract class BaseUpgrade : IUpgrade
	{
		public abstract UpgradeType Type { get; }
		public abstract string Title { get; }
		public abstract string Description { get; }
		public abstract bool Eligible(IPlayer player);
		public abstract void Apply(IPlayer player);
	}
}