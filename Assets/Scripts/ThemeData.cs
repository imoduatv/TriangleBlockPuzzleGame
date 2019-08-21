using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeData", menuName = "ThemeConfig/ThemeData")]
public class ThemeData : ScriptableObject
{
	[SerializeField]
	private ThemeName m_ThemeName;

	[SerializeField]
	private int m_ThemePrice;

	[SerializeField]
	private bool m_IsHaveBorder;

	[SerializeField]
	private bool m_IsSolidColorTheme;

	[Header("Shop")]
	[SerializeField]
	public string m_ShopName;

	public TextAsset BackGroundBytes;

	public int VideoNeedToUnlock;

	[Header("Main UI")]
	[SerializeField]
	private ThemeElement[] m_MainElements;

	[Header("Pause UI")]
	[SerializeField]
	private ThemeElement[] m_PauseElements;

	[Header("Setting UI")]
	[SerializeField]
	private ThemeElement[] m_SettingElements;

	[Header("In-game UI")]
	[SerializeField]
	private ThemeElement[] m_IngameElements;

	[Header("Game Over UI")]
	[SerializeField]
	private ThemeElement[] m_GameOverElements;

	[Header("Continue UI")]
	[SerializeField]
	private ThemeElement[] m_ContinueElements;

	[Header("No Ads UI")]
	[SerializeField]
	private ThemeElement[] m_NoAdsElements;

	[Header("Cell")]
	[SerializeField]
	private Sprite[] m_CellSprites;

	[SerializeField]
	private Color[] m_CellColors;

	[Header("Board")]
	[SerializeField]
	private Sprite m_BoardSprite;

	[SerializeField]
	private Color m_BoardColor;

	[SerializeField]
	private Color m_BoardColorLock;

	private Dictionary<string, ThemeElement> m_DictMainUI;

	private Dictionary<string, ThemeElement> m_DictPauseUI;

	private Dictionary<string, ThemeElement> m_DictSettingUI;

	private Dictionary<string, ThemeElement> m_DictIngameUI;

	private Dictionary<string, ThemeElement> m_DictOverUI;

	private Dictionary<string, ThemeElement> m_DictNoAdsUI;

	private Dictionary<string, ThemeElement> m_DictContinueUI;

	public int ThemePrice => m_ThemePrice;

	public bool IsSolidColorTheme => m_IsSolidColorTheme;

	public bool IsHaveBorder => m_IsHaveBorder;

	public Sprite[] CellSprites => m_CellSprites;

	public Color[] CellColors => m_CellColors;

	public Color BoardColor => m_BoardColor;

	public Color BoardColorLock => m_BoardColorLock;

	public Sprite BoardSprite => m_BoardSprite;

	public ThemeName Name => m_ThemeName;

	public Dictionary<string, ThemeElement> DictMainUI
	{
		get
		{
			if (m_DictMainUI == null)
			{
				m_DictMainUI = new Dictionary<string, ThemeElement>();
				ThemeElement[] mainElements = m_MainElements;
				foreach (ThemeElement themeElement in mainElements)
				{
					m_DictMainUI.Add(themeElement.NameUI, themeElement);
				}
			}
			return m_DictMainUI;
		}
	}

	public Dictionary<string, ThemeElement> DictPauseUI
	{
		get
		{
			if (m_DictPauseUI == null)
			{
				m_DictPauseUI = new Dictionary<string, ThemeElement>();
				ThemeElement[] pauseElements = m_PauseElements;
				foreach (ThemeElement themeElement in pauseElements)
				{
					m_DictPauseUI.Add(themeElement.NameUI, themeElement);
				}
			}
			return m_DictPauseUI;
		}
	}

	public Dictionary<string, ThemeElement> DictSettingUI
	{
		get
		{
			if (m_DictSettingUI == null)
			{
				m_DictSettingUI = new Dictionary<string, ThemeElement>();
				ThemeElement[] settingElements = m_SettingElements;
				foreach (ThemeElement themeElement in settingElements)
				{
					m_DictSettingUI.Add(themeElement.NameUI, themeElement);
				}
			}
			return m_DictSettingUI;
		}
	}

	public Dictionary<string, ThemeElement> DictIngameUI
	{
		get
		{
			if (m_DictIngameUI == null)
			{
				m_DictIngameUI = new Dictionary<string, ThemeElement>();
				ThemeElement[] ingameElements = m_IngameElements;
				foreach (ThemeElement themeElement in ingameElements)
				{
					m_DictIngameUI.Add(themeElement.NameUI, themeElement);
				}
			}
			return m_DictIngameUI;
		}
	}

	public Dictionary<string, ThemeElement> DictGameOverUI
	{
		get
		{
			if (m_DictOverUI == null)
			{
				m_DictOverUI = new Dictionary<string, ThemeElement>();
				ThemeElement[] gameOverElements = m_GameOverElements;
				foreach (ThemeElement themeElement in gameOverElements)
				{
					m_DictOverUI.Add(themeElement.NameUI, themeElement);
				}
			}
			return m_DictOverUI;
		}
	}

	public Dictionary<string, ThemeElement> DictNoAdsUI
	{
		get
		{
			if (m_DictNoAdsUI == null)
			{
				m_DictNoAdsUI = new Dictionary<string, ThemeElement>();
				ThemeElement[] noAdsElements = m_NoAdsElements;
				foreach (ThemeElement themeElement in noAdsElements)
				{
					m_DictNoAdsUI.Add(themeElement.NameUI, themeElement);
				}
			}
			return m_DictNoAdsUI;
		}
	}

	public Dictionary<string, ThemeElement> DictContinueUI
	{
		get
		{
			if (m_DictContinueUI == null)
			{
				m_DictContinueUI = new Dictionary<string, ThemeElement>();
				ThemeElement[] continueElements = m_ContinueElements;
				foreach (ThemeElement themeElement in continueElements)
				{
					m_DictContinueUI.Add(themeElement.NameUI, themeElement);
				}
			}
			return m_DictContinueUI;
		}
	}
}
