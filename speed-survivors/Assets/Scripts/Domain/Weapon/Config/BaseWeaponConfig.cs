using System.Collections.Generic;
using Domain.Interface.Weapon.Base;
using Domain.Interface.Weapon.Config;
using UnityEngine;

namespace Domain.Weapon.Config
{
	public abstract class BaseWeaponConfig : IWeaponConfig
	{
		protected abstract Dictionary<WeaponStatType, float[]> StatsByLevel { get; }
		public abstract WeaponType WeaponType { get; }

		public float GetStat(WeaponStatType type, int level)
		{
			StatsByLevel.TryGetValue(type, out var statLevels);
			if (statLevels == null)
				throw new KeyNotFoundException($"Stat type {type} not found in weapon config {WeaponType}");

			return statLevels[Mathf.Clamp(level - 1, 0, statLevels.Length - 1)];
		}
	}
}