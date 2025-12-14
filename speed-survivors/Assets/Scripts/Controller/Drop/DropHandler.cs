using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Controller.General;
using Controller.Player;
using Domain.Interface.Loot;
using Domain.Loot;
using Engine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.Drop
{
	[Serializable]
	public class DropHandler : Initializable
	{
		[Header("Magnet Config")]
		private const float MagnetSpeed = 5f;

		[Header("Drop Movement Config")]
		private const float DropArcHeight = 1.2f;

		private const float PopDuration = 1f;
		private readonly Range<float> _dropScatterRadius = new(1.3f, 1.9f);

		[Header("Drop Movement Config")]
		private const int DropValuePerSpawn = 5;

		/// <summary>
		/// HardCap to avoid crashes.
		/// </summary>
		private const int MaxOrbsPerDrop = 20;

		[field: SerializeField]
		private DropController XpPrefab { get; set; }

		[field: SerializeField]
		private DropController CoinPrefab { get; set; }

		[field: SerializeField]
		private DropController ItemPrefab { get; set; }

		private PlayerController PlayerController { get; set; }
		private Transform CachedPlayerTransform { get; set; }
		private List<DropController> ActiveDrops { get; set; }

		public void Init(PlayerController playerController)
		{
			EnsureStillNotInitialized();

			PlayerController = playerController;
			CachedPlayerTransform = playerController.transform;
			ActiveDrops = new List<DropController>(PoolManager.DefaultPoolMaxSize);

			Initialized = true;
		}

		public void Tick()
		{
			CheckInit();

			var playerPosition = CachedPlayerTransform.position;
			var deltaTime = Time.deltaTime;
			var activeCount = ActiveDrops.Count;

			for (var i = activeCount - 1; i >= 0; i--)
			{
				var item = ActiveDrops[i];

				StartMagnetismIfInRange(item, playerPosition);

				if (!item.IsMagnetized && !item.IsAnimationFinished)
				{
					TickProcessSpawnAnimation(item, deltaTime);
					continue;
				}

				TickProcessMovementAndCollection(item, playerPosition, deltaTime, i);
			}
		}

		public void SpawnLootCluster(Vector3 originPosition, ILoot lootData)
		{
			CheckInit();

			if (lootData.Type == LootType.Item)
			{
				SpawnSingleDrop(originPosition, lootData);
			}
			else
			{
				SpawnMultipleDrops(originPosition, lootData);
			}
		}

		private void SpawnMultipleDrops(Vector3 origin, ILoot lootData)
		{
			var total = lootData.Amount;

			var orbCount = Mathf.Clamp(total / DropValuePerSpawn, 1, MaxOrbsPerDrop);

			var valuePerOrb = total / orbCount;
			var remainder = total % orbCount;

			for (var i = 0; i < orbCount; i++)
			{
				var finalValue = valuePerOrb;
				if (i == 0)
				{
					finalValue += remainder;
				}

				var orbLoot = new Loot(lootData.Type, finalValue, lootData.Id);
				SpawnSingleDrop(origin, orbLoot);
			}
		}

		private void SpawnSingleDrop(Vector3 position, ILoot loot)
		{
			var dropObj = PoolManager.Instance.Spawn(
				GetPrefabForDropType(loot.Type),
				position,
				Quaternion.identity
			);

			var randomCircle = Random.insideUnitCircle * Random.Range(_dropScatterRadius.Start, _dropScatterRadius.End);
			var targetPos = position + new Vector3(randomCircle.x, 0, randomCircle.y);

			dropObj.Init(position, targetPos, loot);
			ActiveDrops.Add(dropObj);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TickProcessSpawnAnimation(DropController item, float deltaTime)
		{
			item.AnimationTimer += deltaTime;
			var progress = item.AnimationTimer / PopDuration;

			var currentLinearPos = Vector3.Lerp(item.StartPosition, item.TargetPosition, progress);

			var heightOffset = Mathf.Sin(progress * Mathf.PI) * DropArcHeight;

			item.TransformCache.position =
				new Vector3(currentLinearPos.x, currentLinearPos.y + heightOffset, currentLinearPos.z);

			if (item.AnimationTimer >= PopDuration)
			{
				item.IsAnimationFinished = true;
				item.TransformCache.position = item.TargetPosition;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void StartMagnetismIfInRange(DropController item, Vector3 playerPosition)
		{
			if (item.IsMagnetized)
				return;

			var distanceX = playerPosition.x - item.TransformCache.position.x;
			var distanceZ = playerPosition.z - item.TransformCache.position.z;

			var distanceSquared = (distanceX * distanceX) + (distanceZ * distanceZ);

			if (distanceSquared < PlayerController.GetMagnetRangeSquared())
			{
				item.SetMagnetized();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TickProcessMovementAndCollection(DropController item, Vector3 playerPosition, float deltaTime,
			int itemIndex)
		{
			if (!item.IsMagnetized)
				return;

			var currentPosition = item.TransformCache.position;
			var directionVector = playerPosition - currentPosition;
			var moveDistance = (PlayerController.GetCurrentForwardSpeed() + MagnetSpeed) * deltaTime;

			if (directionVector.sqrMagnitude <= moveDistance * moveDistance)
			{
				CollectItemAtIndex(item, itemIndex);
			}
			else
			{
				item.TransformCache.position = currentPosition + directionVector.normalized * moveDistance;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CollectItemAtIndex(DropController item, int index)
		{
			var lootData = item.GetLootDataCopy();
			PlayerController.OnLootCollected(lootData);

			var last = ActiveDrops.Count - 1;
			ActiveDrops[index] = ActiveDrops[last];
			ActiveDrops.RemoveAt(last);

			PoolManager.Instance.Despawn(GetPrefabForDropType(lootData.Type), item);
		}

		private DropController GetPrefabForDropType(LootType lootType)
		{
			return lootType switch
			{
				LootType.XP => XpPrefab,
				LootType.Coin => CoinPrefab,
				LootType.Item => ItemPrefab,
				_ => throw new ArgumentOutOfRangeException(lootType.ToString(),
					"Invalid drop type in GetPrefabForDropType")
			};
		}

		public void TriggerBigVacuum()
		{
			CheckInit();

			foreach (var item in ActiveDrops)
			{
				item.SetMagnetized();
			}
		}
	}
}