using System.Collections.Generic;

namespace Dta.TenTen.Triangle
{
	public class ShapeRotate
	{
		private int rows = 4;

		private int cols = 8;

		private int[,] mapIn;

		private List<int> circle1;

		private List<int> circle2;

		public ShapeRotate()
		{
			mapIn = new int[rows, cols];
			SetEmpty();
			circle1 = new List<int>();
			circle2 = new List<int>();
			CreateCircle1();
			CreateCircle2();
		}

		private void SetEmpty()
		{
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					mapIn[i, j] = 0;
				}
			}
		}

		private void CreateCircle1()
		{
			circle1.Clear();
			circle1.Add(mapIn[2, 2]);
			circle1.Add(mapIn[2, 3]);
			circle1.Add(mapIn[2, 4]);
			circle1.Add(mapIn[1, 5]);
			circle1.Add(mapIn[1, 4]);
			circle1.Add(mapIn[1, 3]);
		}

		private void SetCircle1()
		{
			circle1[0] = mapIn[2, 2];
			circle1[1] = mapIn[2, 3];
			circle1[2] = mapIn[2, 4];
			circle1[3] = mapIn[1, 5];
			circle1[4] = mapIn[1, 4];
			circle1[5] = mapIn[1, 3];
		}

		private void ApplyCircle1()
		{
			mapIn[2, 2] = circle1[0];
			mapIn[2, 3] = circle1[1];
			mapIn[2, 4] = circle1[2];
			mapIn[1, 5] = circle1[3];
			mapIn[1, 4] = circle1[4];
			mapIn[1, 3] = circle1[5];
		}

		private void CreateCircle2()
		{
			circle2.Clear();
			circle2.Add(mapIn[2, 0]);
			circle2.Add(mapIn[2, 1]);
			circle2.Add(mapIn[3, 0]);
			circle2.Add(mapIn[3, 1]);
			circle2.Add(mapIn[3, 2]);
			circle2.Add(mapIn[3, 3]);
			circle2.Add(mapIn[3, 4]);
			circle2.Add(mapIn[2, 5]);
			circle2.Add(mapIn[2, 6]);
			circle2.Add(mapIn[1, 7]);
			circle2.Add(mapIn[1, 6]);
			circle2.Add(mapIn[0, 7]);
			circle2.Add(mapIn[0, 6]);
			circle2.Add(mapIn[0, 5]);
			circle2.Add(mapIn[0, 4]);
			circle2.Add(mapIn[0, 3]);
			circle2.Add(mapIn[1, 2]);
			circle2.Add(mapIn[1, 1]);
		}

		private void SetCircle2()
		{
			circle2[0] = mapIn[2, 0];
			circle2[1] = mapIn[2, 1];
			circle2[2] = mapIn[3, 0];
			circle2[3] = mapIn[3, 1];
			circle2[4] = mapIn[3, 2];
			circle2[5] = mapIn[3, 3];
			circle2[6] = mapIn[3, 4];
			circle2[7] = mapIn[2, 5];
			circle2[8] = mapIn[2, 6];
			circle2[9] = mapIn[1, 7];
			circle2[10] = mapIn[1, 6];
			circle2[11] = mapIn[0, 7];
			circle2[12] = mapIn[0, 6];
			circle2[13] = mapIn[0, 5];
			circle2[14] = mapIn[0, 4];
			circle2[15] = mapIn[0, 3];
			circle2[16] = mapIn[1, 2];
			circle2[17] = mapIn[1, 1];
		}

		private void ApplyCircle2()
		{
			mapIn[2, 0] = circle2[0];
			mapIn[2, 1] = circle2[1];
			mapIn[3, 0] = circle2[2];
			mapIn[3, 1] = circle2[3];
			mapIn[3, 2] = circle2[4];
			mapIn[3, 3] = circle2[5];
			mapIn[3, 4] = circle2[6];
			mapIn[2, 5] = circle2[7];
			mapIn[2, 6] = circle2[8];
			mapIn[1, 7] = circle2[9];
			mapIn[1, 6] = circle2[10];
			mapIn[0, 7] = circle2[11];
			mapIn[0, 6] = circle2[12];
			mapIn[0, 5] = circle2[13];
			mapIn[0, 4] = circle2[14];
			mapIn[0, 3] = circle2[15];
			mapIn[1, 2] = circle2[16];
			mapIn[1, 1] = circle2[17];
		}

		private void RotateClockWise(int time)
		{
			if (time <= 0)
			{
				return;
			}
			List<int> list = new List<int>(circle1);
			List<int> list2 = new List<int>(circle2);
			int num = 0;
			int count = circle1.Count;
			int count2 = circle2.Count;
			int num2 = time * 3;
			for (int i = 0; i < count; i++)
			{
				num = i + time;
				if (num >= count)
				{
					num -= count;
				}
				circle1[num] = list[i];
			}
			for (int j = 0; j < count2; j++)
			{
				num = j + num2;
				if (num >= count2)
				{
					num -= count2;
				}
				circle2[num] = list2[j];
			}
		}

		public void SetShape1()
		{
			SetEmpty();
			mapIn[1, 3] = 1;
			mapIn[1, 4] = 1;
			mapIn[1, 5] = 1;
			SetCircle1();
			SetCircle2();
		}

		public void SetShape2()
		{
			SetEmpty();
			mapIn[1, 3] = 1;
			mapIn[1, 4] = 1;
			mapIn[1, 5] = 1;
			mapIn[2, 4] = 1;
			SetCircle1();
			SetCircle2();
		}

		public void SetShape3()
		{
			SetEmpty();
			mapIn[2, 2] = 1;
			mapIn[2, 3] = 1;
			mapIn[2, 4] = 1;
			mapIn[1, 5] = 1;
			mapIn[1, 4] = 1;
			mapIn[1, 3] = 1;
			SetCircle1();
			SetCircle2();
		}

		public void SetShape4()
		{
			SetEmpty();
			mapIn[1, 4] = 1;
			SetCircle1();
			SetCircle2();
		}

		public void SetShape5()
		{
			SetEmpty();
			mapIn[1, 2] = 1;
			mapIn[1, 3] = 1;
			mapIn[1, 4] = 1;
			mapIn[1, 5] = 1;
			SetCircle1();
			SetCircle2();
		}

		public void SetShape5f()
		{
			SetEmpty();
			mapIn[1, 1] = 1;
			mapIn[1, 2] = 1;
			mapIn[1, 3] = 1;
			mapIn[1, 4] = 1;
			SetCircle1();
			SetCircle2();
		}

		public void SetShape6()
		{
			SetEmpty();
			mapIn[1, 5] = 1;
			mapIn[1, 4] = 1;
			SetCircle1();
			SetCircle2();
		}

		public void SetShape7()
		{
			SetEmpty();
			mapIn[1, 2] = 1;
			mapIn[1, 3] = 1;
			mapIn[1, 4] = 1;
			mapIn[2, 2] = 1;
			SetCircle1();
			SetCircle2();
		}

		public int[,] GetRotateMatrix(int time)
		{
			RotateClockWise(time);
			ApplyCircle1();
			ApplyCircle2();
			return removeEmptyRowCol(mapIn);
		}

		private int[,] removeEmptyRowCol(int[,] matrix)
		{
			int length = matrix.GetLength(0);
			int length2 = matrix.GetLength(1);
			bool flag = true;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			for (int k = 0; k < length; k++)
			{
				flag = true;
				for (int l = 0; l < length2; l++)
				{
					if (matrix[k, l] != 0)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(k);
				}
			}
			bool flag2 = false;
			for (int m = 0; m < length2; m++)
			{
				flag = true;
				for (int n = 0; n < length; n++)
				{
					if (matrix[n, m] != 0)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list2.Add(m);
				}
				else if (!flag2)
				{
					flag2 = true;
					if (m % 2 != 0 && list2.Count > 0)
					{
						list2.RemoveAt(list2.Count - 1);
					}
				}
			}
			int[,] array = new int[length - list.Count, length2];
			int num = 0;
			int j;
			for (j = 0; j < length; j++)
			{
				if (!list.Exists((int x) => x == j))
				{
					for (int num2 = 0; num2 < length2; num2++)
					{
						array[num, num2] = matrix[j, num2];
					}
					num++;
				}
			}
			length -= list.Count;
			int[,] array2 = new int[length, length2 - list2.Count];
			num = 0;
			int i;
			for (i = 0; i < length2; i++)
			{
				if (!list2.Exists((int x) => x == i))
				{
					for (int num3 = 0; num3 < length; num3++)
					{
						array2[num3, num] = array[num3, i];
					}
					num++;
				}
			}
			return array2;
		}
	}
}
