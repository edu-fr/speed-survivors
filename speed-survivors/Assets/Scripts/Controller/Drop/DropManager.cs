using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Controller.General;
using Domain.Interface.Loot;
using UnityEngine;

namespace Controller.Drop
{
	public class DropManager : MonoBehaviour
	{
		private const float MagnetRadiusSqr = 5f * 5f;
		private const float PopDuration = 0.5f;
		private const float MagnetSpeed = 15f;

		[field: SerializeField]
		private DropController XpPrefab { get; set; }

		[field: SerializeField]
		private DropController CoinPrefab { get; set; }

		[field: SerializeField]
		private DropController ItemPrefab { get; set; }

		[field: SerializeField]
		public Transform XpDropPoolParent { get; private set; }

		[field: SerializeField]
		public Transform CoinDropPoolParent { get; private set; }

		[field: SerializeField]
		public Transform ItemDropPoolParent { get; private set; }

		public static DropManager Instance { get; private set; }
		private Transform PlayerTransform { get; set; }
		private float MagnetRadius { get; set; }
		private List<DropController> ActiveDrops { get; set; }
		private bool Initialized { get; set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				ActiveDrops = new List<DropController>(PoolManager.DefaultPoolMaxSize);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void Update()
		{
			if (!Initialized)
				return;

			var playerPosition = PlayerTransform.position;
			var deltaTime = Time.deltaTime;
			var activeCount = ActiveDrops.Count;

			for (var i = activeCount - 1; i >= 0; i--)
			{
				var item = ActiveDrops[i];

				StartMagnetismIfInRange(item, playerPosition);

				if (!item.IsMagnetized && !item.IsAnimationFinished)
				{
					ProcessSpawnAnimation(item, deltaTime);
					continue;
				}

				ProcessMovementAndCollection(item, playerPosition, deltaTime, i);
			}
		}

		public void Init(Transform playerTransform, float magnetRadius)
		{
			if (Initialized)
				throw new InvalidOperationException("DropManager already initialized");

			PlayerTransform = playerTransform;
			MagnetRadius = magnetRadius;

			Initialized = true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ProcessSpawnAnimation(DropController item, float deltaTime)
		{
			item.AnimationTimer += deltaTime;
			var progress = item.AnimationTimer / PopDuration;

			item.Transform.position = Vector3.Lerp(item.StartPosition, item.TargetPosition, progress);

			if (item.AnimationTimer >= PopDuration)
			{
				item.IsAnimationFinished = true;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void StartMagnetismIfInRange(DropController item, Vector3 playerPosition)
		{
			if (item.IsMagnetized)
				return;

			var distanceX = playerPosition.x - item.Transform.position.x;
			var distanceZ = playerPosition.z - item.Transform.position.z;

			var distanceSquared = (distanceX * distanceX) + (distanceZ * distanceZ);

			if (distanceSquared < MagnetRadiusSqr)
			{
				item.SetMagnetized();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ProcessMovementAndCollection(DropController item, Vector3 playerPosition, float deltaTime,
			int itemIndex)
		{
			if (!item.IsMagnetized)
				return;

			var currentPosition = item.Transform.position;
			var directionVector = playerPosition - currentPosition;
			var moveDistance = MagnetSpeed * deltaTime;

			if (directionVector.sqrMagnitude <= moveDistance * moveDistance)
			{
				CollectItemAtIndex(item, itemIndex);
			}
			else
			{
				item.Transform.position = currentPosition + directionVector.normalized * moveDistance;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CollectItemAtIndex(DropController item, int index)
		{
			// Player.AddXp(item.Value);

			int last = ActiveDrops.Count - 1;
			ActiveDrops[index] = ActiveDrops[last];
			ActiveDrops.RemoveAt(last);

			PoolManager.Instance.Despawn(GetPrefabForDropType(item.GetLootType()), item);
		}

		public void SpawnDrop(Vector3 position, ILoot loot)
		{
			var dropObj = PoolManager.Instance.Spawn(XpPrefab, position, Quaternion.identity, GetPoolParent(loot));
			dropObj.Initialize(position, loot);
			ActiveDrops.Add(dropObj);
		}

		private Transform GetPoolParent(ILoot loot)
		{
			return loot.Type switch
			{
				LootType.Xp => XpDropPoolParent,
				LootType.Coin => CoinDropPoolParent,
				LootType.Item => ItemDropPoolParent,
				_ => throw new ArgumentOutOfRangeException(loot.Type.ToString(), "Invalid drop type in SpawnDrop")
			};
		}

		private DropController GetPrefabForDropType(LootType lootType)
		{
			return lootType switch
			{
				LootType.Xp => XpPrefab,
				LootType.Coin => CoinPrefab,
				LootType.Item => ItemPrefab,
				_ => throw new ArgumentOutOfRangeException(lootType.ToString(),
					"Invalid drop type in GetPrefabForDropType")
			};
		}

		public void TriggerBigVacuum()
		{
			foreach (var item in ActiveDrops)
			{
				item.SetMagnetized();
			}
		}
	}
}