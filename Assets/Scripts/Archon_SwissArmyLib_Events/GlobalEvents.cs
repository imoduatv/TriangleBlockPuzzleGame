using System;
using System.Collections.Generic;

namespace Archon.SwissArmyLib.Events
{
	public static class GlobalEvents
	{
		private static readonly Dictionary<int, Event> Events = new Dictionary<int, Event>();

		public static void Invoke(int eventId)
		{
			if (Events.TryGetValue(eventId, out Event value))
			{
				value.Invoke();
			}
		}

		public static void AddListener(int eventId, IEventListener listener, int priority = 0)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			if (!Events.TryGetValue(eventId, out Event value))
			{
				value = (Events[eventId] = new Event(eventId));
			}
			value.AddListener(listener, priority);
		}

		public static void AddListener(int eventId, Action listener, int priority = 0)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			if (!Events.TryGetValue(eventId, out Event value))
			{
				value = (Events[eventId] = new Event(eventId));
			}
			value.AddListener(listener, priority);
		}

		public static void RemoveListener(int eventId, IEventListener listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			if (Events.TryGetValue(eventId, out Event value))
			{
				value.RemoveListener(listener);
			}
		}

		public static void RemoveListener(int eventId, Action listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			if (Events.TryGetValue(eventId, out Event value))
			{
				value.RemoveListener(listener);
			}
		}

		public static void RemoveListener(IEventListener listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			foreach (int key in Events.Keys)
			{
				RemoveListener(key, listener);
			}
		}

		public static void RemoveListener(Action listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			foreach (int key in Events.Keys)
			{
				RemoveListener(key, listener);
			}
		}

		public static void Clear()
		{
			foreach (Event value in Events.Values)
			{
				value.Clear();
			}
		}

		public static void Clear(int eventId)
		{
			if (Events.TryGetValue(eventId, out Event value))
			{
				value.Clear();
			}
		}
	}
	public static class GlobalEvents<T>
	{
		private static readonly Dictionary<int, Event<T>> Events = new Dictionary<int, Event<T>>();

		public static void Invoke(int eventId, T args)
		{
			if (Events.TryGetValue(eventId, out Event<T> value))
			{
				value.Invoke(args);
			}
		}

		public static void AddListener(int eventId, IEventListener<T> listener, int priority = 0)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			if (!Events.TryGetValue(eventId, out Event<T> value))
			{
				value = (Events[eventId] = new Event<T>(eventId));
			}
			value.AddListener(listener, priority);
		}

		public static void AddListener(int eventId, Action<T> listener, int priority = 0)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			if (!Events.TryGetValue(eventId, out Event<T> value))
			{
				value = (Events[eventId] = new Event<T>(eventId));
			}
			value.AddListener(listener, priority);
		}

		public static void RemoveListener(int eventId, IEventListener<T> listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			if (Events.TryGetValue(eventId, out Event<T> value))
			{
				value.RemoveListener(listener);
			}
		}

		public static void RemoveListener(int eventId, Action<T> listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			if (Events.TryGetValue(eventId, out Event<T> value))
			{
				value.RemoveListener(listener);
			}
		}

		public static void RemoveListener(IEventListener<T> listener)
		{
			foreach (int key in Events.Keys)
			{
				RemoveListener(key, listener);
			}
		}

		public static void RemoveListener(Action<T> listener)
		{
			foreach (int key in Events.Keys)
			{
				RemoveListener(key, listener);
			}
		}

		public static void Clear()
		{
			foreach (Event<T> value in Events.Values)
			{
				value.Clear();
			}
		}

		public static void Clear(int eventId)
		{
			if (Events.TryGetValue(eventId, out Event<T> value))
			{
				value.Clear();
			}
		}
	}
}
