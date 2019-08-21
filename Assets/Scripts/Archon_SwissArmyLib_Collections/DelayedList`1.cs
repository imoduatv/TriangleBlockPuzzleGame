using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Archon.SwissArmyLib.Collections
{
	public class DelayedList<T> : IList<T>, IEnumerable, ICollection<T>, IEnumerable<T>
	{
		private enum Action
		{
			Add,
			Remove,
			Clear
		}

		private struct PendingChange
		{
			public readonly Action Action;

			public readonly T Value;

			public PendingChange(Action action)
			{
				this = default(PendingChange);
				Action = action;
			}

			public PendingChange(Action action, T value)
			{
				this = new PendingChange(action);
				Value = value;
			}
		}

		private readonly IList<T> _items;

		private readonly Queue<PendingChange> _changeQueue;

		private readonly ReadOnlyCollection<T> _readonlyCollection;

		bool ICollection<T>.IsReadOnly => false;

		T IList<T>.this[int index]
		{
			get
			{
				return _items[index];
			}
			set
			{
				throw new NotSupportedException("Inserting at a specific index is not supported.");
			}
		}

		public int Count => _items.Count;

		public ReadOnlyCollection<T> BackingList => _readonlyCollection;

		public T this[int index] => _items[index];

		public DelayedList()
			: this((IList<T>)new List<T>())
		{
		}

		public DelayedList(int capacity, int changeCapacity)
			: this((IList<T>)new List<T>(capacity), changeCapacity)
		{
		}

		public DelayedList(IList<T> list)
		{
			_items = list;
			_readonlyCollection = new ReadOnlyCollection<T>(_items);
			_changeQueue = new Queue<PendingChange>();
		}

		public DelayedList(IList<T> list, int changeCapacity)
		{
			_items = list;
			_readonlyCollection = new ReadOnlyCollection<T>(_items);
			_changeQueue = new Queue<PendingChange>(changeCapacity);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(T item)
		{
			_changeQueue.Enqueue(new PendingChange(Action.Add, item));
		}

		public void AddRange(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				Add(item);
			}
		}

		public bool Remove(T item)
		{
			_changeQueue.Enqueue(new PendingChange(Action.Remove, item));
			return true;
		}

		public void Clear()
		{
			ClearPending();
			_changeQueue.Enqueue(new PendingChange(Action.Clear));
		}

		public void ClearInstantly()
		{
			_items.Clear();
			_changeQueue.Clear();
		}

		public void ClearPending()
		{
			_changeQueue.Clear();
		}

		public bool Contains(T item)
		{
			return _items.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_items.CopyTo(array, arrayIndex);
		}

		public int IndexOf(T item)
		{
			return _items.IndexOf(item);
		}

		void IList<T>.Insert(int index, T item)
		{
			throw new NotSupportedException("Inserting at a specific index is not supported.");
		}

		public void RemoveAt(int index)
		{
			Remove(_items[index]);
		}

		public void ProcessPending()
		{
			int count = _changeQueue.Count;
			for (int i = 0; i < count; i++)
			{
				PendingChange pendingChange = _changeQueue.Dequeue();
				switch (pendingChange.Action)
				{
				case Action.Add:
					_items.Add(pendingChange.Value);
					break;
				case Action.Remove:
					_items.Remove(pendingChange.Value);
					break;
				case Action.Clear:
					_items.Clear();
					break;
				}
			}
		}
	}
}
