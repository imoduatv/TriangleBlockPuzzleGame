using Dta.TenTen.Normal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dta.TenTen.Triangle
{
	public class Table : Dta.TenTen.Normal.Table
	{
		private int triRow;

		private int triCol;

		private int halfTriRows;

		private int halfTriCols;

		private float[,] timeDelayAnim;

		private Dictionary<int, int> dictIntersect;

		private Dictionary<int, List<int>> dictRowClear;

		private Dictionary<int, List<int>> dictRowsAllBlockClear;

		private Dictionary<int, List<int>> dictColsAllBlockClear;

		private Dictionary<int, List<int>> dictSumsAllBlockClear;

		private Dictionary<int, List<int>> dictColClear;

		private Dictionary<int, List<int>> dictSumClear;

		private List<int> blockStartAnim;

		private int numColClear;

		private List<List<GridCell>> allLines = new List<List<GridCell>>();

		private List<GridCell> listCellsCanClear = new List<GridCell>();

		public Table(int row, int col)
			: base(row, col)
		{
			triRow = row;
			triCol = col;
			halfTriRows = triRow / 2;
			halfTriCols = triCol / 2;
			BOMB_TURN = 9;
			MIN_BOMB_TIME = 8;
			MAX_BOMB_TIME = 9;
			dictIntersect = new Dictionary<int, int>();
			dictSumClear = new Dictionary<int, List<int>>();
			dictRowClear = new Dictionary<int, List<int>>();
			dictColClear = new Dictionary<int, List<int>>();
			dictColsAllBlockClear = new Dictionary<int, List<int>>();
			dictRowsAllBlockClear = new Dictionary<int, List<int>>();
			dictSumsAllBlockClear = new Dictionary<int, List<int>>();
			blockStartAnim = new List<int>();
			timeDelayAnim = new float[sizeX, sizeY];
		}

		public override void Reset()
		{
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					blocks[i, j] = null;
					isBombed[i, j] = false;
					bombTime[i, j] = 0;
				}
			}
			blocksNeedClear.Clear();
			for (int k = 0; k < sizeX; k++)
			{
				for (int l = 0; l < sizeY; l++)
				{
					isBlackMap[k, l] = true;
				}
			}
			int num = -halfTriRows;
			int num2 = halfTriRows - 1;
			int num3 = -halfTriCols;
			int num4 = halfTriCols - 1;
			for (int m = num; m <= num2; m++)
			{
				for (int n = num3; n <= num4; n++)
				{
					int num5 = n + halfTriCols;
					int num6 = m + halfTriRows;
					if ((m * n > 0 && ((m > 0 && 2 * m + n <= num4 - 1) || (m < 0 && 2 * Math.Abs(m) + Math.Abs(n) <= halfTriCols + 1))) || (m * n <= 0 && m + n < num4))
					{
						isBlackMap[num6, num5] = false;
					}
					else
					{
						isBlackMap[num6, num5] = true;
					}
				}
			}
			CreateLines();
		}

		public void SetTableTutorial1()
		{
			for (int i = 0; i < triCol; i++)
			{
				isBlackMap[halfTriRows - 1, i] = (false || isBlackMap[halfTriRows - 1, i]);
				isBlackMap[halfTriRows, i] = (false || isBlackMap[halfTriRows, i]);
				if (i != halfTriCols - 1 && i != halfTriCols - 2)
				{
					blocks[halfTriRows - 1, i] = new Block(1, halfTriRows - 1, i);
					blocks[halfTriRows, i] = new Block(1, halfTriRows, i);
				}
			}
			for (int j = 0; j < sizeX; j++)
			{
				for (int k = 0; k < sizeY; k++)
				{
					if (j != halfTriRows - 1 && j != halfTriRows)
					{
						isBlackMap[j, k] = true;
					}
				}
			}
		}

		public void SetTableTutorial2()
		{
			for (int i = 0; i < triRow; i++)
			{
				isBlackMap[i, halfTriCols - 1] = (false || isBlackMap[i, halfTriCols - 1]);
				isBlackMap[i, halfTriCols - 2] = (false || isBlackMap[i, halfTriCols - 2]);
				if (i != halfTriRows - 1 && i != halfTriRows - 2)
				{
					blocks[i, halfTriCols - 1] = new Block(1, i, halfTriCols - 1);
					blocks[i, halfTriCols - 2] = new Block(1, i, halfTriCols - 2);
				}
			}
			for (int j = 0; j < sizeX; j++)
			{
				for (int k = 0; k < sizeY; k++)
				{
					if (k != halfTriCols - 1 && k != halfTriCols - 2)
					{
						isBlackMap[j, k] = true;
					}
				}
			}
		}

		public void SetTableTutorial3()
		{
			for (int i = 0; i < triCol; i++)
			{
				if (i != halfTriCols - 1 && i != halfTriCols - 2 && i != halfTriCols)
				{
					blocks[halfTriRows - 1, i] = new Block(1, halfTriRows - 1, i);
				}
			}
			for (int j = 0; j < triRow; j++)
			{
				if (j != halfTriRows - 1)
				{
					if (j == halfTriRows)
					{
						blocks[j, halfTriCols - 1] = new Block(1, j, halfTriCols - 1);
						continue;
					}
					blocks[j, halfTriCols - 1] = new Block(1, j, halfTriCols - 1);
					blocks[j, halfTriCols - 2] = new Block(1, j, halfTriCols - 2);
				}
			}
		}

		public override bool IsPlaceable(Shape shape)
		{
			int num = shape.firstGrid.y % 2;
			for (int i = 0; i < sizeY; i++)
			{
				for (int j = 0; j < sizeX; j++)
				{
					if (!isBlackMap[j, i] && blocks[j, i] == null && i % 2 == num)
					{
						int posX = j - shape.firstGrid.x;
						int posY = i - shape.firstGrid.y;
						if (!IsOccupied(shape, posX, posY))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool IsCanClearLine(Shape shape, int X, int Y)
		{
			AddToBlocksTemp(shape, X, Y);
			bool result = IsHaveLineClear();
			UndoAddToBlocksTemp(shape, X, Y);
			return result;
		}

		public bool IsCanClearLine(Shape shape)
		{
			int num = shape.firstGrid.y % 2;
			for (int i = 0; i < sizeY; i++)
			{
				for (int j = 0; j < sizeX; j++)
				{
					if (!isBlackMap[j, i] && blocks[j, i] == null && i % 2 == num)
					{
						int num2 = j - shape.firstGrid.x;
						int num3 = i - shape.firstGrid.y;
						if (!IsOccupied(shape, num2, num3))
						{
							return IsCanClearLine(shape, num2, num3);
						}
					}
				}
			}
			return false;
		}

		public List<GridCell> GetCellsCanClear()
		{
			return listCellsCanClear;
		}

		private bool IsHaveLineClear()
		{
			listCellsCanClear.Clear();
			int count = allLines.Count;
			bool result = false;
			for (int i = 0; i < count; i++)
			{
				bool flag = true;
				int count2 = allLines[i].Count;
				for (int j = 0; j < count2; j++)
				{
					int x = allLines[i][j].x;
					int y = allLines[i][j].y;
					if (blocks[x, y] == null)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					result = true;
					for (int k = 0; k < count2; k++)
					{
						listCellsCanClear.Add(allLines[i][k]);
					}
				}
			}
			return result;
		}

		private void CreateLines()
		{
			if (allLines.Count > 0)
			{
				return;
			}
			for (int i = 0; i < triRow; i++)
			{
				List<GridCell> list = CreateRowLine(i);
				if (list.Count > 0)
				{
					allLines.Add(list);
				}
			}
			for (int j = 0; j < triCol; j += 2)
			{
				List<GridCell> list2 = CreateColLine(j);
				if (list2.Count > 0)
				{
					allLines.Add(list2);
				}
			}
			int num = halfTriCols;
			int num2 = (halfTriCols - 1) * 2 + num;
			for (int k = num; k <= num2; k += 2)
			{
				List<GridCell> list3 = CreateTriSumLine(k);
				if (list3.Count > 0)
				{
					allLines.Add(list3);
				}
			}
		}

		public override int ClearLines()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			blockStartAnim.Clear();
			dictIntersect.Clear();
			dictRowClear.Clear();
			dictColClear.Clear();
			dictSumClear.Clear();
			dictSumsAllBlockClear.Clear();
			dictRowsAllBlockClear.Clear();
			dictColsAllBlockClear.Clear();
			numColClear = 0;
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					timeDelayAnim[i, j] = -1f;
				}
			}
			int num = 0;
			for (int k = 0; k < triRow; k++)
			{
				if (IsTriRowLineDone(k))
				{
					numColClear++;
					list.Add(k);
				}
			}
			for (int l = 0; l < triCol; l += 2)
			{
				if (IsTriColLineDone(l))
				{
					numColClear++;
					list2.Add(l);
					list2.Add(l + 1);
				}
			}
			int num2 = halfTriCols;
			int num3 = (halfTriCols - 1) * 2 + num2;
			for (int m = num2; m <= num3; m += 2)
			{
				if (IsTriSumLineDone(m))
				{
					numColClear++;
					list3.Add(m);
				}
			}
			DefineBlockStartAnim(list, list2, list3);
			CalculateDelayAnim(list, list2, list3);
			for (int n = 0; n < list.Count; n++)
			{
				num += RemoveColLine(list[n]);
			}
			for (int num4 = 0; num4 < list2.Count; num4++)
			{
				num += RemoveRowLine(list2[num4]);
			}
			for (int num5 = 0; num5 < list3.Count; num5++)
			{
				num += RemoveTriSumLine(list3[num5]);
			}
			return num;
		}

		private List<GridCell> CreateRowLine(int x)
		{
			List<GridCell> list = new List<GridCell>();
			int num = 0;
			for (int i = 0; i < sizeY; i++)
			{
				if (isBlackMap[x, i])
				{
					num++;
				}
				else
				{
					list.Add(new GridCell(x, i));
				}
			}
			if (num == sizeY)
			{
				list.Clear();
			}
			return list;
		}

		private List<GridCell> CreateColLine(int x)
		{
			List<GridCell> list = new List<GridCell>();
			int num = 0;
			int num2 = 0;
			int num3 = x;
			for (int i = 0; i < triRow; i++)
			{
				for (x = num3; x <= num3 + 1; x++)
				{
					if (isBlackMap[i, x])
					{
						if (x == num3)
						{
							num++;
						}
						else
						{
							num2++;
						}
					}
					else
					{
						list.Add(new GridCell(i, x));
					}
				}
			}
			if (num == sizeX || num2 == sizeX)
			{
				list.Clear();
			}
			return list;
		}

		private List<GridCell> CreateTriSumLine(int sum)
		{
			List<GridCell> list = new List<GridCell>();
			int num = 0;
			int num2 = sizeX - 1;
			int num3 = 0;
			int num4 = 0;
			for (int i = num; i <= num2; i++)
			{
				for (int num5 = sum; num5 >= sum - 1; num5--)
				{
					int num6 = num5 - 2 * i;
					int num7 = num6;
					int num8 = i;
					if (IsInside(num8, num7))
					{
						if (isBlackMap[num8, num7])
						{
							if (num5 == sum - 1)
							{
								num3++;
							}
							else
							{
								num4++;
							}
						}
						else
						{
							list.Add(new GridCell(num8, num7));
						}
					}
				}
			}
			if (num3 == triRow || num4 == triRow)
			{
				list.Clear();
			}
			return list;
		}

		protected bool IsTriRowLineDone(int x)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			bool flag = true;
			int num = 0;
			for (int i = 0; i < sizeY; i++)
			{
				if (isBlackMap[x, i])
				{
					num++;
					continue;
				}
				if (blocks[x, i] == null)
				{
					flag = false;
					break;
				}
				if (newShapeBlocks[x, i])
				{
					list.Add(Encode(x, i));
					if (dictIntersect.ContainsKey(Encode(x, i)))
					{
						Dictionary<int, int> dictionary;
						int key;
						(dictionary = dictIntersect)[key = Encode(x, i)] = dictionary[key] + 1;
					}
					else
					{
						dictIntersect[Encode(x, i)] = 1;
					}
				}
				list2.Add(Encode(x, i));
			}
			if (num == sizeY)
			{
				flag = false;
			}
			if (flag)
			{
				dictRowClear.Add(x, list);
				dictRowsAllBlockClear.Add(x, list2);
			}
			else
			{
				list = null;
			}
			return flag;
		}

		protected bool IsTriColLineDone(int x)
		{
			bool flag = true;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			int num = 0;
			int num2 = 0;
			int num3 = x;
			for (int i = 0; i < triRow; i++)
			{
				for (x = num3; x <= num3 + 1; x++)
				{
					if (isBlackMap[i, x])
					{
						if (x == num3)
						{
							num++;
						}
						else
						{
							num2++;
						}
					}
					else
					{
						if (blocks[i, x] == null)
						{
							flag = false;
							break;
						}
						if (newShapeBlocks[i, x])
						{
							list.Add(Encode(i, x));
							if (dictIntersect.ContainsKey(Encode(i, x)))
							{
								Dictionary<int, int> dictionary;
								int key;
								(dictionary = dictIntersect)[key = Encode(i, x)] = dictionary[key] + 1;
							}
							else
							{
								dictIntersect[Encode(i, x)] = 1;
							}
						}
						list2.Add(Encode(i, x));
					}
				}
			}
			if (num == sizeX || num2 == sizeX)
			{
				flag = false;
			}
			if (flag)
			{
				dictColClear.Add(num3, list);
				dictColsAllBlockClear.Add(num3, list2);
			}
			else
			{
				list = null;
			}
			return flag;
		}

		protected bool IsTriSumLineDone(int sum)
		{
			bool flag = true;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			int num = 0;
			int num2 = sizeX - 1;
			int num3 = 0;
			int num4 = 0;
			for (int i = num; i <= num2; i++)
			{
				for (int num5 = sum; num5 >= sum - 1; num5--)
				{
					int num6 = num5 - 2 * i;
					int num7 = num6;
					int num8 = i;
					if (IsInside(num8, num7))
					{
						if (isBlackMap[num8, num7])
						{
							if (num5 == sum - 1)
							{
								num3++;
							}
							else
							{
								num4++;
							}
						}
						else
						{
							if (blocks[num8, num7] == null)
							{
								flag = false;
								return false;
							}
							if (newShapeBlocks[num8, num7])
							{
								list.Add(Encode(num8, num7));
								if (dictIntersect.ContainsKey(Encode(num8, num7)))
								{
									Dictionary<int, int> dictionary;
									int key;
									(dictionary = dictIntersect)[key = Encode(num8, num7)] = dictionary[key] + 1;
								}
								else
								{
									dictIntersect[Encode(num8, num7)] = 1;
								}
							}
							list2.Add(Encode(num8, num7));
						}
					}
				}
			}
			if (num3 == triRow || num4 == triRow)
			{
				flag = false;
				return false;
			}
			if (flag)
			{
				dictSumClear.Add(sum, list);
				dictSumsAllBlockClear.Add(sum, list2);
				for (int j = 0; j < list2.Count; j++)
				{
					MyVector2 myVector = Decode(list2[j]);
					int x = myVector.x;
					MyVector2 myVector2 = Decode(list2[j]);
					int y = myVector2.y;
				}
			}
			else
			{
				list = null;
				list2 = null;
			}
			return flag;
		}

		protected int RemoveTriSumLine(int sum)
		{
			int num = 0;
			int num2 = sizeX - 1;
			int num3 = 0;
			for (int i = sum - 1; i <= sum; i++)
			{
				for (int j = num; j <= num2; j++)
				{
					int num4 = i - 2 * j;
					int num5 = num4;
					int num6 = j;
					if (IsInside(num6, num5) && !isBlackMap[num6, num5])
					{
						if (isBombMode && isBombed[num6, num5])
						{
							num3 += POINT_PER_BOMB;
							ClearBombAt(num6, num5);
						}
						if (blocks[num6, num5] != null)
						{
							num3 += POINT_PER_GRID;
							blocksNeedClear.Add(blocks[num6, num5]);
							blocks[num6, num5] = null;
						}
					}
				}
			}
			return num3;
		}

		private int Encode(int x, int y)
		{
			return x * 100 + y;
		}

		private MyVector2 Decode(int value)
		{
			return new MyVector2(value / 100, value % 100);
		}

		public int GetNumColClear()
		{
			return numColClear;
		}

		private void CalculateDelayAnim(List<int> listRows, List<int> listCols, List<int> listTriSum)
		{
			for (int i = 0; i < listRows.Count; i++)
			{
				CalculateDelayRow(listRows[i]);
			}
			for (int j = 0; j < listTriSum.Count; j++)
			{
				CalculateDelayTriSum(listTriSum[j]);
			}
			for (int k = 0; k < listCols.Count; k += 2)
			{
				CalculateDelayCol(listCols[k]);
			}
		}

		private void CalculateDelayRow(int x)
		{
			List<int> list = new List<int>();
			List<int> list2 = dictRowsAllBlockClear[x];
			for (int i = 0; i < list2.Count; i++)
			{
				if (blockStartAnim.Contains(list2[i]))
				{
					list.Add(i);
				}
			}
			int count = list2.Count;
			if (list.Count > 1)
			{
				int num = Math.Max(list[0], list[1]);
				int num2 = Math.Min(list[0], list[1]);
				for (int j = 0; j < count; j++)
				{
					MyVector2 myVector = Decode(list2[j]);
					int x2 = myVector.x;
					MyVector2 myVector2 = Decode(list2[j]);
					int y = myVector2.y;
					if (j <= num2)
					{
						timeDelayAnim[x2, y] = Math.Abs(num2 - j);
					}
					else if (j >= num)
					{
						timeDelayAnim[x2, y] = Math.Abs(num - j);
					}
				}
			}
			else if (list.Count == 1)
			{
				for (int k = 0; k < list2.Count; k++)
				{
					MyVector2 myVector3 = Decode(list2[k]);
					int x3 = myVector3.x;
					MyVector2 myVector4 = Decode(list2[k]);
					int y2 = myVector4.y;
					timeDelayAnim[x3, y2] = Math.Abs(list[0] - k);
				}
			}
		}

		private void CalculateDelayCol(int y)
		{
			List<int> list = new List<int>();
			List<int> list2 = dictColsAllBlockClear[y];
			for (int i = 0; i < list2.Count; i++)
			{
				if (blockStartAnim.Contains(list2[i]))
				{
					list.Add(i);
				}
			}
			int count = list2.Count;
			if (list.Count > 1)
			{
				int num = Math.Max(list[0], list[1]);
				int num2 = Math.Min(list[0], list[1]);
				for (int j = 0; j < list2.Count; j++)
				{
					MyVector2 myVector = Decode(list2[j]);
					int x = myVector.x;
					MyVector2 myVector2 = Decode(list2[j]);
					int y2 = myVector2.y;
					if (j <= num2)
					{
						timeDelayAnim[x, y2] = Math.Abs(num2 - j);
					}
					else if (j >= num)
					{
						timeDelayAnim[x, y2] = Math.Abs(num - j);
					}
				}
			}
			else if (list.Count == 1)
			{
				for (int k = 0; k < list2.Count; k++)
				{
					MyVector2 myVector3 = Decode(list2[k]);
					int x2 = myVector3.x;
					MyVector2 myVector4 = Decode(list2[k]);
					int y3 = myVector4.y;
					timeDelayAnim[x2, y3] = Math.Abs(list[0] - k);
				}
			}
		}

		private void CalculateDelayTriSum(int sum)
		{
			List<int> list = new List<int>();
			List<int> list2 = dictSumsAllBlockClear[sum];
			for (int i = 0; i < list2.Count; i++)
			{
				if (blockStartAnim.Contains(list2[i]))
				{
					list.Add(i);
				}
			}
			int count = list2.Count;
			if (list.Count > 1)
			{
				int num = Math.Max(list[0], list[1]);
				int num2 = Math.Min(list[0], list[1]);
				for (int j = 0; j < list2.Count; j++)
				{
					MyVector2 myVector = Decode(list2[j]);
					int x = myVector.x;
					MyVector2 myVector2 = Decode(list2[j]);
					int y = myVector2.y;
					if (j <= num2)
					{
						timeDelayAnim[x, y] = Math.Abs(num2 - j);
					}
					else if (j >= num)
					{
						timeDelayAnim[x, y] = Math.Abs(num - j);
					}
				}
			}
			else if (list.Count == 1)
			{
				for (int k = 0; k < list2.Count; k++)
				{
					MyVector2 myVector3 = Decode(list2[k]);
					int x2 = myVector3.x;
					MyVector2 myVector4 = Decode(list2[k]);
					int y2 = myVector4.y;
					timeDelayAnim[x2, y2] = Math.Abs(list[0] - k);
				}
			}
		}

		private void DefineBlockStartAnim(List<int> listRows, List<int> listCols, List<int> listSums)
		{
			if (numColClear > 1)
			{
				foreach (KeyValuePair<int, int> item in dictIntersect)
				{
					if (item.Value > 1)
					{
						blockStartAnim.Add(item.Key);
					}
				}
				if (blockStartAnim.Count == 0)
				{
					DefineBlockAnimEachColDone(listRows, listCols, listSums);
				}
			}
			else if (numColClear == 1)
			{
				DefineBlockAnimEachColDone(listRows, listCols, listSums);
			}
		}

		private void DefineBlockAnimEachColDone(List<int> listRows, List<int> listCols, List<int> listSums)
		{
			for (int i = 0; i < listRows.Count; i++)
			{
				DefineBlockAnimList(dictRowClear[listRows[i]]);
			}
			for (int j = 0; j < listCols.Count; j += 2)
			{
				DefineBlockAnimList(dictColClear[listCols[j]]);
			}
			for (int k = 0; k < listSums.Count; k++)
			{
				DefineBlockAnimList(dictSumClear[listSums[k]]);
			}
		}

		private void DefineBlockAnimList(List<int> list)
		{
			int count = list.Count;
			if (count > 0)
			{
				if (count % 2 == 0)
				{
					int item = list[count / 2 - 1];
					int item2 = list[count / 2];
					blockStartAnim.Add(item);
					blockStartAnim.Add(item2);
				}
				else
				{
					int item3 = list[count / 2];
					blockStartAnim.Add(item3);
				}
			}
		}

		public List<MyVector2> GetListClearFourRow(bool isTop)
		{
			List<MyVector2> list = new List<MyVector2>();
			timeDelayAnim = new float[sizeX, sizeY];
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					timeDelayAnim[i, j] = -1f;
				}
			}
			UnityEngine.Debug.Log("List Clear 4 rows");
			if (isTop)
			{
				for (int k = 0; k < 4; k++)
				{
					for (int l = 0; l < sizeY; l++)
					{
						if (!isBlackMap[k, l])
						{
							list.Add(new MyVector2(k, l));
							blocks[k, l] = null;
						}
					}
				}
			}
			else
			{
				for (int num = sizeX - 1; num > sizeX - 5; num--)
				{
					for (int m = 0; m < sizeY; m++)
					{
						if (!isBlackMap[num, m])
						{
							list.Add(new MyVector2(num, m));
							blocks[num, m] = null;
						}
					}
				}
			}
			List<MyVector2> list2 = new List<MyVector2>();
			float num2 = 0f;
			while (list.Count > 0)
			{
				int count = list.Count;
				int index = UnityEngine.Random.Range(0, count);
				MyVector2 item = list[index];
				timeDelayAnim[item.x, item.y] = num2 / 4f;
				num2 += 1f;
				list2.Add(item);
				list.RemoveAt(index);
			}
			return list2;
		}

		public float[,] GetTimeDelay()
		{
			return timeDelayAnim;
		}

		public void SetTableLegendPlayer()
		{
			for (int i = 7; i < 15; i++)
			{
				int c = UnityEngine.Random.Range(0, 10);
				blocks[0, i] = new Block(c, 0, i);
			}
			for (int j = 5; j < 9; j++)
			{
				int c2 = UnityEngine.Random.Range(0, 10);
				blocks[1, j] = new Block(c2, 1, j);
			}
			for (int k = 3; k < 7; k++)
			{
				int c3 = UnityEngine.Random.Range(0, 10);
				blocks[2, k] = new Block(c3, 2, k);
			}
		}

		public void SetTableCantDecide()
		{
			int num = 0;
			for (int i = 7; i < 15; i++)
			{
				num = UnityEngine.Random.Range(0, 10);
				blocks[0, i] = new Block(num, 0, i);
			}
			for (int j = 5; j < 13; j++)
			{
				num = UnityEngine.Random.Range(0, 10);
				blocks[1, j] = new Block(num, 1, j);
			}
			for (int k = 3; k < 16; k++)
			{
				if (k != 11)
				{
					num = UnityEngine.Random.Range(0, 10);
					blocks[2, k] = new Block(num, 2, k);
				}
			}
			for (int l = 1; l < 10; l++)
			{
				num = UnityEngine.Random.Range(0, 10);
				blocks[3, l] = new Block(num, 3, l);
			}
			for (int m = 2; m < 9; m++)
			{
				if (m != 5)
				{
					num = UnityEngine.Random.Range(0, 10);
					blocks[4, m] = new Block(num, 4, m);
				}
			}
			num = UnityEngine.Random.Range(0, 10);
			blocks[5, 2] = new Block(num, 5, 2);
		}
	}
}
