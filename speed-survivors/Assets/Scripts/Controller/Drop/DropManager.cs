using System;
using System.Collections.Generic;
using Controller.General;
using Domain.Interface.Loot;
using UnityEngine;

namespace Controller.Drop
{
	public class DropManager : MonoBehaviour
	{
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
		private float MagnetSpeed { get; set; } = 15f;
		private List<DropController> _activeDrops = new List<DropController>(2000);
		private bool Initialized { get; set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
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

			var playerPos = PlayerTransform.position;
			var dt = Time.deltaTime;

			for (var i = _activeDrops.Count - 1; i >= 0; i--)
			{
				var item = _activeDrops[i];
				var collected = item.TickMovementAndCheckCollected(dt, playerPos, MagnetRadius, MagnetSpeed);
				if (collected)
				{
					CollectItem(item, i);
				}
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

		public void SpawnDrop(Vector3 position, ILoot loot)
		{
			var dropObj = PoolManager.Instance.Spawn(XpPrefab, position, Quaternion.identity, GetPoolParent(loot));
			dropObj.Initialize(position, loot);
			_activeDrops.Add(dropObj);
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

		private void CollectItem(DropController item, int index)
		{
			// Apply drop on player via game manager or something like that
			// PlayerLevelManager.Instance.AddXp(item.XpValue);

			var lastIndex = _activeDrops.Count - 1;
			_activeDrops[index] = _activeDrops[lastIndex];
			_activeDrops.RemoveAt(lastIndex);

			PoolManager.Instance.Despawn(GetPrefabForDropType(item.Loot.Type), item);
		}

		private DropController GetPrefabForDropType(LootType lootType)
		{
			return lootType switch
			{
				LootType.Xp => XpPrefab,
				LootType.Coin => CoinPrefab,
				LootType.Item => ItemPrefab,
				_ => throw new ArgumentOutOfRangeException(lootType.ToString(), "Invalid drop type in GetPrefabForDropType")
			};
		}

		public void TriggerBigVacuum()
		{
			foreach (var item in _activeDrops)
			{
				item.SetMagnetized();
			}
		}
	}
}