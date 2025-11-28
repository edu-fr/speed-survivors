namespace Domain.Interface.Loot
{
	public interface ILoot
	{
		LootType Type { get; }
		int Amount { get; }
		string Id { get; }
	}
}