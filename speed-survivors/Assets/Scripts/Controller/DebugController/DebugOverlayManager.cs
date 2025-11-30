using System.Collections.Generic;
using UnityEngine;
using View.Debug;

namespace Controller.DebugController
{
	public class DebugOverlayManager : MonoBehaviour
	{
		public static DebugOverlayManager Instance { get; private set; }

		[field: SerializeField]
		private DebugEntryView EntryPrefab { get; set; }

		[field: SerializeField]
		private Transform ContainerContent { get; set; }

		private Dictionary<string, DebugEntryView> ActiveEntries { get; set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				ActiveEntries = new Dictionary<string, DebugEntryView>();
				DontDestroyOnLoad(gameObject.transform.root);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void Track(string key, object value)
		{
			if (ActiveEntries.TryGetValue(key, out var entry))
			{
				entry.UpdateValue(value);
			}
			else
			{
				CreateNewEntry(key, value);
			}
		}

		public void Track(string key, object value, Color color)
		{
			Track(key, value);
			if (ActiveEntries.TryGetValue(key, out var entry))
			{
				entry.UpdateColor(color);
			}
		}

		public void Remove(string key)
		{
			if (!ActiveEntries.TryGetValue(key, out var entry))
				return;

			Destroy(entry.gameObject);
			ActiveEntries.Remove(key);
		}

		private void CreateNewEntry(string key, object value)
		{
			var newEntry = Instantiate(EntryPrefab, ContainerContent);
			newEntry.Initialize(key);
			newEntry.UpdateValue(value);
			newEntry.name = $"Debug_{key}";

			ActiveEntries.Add(key, newEntry);
		}
	}
}