using System.Collections.Generic;

namespace Dta.TenTen
{
	public interface ITable
	{
		int GetSizeX();

		int GetSizeY();

		void Reset();

		Block[,] GetBlocks();

		int ClearLines();

		bool Place(Shape shape, GridCell position);

		bool IsPlaceable(Shape shape);

		bool IsTryPlaceOk(Shape shape, GridCell position);

		void AddToBlocksAfterTry(Shape shape, GridCell position);

		List<Block> GetBlocksNeedClear();

		void AfterSync();

		void SetBombMode(bool isBombMode);

		bool[,] GetIsBomb();

		void LoadTable(BlockData[,] blockDatas, bool[,] isBombs, int[,] bombTimes);

		int[,] GetBombTime();

		void Undo();

		void StopUndo();

		bool[,] GetBlackMap();
	}
}
