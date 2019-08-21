using Dta.TenTen;
using UnityEngine;

public class GameData
{
	private static GameData _instance;

	private int m_ScoreTest;

	private int m_HighScoreTest;

	private bool m_IsAdsTest;

	private bool m_IsSoundTest;

	private int m_MoneyTest;

	private ThemeName m_ThemeCurrentTest;

	private bool[] m_IsThemeUnlocksTest;

	private float tempFloatScore;

	private int score;

	private int highScore;

	private int money;

	private bool isSound;

	private bool isAds;

	private bool isPremium;

	private bool[] isThemeUnlocks;

	private int playCount;

	private int[] numVideoUnlocks;

	private int[] countCombo;

	private int skill3Cost;

	private int skillUndoCost;

	private int startSkill3Cost;

	private int startSkillUndoCost;

	private int snowFlake;

	private DailyStat[] dailyStats;

	private int seasonIndex;

	private ThemeName currentTheme = ThemeName.Night;

	private ThemeName currentTrialTheme = ThemeName.Night;

	private string isSoundKey = "IsSound";

	private string isAdsKey = "IsAds";

	private string isPremiumKey = "IsPremium";

	private string seasonKey = "Season";

	private string themeKey = "Theme";

	private string trialThemeKey = "Trial Theme";

	private string trialCountKey = "Trial Count";

	private string isCanTrialKey = "Is Can Trial";

	private string themeUnlockKey = "Theme Unlock";

	private string numVideoUnlockKey = "Num Vid Unlock";

	private string comboCountKey = "Combo";

	private string moneyKey = "Money";

	private string dailyKey = "Daily";

	private string playCountKey = "PlayCount";

	private string lb_normal = "LB_Normal";

	private string lb_normal_bomb = "LB_Normal_Bomb";

	private string lb_hex = "LB_Hex";

	private string lb_hex_bomb = "LB_Hex_Bomb";

	private string lb_tri = "LB_Tri";

	private string lb_tri_bomb = "LB_Tri_Bomb";

	private const string skill3ShapesCostKey = "Skill 3 cost";

	private const string skillUndoCostKey = "Skill undo cost";

	private const string rateKey = "Rate";

	private const string snowFlakeKey = "SnowFlake";

	private const string timePlayLastKey = "TimePlayLast";

	private const string moveCountKey = "MoveCountKey";

	public const int MONEY_EARN_VIDEO = 20;

	public const int MONEY_EARN_DAILY = 5;

	public const int SCORE_CONTINUE = 100;

	public int ScoreTest
	{
		set
		{
			m_ScoreTest = value;
		}
	}

	public int HighScoreTest
	{
		set
		{
			m_HighScoreTest = value;
		}
	}

	public bool IsAdsTest
	{
		set
		{
			m_IsAdsTest = value;
		}
	}

	public bool IsSoundTest
	{
		set
		{
			m_IsSoundTest = value;
		}
	}

	public ThemeName ThemeCurrentTest
	{
		set
		{
			m_ThemeCurrentTest = value;
		}
	}

	public bool[] IsThemeUnlocksTest
	{
		set
		{
			m_IsThemeUnlocksTest = value;
		}
	}

	public int MoneyTest
	{
		set
		{
			m_MoneyTest = value;
		}
	}

	public static GameData Instance()
	{
		if (_instance == null)
		{
			_instance = new GameData();
			_instance.Init();
		}
		return _instance;
	}

	private void Init()
	{
		score = 0;
		tempFloatScore = score;
		Singleton<ServicesManager>.Instance.InitServices(isShowAds: true, isHaveIAP: true);
		isSound = ServicesManager.DataNormal().GetBool(isSoundKey, defaultValue: true);
		isAds = ServicesManager.DataSecure().GetBool(isAdsKey, defaultValue: true);
		isPremium = ServicesManager.DataSecure().GetBool(isPremiumKey, defaultValue: false);
		money = ServicesManager.DataSecure().GetInt(moneyKey, 0);
		seasonIndex = ServicesManager.DataNormal().GetInt(seasonKey, 0);
		currentTheme = (ThemeName)ServicesManager.DataNormal().GetInt(themeKey, 1);
		currentTrialTheme = (ThemeName)ServicesManager.DataNormal().GetInt(trialThemeKey, 1);
		playCount = ServicesManager.DataNormal().GetInt(playCountKey, 0);
		snowFlake = ServicesManager.DataSecure().GetInt("SnowFlake", 0);
		isThemeUnlocks = new bool[21];
		numVideoUnlocks = new int[21];
		countCombo = new int[5];
		for (int i = 0; i < countCombo.Length; i++)
		{
			string text = comboCountKey + (i + 2).ToString();
			countCombo[i] = ServicesManager.DataSecure().GetInt(comboCountKey, 0);
		}
		for (int j = 0; j < isThemeUnlocks.Length; j++)
		{
			string key = themeUnlockKey + " " + j;
			isThemeUnlocks[j] = ServicesManager.DataSecure().GetBool(key, defaultValue: false);
		}
		for (int k = 0; k < isThemeUnlocks.Length; k++)
		{
			if (k == 0 || k == 1 || k == 3)
			{
				isThemeUnlocks[k] = (isThemeUnlocks[k] || true);
			}
			else
			{
				isThemeUnlocks[k] = (isThemeUnlocks[k] ? true : false);
			}
		}
		for (int l = 0; l < numVideoUnlocks.Length; l++)
		{
			string key2 = numVideoUnlockKey + " " + l;
			numVideoUnlocks[l] = ServicesManager.DataSecure().GetInt(key2, 0);
		}
		dailyStats = new DailyStat[28];
		for (int m = 0; m < dailyStats.Length; m++)
		{
			string key3 = dailyKey + (m + 1).ToString();
			dailyStats[m] = (DailyStat)ServicesManager.DataNormal().GetInt(key3, 3);
		}
	}

	public string GetLeaderBoardName(BoardType type, bool isBomb)
	{
		switch (type)
		{
		case BoardType.Normal:
			if (isBomb)
			{
				return lb_normal_bomb;
			}
			return lb_normal;
		case BoardType.Hexagonal:
			if (isBomb)
			{
				return lb_hex_bomb;
			}
			return lb_hex;
		case BoardType.Triangle:
			if (isBomb)
			{
				return lb_tri_bomb;
			}
			return lb_tri;
		default:
			return null;
		}
	}

	public int GetServerHighScore(BoardType type, bool isBomb)
	{
		string leaderBoardName = GetLeaderBoardName(type, isBomb);
		return ServicesManager.Score().GetBestHighScore(leaderBoardName);
	}

	public void CheckSendAllLocalHighScore()
	{
		UnityEngine.Debug.Log("Check send local highscore");
		SubmitHighScore(BoardType.Triangle, isBomb: false);
	}

	public int GetHighScore(BoardType type, bool isBomb)
	{
		string leaderBoardName = GetLeaderBoardName(type, isBomb);
		return ServicesManager.Score().GetBestHighScore(leaderBoardName);
	}

	public void SaveHighScore(BoardType type, bool isBomb, int highScore)
	{
		string leaderBoardName = GetLeaderBoardName(type, isBomb);
		ServicesManager.Score().SetNewBestHighScore(highScore, leaderBoardName);
	}

	public void UpdateHighScore(BoardType type, bool isBomb)
	{
		string leaderBoardName = GetLeaderBoardName(type, isBomb);
		ServicesManager.Score().SetNewBestHighScore(score, leaderBoardName);
	}

	public int GetSeasonIndex()
	{
		return seasonIndex;
	}

	public void SetSeasonIndex(int value)
	{
		seasonIndex = value;
		ServicesManager.DataNormal().SetInt(seasonKey, seasonIndex);
	}

	public ThemeName GetCurrentTheme()
	{
		return currentTheme;
	}

	public void SetCurrentTheme(ThemeName value)
	{
		currentTheme = value;
		ServicesManager.DataNormal().SetInt(themeKey, (int)currentTheme);
	}

	public void AddMoney(int value)
	{
		money += value;
		if (value > 0)
		{
		}
		ServicesManager.DataSecure().SetInt(moneyKey, money);
	}

	public int GetMoney()
	{
		return money;
	}

	public bool IsUnlockTheme(ThemeName value)
	{
		return isThemeUnlocks[(int)value];
	}

	public bool IsUnlockTheme(int index)
	{
		return isThemeUnlocks[index];
	}

	public void SetUnlockTheme(int index, bool value)
	{
		isThemeUnlocks[index] = value;
		string key = themeUnlockKey + " " + index;
		ServicesManager.DataSecure().SetBool(key, value);
	}

	public void SetUnlockTheme(ThemeName themeName, bool value)
	{
		isThemeUnlocks[(int)themeName] = value;
		string key = themeUnlockKey + " " + (int)themeName;
		ServicesManager.DataSecure().SetBool(key, value);
	}

	public bool GetIsSound()
	{
		return isSound;
	}

	public void SetIsSound(bool value)
	{
		isSound = value;
		ServicesManager.DataNormal().SetBool(isSoundKey, isSound);
	}

	public bool GetIsAds()
	{
		return isAds;
	}

	public bool GetIsPremium()
	{
		return isPremium;
	}

	public void SetIsPremium(bool value)
	{
		isPremium = value;
		ServicesManager.DataSecure().SetBool(isPremiumKey, isPremium);
	}

	public void SetIsAds(bool value)
	{
		isAds = value;
		ServicesManager.DataSecure().SetBool(isAdsKey, isAds);
	}

	public int GetCurrentScore()
	{
		return score;
	}

	public void StartScoring()
	{
		score = 0;
		tempFloatScore = 0f;
	}

	public void StartScoring(int value)
	{
		score = value;
		tempFloatScore = value;
	}

	public void AddScore(int value)
	{
		tempFloatScore += value;
		score = Mathf.RoundToInt(tempFloatScore);
	}

	public void AddScore(float value)
	{
		tempFloatScore += value;
		score = Mathf.RoundToInt(tempFloatScore);
	}

	public void SubmitHighScore(BoardType type, bool isBomb)
	{
		string leaderBoardName = GetLeaderBoardName(type, isBomb);
		ServicesManager.Score().SubmitBestHighScore(leaderBoardName);
	}

	public void SaveData()
	{
		PlayerPrefs.Save();
	}

	public void ClearData()
	{
		PlayerPrefs.DeleteAll();
	}

	public void EnableTest(bool value)
	{
		if (value)
		{
			isAds = m_IsAdsTest;
			isSound = m_IsSoundTest;
			highScore = m_HighScoreTest;
			money = m_MoneyTest;
			currentTheme = m_ThemeCurrentTest;
			isThemeUnlocks = m_IsThemeUnlocksTest;
		}
	}

	public DailyStat[] GetDailyStat()
	{
		return dailyStats;
	}

	public void SetDailyStat(DailyStat[] stats)
	{
		for (int i = 0; i < dailyStats.Length; i++)
		{
			string key = dailyKey + (i + 1).ToString();
			dailyStats[i] = stats[i];
			ServicesManager.DataNormal().SetInt(key, (int)stats[i]);
		}
	}

	public void SetNumVidUnlock(ThemeName name, int value)
	{
		numVideoUnlocks[(int)name] = value;
		string key = numVideoUnlockKey + " " + (int)name;
		ServicesManager.DataSecure().SetInt(key, value);
	}

	public void SetNumVidUnlock(int index, int value)
	{
		numVideoUnlocks[index] = value;
		string key = numVideoUnlockKey + " " + index;
		ServicesManager.DataSecure().SetInt(key, value);
	}

	public void AddNumVidUnlock(ThemeName name, int value)
	{
		numVideoUnlocks[(int)name] += value;
		string key = numVideoUnlockKey + " " + (int)name;
		ServicesManager.DataSecure().SetInt(key, numVideoUnlocks[(int)name]);
	}

	public void AddNumVidUnlock(int index, int value)
	{
		numVideoUnlocks[index] += value;
		string key = numVideoUnlockKey + " " + index;
		ServicesManager.DataSecure().SetInt(key, numVideoUnlocks[index]);
	}

	public void SetCountVideoUnlock(ThemeName name, int value)
	{
		string key = numVideoUnlockKey + " " + (int)name;
		numVideoUnlocks[(int)name] = value;
		ServicesManager.DataSecure().SetInt(key, value);
	}

	public int GetCountVideUnlock(ThemeName name)
	{
		return numVideoUnlocks[(int)name];
	}

	public void SetRate(bool value)
	{
		ServicesManager.DataNormal().SetBool("Rate", value);
	}

	public bool IsRate()
	{
		return ServicesManager.DataNormal().GetBool("Rate", defaultValue: false);
	}

	public void SetCountCombo(int clearCount, int value)
	{
		string key = comboCountKey + clearCount.ToString();
		countCombo[clearCount - 2] = value;
		ServicesManager.DataSecure().SetInt(key, value);
	}

	public int GetCountCombo(int clearCount)
	{
		return countCombo[clearCount - 2];
	}

	public void SetPlayCount(int value)
	{
		playCount = value;
		ServicesManager.DataNormal().SetInt(playCountKey, value);
	}

	public int GetPlayCount()
	{
		return playCount;
	}

	public int GetCostUndo()
	{
		return skillUndoCost;
	}

	public int GetCostNew3Shapes()
	{
		return skill3Cost;
	}

	public void SetCostUndo(int value)
	{
		skillUndoCost = value;
		ServicesManager.DataNormal().SetInt("Skill undo cost", skillUndoCost);
	}

	public void SetCostNew3Shapes(int value)
	{
		skill3Cost = value;
		ServicesManager.DataNormal().SetInt("Skill 3 cost", skill3Cost);
	}

	public void ClearCostSkill()
	{
		SetCostUndo(startSkillUndoCost);
		SetCostNew3Shapes(startSkill3Cost);
	}

	public void SetStartCost(int startSkill3Cost, int startSkillUndoCost)
	{
		this.startSkill3Cost = startSkill3Cost;
		this.startSkillUndoCost = startSkillUndoCost;
		skill3Cost = ServicesManager.DataNormal().GetInt("Skill 3 cost", startSkill3Cost);
		skillUndoCost = ServicesManager.DataNormal().GetInt("Skill undo cost", startSkillUndoCost);
	}

	public int GetCurrentSnowFlake()
	{
		return snowFlake;
	}

	public void SetSnowFlake(int value)
	{
		snowFlake = value;
		ServicesManager.DataSecure().SetInt("SnowFlake", snowFlake);
	}

	public void AddSnowFlake(int plus)
	{
		snowFlake += plus;
		ServicesManager.DataSecure().SetInt("SnowFlake", snowFlake);
	}

	public void SaveTimePlayLast(float timePlay)
	{
		ServicesManager.DataNormal().SetFloat("TimePlayLast", timePlay);
	}

	public void SaveMoveCount(int moveCount)
	{
		ServicesManager.DataNormal().SetInt("MoveCountKey", moveCount);
	}

	public float GetTimePlayLast()
	{
		return ServicesManager.DataNormal().GetFloat("TimePlayLast", 0f);
	}

	public int GetMoveCountLast()
	{
		return ServicesManager.DataNormal().GetInt("MoveCountKey", 0);
	}
}
