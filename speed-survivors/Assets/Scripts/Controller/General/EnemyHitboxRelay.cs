using Controller.Enemy;
using UnityEngine;

namespace Controller.General
{
	/// <summary>
	/// Component to be present on hurtboxes
	/// </summary>
	public class EnemyHitboxRelay : MonoBehaviour
	{
		[field: SerializeField]
		public EnemyController EnemyController { get; private set; }
	}
}