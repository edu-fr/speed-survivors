using UnityEngine;

namespace Controller.General
{
	public abstract class InitializableMono : MonoBehaviour
	{
		protected bool Initialized { get; set; }

		protected void EnsureStillNotInit()
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