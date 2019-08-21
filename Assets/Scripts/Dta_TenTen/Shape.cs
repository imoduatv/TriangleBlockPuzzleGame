using Dta.TenTen.Hexagonal;
using Dta.TenTen.Normal;
using Dta.TenTen.Triangle;
using UnityEngine;

namespace Dta.TenTen
{
	public class Shape
	{
		public int size1;

		public int size2;

		public int[,] map;

		public int color;

		public GridCell firstGrid;

		public bool isFirstColEmpty;

		private int shapeId;

		private int rotateTime;

		public bool IsBackUpShape;

		public Vector3 BackUpShapePos;

		public bool IsNewIndex
		{
			get;
			set;
		}

		public int GridsCount
		{
			get;
			private set;
		}

		public int ShapeId => shapeId;

		public int RotateTime => rotateTime;

		public Shape(BoardType boardType)
		{
			switch (boardType)
			{
			case BoardType.Normal:
			{
				color = 0;
				Dta.TenTen.Normal.ShapeType random = Dta.TenTen.Normal.ShapeTypeUtil.GetRandom();
				map = Dta.TenTen.Normal.ShapeTypeUtil.GetShape(random);
				break;
			}
			case BoardType.Hexagonal:
				color = 0;
				map = Dta.TenTen.Hexagonal.ShapeTypeUtil.GetRandomShape();
				break;
			case BoardType.Triangle:
				color = 0;
				map = Dta.TenTen.Triangle.ShapeTypeUtil.GetRandomShape();
				break;
			}
			IsNewIndex = false;
			size1 = map.GetLength(0);
			size2 = map.GetLength(1);
			FindFirstGrid();
			CheckFirstColEmpty();
			CountNumberOfGrids();
		}

		public Shape(ShapeData shapeData)
		{
			color = 0;
			map = shapeData.map;
			size1 = map.GetLength(0);
			size2 = map.GetLength(1);
			IsNewIndex = false;
			FindFirstGrid();
			CheckFirstColEmpty();
			CountNumberOfGrids();
		}

		public Shape(BoardType boardType, int shapeId, int rotateTime)
		{
			this.shapeId = shapeId;
			this.rotateTime = rotateTime;
			switch (boardType)
			{
			case BoardType.Normal:
			{
				color = 0;
				Dta.TenTen.Normal.ShapeType random = Dta.TenTen.Normal.ShapeTypeUtil.GetRandom();
				map = Dta.TenTen.Normal.ShapeTypeUtil.GetShape(random);
				break;
			}
			case BoardType.Hexagonal:
				color = 0;
				map = Dta.TenTen.Hexagonal.ShapeTypeUtil.GetRandomShape();
				break;
			case BoardType.Triangle:
				color = 0;
				map = Dta.TenTen.Triangle.ShapeTypeUtil.GetShape(shapeId, rotateTime);
				break;
			}
			IsNewIndex = false;
			size1 = map.GetLength(0);
			size2 = map.GetLength(1);
			FindFirstGrid();
			CheckFirstColEmpty();
			CountNumberOfGrids();
		}

		public Shape(BoardType boardType, GameManager.TutStep tutStep, bool isRotate)
		{
			switch (tutStep)
			{
			case GameManager.TutStep.Tut1:
				shapeId = 7;
				rotateTime = 2;
				map = Dta.TenTen.Triangle.ShapeTypeUtil.GetShape(shapeId, rotateTime);
				break;
			case GameManager.TutStep.Tut2:
				shapeId = 7;
				rotateTime = 2;
				rotateTime += (isRotate ? 5 : 0);
				rotateTime %= 6;
				map = Dta.TenTen.Triangle.ShapeTypeUtil.GetShape(shapeId, rotateTime);
				break;
			case GameManager.TutStep.Tut3:
				shapeId = 6;
				rotateTime = 0;
				rotateTime += (isRotate ? 1 : 0);
				rotateTime %= 6;
				map = Dta.TenTen.Triangle.ShapeTypeUtil.GetShape(shapeId, rotateTime);
				break;
			case GameManager.TutStep.None:
				switch (boardType)
				{
				case BoardType.Normal:
				{
					color = 0;
					Dta.TenTen.Normal.ShapeType random = Dta.TenTen.Normal.ShapeTypeUtil.GetRandom();
					map = Dta.TenTen.Normal.ShapeTypeUtil.GetShape(random);
					break;
				}
				case BoardType.Hexagonal:
					color = 0;
					map = Dta.TenTen.Hexagonal.ShapeTypeUtil.GetRandomShape();
					break;
				case BoardType.Triangle:
					color = 0;
					if (Singleton<GameManager>.Instance.RatioShape == Ratio2.TungRatio)
					{
						shapeId = Dta.TenTen.Triangle.ShapeTypeUtil.GetRandomTungRatio();
					}
					else if (Singleton<GameManager>.instance.RatioShape == Ratio2.HaEasyRaio)
					{
						shapeId = Dta.TenTen.Triangle.ShapeTypeUtil.GetRandomHaEasyRatio();
					}
					else
					{
						shapeId = Dta.TenTen.Triangle.ShapeTypeUtil.GetRandomHaHardRatio();
					}
					shapeId = Dta.TenTen.Triangle.ShapeTypeUtil.GetRandomShapeFromRemote();
					if (Singleton<GameManager>.instance.SetupType == GameManager.SetUpType.BadPlayer)
					{
						shapeId = Dta.TenTen.Triangle.ShapeTypeUtil.GetBadPlayer();
					}
					rotateTime = Dta.TenTen.Triangle.ShapeTypeUtil.GetRandomRotateTime(6);
					rotateTime = ((rotateTime < 6) ? rotateTime : 0);
					if (Singleton<GameManager>.instance.SetupType == GameManager.SetUpType.GoodPlayer)
					{
						MyVector2 goodPlayerShape = Dta.TenTen.Triangle.ShapeTypeUtil.GetGoodPlayerShape();
						shapeId = goodPlayerShape.x;
						rotateTime = goodPlayerShape.y;
					}
					else if (Singleton<GameManager>.instance.SetupType == GameManager.SetUpType.CantDecide)
					{
						MyVector2 cantDecideShape = Dta.TenTen.Triangle.ShapeTypeUtil.GetCantDecideShape();
						shapeId = cantDecideShape.x;
						rotateTime = cantDecideShape.y;
					}
					else if (Singleton<GameManager>.instance.SetupType == GameManager.SetUpType.Legendary)
					{
						MyVector2 legendaryShape = Dta.TenTen.Triangle.ShapeTypeUtil.GetLegendaryShape();
						shapeId = legendaryShape.x;
						rotateTime = legendaryShape.y;
					}
					map = Dta.TenTen.Triangle.ShapeTypeUtil.GetShape(shapeId, rotateTime);
					break;
				}
				break;
			}
			size1 = map.GetLength(0);
			size2 = map.GetLength(1);
			FindFirstGrid();
			CheckFirstColEmpty();
			CountNumberOfGrids();
		}

		private void CountNumberOfGrids()
		{
			GridsCount = 0;
			for (int i = 0; i < size1; i++)
			{
				for (int j = 0; j < size2; j++)
				{
					if (map[i, j] != 0)
					{
						GridsCount++;
					}
				}
			}
		}

		private void FindFirstGrid()
		{
			for (int i = 0; i < size1; i++)
			{
				for (int j = 0; j < size2; j++)
				{
					if (map[i, j] != 0)
					{
						firstGrid = new GridCell(i, j);
						return;
					}
				}
			}
		}

		private void CheckFirstColEmpty()
		{
			int num = 0;
			for (int i = 0; i < size1; i++)
			{
				if (map[i, 0] == 1)
				{
					isFirstColEmpty = false;
					return;
				}
			}
			isFirstColEmpty = true;
		}

		public void LogMap()
		{
			LogUtils.Log("LogMap: ");
			for (int num = map.GetLength(0) - 1; num >= 0; num--)
			{
				string text = string.Empty;
				for (int i = 0; i < map.GetLength(1); i++)
				{
					text = text + " " + map[num, i];
				}
				LogUtils.Log(text);
			}
		}
	}
}
