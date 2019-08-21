using UnityEngine;

public class TestManager : MonoBehaviour
{
	[Header("Data Test")]
	[SerializeField]
	private bool m_IsTest;

	[SerializeField]
	private GameDataTest m_DataTest;

	private GameData m_GameData;

	private void Awake()
	{
		m_GameData = GameData.Instance();
		if (m_IsTest)
		{
			m_GameData.HighScoreTest = m_DataTest.m_HighScore;
			m_GameData.IsAdsTest = !m_DataTest.m_IsNoAds;
			m_GameData.IsSoundTest = m_DataTest.m_IsSound;
			m_GameData.IsThemeUnlocksTest = m_DataTest.m_IsUnlockThemes;
			m_GameData.MoneyTest = m_DataTest.m_Money;
			m_GameData.ThemeCurrentTest = m_DataTest.m_CurrentTheme;
			m_GameData.SetPlayCount(m_DataTest.m_PlayCount);
			m_GameData.EnableTest(m_IsTest);
		}
	}
}
