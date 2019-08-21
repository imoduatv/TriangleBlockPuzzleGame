using Archon.SwissArmyLib.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Partitioning
{
	public class Quadtree<T> : IPoolable, IDisposable
	{
		private struct ItemBounds
		{
			public readonly T Item;

			public readonly Rect Bounds;

			public ItemBounds(T item, Rect bounds)
			{
				Item = item;
				Bounds = bounds;
			}
		}

		[Flags]
		private enum SubNodes
		{
			None = 0x0,
			TopLeft = 0x1,
			TopRight = 0x2,
			BottomLeft = 0x4,
			BottomRight = 0x8,
			All = 0xF
		}

		private static readonly Pool<Quadtree<T>> Pool = new Pool<Quadtree<T>>(() => new Quadtree<T>());

		private readonly Quadtree<T>[] _subNodes = new Quadtree<T>[4];

		private readonly List<ItemBounds> _items = new List<ItemBounds>();

		public Rect Bounds
		{
			get;
			private set;
		}

		public int Depth
		{
			get;
			private set;
		}

		public int MaxDepth
		{
			get;
			private set;
		}

		public int MaxItems
		{
			get;
			private set;
		}

		public bool IsSplit => _subNodes[0] != null;

		public int Count
		{
			get;
			private set;
		}

		private Quadtree()
		{
		}

		public static Quadtree<T> Create(Rect bounds, int maxItems, int maxDepth)
		{
			Quadtree<T> quadtree = Pool.Spawn();
			quadtree.Bounds = bounds;
			quadtree.MaxItems = maxItems;
			quadtree.MaxDepth = maxDepth;
			return quadtree;
		}

		private static Quadtree<T> CreateSubtree(Rect bounds, int maxItems, int depth, int maxDepth)
		{
			Quadtree<T> quadtree = Create(bounds, maxItems, maxDepth);
			quadtree.Depth = depth;
			return quadtree;
		}

		public static void Destroy(Quadtree<T> tree)
		{
			Pool.Despawn(tree);
		}

		public void Insert(T item, Rect bounds)
		{
			Count++;
			if (IsSplit)
			{
				SubNodes containingNodes = GetContainingNodes(bounds);
				if (containingNodes != 0 && containingNodes != SubNodes.All)
				{
					for (int i = 0; i < _subNodes.Length; i++)
					{
						int num = 1 << i;
						if (((byte)containingNodes & num) == num)
						{
							_subNodes[i].Insert(item, bounds);
						}
					}
					return;
				}
			}
			_items.Add(new ItemBounds(item, bounds));
			if (!IsSplit && Depth < MaxDepth && _items.Count > MaxItems)
			{
				Split();
			}
		}

		public bool Remove(T item, Rect bounds)
		{
			if (IsSplit)
			{
				SubNodes containingNodes = GetContainingNodes(bounds);
				if (containingNodes != 0 && containingNodes != SubNodes.All)
				{
					bool flag = false;
					for (int i = 0; i < _subNodes.Length; i++)
					{
						int num = 1 << i;
						if (((byte)containingNodes & num) == num)
						{
							flag |= _subNodes[i].Remove(item, bounds);
						}
					}
					if (flag)
					{
						Count--;
						if (Count <= MaxItems)
						{
							Merge();
						}
					}
					return flag;
				}
			}
			for (int j = 0; j < _items.Count; j++)
			{
				ItemBounds itemBounds = _items[j];
				if (itemBounds.Item.Equals(item))
				{
					_items.RemoveAt(j);
					Count--;
					return true;
				}
			}
			return false;
		}

		public HashSet<T> Retrieve(Rect rect)
		{
			HashSet<T> hashSet = new HashSet<T>();
			Retrieve(rect, hashSet);
			return hashSet;
		}

		public void Retrieve(Rect rect, HashSet<T> results)
		{
			if (IsSplit)
			{
				SubNodes containingNodes = GetContainingNodes(rect);
				if (containingNodes != 0)
				{
					for (int i = 0; i < _subNodes.Length; i++)
					{
						int num = 1 << i;
						if (((byte)containingNodes & num) == num)
						{
							_subNodes[i].Retrieve(rect, results);
						}
					}
				}
			}
			for (int j = 0; j < _items.Count; j++)
			{
				ItemBounds itemBounds = _items[j];
				results.Add(itemBounds.Item);
			}
		}

		public void Clear()
		{
			_items.Clear();
			Count = 0;
			if (IsSplit)
			{
				for (int i = 0; i < _subNodes.Length; i++)
				{
					Destroy(_subNodes[i]);
					_subNodes[i] = null;
				}
			}
		}

		public void Dispose()
		{
			Destroy(this);
		}

		private void Split()
		{
			Rect bounds = Bounds;
			Vector2 size = bounds.size * 0.5f;
			float xMin = bounds.xMin;
			Vector2 center = bounds.center;
			Rect bounds2 = new Rect(xMin, center.y, size.x, size.y);
			Rect bounds3 = new Rect(bounds.center, size);
			Rect bounds4 = new Rect(bounds.min, size);
			Vector2 center2 = bounds.center;
			Rect bounds5 = new Rect(center2.x, bounds.yMin, size.x, size.y);
			int depth = Depth + 1;
			_subNodes[0] = CreateSubtree(bounds2, MaxItems, depth, MaxDepth);
			_subNodes[1] = CreateSubtree(bounds3, MaxItems, depth, MaxDepth);
			_subNodes[2] = CreateSubtree(bounds4, MaxItems, depth, MaxDepth);
			_subNodes[3] = CreateSubtree(bounds5, MaxItems, depth, MaxDepth);
			for (int num = _items.Count - 1; num >= 0; num--)
			{
				ItemBounds itemBounds = _items[num];
				SubNodes containingNodes = GetContainingNodes(itemBounds.Bounds);
				if (containingNodes != 0 && containingNodes != SubNodes.All)
				{
					for (int i = 0; i < _subNodes.Length; i++)
					{
						int num2 = 1 << i;
						if (((byte)containingNodes & num2) == num2)
						{
							_subNodes[i].Insert(itemBounds.Item, itemBounds.Bounds);
						}
					}
					_items.RemoveAt(num);
				}
			}
		}

		private void Merge()
		{
			for (int i = 0; i < _subNodes.Length; i++)
			{
				Quadtree<T> quadtree = _subNodes[i];
				if (quadtree.IsSplit)
				{
					quadtree.Merge();
				}
				for (int j = 0; j < quadtree._items.Count; j++)
				{
					ItemBounds item = quadtree._items[j];
					if (!_items.Contains(item))
					{
						_items.Add(item);
					}
				}
				Destroy(quadtree);
				_subNodes[i] = null;
			}
		}

		private SubNodes GetContainingNodes(Rect target)
		{
			SubNodes subNodes = SubNodes.None;
			Vector2 center = Bounds.center;
			bool flag = target.xMin < center.x;
			bool flag2 = target.xMax > center.x;
			bool flag3 = target.yMin < center.y;
			bool flag4 = target.yMax > center.y;
			if (flag)
			{
				if (flag4)
				{
					subNodes |= SubNodes.TopLeft;
				}
				if (flag3)
				{
					subNodes |= SubNodes.BottomLeft;
				}
			}
			if (flag2)
			{
				if (flag4)
				{
					subNodes |= SubNodes.TopRight;
				}
				if (flag3)
				{
					subNodes |= SubNodes.BottomRight;
				}
			}
			return subNodes;
		}

		void IPoolable.OnSpawned()
		{
		}

		void IPoolable.OnDespawned()
		{
			Clear();
			Depth = 0;
		}
	}
}
