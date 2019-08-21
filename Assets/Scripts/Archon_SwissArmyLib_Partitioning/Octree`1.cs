using Archon.SwissArmyLib.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Partitioning
{
	public class Octree<T> : IPoolable, IDisposable
	{
		private struct ItemBounds
		{
			public readonly T Item;

			public readonly Bounds Bounds;

			public ItemBounds(T item, Bounds bounds)
			{
				Item = item;
				Bounds = bounds;
			}
		}

		[Flags]
		private enum SubNodes
		{
			None = 0x0,
			FrontTopLeft = 0x1,
			FrontTopRight = 0x2,
			FrontBottomLeft = 0x4,
			FrontBottomRight = 0x8,
			BackTopLeft = 0x10,
			BackTopRight = 0x20,
			BackBottomLeft = 0x40,
			BackBottomRight = 0x80,
			Front = 0xF,
			Back = -16,
			Top = 0x33,
			Bottom = -52,
			Left = 0x55,
			Right = -86,
			All = -1
		}

		private static readonly Pool<Octree<T>> Pool = new Pool<Octree<T>>(() => new Octree<T>());

		private readonly Octree<T>[] _subNodes = new Octree<T>[8];

		private readonly List<ItemBounds> _items = new List<ItemBounds>();

		public Bounds Bounds
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

		private Octree()
		{
		}

		public static Octree<T> Create(Bounds bounds, int maxItems, int maxDepth)
		{
			Octree<T> octree = Pool.Spawn();
			octree.Bounds = bounds;
			octree.MaxItems = maxItems;
			octree.MaxDepth = maxDepth;
			return octree;
		}

		private static Octree<T> CreateSubtree(Bounds bounds, int maxItems, int depth, int maxDepth)
		{
			Octree<T> octree = Create(bounds, maxItems, maxDepth);
			octree.Depth = depth;
			return octree;
		}

		public static void Destroy(Octree<T> tree)
		{
			Pool.Despawn(tree);
		}

		public void Insert(T item, Bounds bounds)
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

		public bool Remove(T item, Bounds bounds)
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

		public HashSet<T> Retrieve(Bounds bounds)
		{
			HashSet<T> hashSet = new HashSet<T>();
			Retrieve(bounds, hashSet);
			return hashSet;
		}

		public void Retrieve(Bounds bounds, HashSet<T> results)
		{
			if (IsSplit)
			{
				SubNodes containingNodes = GetContainingNodes(bounds);
				if (containingNodes != 0)
				{
					for (int i = 0; i < _subNodes.Length; i++)
					{
						int num = 1 << i;
						if (((byte)containingNodes & num) == num)
						{
							_subNodes[i].Retrieve(bounds, results);
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
			int depth = Depth + 1;
			Bounds bounds = Bounds;
			Vector3 b = bounds.size * 0.5f;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						Vector3 vector = bounds.min + new Vector3((float)k * b.x, (float)j * b.y, (float)i * b.z);
						Vector3 max = vector + b;
						Bounds bounds2 = default(Bounds);
						bounds2.SetMinMax(vector, max);
						_subNodes[i * 4 + j * 2 + k] = CreateSubtree(bounds2, MaxItems, depth, MaxDepth);
					}
				}
			}
			for (int num = _items.Count - 1; num >= 0; num--)
			{
				ItemBounds itemBounds = _items[num];
				SubNodes containingNodes = GetContainingNodes(itemBounds.Bounds);
				if (containingNodes != 0 && containingNodes != SubNodes.All)
				{
					for (int l = 0; l < _subNodes.Length; l++)
					{
						int num2 = 1 << l;
						if (((byte)containingNodes & num2) == num2)
						{
							_subNodes[l].Insert(itemBounds.Item, itemBounds.Bounds);
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
				Octree<T> octree = _subNodes[i];
				if (octree.IsSplit)
				{
					octree.Merge();
				}
				for (int j = 0; j < octree._items.Count; j++)
				{
					ItemBounds item = octree._items[j];
					if (!_items.Contains(item))
					{
						_items.Add(item);
					}
				}
				Destroy(octree);
				_subNodes[i] = null;
			}
		}

		private SubNodes GetContainingNodes(Bounds target)
		{
			SubNodes subNodes = SubNodes.All;
			Vector3 center = Bounds.center;
			Vector3 min = target.min;
			bool flag = min.z < center.z;
			Vector3 max = target.max;
			bool flag2 = max.z > center.z;
			Vector3 min2 = target.min;
			bool flag3 = min2.y < center.y;
			Vector3 max2 = target.max;
			bool flag4 = max2.y > center.y;
			Vector3 min3 = target.min;
			bool flag5 = min3.x < center.x;
			Vector3 max3 = target.max;
			bool flag6 = max3.x > center.x;
			if (!flag)
			{
				subNodes &= SubNodes.Back;
			}
			if (!flag2)
			{
				subNodes &= SubNodes.Front;
			}
			if (!flag3)
			{
				subNodes &= SubNodes.Top;
			}
			if (!flag4)
			{
				subNodes &= SubNodes.Bottom;
			}
			if (!flag5)
			{
				subNodes &= SubNodes.Right;
			}
			if (!flag6)
			{
				subNodes &= SubNodes.Left;
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
