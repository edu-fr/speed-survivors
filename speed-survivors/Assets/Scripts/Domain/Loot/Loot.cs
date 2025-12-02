using Domain.Interface.Loot;

namespace Domain.Loot
{
	public class Loot : ILoot
	{
		public LootType Type { get; private set; }
		public int Amount { get; private set; }
		public string Id { get; private set; }

		public Loot(LootType type, int amount, string id)
		{
			Type = type;
			Amount = amount;
			Id = id;
		}

		public static ILoot Xp(int amount)
		{
			return new Loot(LootType.XP, amount, string.Empty);
		}

		public static ILoot Coin(int amount)
		{
			return new Loot(LootType.Coin, amount, string.Empty);
		}

		public static ILoot Item(string itemId, int amount = 1)
		{
			return new Loot(LootType.Item, amount, itemId);
		}
	}
}