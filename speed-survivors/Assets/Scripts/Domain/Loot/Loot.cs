using Domain.Interface.Loot;

namespace Domain.Drop
{
	public class Loot : ILoot
	{
		public LootType Type { get; private set; }
		public int Amount { get; private set; }
		public string Id { get; private set; }

		public static ILoot Xp(int amount)
		{
			return new Loot
			{
				Type = LootType.Xp,
				Amount = amount,
				Id = string.Empty
			};
		}

		public static ILoot Coin(int amount)
		{
			return new Loot
			{
				Type = LootType.Coin,
				Amount = amount,
				Id = string.Empty
			};
		}

		public static ILoot Item(string itemId, int amount = 1)
		{
			return new Loot
			{
				Type = LootType.Item,
				Amount = amount,
				Id = itemId
			};
		}
	}
}