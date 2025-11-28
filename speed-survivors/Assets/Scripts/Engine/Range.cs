using System;
using UnityEngine;

namespace Engine
{
	[Serializable]
	public struct Range<T> where T : struct
	{
		[field: SerializeField]
		public T Start { get; private set; }

		[field: SerializeField]
		public T End { get; private set; }

		public Range(T start, T end)
		{
			Start = start;
			End = end;
		}
	}
}