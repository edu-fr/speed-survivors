using Controller.Drop;
using Controller.General;
using Controller.Interface;
using Controller.UI;
using Domain.Interface.World.Objects;
using UnityEngine;
using View.World.Objects;

namespace Controller.World.Objects
{
	public class BreakableObjectController : InitializableMono, ISpawnable, IHitable
	{
		[field: SerializeField]
		private BreakableObjectView View { get; set; }

		private IBreakableObject BreakableObject { get; set; }

		public void Init(IBreakableObject breakableObject)
		{
			EnsureStillNotInit();

			BreakableObject = breakableObject;

			Initialized = true;
		}

		public void OnDespawn()
		{
			BreakableObject = null;
			Initialized = false;
		}

		public bool TakeHit(float damage, bool isCritical)
		{
			CheckInit();

			if (BreakableObject.IsAlive())
			{
				return false;
			}

			View.PlayHitFeedback();
			BreakableObject.TakeDamage(damage);
			var positionWithOffset = transform.position + new Vector3(0f, GetHeight() * 0.5f, 0f);
			DamageNumbersManager.Instance.SpawnDamagePopup(positionWithOffset, (int) damage, isCritical);

			return BreakableObject.IsAlive();
		}

		public bool IsAlive()
		{
			return BreakableObject.IsAlive();
		}

		private float GetHeight()
		{
			return View.Collider.bounds.size.y;
		}
	}
}