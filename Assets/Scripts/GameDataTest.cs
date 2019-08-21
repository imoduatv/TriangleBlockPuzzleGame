using UnityEngine;

[CreateAssetMenu(fileName = "GameDataTest", menuName = "GameData/GameDataTest")]
public class GameDataTest : ScriptableObject
{
	public int m_Money;

	public int m_Score;

	public int m_HighScore;

	public bool m_IsSound;

	public bool m_IsNoAds;

	public int m_PlayCount;

	public bool[] m_IsUnlockThemes = new bool[21];

	public ThemeName m_CurrentTheme;
}
