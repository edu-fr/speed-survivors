using System;
using System.Collections.Generic;
using Domain.Interface.General;

namespace Domain.General
{
	public abstract class BaseStats<T> : IStats<T> where T : Enum
	{
		protected abstract Dictionary<T, float> StatDict { get; set; }

		public float GetStat(T statType)
		{
			if (!StatDict.TryGetValue(statType, out var statValue))
				throw new ArgumentOutOfRangeException(nameof(statType),
					$"Fail to get. StatType {statType} not found in stats dictionary.");

			return statValue;
		}

		public void SetStat(T statType, float value)
		{
			if (!StatDict.TryGetValue(statType, out _))
				throw new ArgumentOutOfRangeException(nameof(statType),
					$"Failed to set. StatType {statType} not found in stats dictionary.");

			StatDict[statType] = value;
		}

		public void IncreaseStat(T statType, float value)
		{
			if (!StatDict.TryGetValue(statType, out var currentValue))
				throw new ArgumentOutOfRangeException(nameof(statType),
					$"Failed to set. StatType {statType} not found in stats dictionary.");

			StatDict[statType] = currentValue + value;
		}

		public void DecreaseStat(T statType, float value)
		{
			if (!StatDict.TryGetValue(statType, out var currentValue))
				throw new ArgumentOutOfRangeException(nameof(statType),
					$"Failed to set. StatType {statType} not found in stats dictionary.");

			StatDict[statType] = Math.Clamp(currentValue - value, 0, float.MaxValue);
		}
	}
}