using Archon.SwissArmyLib.Collections;
using Archon.SwissArmyLib.Events.Loops;
using Archon.SwissArmyLib.Pooling;
using Archon.SwissArmyLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Events
{
	public class TellMeWhen : IEventListener
	{
		private struct Entry
		{
			public int Id;

			public object Args;

			public ITimerCallback Callback;

			public float Time;

			public bool Repeating;

			public float RepeatingInterval;

			public Entry(float time, ITimerCallback callback, int id = -1, object args = null)
			{
				Time = time;
				Callback = callback;
				Id = id;
				Args = args;
				Repeating = false;
				RepeatingInterval = 0f;
			}

			public void Invoke()
			{
				try
				{
					Callback.OnTimesUp(Id, Args);
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
			}
		}

		public interface ITimerCallback
		{
			void OnTimesUp(int id, object args);
		}

		public const int NoId = -1;

		private static readonly Pool<LinkedListNode<Entry>> SharedNodePool;

		private static readonly PooledLinkedList<Entry> EntriesScaled;

		private static readonly PooledLinkedList<Entry> EntriesUnscaled;

		static TellMeWhen()
		{
			SharedNodePool = new Pool<LinkedListNode<Entry>>(() => new LinkedListNode<Entry>(default(Entry)));
			EntriesScaled = new PooledLinkedList<Entry>(SharedNodePool);
			EntriesUnscaled = new PooledLinkedList<Entry>(SharedNodePool);
			TellMeWhen instance = new TellMeWhen();
			ServiceLocator.RegisterSingleton(instance);
			ServiceLocator.GlobalReset += delegate
			{
				ServiceLocator.RegisterSingleton(instance);
			};
		}

		private TellMeWhen()
		{
			ManagedUpdate.OnUpdate.AddListener(this);
		}

		~TellMeWhen()
		{
			ManagedUpdate.OnUpdate.RemoveListener(this);
		}

		public static void Exact(float time, ITimerCallback callback, int id = -1, object args = null)
		{
			if (object.ReferenceEquals(callback, null))
			{
				throw new ArgumentNullException("callback");
			}
			Entry entry = new Entry(time, callback, id, args);
			InsertIntoList(entry, EntriesScaled);
		}

		public static void Exact(float time, float repeatInterval, ITimerCallback callback, int id = -1, object args = null)
		{
			if (object.ReferenceEquals(callback, null))
			{
				throw new ArgumentNullException("callback");
			}
			Entry entry = new Entry(time, callback, id, args);
			entry.Repeating = true;
			entry.RepeatingInterval = repeatInterval;
			Entry entry2 = entry;
			InsertIntoList(entry2, EntriesScaled);
		}

		public static void Seconds(float seconds, ITimerCallback callback, int id = -1, object args = null, bool repeating = false)
		{
			if (repeating)
			{
				Exact(BetterTime.Time + seconds, seconds, callback, id, args);
			}
			else
			{
				Exact(BetterTime.Time + seconds, callback, id, args);
			}
		}

		public static void Minutes(float minutes, ITimerCallback callback, int id = -1, object args = null, bool repeating = false)
		{
			Seconds(minutes * 60f, callback, id, args, repeating);
		}

		public static void ExactUnscaled(float time, ITimerCallback callback, int id = -1, object args = null)
		{
			if (object.ReferenceEquals(callback, null))
			{
				throw new ArgumentNullException("callback");
			}
			Entry entry = new Entry(time, callback, id, args);
			InsertIntoList(entry, EntriesUnscaled);
		}

		public static void ExactUnscaled(float time, float repeatInterval, ITimerCallback callback, int id = -1, object args = null)
		{
			if (object.ReferenceEquals(callback, null))
			{
				throw new ArgumentNullException("callback");
			}
			Entry entry = new Entry(time, callback, id, args);
			entry.Repeating = true;
			entry.RepeatingInterval = repeatInterval;
			Entry entry2 = entry;
			InsertIntoList(entry2, EntriesUnscaled);
		}

		public static void SecondsUnscaled(float seconds, ITimerCallback callback, int id = -1, object args = null, bool repeating = false)
		{
			if (repeating)
			{
				ExactUnscaled(BetterTime.UnscaledTime + seconds, seconds, callback, id, args);
			}
			else
			{
				ExactUnscaled(BetterTime.UnscaledTime + seconds, callback, id, args);
			}
		}

		public static void MinutesUnscaled(float minutes, ITimerCallback callback, int id = -1, object args = null, bool repeating = false)
		{
			SecondsUnscaled(minutes * 60f, callback, id, args, repeating);
		}

		private static void CancelInternal(ITimerCallback callback, PooledLinkedList<Entry> list)
		{
			if (object.ReferenceEquals(callback, null))
			{
				throw new ArgumentNullException("callback");
			}
			LinkedListNode<Entry> linkedListNode = list.First;
			while (linkedListNode != null)
			{
				Entry value = linkedListNode.Value;
				if (value.Callback == callback)
				{
					LinkedListNode<Entry> next = linkedListNode.Next;
					list.Remove(linkedListNode);
					linkedListNode = next;
				}
				else
				{
					linkedListNode = linkedListNode.Next;
				}
			}
		}

		private static void CancelInternal(ITimerCallback callback, int id, PooledLinkedList<Entry> list)
		{
			if (object.ReferenceEquals(callback, null))
			{
				throw new ArgumentNullException("callback");
			}
			LinkedListNode<Entry> linkedListNode = list.First;
			while (linkedListNode != null)
			{
				Entry value = linkedListNode.Value;
				if (value.Callback == callback && value.Id == id)
				{
					LinkedListNode<Entry> next = linkedListNode.Next;
					list.Remove(linkedListNode);
					linkedListNode = next;
				}
				else
				{
					linkedListNode = linkedListNode.Next;
				}
			}
		}

		public static void CancelScaled(ITimerCallback callback)
		{
			CancelInternal(callback, EntriesScaled);
		}

		public static void CancelScaled(ITimerCallback callback, int id)
		{
			CancelInternal(callback, id, EntriesScaled);
		}

		public static void CancelUnscaled(ITimerCallback callback)
		{
			CancelInternal(callback, EntriesUnscaled);
		}

		public static void CancelUnscaled(ITimerCallback callback, int id)
		{
			CancelInternal(callback, id, EntriesUnscaled);
		}

		public static void CancelAll()
		{
			EntriesScaled.Clear();
			EntriesUnscaled.Clear();
		}

		private static void UpdateList(float time, PooledLinkedList<Entry> list)
		{
			LinkedListNode<Entry> first;
			while ((first = list.First) != null)
			{
				Entry value = first.Value;
				if (value.Time > time)
				{
					break;
				}
				value.Invoke();
				list.RemoveFirst();
				if (value.Repeating)
				{
					value.Time = time + value.RepeatingInterval + 1E-05f;
					InsertIntoList(value, list);
				}
			}
		}

		private static void InsertIntoList(Entry entry, PooledLinkedList<Entry> list)
		{
			for (LinkedListNode<Entry> linkedListNode = list.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				Entry value = linkedListNode.Value;
				if (value.Time > entry.Time)
				{
					list.AddBefore(linkedListNode, entry);
					return;
				}
			}
			list.AddLast(entry);
		}

		void IEventListener.OnEvent(int eventId)
		{
			if (eventId == -1000)
			{
				UpdateList(BetterTime.Time, EntriesScaled);
				UpdateList(BetterTime.UnscaledTime, EntriesUnscaled);
			}
		}
	}
}
