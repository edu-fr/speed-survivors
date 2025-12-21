using System;
using UnityEngine;

namespace Engine
{
	[Serializable]
	public struct Range<T> where T : struct
	{
		[field: SerializeField]
		public T Start { get; set; }

		[field: SerializeField]
		public T End { get; set; }

		public Range(T start, T end)
		{
			Start = start;
			End = end;
		}
	}
}