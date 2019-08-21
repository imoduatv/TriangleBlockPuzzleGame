namespace Dta.TenTen.Hexagonal
{
	public class ShapeTypeUtil
	{
		public static ShapeRotate shapeRotate = new ShapeRotate();

		public static int[,] GetRandomShape()
		{
			return GetShape(MathUtils.Random(0, 8));
		}

		public static int GetRandomRotateTime(int maxTime)
		{
			return MathUtils.Random(0, maxTime);
		}

		public static int[,] GetShape(int id)
		{
			switch (id)
			{
			case 0:
				shapeRotate.SetShape1();
				return shapeRotate.GetRotateMatrix(0);
			case 1:
				shapeRotate.SetShape2();
				return shapeRotate.GetRotateMatrix(GetRandomRotateTime(6));
			case 2:
				shapeRotate.SetShape3();
				return shapeRotate.GetRotateMatrix(GetRandomRotateTime(6));
			case 3:
				shapeRotate.SetShape4();
				return shapeRotate.GetRotateMatrix(GetRandomRotateTime(6));
			case 4:
				shapeRotate.SetShape5();
				return shapeRotate.GetRotateMatrix(GetRandomRotateTime(6));
			case 5:
				shapeRotate.SetShape6();
				return shapeRotate.GetRotateMatrix(GetRandomRotateTime(6));
			case 6:
				shapeRotate.SetShape7();
				return shapeRotate.GetRotateMatrix(GetRandomRotateTime(6));
			case 7:
				shapeRotate.SetShape8();
				return shapeRotate.GetRotateMatrix(GetRandomRotateTime(6));
			default:
				shapeRotate.SetShape1();
				return shapeRotate.GetRotateMatrix(0);
			}
		}
	}
}
