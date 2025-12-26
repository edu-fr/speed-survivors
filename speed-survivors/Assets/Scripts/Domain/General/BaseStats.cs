using System;
using System.Collections.Generic;
using Domain.Interface.General;

namespace Domain.General
{
	public abstract class BaseStats<T> : IStats<T> where T : Enum
	{
		protected abstract Dictionary<T, float> StatDict { get; set; }
		public event Action<T, float, float> OnStatChanged;

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

			var oldValue = StatDict[statType];
			StatDict[statType] = value;
			OnStatChanged?.Invoke(statType, value, value - oldValue);
		}

		public void IncreaseStat(T statType, float value)
		{
			if (!StatDict.TryGetValue(statType, out var currentValue))
				throw new ArgumentOutOfRangeException(nameof(statType),
					$"Failed to set. StatType {statType} not found in stats dictionary.");

			var oldValue = StatDict[statType];
			StatDict[statType] = currentValue + value;
			OnStatChanged?.Invoke(statType, StatDict[statType], StatDict[statType] - oldValue);
		}

		public void DecreaseStat(T statType, float value)
		{
			if (!StatDict.TryGetValue(statType, out var currentValue))
				throw new ArgumentOutOfRangeException(nameof(statType),
					$"Failed to set. StatType {statType} not found in stats dictionary.");

			var oldValue = StatDict[statType];
			StatDict[statType] = Math.Clamp(currentValue - value, 0, float.MaxValue);
			OnStatChanged?.Invoke(statType, StatDict[statType], oldValue - StatDict[statType]);
		}
	}
}