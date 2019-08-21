using System;

namespace Archon.SwissArmyLib.Collections
{
	public class Grid2D<T>
	{
		private T[][] _data;

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

		public T DefaultValue
		{
			get;
			set;
		}

		public T this[int x, int y]
		{
			get
			{
				if (x < 0 || y < 0 || x >= Width || y >= Height)
				{
					throw new IndexOutOfRangeException();
				}
				return _data[x][y];
			}
			set
			{
				if (x < 0 || y < 0 || x >= Width || y >= Height)
				{
					throw new IndexOutOfRangeException();
				}
				_data[x][y] = value;
			}
		}

		private int InternalWidth => _data.Length;

		private int InternalHeight => _data[0].Length;

		public Grid2D(int width, int height)
		{
			Width = width;
			Height = height;
			_data = CreateArrays(width, height);
		}

		public Grid2D(int width, int height, T defaultValue)
			: this(width, height)
		{
			DefaultValue = defaultValue;
			Clear();
		}

		public T Get(int x, int y)
		{
			return this[x, y];
		}

		public void Set(int x, int y, T value)
		{
			this[x, y] = value;
		}

		public void Clear()
		{
			Clear(DefaultValue);
		}

		public void Clear(T clearValue)
		{
			Fill(clearValue, 0, 0, Width - 1, Height - 1);
		}

		public void Fill(T value, int minX, int minY, int maxX, int maxY)
		{
			for (int i = minX; i <= maxX; i++)
			{
				T[] array = _data[i];
				for (int j = minY; j <= maxY; j++)
				{
					array[j] = value;
				}
			}
		}

		public void Resize(int width, int height)
		{
			int width2 = Width;
			int height2 = Height;
			if (width > InternalWidth || height > InternalHeight)
			{
				T[][] data = _data;
				_data = CreateArrays(width, height);
				CopyArraysContents(data, _data);
			}
			Width = width;
			Height = height;
			if (width > width2 || height > height2)
			{
				Fill(DefaultValue, 0, height2, width - 1, height - 1);
				Fill(DefaultValue, width2, 0, width - 1, height2 - 1);
			}
		}

		private static T[][] CreateArrays(int width, int height)
		{
			T[][] array = new T[width][];
			for (int i = 0; i < width; i++)
			{
				array[i] = new T[height];
			}
			return array;
		}

		private static void CopyArraysContents(T[][] src, T[][] dst)
		{
			int num = src.Length;
			int num2 = src[0].Length;
			int num3 = dst.Length;
			int num4 = dst[0].Length;
			for (int i = 0; i < num && i < num3; i++)
			{
				T[] array = src[i];
				T[] array2 = dst[i];
				for (int j = 0; j < num2 && j < num4; j++)
				{
					array2[j] = array[j];
				}
			}
		}
	}
}
