using Dta.TenTen;
using Entitas;
using System.Collections.Generic;

public class SaveGameManager
{
	private static SaveGameManager _instance;

	private const string m_SaveGameNormalKey = "SaveGameNormal";

	private const string m_SaveGameBombKey = "SaveGameBomb";

	private SaveGameData m_SaveDataNormal;

	private SaveGameData m_SaveDataBomb;

	private bool m_IsHasDataNormal;

	private bool m_IsHasDataBomb;

	public static SaveGameManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SaveGameManager();
				_instance.Init();
			}
			return _instance;
		}
	}

	public SaveGameData SaveDataNormal => m_SaveDataNormal;

	public SaveGameData SaveDataBomb => m_SaveDataBomb;

	public bool IsHasDataNormal => m_IsHasDataNormal;

	public bool IsHasDataBomb => m_IsHasDataBomb;

	private void Init()
	{
		m_SaveDataNormal = new SaveGameData();
		m_SaveDataBomb = new SaveGameData();
	}

	public void SaveShapes(bool isBomb)
	{
		List<ShapeData> list = new List<ShapeData>();
		Entity[] entities = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Shape, Matcher.Prefab)).GetEntities();
		Entity[] array = entities;
		foreach (Entity entity in array)
		{
			if (!entity.isDestroy)
			{
				ShapeData item = new ShapeData(entity.shape.data.map, entity.shape.data.ShapeId);
				list.Add(item);
			}
		}
		if (isBomb)
		{
			m_SaveDataBomb.m_ShapeData = list;
		}
		else
		{
			m_SaveDataNormal.m_ShapeData = list;
		}
	}

	public void SaveTable(bool isBomb, ITable table, Entity[,] board)
	{
		SaveGameData saveGameData = (!isBomb) ? m_SaveDataNormal : m_SaveDataBomb;
		Block[,] blocks = table.GetBlocks();
		bool[,] isBomb2 = table.GetIsBomb();
		int[,] bombTime = table.GetBombTime();
		int sizeX = table.GetSizeX();
		int sizeY = table.GetSizeY();
		saveGameData.m_BlockDatas = new BlockData[sizeX, sizeY];
		saveGameData.m_IsBombs = new bool[sizeX, sizeY];
		saveGameData.m_BombTimes = new int[sizeX, sizeY];
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				Block block = blocks[i, j];
				if (block != null)
				{
					if (board[i, j] == null)
					{
						continue;
					}
					BlockData blockData = new BlockData();
					int data = board[i, j].colorID.data;
					SaveBlocks(blockData, block, data);
					saveGameData.m_BlockDatas[i, j] = blockData;
				}
				saveGameData.m_IsBombs[i, j] = isBomb2[i, j];
				saveGameData.m_BombTimes[i, j] = bombTime[i, j];
			}
		}
	}

	public void SaveGame(bool isBomb, ITable table, Entity[,] board)
	{
		SaveGameData saveGameData = (!isBomb) ? m_SaveDataNormal : m_SaveDataBomb;
		saveGameData.m_CurrentScore = GameData.Instance().GetCurrentScore();
		SaveTable(isBomb, table, board);
		SaveShapes(isBomb);
		string key = (!isBomb) ? "SaveGameNormal" : "SaveGameBomb";
		string value = ServicesManager.Json().ToJson(saveGameData);
		ServicesManager.DataNormal().SetString(key, value);
	}

	private void SaveBlocks(BlockData blockData, Block block, int colorID)
	{
		blockData.color = block.Color();
		blockData.x = block.GetX();
		blockData.y = block.GetY();
		blockData.colorId = colorID;
	}

	public void LoadGame()
	{
		string @string = ServicesManager.DataNormal().GetString("SaveGameNormal", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			m_IsHasDataNormal = false;
		}
		else
		{
			m_IsHasDataNormal = true;
			m_SaveDataNormal = ServicesManager.Json().FromJson<SaveGameData>(@string);
		}
		string string2 = ServicesManager.DataNormal().GetString("SaveGameBomb", string.Empty);
		if (string.IsNullOrEmpty(string2))
		{
			m_IsHasDataBomb = false;
			return;
		}
		m_IsHasDataBomb = true;
		m_SaveDataBomb = ServicesManager.Json().FromJson<SaveGameData>(string2);
	}

	public void ClearData(bool isBomb)
	{
		string empty = string.Empty;
		if (isBomb)
		{
			m_IsHasDataBomb = false;
			ServicesManager.DataNormal().SetString("SaveGameBomb", empty);
		}
		else
		{
			m_IsHasDataNormal = false;
			ServicesManager.DataNormal().SetString("SaveGameNormal", empty);
		}
	}
}
