namespace Controller.General.Base
{
	public abstract class Initializable
	{
		protected bool Initialized { get; set; }

		protected void EnsureStillNotInitialized()
		{
			if (Initialized)
				throw new System.InvalidOperationException(
					$"{GetType().Name} already initialized. Cannot initialize more than once.");
		}

		protected void CheckInit()
		{
			if (!Initialized)
				throw new System.InvalidOperationException(
					$"{GetType().Name} not initialized. Call Initialize() before using this instance.");
		}
	}
}