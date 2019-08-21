using System.Collections.Generic;

namespace Dta.TenTen.Normal
{
	public class Table : ITable
	{
		protected int sizeX;

		protected int sizeY;

		protected Block[,] blocks;

		protected bool[,] newShapeBlocks;

		protected List<Block> blocksNeedClear;

		protected int BOMB_TURN = 8;

		protected int POINT_PER_LINE = 10;

		protected int POINT_PER_GRID = 10;

		protected int POINT_PER_BOMB = 10;

		protected int MIN_BOMB_TIME = 7;

		protected int MAX_BOMB_TIME = 9;

		protected bool[,] isBlackMap;

		protected bool isBombMode;

		protected bool[,] isBombed;

		protected int[,] bombTime;

		protected int bombTurn;

		protected bool isCanUndo;

		protected Block[,] undoBlocks;

		protected bool[,] undoBlackMap;

		protected bool[,] undoIsBombed;

		protected int[,] undoBombTime;

		protected int undoBombTurn;

		public Table(int sizeX, int sizeY)
		{
			this.sizeX = sizeX;
			this.sizeY = sizeY;
			blocks = new Block[sizeX, sizeY];
			blocksNeedClear = new List<Block>();
			newShapeBlocks = new bool[sizeX, sizeY];
			isBlackMap = new bool[sizeX, sizeY];
			isBombed = new bool[sizeX, sizeY];
			bombTime = new int[sizeX, sizeY];
			bombTurn = BOMB_TURN;
			isBombMode = false;
			isCanUndo = false;
			undoBlocks = new Block[sizeX, sizeY];
			undoBlackMap = new bool[sizeX, sizeY];
			undoIsBombed = new bool[sizeX, sizeY];
			undoBombTime = new int[sizeX, sizeY];
			undoBombTurn = bombTurn;
		}

		public void Undo()
		{
			if (!isCanUndo)
			{
				return;
			}
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					if (undoBlocks[i, j] != null)
					{
						blocks[i, j] = new Block(undoBlocks[i, j]);
					}
					else
					{
						blocks[i, j] = null;
					}
					isBlackMap[i, j] = undoBlackMap[i, j];
					isBombed[i, j] = undoIsBombed[i, j];
					bombTime[i, j] = undoBombTime[i, j];
				}
			}
			bombTurn = undoBombTurn;
			isCanUndo = false;
		}

		public void LoadTable(BlockData[,] blockDatas, bool[,] isBombs, int[,] bombTimes)
		{
			Reset();
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					BlockData blockData = blockDatas[i, j];
					if (blockData != null)
					{
						blocks[i, j] = new Block(blockData.color, blockData.x, blockData.y);
					}
					isBombed[i, j] = isBombs[i, j];
					bombTime[i, j] = bombTimes[i, j];
				}
			}
		}

		public void StopUndo()
		{
			isCanUndo = false;
		}

		private void LogBlocks(Block[,] blocks)
		{
			LogUtils.Log("LogBlocks");
			for (int num = sizeX - 1; num >= 0; num--)
			{
				string text = string.Empty;
				for (int i = 0; i < sizeY; i++)
				{
					text = ((blocks[num, i] != null && !isBlackMap[num, i]) ? (text + "1 ") : (text + "0 "));
				}
				LogUtils.Log(text);
			}
		}

		private void CreateUndo()
		{
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					if (blocks[i, j] != null)
					{
						undoBlocks[i, j] = new Block(blocks[i, j]);
					}
					else
					{
						undoBlocks[i, j] = null;
					}
					undoBlackMap[i, j] = isBlackMap[i, j];
					undoIsBombed[i, j] = isBombed[i, j];
					undoBombTime[i, j] = bombTime[i, j];
				}
			}
			undoBombTurn = bombTurn;
			isCanUndo = true;
		}

		public virtual void Reset()
		{
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					blocks[i, j] = null;
				}
			}
			for (int k = 0; k < sizeX; k++)
			{
				for (int l = 0; l < sizeY; l++)
				{
					isBlackMap[k, l] = false;
					isBombed[k, l] = false;
					bombTime[k, l] = 0;
				}
			}
			bombTurn = BOMB_TURN;
			isBombMode = false;
			blocksNeedClear.Clear();
		}

		public Block[,] GetBlocks()
		{
			return blocks;
		}

		public void SetBombMode(bool isBombMode)
		{
			this.isBombMode = isBombMode;
			bombTurn = BOMB_TURN;
			undoBombTurn = bombTurn;
		}

		protected void CheckCreateBomb()
		{
			bombTurn--;
			if (bombTurn > 0)
			{
				return;
			}
			List<GridCell> list = new List<GridCell>();
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					if (!isBlackMap[i, j] && !isBombed[i, j] && blocks[i, j] != null)
					{
						list.Add(new GridCell(i, j));
					}
				}
			}
			if (list.Count < 10)
			{
				bombTurn++;
				return;
			}
			GridCell gridCell = list[MathUtils.Random(0, list.Count)];
			int x = gridCell.x;
			int y = gridCell.y;
			isBombed[x, y] = true;
			bombTime[x, y] = MathUtils.Random(MIN_BOMB_TIME, MAX_BOMB_TIME + 1);
			bombTurn = BOMB_TURN;
		}

		protected void UpdateBombTime()
		{
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					if (isBombed[i, j])
					{
						bombTime[i, j]--;
					}
				}
			}
		}

		protected void ClearBombAt(int x, int y)
		{
			isBombed[x, y] = false;
			bombTime[x, y] = 0;
		}

		public bool[,] GetIsBomb()
		{
			return isBombed;
		}

		public int[,] GetBombTime()
		{
			return bombTime;
		}

		public bool IsInside(int x, int y)
		{
			return x >= 0 && x < sizeX && y >= 0 && y < sizeY;
		}

		public bool Place(Shape shape, GridCell position)
		{
			if (!IsTryPlaceOk(shape, position))
			{
				return false;
			}
			CreateUndo();
			AddToBlocks(shape, position);
			if (isBombMode)
			{
				UpdateBombTime();
				CheckCreateBomb();
			}
			return true;
		}

		public bool IsTryPlaceOk(Shape shape, GridCell position)
		{
			if (shape == null)
			{
				return false;
			}
			if (position == null || position.x < 0 || position.y < 0)
			{
				return false;
			}
			if (!IsInTable(shape, position))
			{
				return false;
			}
			if (IsOccupied(shape, position.x, position.y))
			{
				return false;
			}
			return true;
		}

		public void AddToBlocksAfterTry(Shape shape, GridCell position)
		{
			CreateUndo();
			AddToBlocks(shape, position);
			if (isBombMode)
			{
				UpdateBombTime();
				CheckCreateBomb();
			}
		}

		protected void AddToBlocksTemp(Shape shape, int posX, int posY)
		{
			int[,] map = shape.map;
			for (int i = 0; i < shape.size1; i++)
			{
				for (int j = 0; j < shape.size2; j++)
				{
					if (map[i, j] == 1)
					{
						int num = posX + i;
						int num2 = posY + j;
						Block block = new Block(shape.color, num, num2);
						blocks[num, num2] = block;
					}
				}
			}
		}

		protected void UndoAddToBlocksTemp(Shape shape, int posX, int posY)
		{
			int[,] map = shape.map;
			for (int i = 0; i < shape.size1; i++)
			{
				for (int j = 0; j < shape.size2; j++)
				{
					if (map[i, j] == 1)
					{
						int num = posX + i;
						int num2 = posY + j;
						blocks[num, num2] = null;
					}
				}
			}
		}

		protected void AddToBlocks(Shape shape, GridCell position)
		{
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					newShapeBlocks[i, j] = false;
				}
			}
			int[,] map = shape.map;
			for (int k = 0; k < shape.size1; k++)
			{
				for (int l = 0; l < shape.size2; l++)
				{
					if (map[k, l] == 1)
					{
						int num = position.x + k;
						int num2 = position.y + l;
						Block block = new Block(shape.color, num, num2);
						blocks[num, num2] = block;
						newShapeBlocks[num, num2] = true;
					}
				}
			}
		}

		protected bool IsInTable(Shape shape, GridCell position)
		{
			int[,] map = shape.map;
			for (int i = 0; i < shape.size1; i++)
			{
				for (int j = 0; j < shape.size2; j++)
				{
					if (map[i, j] == 1)
					{
						int num = position.x + i;
						int num2 = position.y + j;
						if (num >= sizeX || num2 >= sizeY)
						{
							return false;
						}
						if (isBlackMap[num, num2])
						{
							return false;
						}
					}
				}
			}
			return position.x + shape.size1 <= sizeX && position.y + shape.size2 <= sizeY;
		}

		public virtual bool IsPlaceable(Shape shape)
		{
			int size = shape.size1;
			int size2 = shape.size2;
			if (shape.isFirstColEmpty)
			{
				size2--;
			}
			for (int i = 0; i < sizeY; i++)
			{
				for (int j = 0; j < sizeX; j++)
				{
					if (!isBlackMap[j, i] && blocks[j, i] == null)
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

		protected bool IsOccupied(Shape shape, int posX, int posY)
		{
			int[,] map = shape.map;
			for (int i = 0; i < shape.size1; i++)
			{
				for (int j = 0; j < shape.size2; j++)
				{
					if (map[i, j] == 1)
					{
						int num = posX + i;
						int num2 = posY + j;
						if (!IsInside(num, num2))
						{
							return true;
						}
						if (blocks[num, num2] != null || isBlackMap[num, num2])
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public virtual int ClearLines()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			int num = 0;
			for (int i = 0; i < sizeY; i++)
			{
				if (IsRowLineDone(i))
				{
					list.Add(i);
				}
			}
			for (int j = 0; j < sizeX; j++)
			{
				if (IsColLineDone(j))
				{
					list2.Add(j);
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
			return num;
		}

		protected bool IsRowLineDone(int y)
		{
			bool result = true;
			int num = 0;
			for (int i = 0; i < sizeX; i++)
			{
				if (isBlackMap[i, y])
				{
					num++;
				}
				else if (blocks[i, y] == null)
				{
					result = false;
					break;
				}
			}
			if (num == sizeX)
			{
				result = false;
			}
			return result;
		}

		protected bool IsColLineDone(int x)
		{
			bool result = true;
			int num = 0;
			for (int i = 0; i < sizeY; i++)
			{
				if (isBlackMap[x, i])
				{
					num++;
				}
				else if (blocks[x, i] == null)
				{
					result = false;
					break;
				}
			}
			if (num == sizeY)
			{
				result = false;
			}
			return result;
		}

		protected int RemoveRowLine(int y)
		{
			int num = 0;
			for (int i = 0; i < sizeX; i++)
			{
				if (!isBlackMap[i, y])
				{
					if (isBombMode && isBombed[i, y])
					{
						num += POINT_PER_BOMB;
						ClearBombAt(i, y);
					}
					if (blocks[i, y] != null)
					{
						num += POINT_PER_GRID;
						blocksNeedClear.Add(blocks[i, y]);
						blocks[i, y] = null;
					}
				}
			}
			return num;
		}

		protected int RemoveColLine(int x)
		{
			int num = 0;
			for (int i = 0; i < sizeY; i++)
			{
				if (!isBlackMap[x, i])
				{
					if (isBombMode && isBombed[x, i])
					{
						num += POINT_PER_BOMB;
						ClearBombAt(x, i);
					}
					if (blocks[x, i] != null)
					{
						num += POINT_PER_GRID;
						blocksNeedClear.Add(blocks[x, i]);
						blocks[x, i] = null;
					}
				}
			}
			return num;
		}

		public List<Block> GetBlocksNeedClear()
		{
			return blocksNeedClear;
		}

		public void AfterSync()
		{
			blocksNeedClear.Clear();
		}

		public int GetSizeX()
		{
			return sizeX;
		}

		public int GetSizeY()
		{
			return sizeY;
		}

		public bool[,] GetBlackMap()
		{
			return isBlackMap;
		}
	}
}
