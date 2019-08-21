using UnityEngine;

public class ShapeBorder : MonoBehaviour
{
	[SerializeField]
	private GameObject m_Border;

	private SpriteRenderer m_BorderSpr;

	private bool m_IsEnable;

	private bool m_IsShow;

	private void Start()
	{
		LoadShapeBorder();
	}

	public void LoadShapeBorder()
	{
		if (m_BorderSpr == null)
		{
			m_BorderSpr = m_Border.GetComponent<SpriteRenderer>();
		}
	}

	public void Show(bool isShow)
	{
		ThemeName currentTheme = GameData.Instance().GetCurrentTheme();
		m_IsEnable = Singleton<ThemeManager>.instance.GetThemeData(currentTheme).IsHaveBorder;
		if (m_IsEnable)
		{
			m_Border.SetActive(isShow);
		}
		else
		{
			m_Border.SetActive(value: false);
		}
	}

	public void ReplaceColor(Color color)
	{
		m_BorderSpr.color = color;
	}

	public void SetEnableBorder(bool isEnable)
	{
		m_IsEnable = isEnable;
	}
}
