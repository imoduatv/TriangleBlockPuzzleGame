using Archon.SwissArmyLib.Collections;
using Archon.SwissArmyLib.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Partitioning
{
	public class Bin3D<T> : IDisposable
	{
		private struct InternalBounds
		{
			public int MinX;

			public int MinY;

			public int MinZ;

			public int MaxX;

			public int MaxY;

			public int MaxZ;
		}

		private static readonly Pool<LinkedListNode<T>> SharedNodePool = new Pool<LinkedListNode<T>>(() => new LinkedListNode<T>(default(T)));

		private static readonly Pool<PooledLinkedList<T>> ListPool = new Pool<PooledLinkedList<T>>(() => new PooledLinkedList<T>(SharedNodePool));

		private readonly Grid3D<PooledLinkedList<T>> _grid;

		private readonly Vector3 _bottomLeftFront;

		private readonly Vector3 _topRightBack;

		public int Width => _grid.Width;

		public int Height => _grid.Height;

		public int Depth => _grid.Depth;

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

		public float CellDepth
		{
			get;
			private set;
		}

		public Vector3 Origin => _bottomLeftFront;

		public IEnumerable<T> this[int x, int y, int z] => _grid[x, y, z];

		public Bin3D(int gridWidth, int gridHeight, int gridDepth, float cellWidth, float cellHeight, float cellDepth)
			: this(gridWidth, gridHeight, gridDepth, cellWidth, cellHeight, cellDepth, Vector3.zero)
		{
		}

		public Bin3D(int gridWidth, int gridHeight, int gridDepth, float cellWidth, float cellHeight, float cellDepth, Vector3 origin)
		{
			_grid = new Grid3D<PooledLinkedList<T>>(gridWidth, gridHeight, gridDepth);
			CellWidth = cellWidth;
			CellHeight = cellHeight;
			CellDepth = cellDepth;
			_bottomLeftFront = origin;
			_topRightBack = new Vector3(origin.x + (float)gridWidth * cellWidth, origin.y + (float)gridHeight * cellHeight, origin.z + (float)gridDepth * cellDepth);
		}

		public void Insert(T item, Bounds bounds)
		{
			if (IsOutOfBounds(bounds))
			{
				return;
			}
			InternalBounds internalBounds = GetInternalBounds(bounds);
			for (int i = internalBounds.MinZ; i <= internalBounds.MaxZ; i++)
			{
				for (int j = internalBounds.MinY; j <= internalBounds.MaxY; j++)
				{
					for (int k = internalBounds.MinX; k <= internalBounds.MaxX; k++)
					{
						PooledLinkedList<T> pooledLinkedList = _grid[k, j, i];
						if (pooledLinkedList == null)
						{
							pooledLinkedList = (_grid[k, j, i] = ListPool.Spawn());
						}
						pooledLinkedList.AddLast(item);
					}
				}
			}
		}

		public void Remove(T item, Bounds bounds)
		{
			if (IsOutOfBounds(bounds))
			{
				return;
			}
			InternalBounds internalBounds = GetInternalBounds(bounds);
			for (int i = internalBounds.MinZ; i <= internalBounds.MaxZ; i++)
			{
				for (int j = internalBounds.MinY; j <= internalBounds.MaxY; j++)
				{
					for (int k = internalBounds.MinX; k <= internalBounds.MaxX; k++)
					{
						PooledLinkedList<T> pooledLinkedList = _grid[k, j, i];
						if (pooledLinkedList != null)
						{
							pooledLinkedList.Remove(item);
							if (pooledLinkedList.Count == 0)
							{
								ListPool.Despawn(pooledLinkedList);
								_grid[k, j, i] = null;
							}
						}
					}
				}
			}
		}

		public void Remove(T item)
		{
			for (int i = 0; i < Depth; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					for (int k = 0; k < Width; k++)
					{
						_grid[k, j, i]?.Remove(item);
					}
				}
			}
		}

		public void Update(T item, Bounds prevBounds, Bounds newBounds)
		{
			Remove(item, prevBounds);
			Insert(item, newBounds);
		}

		public void Retrieve(Bounds bounds, HashSet<T> results)
		{
			if (IsOutOfBounds(bounds))
			{
				return;
			}
			InternalBounds internalBounds = GetInternalBounds(bounds);
			for (int i = internalBounds.MinZ; i <= internalBounds.MaxZ; i++)
			{
				for (int j = internalBounds.MinY; j <= internalBounds.MaxY; j++)
				{
					for (int k = internalBounds.MinX; k <= internalBounds.MaxX; k++)
					{
						PooledLinkedList<T> pooledLinkedList = _grid[k, j, i];
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
		}

		public void Clear()
		{
			for (int i = 0; i < _grid.Depth; i++)
			{
				for (int j = 0; j < _grid.Height; j++)
				{
					for (int k = 0; k < _grid.Width; k++)
					{
						PooledLinkedList<T> pooledLinkedList = _grid[k, j, i];
						if (pooledLinkedList != null)
						{
							pooledLinkedList.Clear();
							ListPool.Despawn(pooledLinkedList);
							_grid[k, j, i] = null;
						}
					}
				}
			}
		}

		public void Dispose()
		{
			Clear();
		}

		private bool IsOutOfBounds(Bounds bounds)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			float x = max.x;
			Vector3 bottomLeftFront = _bottomLeftFront;
			int num;
			if (x > bottomLeftFront.x)
			{
				float x2 = min.x;
				Vector3 topRightBack = _topRightBack;
				if (x2 < topRightBack.x)
				{
					float y = max.y;
					Vector3 bottomLeftFront2 = _bottomLeftFront;
					if (y > bottomLeftFront2.y)
					{
						float y2 = min.y;
						Vector3 topRightBack2 = _topRightBack;
						if (y2 < topRightBack2.y)
						{
							float z = max.z;
							Vector3 bottomLeftFront3 = _bottomLeftFront;
							if (z > bottomLeftFront3.z)
							{
								float z2 = min.z;
								Vector3 topRightBack3 = _topRightBack;
								num = ((z2 < topRightBack3.z) ? 1 : 0);
								goto IL_00b0;
							}
						}
					}
				}
			}
			num = 0;
			goto IL_00b0;
			IL_00b0:
			return num == 0;
		}

		private InternalBounds GetInternalBounds(Bounds bounds)
		{
			Vector3 vector = bounds.min - _bottomLeftFront;
			Vector3 vector2 = bounds.max - _bottomLeftFront;
			InternalBounds result = default(InternalBounds);
			result.MinX = Mathf.Max(0, (int)(vector.x / CellWidth));
			result.MinY = Mathf.Max(0, (int)(vector.y / CellHeight));
			result.MinZ = Mathf.Max(0, (int)(vector.z / CellDepth));
			result.MaxX = Mathf.Min(Width - 1, (int)(vector2.x / CellWidth));
			result.MaxY = Mathf.Min(Height - 1, (int)(vector2.y / CellHeight));
			result.MaxZ = Mathf.Min(Depth - 1, (int)(vector2.z / CellDepth));
			return result;
		}
	}
}
