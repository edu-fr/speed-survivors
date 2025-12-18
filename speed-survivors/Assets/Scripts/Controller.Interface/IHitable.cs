namespace Controller.Interface
{
	public interface IHitable
	{
		bool TakeHit(float damage, bool isCritical);
	}
}