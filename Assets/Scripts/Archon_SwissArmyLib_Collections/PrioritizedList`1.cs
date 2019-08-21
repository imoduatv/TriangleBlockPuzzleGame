using System;
using System.Collections;
using System.Collections.Generic;

namespace Archon.SwissArmyLib.Collections
{
	public class PrioritizedList<T> : IList<PrioritizedItem<T>>, IEnumerable, ICollection<PrioritizedItem<T>>, IEnumerable<PrioritizedItem<T>>
	{
		public readonly ListSortDirection SortDirection;

		private readonly List<PrioritizedItem<T>> _items;

		bool ICollection<PrioritizedItem<T>>.IsReadOnly => false;

		PrioritizedItem<T> IList<PrioritizedItem<T>>.this[int index]
		{
			get
			{
				return _items[index];
			}
			set
			{
				throw new NotImplementedException("Setting a specific index is not supported.");
			}
		}

		public int Count => _items.Count;

		public PrioritizedItem<T> this[int index] => _items[index];

		public PrioritizedList(ListSortDirection sortDirection = ListSortDirection.Ascending)
		{
			SortDirection = sortDirection;
			_items = new List<PrioritizedItem<T>>();
		}

		public PrioritizedList(int capacity, ListSortDirection sortDirection = ListSortDirection.Ascending)
		{
			SortDirection = sortDirection;
			_items = new List<PrioritizedItem<T>>(capacity);
		}

		public void Add(PrioritizedItem<T> item)
		{
			int count = _items.Count;
			if (count == 0)
			{
				_items.Add(item);
				return;
			}
			PrioritizedItem<T> prioritizedItem = _items[count - 1];
			int priority = prioritizedItem.Priority;
			if ((SortDirection == ListSortDirection.Ascending && item.Priority >= priority) || (SortDirection == ListSortDirection.Descending && item.Priority <= priority))
			{
				_items.Add(item);
				return;
			}
			int num = 0;
			while (true)
			{
				if (num >= count)
				{
					return;
				}
				if (SortDirection == ListSortDirection.Ascending)
				{
					int priority2 = item.Priority;
					PrioritizedItem<T> prioritizedItem2 = _items[num];
					if (priority2 < prioritizedItem2.Priority)
					{
						break;
					}
				}
				if (SortDirection == ListSortDirection.Descending)
				{
					int priority3 = item.Priority;
					PrioritizedItem<T> prioritizedItem3 = _items[num];
					if (priority3 > prioritizedItem3.Priority)
					{
						break;
					}
				}
				num++;
			}
			_items.Insert(num, item);
		}

		public void Add(T item)
		{
			Add(item, 0);
		}

		public void Add(T item, int priority)
		{
			PrioritizedItem<T> item2 = new PrioritizedItem<T>(item, priority);
			Add(item2);
		}

		public bool Remove(PrioritizedItem<T> item)
		{
			EqualityComparer<PrioritizedItem<T>> @default = EqualityComparer<PrioritizedItem<T>>.Default;
			int count = _items.Count;
			for (int i = 0; i < count; i++)
			{
				if (@default.Equals(item, _items[i]))
				{
					_items.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public bool Remove(T item)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			int count = _items.Count;
			for (int i = 0; i < count; i++)
			{
				EqualityComparer<T> equalityComparer = @default;
				PrioritizedItem<T> prioritizedItem = _items[i];
				if (equalityComparer.Equals(item, prioritizedItem.Item))
				{
					_items.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public void RemoveAt(int index)
		{
			_items.RemoveAt(index);
		}

		public void Clear()
		{
			_items.Clear();
		}

		public bool Contains(PrioritizedItem<T> item)
		{
			EqualityComparer<PrioritizedItem<T>> @default = EqualityComparer<PrioritizedItem<T>>.Default;
			int count = _items.Count;
			for (int i = 0; i < count; i++)
			{
				if (@default.Equals(item, _items[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(T item)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			int count = _items.Count;
			for (int i = 0; i < count; i++)
			{
				EqualityComparer<T> equalityComparer = @default;
				PrioritizedItem<T> prioritizedItem = _items[i];
				if (equalityComparer.Equals(item, prioritizedItem.Item))
				{
					return true;
				}
			}
			return false;
		}

		public void CopyTo(PrioritizedItem<T>[] array, int arrayIndex)
		{
			_items.CopyTo(array, arrayIndex);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			int num = array.Length;
			int count = _items.Count;
			for (int i = arrayIndex; i < num && i < count; i++)
			{
				int num2 = i;
				PrioritizedItem<T> prioritizedItem = _items[i];
				array[num2] = prioritizedItem.Item;
			}
		}

		public int IndexOf(PrioritizedItem<T> item)
		{
			EqualityComparer<PrioritizedItem<T>> @default = EqualityComparer<PrioritizedItem<T>>.Default;
			for (int i = 0; i < _items.Count; i++)
			{
				if (@default.Equals(item, _items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOf(T item)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < _items.Count; i++)
			{
				EqualityComparer<T> equalityComparer = @default;
				PrioritizedItem<T> prioritizedItem = _items[i];
				if (equalityComparer.Equals(item, prioritizedItem.Item))
				{
					return i;
				}
			}
			return -1;
		}

		void IList<PrioritizedItem<T>>.Insert(int index, PrioritizedItem<T> item)
		{
			throw new NotImplementedException("Inserting at a specific index is not supported.");
		}

		public IEnumerator<PrioritizedItem<T>> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
