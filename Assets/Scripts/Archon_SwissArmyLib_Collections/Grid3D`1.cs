using System;

namespace Archon.SwissArmyLib.Collections
{
	public class Grid3D<T>
	{
		private T[][][] _data;

		public int Width
		{
			get;
			private set;
		}

		public int Height
		{
			get;
			private set;
		}

		public int Depth
		{
			get;
			private set;
		}

		public T DefaultValue
		{
			get;
			set;
		}

		public T this[int x, int y, int z]
		{
			get
			{
				if (x < 0 || y < 0 || z < 0 || x >= Width || y >= Height || z >= Depth)
				{
					throw new IndexOutOfRangeException();
				}
				return _data[x][y][z];
			}
			set
			{
				if (x < 0 || y < 0 || z < 0 || x >= Width || y >= Height || z >= Depth)
				{
					throw new IndexOutOfRangeException();
				}
				_data[x][y][z] = value;
			}
		}

		private int InternalWidth => _data.Length;

		private int InternalHeight => _data[0].Length;

		private int InternalDepth => _data[0][0].Length;

		public Grid3D(int width, int height, int depth)
		{
			Width = width;
			Height = height;
			Depth = depth;
			_data = CreateArrays(width, height, depth);
		}

		public Grid3D(int width, int height, int depth, T defaultValue)
			: this(width, height, depth)
		{
			DefaultValue = defaultValue;
			Clear();
		}

		public T Get(int x, int y, int z)
		{
			return this[x, y, z];
		}

		public void Set(int x, int y, int z, T value)
		{
			this[x, y, z] = value;
		}

		public void Clear()
		{
			Clear(DefaultValue);
		}

		public void Clear(T clearValue)
		{
			Fill(clearValue, 0, 0, 0, Width - 1, Height - 1, Depth - 1);
		}

		public void Fill(T value, int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
		{
			for (int i = minX; i <= maxX; i++)
			{
				T[][] array = _data[i];
				for (int j = minY; j <= maxY; j++)
				{
					T[] array2 = array[j];
					for (int k = minZ; k <= maxZ; k++)
					{
						array2[k] = value;
					}
				}
			}
		}

		public void Resize(int width, int height, int depth)
		{
			int width2 = Width;
			int height2 = Height;
			int depth2 = Depth;
			if (width > InternalWidth || height > InternalHeight || depth > InternalDepth)
			{
				T[][][] data = _data;
				_data = CreateArrays(width, height, depth);
				CopyArraysContents(data, _data);
			}
			Width = width;
			Height = height;
			Depth = depth;
			if (width > width2 || height > height2 || depth > depth2)
			{
				Fill(DefaultValue, width2, 0, 0, width - 1, height - 1, depth - 1);
				Fill(DefaultValue, 0, height2, 0, width2 - 1, height - 1, depth - 1);
				Fill(DefaultValue, 0, 0, depth2, width2 - 1, height2 - 1, depth - 1);
			}
		}

		private static T[][][] CreateArrays(int width, int height, int depth)
		{
			T[][][] array = new T[width][][];
			for (int i = 0; i < width; i++)
			{
				T[][] array2 = new T[height][];
				for (int j = 0; j < height; j++)
				{
					array2[j] = new T[depth];
				}
				array[i] = array2;
			}
			return array;
		}

		private static void CopyArraysContents(T[][][] src, T[][][] dst)
		{
			int num = src.Length;
			int num2 = src[0].Length;
			int num3 = src[0][0].Length;
			int num4 = dst.Length;
			int num5 = dst[0].Length;
			int num6 = dst[0][0].Length;
			for (int i = 0; i < num && i < num4; i++)
			{
				T[][] array = src[i];
				T[][] array2 = dst[i];
				for (int j = 0; j < num2 && j < num5; j++)
				{
					T[] array3 = array[j];
					T[] array4 = array2[j];
					for (int k = 0; k < num3 && k < num6; k++)
					{
						array4[k] = array3[k];
					}
				}
			}
		}
	}
}
