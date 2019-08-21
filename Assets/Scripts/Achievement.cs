using System.Collections.Generic;

public class Achievement : Singleton<Achievement>
{
	public const int PLAY_UNLOCK = 50;

	public const int SCORE_UNLOCK = 500;

	public const int COMBO_5_UNLOCK = 1;

	private bool m_IsInit;

	private Dictionary<int, int> m_ComboUnlockDict;

	private void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			m_ComboUnlockDict = new Dictionary<int, int>();
			m_ComboUnlockDict.Add(2, -1);
			m_ComboUnlockDict.Add(3, -1);
			m_ComboUnlockDict.Add(4, -1);
			m_ComboUnlockDict.Add(5, 1);
			m_ComboUnlockDict.Add(6, -1);
		}
	}

	public void CheckUnlockPlay(int value)
	{
		if (value >= 50 && !GameData.Instance().IsUnlockTheme(ThemeName.Purple))
		{
			Analytic.Instance.LogUnlockTheme50Plays();
			GameData.Instance().SetUnlockTheme(ThemeName.Purple, value: true);
		}
	}

	public void CheckUnlockScore(int value)
	{
		if (value >= 500 && !GameData.Instance().IsUnlockTheme(ThemeName.BlackWhite))
		{
			Analytic.Instance.LogUnlock500Score();
			GameData.Instance().SetUnlockTheme(ThemeName.BlackWhite, value: true);
		}
	}

	public void CheckUnlockCombo(int clearCount, int value)
	{
		Init();
		if (m_ComboUnlockDict[clearCount] >= 0 && value >= m_ComboUnlockDict[clearCount] && clearCount == 5 && !GameData.Instance().IsUnlockTheme(ThemeName.Ice))
		{
			GameData.Instance().SetUnlockTheme(ThemeName.Ice, value: true);
			Analytic.Instance.LogUnlockCombo5();
		}
	}
}
