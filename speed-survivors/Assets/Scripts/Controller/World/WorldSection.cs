using UnityEngine;

namespace Controller.World
{
	public class WorldSection : MonoBehaviour
	{
		[Header("References")]
		[field: SerializeField]
		private Transform GroundObject { get; set; }
		public float SizeZ { get; private set; }
		private MeshFilter ActiveMeshFilter { get; set; }

		private void Awake()
		{
			CalculateSize();
		}

		private void OnValidate()
		{
			CalculateSize();
		}

		private void CalculateSize()
		{
			if (ActiveMeshFilter == null)
				ActiveMeshFilter = GroundObject.GetComponent<MeshFilter>();

			if (ActiveMeshFilter == null || ActiveMeshFilter.sharedMesh == null)
			{
				Debug.LogWarning($"WorldSection: No mesh found on {GroundObject.name}", this);
				return;
			}

			var rawMeshSizeZ = ActiveMeshFilter.sharedMesh.bounds.size.z;
			var localScaleZ = GroundObject.localScale.z;
			SizeZ = rawMeshSizeZ * localScaleZ * transform.localScale.z;
		}
	}
}