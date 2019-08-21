using System.Collections.Generic;

public class SaveGameData
{
	public List<ShapeData> m_ShapeData;

	public int m_CurrentScore;

	public BlockData[,] m_BlockDatas;

	public bool[,] m_IsBombs;

	public int[,] m_BombTimes;
}
