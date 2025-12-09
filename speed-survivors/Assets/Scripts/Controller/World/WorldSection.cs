using Controller.Interface;
using UnityEngine;

namespace Controller.World
{
	public class WorldSection : MonoBehaviour, ISpawnable
	{
		[Header("References")]
		[field: SerializeField]
		private Transform GroundObject { get; set; }
		public Vector3 SectionTransformSize { get; set; }
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

			var rawMeshSizeX = ActiveMeshFilter.sharedMesh.bounds.size.x;
			var rawMeshSizeY = ActiveMeshFilter.sharedMesh.bounds.size.y;
			var rawMeshSizeZ = ActiveMeshFilter.sharedMesh.bounds.size.z;
			var sizeX = rawMeshSizeX * GroundObject.localScale.x * transform.localScale.x;
			var sizeY = rawMeshSizeY * GroundObject.localScale.y * transform.localScale.y;
			var sizeZ = rawMeshSizeZ * GroundObject.localScale.z * transform.localScale.z;

			SectionTransformSize = new Vector3(sizeX, sizeY, sizeZ);
		}

		public void OnDespawn()
		{
			// Nothing to clean up for now
		}
	}
}