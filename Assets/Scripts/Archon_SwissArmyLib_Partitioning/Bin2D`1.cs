using Archon.SwissArmyLib.Collections;
using Archon.SwissArmyLib.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Partitioning
{
	public class Bin2D<T> : IDisposable
	{
		private struct InternalBounds
		{
			public int MinX;

			public int MinY;

			public int MaxX;

			public int MaxY;
		}

		private static readonly Pool<LinkedListNode<T>> SharedNodePool = new Pool<LinkedListNode<T>>(() => new LinkedListNode<T>(default(T)));

		private static readonly Pool<PooledLinkedList<T>> ListPool = new Pool<PooledLinkedList<T>>(() => new PooledLinkedList<T>(SharedNodePool));

		private readonly Grid2D<PooledLinkedList<T>> _grid;

		private readonly Vector2 _bottomLeft;

		private readonly Vector2 _topRight;

		public int Width => _grid.Width;

		public int Height => _grid.Height;

		public float CellWidth
		{
			get;
			private set;
		}

		public float CellHeight
		{
			get;
			private set;
		}

		public Vector2 Origin => _bottomLeft;

		public IEnumerable<T> this[int x, int y] => _grid[x, y];

		public Bin2D(int gridWidth, int gridHeight, float cellWidth, float cellHeight)
			: this(gridWidth, gridHeight, cellWidth, cellHeight, Vector2.zero)
		{
		}

		public Bin2D(int gridWidth, int gridHeight, float cellWidth, float cellHeight, Vector2 origin)
		{
			_grid = new Grid2D<PooledLinkedList<T>>(gridWidth, gridHeight);
			CellWidth = cellWidth;
			CellHeight = cellHeight;
			_bottomLeft = origin;
			_topRight = new Vector2(origin.x + (float)gridWidth * cellWidth, origin.y + (float)gridHeight * cellHeight);
		}

		public void Insert(T item, Rect bounds)
		{
			if (IsOutOfBounds(bounds))
			{
				return;
			}
			InternalBounds internalBounds = GetInternalBounds(bounds);
			for (int i = internalBounds.MinY; i <= internalBounds.MaxY; i++)
			{
				for (int j = internalBounds.MinX; j <= internalBounds.MaxX; j++)
				{
					PooledLinkedList<T> pooledLinkedList = _grid[j, i];
					if (pooledLinkedList == null)
					{
						pooledLinkedList = (_grid[j, i] = ListPool.Spawn());
					}
					pooledLinkedList.AddLast(item);
				}
			}
		}

		public void Remove(T item)
		{
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					_grid[j, i]?.Remove(item);
				}
			}
		}

		public void Remove(T item, Rect bounds)
		{
			if (IsOutOfBounds(bounds))
			{
				return;
			}
			InternalBounds internalBounds = GetInternalBounds(bounds);
			for (int i = internalBounds.MinY; i <= internalBounds.MaxY; i++)
			{
				for (int j = internalBounds.MinX; j <= internalBounds.MaxX; j++)
				{
					PooledLinkedList<T> pooledLinkedList = _grid[j, i];
					if (pooledLinkedList != null)
					{
						pooledLinkedList.Remove(item);
						if (pooledLinkedList.Count == 0)
						{
							ListPool.Despawn(pooledLinkedList);
							_grid[j, i] = null;
						}
					}
				}
			}
		}

		public void Update(T item, Rect prevBounds, Rect newBounds)
		{
			Remove(item, prevBounds);
			Insert(item, newBounds);
		}

		public void Retrieve(Rect bounds, HashSet<T> results)
		{
			if (IsOutOfBounds(bounds))
			{
				return;
			}
			InternalBounds internalBounds = GetInternalBounds(bounds);
			for (int i = internalBounds.MinY; i <= internalBounds.MaxY; i++)
			{
				for (int j = internalBounds.MinX; j <= internalBounds.MaxX; j++)
				{
					PooledLinkedList<T> pooledLinkedList = _grid[j, i];
					if (pooledLinkedList != null)
					{
						for (LinkedListNode<T> linkedListNode = pooledLinkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
						{
							results.Add(linkedListNode.Value);
						}
					}
				}
			}
		}

		public void Clear()
		{
			for (int i = 0; i < _grid.Height; i++)
			{
				for (int j = 0; j < _grid.Width; j++)
				{
					PooledLinkedList<T> pooledLinkedList = _grid[j, i];
					if (pooledLinkedList != null)
					{
						pooledLinkedList.Clear();
						ListPool.Despawn(pooledLinkedList);
						_grid[j, i] = null;
					}
				}
			}
		}

		public void Dispose()
		{
			Clear();
		}

		private bool IsOutOfBounds(Rect bounds)
		{
			float xMax = bounds.xMax;
			Vector2 bottomLeft = _bottomLeft;
			int num;
			if (xMax > bottomLeft.x)
			{
				float xMin = bounds.xMin;
				Vector2 topRight = _topRight;
				if (xMin < topRight.x)
				{
					float yMax = bounds.yMax;
					Vector2 bottomLeft2 = _bottomLeft;
					if (yMax > bottomLeft2.y)
					{
						float yMin = bounds.yMin;
						Vector2 topRight2 = _topRight;
						num = ((yMin < topRight2.y) ? 1 : 0);
						goto IL_0068;
					}
				}
			}
			num = 0;
			goto IL_0068;
			IL_0068:
			return num == 0;
		}

		private InternalBounds GetInternalBounds(Rect bounds)
		{
			InternalBounds result = default(InternalBounds);
			float xMin = bounds.xMin;
			Vector2 bottomLeft = _bottomLeft;
			result.MinX = Mathf.Max(0, (int)((xMin - bottomLeft.x) / CellWidth));
			float yMin = bounds.yMin;
			Vector2 bottomLeft2 = _bottomLeft;
			result.MinY = Mathf.Max(0, (int)((yMin - bottomLeft2.y) / CellHeight));
			int a = Width - 1;
			float xMax = bounds.xMax;
			Vector2 bottomLeft3 = _bottomLeft;
			result.MaxX = Mathf.Min(a, (int)((xMax - bottomLeft3.x) / CellWidth));
			int a2 = Height - 1;
			float yMax = bounds.yMax;
			Vector2 bottomLeft4 = _bottomLeft;
			result.MaxY = Mathf.Min(a2, (int)((yMax - bottomLeft4.y) / CellHeight));
			return result;
		}
	}
}
