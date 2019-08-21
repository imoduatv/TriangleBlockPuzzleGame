using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : Singleton<ThemeManager>
{
	[SerializeField]
	private ThemeData[] m_Themes;

	private bool m_IsInit;

	private Dictionary<ThemeName, ThemeData> m_DictTheme;

	private Dictionary<TypeUnlock, List<ThemeData>> m_DictUnlockTypeTheme;

	private GameData gameData;

	private void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			m_DictTheme = new Dictionary<ThemeName, ThemeData>();
			ThemeData[] themes = m_Themes;
			foreach (ThemeData themeData in themes)
			{
				m_DictTheme.Add(themeData.Name, themeData);
			}
			gameData = GameData.Instance();
			m_DictUnlockTypeTheme = new Dictionary<TypeUnlock, List<ThemeData>>();
			List<ThemeData> list = new List<ThemeData>();
			list.Add(m_DictTheme[ThemeName.Soft]);
			list.Add(m_DictTheme[ThemeName.Bee]);
			list.Add(m_DictTheme[ThemeName.Candy]);
			list.Add(m_DictTheme[ThemeName.Baby]);
			list.Add(m_DictTheme[ThemeName.Cat]);
			list.Add(m_DictTheme[ThemeName.Forest]);
			list.Add(m_DictTheme[ThemeName.Ocean]);
			list.Add(m_DictTheme[ThemeName.Season]);
			list.Add(m_DictTheme[ThemeName.Space]);
			list.Add(m_DictTheme[ThemeName.Farm]);
			list.Add(m_DictTheme[ThemeName.Sea]);
			list.Add(m_DictTheme[ThemeName.City]);
			list.Add(m_DictTheme[ThemeName.Mountain]);
			list.Add(m_DictTheme[ThemeName.Pyramid]);
			m_DictUnlockTypeTheme.Add(TypeUnlock.Video, list);
			List<ThemeData> list2 = new List<ThemeData>();
			list2.Add(m_DictTheme[ThemeName.BlackWhite]);
			m_DictUnlockTypeTheme.Add(TypeUnlock.Score, list2);
			List<ThemeData> list3 = new List<ThemeData>();
			list3.Add(m_DictTheme[ThemeName.Purple]);
			m_DictUnlockTypeTheme.Add(TypeUnlock.Play, list3);
			List<ThemeData> list4 = new List<ThemeData>();
			list4.Add(m_DictTheme[ThemeName.Ice]);
			m_DictUnlockTypeTheme.Add(TypeUnlock.Combo, list4);
		}
	}

	public ThemeData GetThemeData(ThemeName themeName)
	{
		Init();
		return m_DictTheme[themeName];
	}

	public bool IsUnlockTheme(ThemeName name)
	{
		Init();
		return gameData.IsUnlockTheme(name);
	}

	public List<ThemeData> GetThemesWithCondition(TypeUnlock type)
	{
		Init();
		return m_DictUnlockTypeTheme[type];
	}

	public bool IsThemeBannerTop(ThemeName curTheme)
	{
		if (curTheme == ThemeName.Day || curTheme == ThemeName.Night || curTheme == ThemeName.Soft || curTheme == ThemeName.Bee || curTheme == ThemeName.BlackWhite || curTheme == ThemeName.Purple)
		{
			return true;
		}
		return false;
	}
}
