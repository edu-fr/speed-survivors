using System.Collections.Generic;
using Controller.Interface;
using UnityEngine;

namespace Controller.World
{
	public class WorldSection : MonoBehaviour, ISpawnable
	{
		[Header("References")]
		[field: SerializeField]
		private Transform GroundObject { get; set; }

		[field: SerializeField]
		private List<MeshFilter> WalkableMeshes { get; set; }

		public Vector3 SectionTransformSize { get; set; }
		private Vector3 SectionCenter { get; set; }

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
			if (WalkableMeshes == null || WalkableMeshes.Count == 0) return;
			var combinedBounds = new Bounds(WalkableMeshes[0].transform.position, Vector3.zero);
			foreach (var meshFilter in WalkableMeshes)
			{
				if (meshFilter != null && meshFilter.TryGetComponent<Renderer>(out var rendererComp))
				{
					combinedBounds.Encapsulate(rendererComp.bounds);
				}
			}

			SectionTransformSize = combinedBounds.size;
			SectionCenter = combinedBounds.center;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			// Usamos SectionCenter em vez de transform.position
			Gizmos.DrawWireCube(SectionCenter, SectionTransformSize);
		}

		public void OnDespawn()
		{
			// Nothing to clean up for now
		}
	}
}