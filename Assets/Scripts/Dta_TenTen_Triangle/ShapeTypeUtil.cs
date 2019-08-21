namespace Dta.TenTen.Triangle
{
	public class ShapeTypeUtil
	{
		public static ShapeRotate shapeRotate = new ShapeRotate();

		private static int[] tuanRatio = new int[20]
		{
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			2,
			3,
			3,
			3,
			4,
			5,
			5,
			5,
			5,
			6,
			6,
			6
		};

		private static int[] tungRatio = new int[11]
		{
			0,
			0,
			1,
			2,
			3,
			4,
			5,
			5,
			5,
			5,
			6
		};

		private static int[] haEasy = new int[20]
		{
			0,
			0,
			0,
			1,
			1,
			2,
			3,
			3,
			4,
			4,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			6,
			6
		};

		private static int[] haHard = new int[20]
		{
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			2,
			2,
			3,
			3,
			4,
			4,
			5,
			5,
			5,
			5,
			6,
			6
		};

		private static int[] badPlayer = new int[5]
		{
			1,
			2,
			4,
			0,
			6
		};

		private static MyVector2[] legendary = new MyVector2[7]
		{
			new MyVector2(4, 0),
			new MyVector2(2, 0),
			new MyVector2(5, 1),
			new MyVector2(0, 1),
			new MyVector2(4, 2),
			new MyVector2(6, 1),
			new MyVector2(4, 2)
		};

		private static MyVector2[] goodPlayer = new MyVector2[14]
		{
			new MyVector2(2, 0),
			new MyVector2(5, 0),
			new MyVector2(4, 0),
			new MyVector2(6, 1),
			new MyVector2(3, 0),
			new MyVector2(0, 0),
			new MyVector2(1, 5),
			new MyVector2(0, 3),
			new MyVector2(4, 0),
			new MyVector2(2, 0),
			new MyVector2(0, 1),
			new MyVector2(6, 1),
			new MyVector2(4, 2),
			new MyVector2(4, 2)
		};

		private static MyVector2[] cantDecide = new MyVector2[3]
		{
			new MyVector2(0, 2),
			new MyVector2(6, 1),
			new MyVector2(7, 0)
		};

		private static int[] remote;

		private static int index = 0;

		public static void SetRemoteDifficulty(int[] remoteData)
		{
			remote = remoteData;
		}

		public static int GetRandomShapeFromRemote()
		{
			if (remote == null)
			{
				remote = Singleton<FirebaseManager>.instance.GetDifficulty();
			}
			int num = MathUtils.Random(0, remote.Length);
			return remote[num];
		}

		public static int[,] GetRandomShape()
		{
			return GetShape(GetRamdomTuanRatio(), GetRandomRotateTime(6));
		}

		public static int GetRamdomTuanRatio()
		{
			int num = MathUtils.Random(0, tuanRatio.Length);
			return tuanRatio[num];
		}

		public static int GetRandomTungRatio()
		{
			int num = MathUtils.Random(0, tungRatio.Length);
			return tungRatio[num];
		}

		public static int GetRandomHaEasyRatio()
		{
			int num = MathUtils.Random(0, haEasy.Length);
			return haEasy[num];
		}

		public static int GetRandomHaHardRatio()
		{
			int num = MathUtils.Random(0, haHard.Length);
			return haHard[num];
		}

		public static int GetRandomRotateTime(int maxTime)
		{
			return MathUtils.Random(0, maxTime);
		}

		public static int GetBadPlayer()
		{
			int num = MathUtils.Random(0, badPlayer.Length);
			return badPlayer[num];
		}

		public static MyVector2 GetGoodPlayerShape()
		{
			if (index >= goodPlayer.Length)
			{
				index = 0;
			}
			return goodPlayer[index++];
		}

		public static MyVector2 GetLegendaryShape()
		{
			if (index >= legendary.Length)
			{
				return new MyVector2(GetRandomTungRatio(), MathUtils.Random(0, 6));
			}
			return legendary[index++];
		}

		public static MyVector2 GetCantDecideShape()
		{
			if (index >= cantDecide.Length)
			{
				return new MyVector2(GetRandomTungRatio(), MathUtils.Random(0, 6));
			}
			return cantDecide[index++];
		}

		public static void ResetIndex()
		{
			index = 0;
		}

		public static int[,] GetShape(int id, int rotate)
		{
			switch (id)
			{
			case 0:
				shapeRotate.SetShape1();
				return shapeRotate.GetRotateMatrix(rotate);
			case 1:
				shapeRotate.SetShape2();
				return shapeRotate.GetRotateMatrix(rotate);
			case 2:
				shapeRotate.SetShape3();
				return shapeRotate.GetRotateMatrix(0);
			case 3:
				shapeRotate.SetShape4();
				return shapeRotate.GetRotateMatrix(rotate);
			case 4:
				shapeRotate.SetShape5();
				return shapeRotate.GetRotateMatrix(rotate);
			case 5:
				shapeRotate.SetShape6();
				return shapeRotate.GetRotateMatrix(rotate);
			case 6:
				shapeRotate.SetShape7();
				return shapeRotate.GetRotateMatrix(rotate);
			case 7:
				shapeRotate.SetShape5f();
				return shapeRotate.GetRotateMatrix(rotate);
			default:
				shapeRotate.SetShape1();
				return shapeRotate.GetRotateMatrix(rotate);
			}
		}
	}
}
