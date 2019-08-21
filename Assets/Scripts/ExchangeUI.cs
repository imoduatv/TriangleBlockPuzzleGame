using UnityEngine;
using UnityEngine.UI;

public class ExchangeUI : MonoBehaviour
{
	[SerializeField]
	private ExchangeItem m_Item;

	[SerializeField]
	private ExchangeData m_Data;

	private Image m_BG;

	private Image m_Icon;

	private Text m_Content;

	private GameObject m_ExchangeObj;

	private GameObject m_ClaimedObj;

	private Image m_PriceType;

	private Text m_Price;

	private Button m_ExchangeBtn;

	private ExchangePanel m_ExchangePanel;

	public void Init(ExchangePanel panel)
	{
		m_ExchangePanel = panel;
		m_BG = GetComponent<Image>();
		m_Icon = base.transform.GetChild(0).GetComponent<Image>();
		m_Content = base.transform.GetChild(1).GetComponent<Text>();
		m_ExchangeObj = base.transform.GetChild(2).GetChild(0).gameObject;
		m_ClaimedObj = base.transform.GetChild(2).GetChild(1).gameObject;
		m_Price = m_ExchangeObj.transform.GetChild(0).GetComponent<Text>();
		m_PriceType = m_ExchangeObj.transform.GetChild(1).GetComponent<Image>();
		m_ExchangeBtn = m_ExchangeObj.GetComponent<Button>();
		m_ExchangeBtn.onClick.AddListener(delegate
		{
			if (m_Item.PriceType == ExchangeItemType.Diamond)
			{
				int money = GameData.Instance().GetMoney();
				if (money < m_Item.PriceValue)
				{
					Singleton<UIManager>.instance.ShowNotEnough();
					return;
				}
				GameData.Instance().AddMoney(-m_Item.PriceValue);
			}
			else if (m_Item.PriceType == ExchangeItemType.SnowFlake)
			{
				int currentSnowFlake = GameData.Instance().GetCurrentSnowFlake();
				if (currentSnowFlake < m_Item.PriceValue)
				{
					Singleton<UIManager>.instance.ShowNotEnoughSnowFlake();
					return;
				}
				GameData.Instance().AddSnowFlake(-m_Item.PriceValue);
			}
			if (m_Item.ItemType == ExchangeItemType.Diamond)
			{
				GameData.Instance().AddMoney(m_Item.DiamondValue);
			}
			else if (m_Item.ItemType == ExchangeItemType.SnowFlake)
			{
				GameData.Instance().AddSnowFlake(m_Item.SnowFlake);
			}
			else
			{
				GameData.Instance().SetUnlockTheme(m_Item.Theme, value: true);
				m_ExchangeObj.SetActive(value: false);
				m_ClaimedObj.SetActive(value: true);
			}
			Singleton<UIManager>.instance.ReloadData();
			m_ExchangePanel.ReloadSnowFlakeCount();
		});
		SetData();
	}

	private void SetData()
	{
		if (m_Item.PriceType == ExchangeItemType.Diamond)
		{
			m_PriceType.sprite = m_Data.Diamond;
		}
		else
		{
			m_PriceType.sprite = m_Data.SnowFlake;
		}
		if (m_Item.ItemType == ExchangeItemType.SnowFlake)
		{
			m_BG.sprite = m_Data.SnowFlakeBG;
			m_Content.text = m_Item.SnowFlake.ToString();
			m_Content.color = m_Data.ContentSnowColor;
			m_Icon.sprite = m_Data.SnowFlake;
			m_Icon.transform.localScale = Vector3.one * 0.7f;
		}
		else
		{
			m_BG.sprite = m_Data.DiamondBG;
			m_Content.color = m_Data.ContentDiamondColor;
			if (m_Item.ItemType == ExchangeItemType.Diamond)
			{
				m_Icon.sprite = m_Data.Diamond;
				m_Icon.transform.localScale = Vector3.one * 0.7f;
				m_Content.text = m_Item.DiamondValue.ToString();
			}
			else
			{
				m_Content.text = m_Item.Theme.ToString() + "\nTheme";
				m_Content.fontSize = 32;
				m_Icon.transform.localScale = Vector3.one;
				switch (m_Item.Theme)
				{
				case ThemeName.Xmas:
					m_Icon.sprite = m_Data.XmasTheme;
					break;
				case ThemeName.Farm:
					m_Icon.sprite = m_Data.FarmTheme;
					break;
				case ThemeName.Ocean:
					m_Icon.sprite = m_Data.OceanTheme;
					break;
				case ThemeName.Space:
					m_Icon.sprite = m_Data.SpaceTheme;
					break;
				}
			}
		}
		m_Price.text = m_Item.PriceValue.ToString();
		if (m_Item.ItemType == ExchangeItemType.Theme)
		{
			bool flag = GameData.Instance().IsUnlockTheme(m_Item.Theme);
			m_ExchangeObj.SetActive(!flag);
			m_ClaimedObj.SetActive(flag);
		}
	}
}
