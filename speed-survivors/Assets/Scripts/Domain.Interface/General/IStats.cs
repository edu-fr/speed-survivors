using System;

namespace Domain.Interface.General
{
	public interface IStats<T> where T : Enum
	{
		float GetStat(T statType);
		void SetStat(T statType, float value);
		void IncreaseStat(T statType, float value);
		void DecreaseStat(T statType, float value);
	}
}