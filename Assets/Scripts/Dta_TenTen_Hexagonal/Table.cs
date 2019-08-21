using Dta.TenTen.Normal;
using System;
using System.Collections.Generic;

namespace Dta.TenTen.Hexagonal
{
	public class Table : Dta.TenTen.Normal.Table
	{
		public Table(int size)
			: base(size, size)
		{
			BOMB_TURN = 8;
			MIN_BOMB_TIME = 7;
			MAX_BOMB_TIME = 9;
		}

		public override void Reset()
		{
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeX; j++)
				{
					blocks[i, j] = null;
					isBombed[i, j] = false;
					bombTime[i, j] = 0;
				}
			}
			blocksNeedClear.Clear();
			int num = sizeX / 2;
			int num2 = sizeX / 2;
			for (int k = -num; k <= num; k++)
			{
				for (int l = -num2; l <= num2; l++)
				{
					if (k * l < 0 && (Math.Abs(k) + Math.Abs(l) > num || Math.Abs(k) + Math.Abs(l) > num2))
					{
						isBlackMap[k + num, l + num2] = true;
					}
					else
					{
						isBlackMap[k + num, l + num2] = false;
					}
				}
			}
		}

		public override int ClearLines()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			int num = 0;
			for (int i = 0; i < sizeX; i++)
			{
				if (IsRowLineDone(i))
				{
					list.Add(i);
				}
				if (IsColLineDone(i))
				{
					list2.Add(i);
				}
			}
			int num2 = sizeX / 2;
			for (int j = -num2; j <= num2; j++)
			{
				if (IsDivLineDone(j))
				{
					list3.Add(j);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				num += RemoveRowLine(list[k]);
			}
			for (int l = 0; l < list2.Count; l++)
			{
				num += RemoveColLine(list2[l]);
			}
			for (int m = 0; m < list3.Count; m++)
			{
				num += RemoveDivLine(list3[m]);
			}
			return num;
		}

		protected bool IsDivLineDone(int div)
		{
			bool result = true;
			for (int i = 0; i < sizeX; i++)
			{
				int num = i - div;
				if (i >= 0 && num >= 0 && i < sizeX && num < sizeX && !isBlackMap[i, num] && blocks[i, num] == null)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		protected int RemoveDivLine(int div)
		{
			int num = 0;
			for (int i = 0; i < sizeX; i++)
			{
				int num2 = i - div;
				if (i >= 0 && num2 >= 0 && i < sizeX && num2 < sizeX && !isBlackMap[i, num2])
				{
					if (isBombMode && isBombed[i, num2])
					{
						num += POINT_PER_BOMB;
						ClearBombAt(i, num2);
					}
					if (blocks[i, num2] != null)
					{
						num += POINT_PER_GRID;
						blocksNeedClear.Add(blocks[i, num2]);
						blocks[i, num2] = null;
					}
				}
			}
			return num;
		}
	}
}
