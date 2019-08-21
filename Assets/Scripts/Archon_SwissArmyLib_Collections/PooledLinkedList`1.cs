using Archon.SwissArmyLib.Pooling;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Archon.SwissArmyLib.Collections
{
	public class PooledLinkedList<T> : ICollection<T>, IEnumerable, IEnumerable<T>
	{
		private readonly LinkedList<T> _list;

		private readonly IPool<LinkedListNode<T>> _pool;

		bool ICollection<T>.IsReadOnly => false;

		public LinkedList<T> BackingList => _list;

		public IPool<LinkedListNode<T>> Pool => _pool;

		public LinkedListNode<T> First => _list.First;

		public LinkedListNode<T> Last => _list.Last;

		public int Count => _list.Count;

		public PooledLinkedList()
			: this((IPool<LinkedListNode<T>>)new Pool<LinkedListNode<T>>(() => new LinkedListNode<T>(default(T))))
		{
		}

		public PooledLinkedList(IPool<LinkedListNode<T>> nodePool)
		{
			if (nodePool == null)
			{
				throw new ArgumentNullException("nodePool");
			}
			_list = new LinkedList<T>();
			_pool = nodePool;
		}

		public PooledLinkedList(IEnumerable<T> collection)
			: this(collection, (IPool<LinkedListNode<T>>)new Pool<LinkedListNode<T>>(() => new LinkedListNode<T>(default(T))))
		{
		}

		public PooledLinkedList(IEnumerable<T> collection, IPool<LinkedListNode<T>> nodePool)
			: this(nodePool)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (T item in collection)
			{
				AddLast(item);
			}
		}

		public void Clear()
		{
			LinkedListNode<T> linkedListNode = First;
			while (linkedListNode != null)
			{
				LinkedListNode<T> target = linkedListNode;
				linkedListNode = linkedListNode.Next;
				_pool.Despawn(target);
			}
			_list.Clear();
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			LinkedListNode<T> linkedListNode = _list.Find(item);
			if (linkedListNode != null)
			{
				_list.Remove(linkedListNode);
				_pool.Despawn(linkedListNode);
				return true;
			}
			return false;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_list).GetEnumerator();
		}

		void ICollection<T>.Add(T item)
		{
			AddLast(item);
		}

		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			LinkedListNode<T> linkedListNode = _pool.Spawn();
			linkedListNode.Value = value;
			AddAfter(node, linkedListNode);
			return linkedListNode;
		}

		public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			_list.AddAfter(node, newNode);
		}

		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			LinkedListNode<T> linkedListNode = _pool.Spawn();
			linkedListNode.Value = value;
			AddBefore(node, linkedListNode);
			return linkedListNode;
		}

		public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			_list.AddBefore(node, newNode);
		}

		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> linkedListNode = _pool.Spawn();
			linkedListNode.Value = value;
			AddFirst(linkedListNode);
			return linkedListNode;
		}

		public void AddFirst(LinkedListNode<T> node)
		{
			_list.AddFirst(node);
		}

		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> linkedListNode = _pool.Spawn();
			linkedListNode.Value = value;
			AddLast(linkedListNode);
			return linkedListNode;
		}

		public void AddLast(LinkedListNode<T> node)
		{
			_list.AddLast(node);
		}

		public LinkedListNode<T> Find(T value)
		{
			return _list.Find(value);
		}

		public LinkedListNode<T> FindLast(T value)
		{
			return _list.FindLast(value);
		}

		public void Remove(LinkedListNode<T> node)
		{
			_list.Remove(node);
			_pool.Despawn(node);
		}

		public void RemoveFirst()
		{
			LinkedListNode<T> first = First;
			_list.RemoveFirst();
			_pool.Despawn(first);
		}

		public void RemoveLast()
		{
			LinkedListNode<T> last = Last;
			_list.RemoveLast();
			_pool.Despawn(last);
		}
	}
}
