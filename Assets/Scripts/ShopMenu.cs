using Dta.TenTen;
using Prime31.ZestKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MenuUI
{
	[Header("Animation Obj")]
	[SerializeField]
	private GameObject m_AnimObj;

	[SerializeField]
	private GameObject m_ContentObj;

	[Header("UI Elements")]
	[SerializeField]
	private Text m_MoneyTxt;

	[SerializeField]
	private RectTransform m_MoneyPanelRect;

	[Header("Themes")]
	[SerializeField]
	private GameObject[] m_Themes;

	[SerializeField]
	private GameObject[] m_ObjCat;

	[SerializeField]
	private GameObject m_ThemeXmas;

	private GameObject[] m_BuyBtnsObj;

	private RectTransform[] m_BuyBtnRect;

	private RectTransform[] m_UseBtnRect;

	private GameObject[] m_UseBtnsObj;

	private RectTransform[] m_Preview;

	private RectTransform[] m_Name;

	private Text[] m_NameTxt;

	private RectTransform m_AnimObj_RT;

	private Text[] m_ConditionTxt;

	private Text[] m_VideoCountNeed;

	private Text[] m_PriceTxt;

	private RectTransform[] m_VideoImg;

	private RectTransform[] m_MultiText;

	private GameObject[] m_VideoUnlockBtn;

	private Action m_AfterShowCallback;

	private Action m_AfterHideCallback;

	private Vector3 m_StartPos;

	private Vector3 m_DesPos;

	private Vector3 m_ContentBeginPos;

	private Vector3 m_PosPreview = new Vector3(-165f, 0f, 0f);

	private Vector3 m_PosName = new Vector3(55f, 10f, 0f);

	private Vector3 m_PosBtn = new Vector3(220f, 0f, 0f);

	private Vector3 m_PosMoney = new Vector3(-125f, -94f, 0f);

	private Vector3 m_PreviewScale = Vector3.one * 0.6f * 0.8f;

	private Vector3 m_ButtonScale = Vector3.one * 0.8f;

	private Vector3 m_ContentPos = new Vector3(0f, 5600f, 0f);

	private RectTransform m_ContentRect;

	private List<GameObject> m_MenuUnderList;

	private bool m_IsOpenEvent;

	public override void Init()
	{
		if (m_IsInit)
		{
			ReloadData();
			return;
		}
		m_IsInit = true;
		m_MenuUnderList = new List<GameObject>();
		m_AnimObj_RT = m_AnimObj.GetComponent<RectTransform>();
		Vector2 anchoredPosition = m_AnimObj_RT.anchoredPosition;
		float y = anchoredPosition.y - m_AnimObj_RT.rect.height;
		m_StartPos = new Vector3(0f, y);
		m_DesPos = Vector3.zero;
		m_AnimObj_RT.anchoredPosition = m_StartPos;
		m_ContentBeginPos = m_ContentObj.transform.localPosition;
		m_BuyBtnsObj = new GameObject[m_Themes.Length];
		m_UseBtnsObj = new GameObject[m_Themes.Length];
		m_Preview = new RectTransform[m_Themes.Length];
		m_Name = new RectTransform[m_Themes.Length];
		m_BuyBtnRect = new RectTransform[m_Themes.Length];
		m_UseBtnRect = new RectTransform[m_Themes.Length];
		m_NameTxt = new Text[m_Themes.Length];
		m_ConditionTxt = new Text[m_Themes.Length];
		m_VideoCountNeed = new Text[m_Themes.Length];
		m_VideoUnlockBtn = new GameObject[m_Themes.Length];
		m_PriceTxt = new Text[m_Themes.Length];
		m_VideoImg = new RectTransform[m_Themes.Length];
		m_MultiText = new RectTransform[m_Themes.Length];
		m_ContentRect = m_ContentObj.GetComponent<RectTransform>();
		for (int i = 0; i < m_Themes.Length; i++)
		{
			m_BuyBtnsObj[i] = m_Themes[i].transform.GetChild(1).GetChild(0).gameObject;
			m_PriceTxt[i] = m_BuyBtnsObj[i].transform.GetChild(0).GetChild(0).GetComponent<Text>();
			m_BuyBtnRect[i] = m_BuyBtnsObj[i].GetComponent<RectTransform>();
			m_UseBtnsObj[i] = m_Themes[i].transform.GetChild(1).GetChild(1).gameObject;
			m_UseBtnRect[i] = m_UseBtnsObj[i].GetComponent<RectTransform>();
			m_Name[i] = m_Themes[i].transform.GetChild(1).GetChild(2).GetComponent<RectTransform>();
			m_NameTxt[i] = m_Name[i].GetComponent<Text>();
			m_Preview[i] = m_Themes[i].transform.GetChild(1).GetChild(3).GetComponent<RectTransform>();
			m_VideoUnlockBtn[i] = m_BuyBtnsObj[i].transform.GetChild(1).gameObject;
			m_ConditionTxt[i] = m_BuyBtnsObj[i].transform.GetChild(2).GetComponent<Text>();
			m_ConditionTxt[i].color = m_Name[i].GetComponent<Text>().color;
			m_VideoCountNeed[i] = m_VideoUnlockBtn[i].transform.GetChild(1).GetComponent<Text>();
			m_VideoImg[i] = m_VideoUnlockBtn[i].transform.GetChild(2).GetComponent<RectTransform>();
			m_MultiText[i] = m_VideoUnlockBtn[i].transform.GetChild(0).GetComponent<RectTransform>();
		}
		SetPrice();
		SetName();
		ReloadData();
	}

	private void SetPrice()
	{
		ThemeManager instance = Singleton<ThemeManager>.instance;
		for (int i = 0; i < m_Themes.Length; i++)
		{
			m_PriceTxt[i].text = instance.GetThemeData((ThemeName)i).ThemePrice.ToString();
		}
	}

	private void SetName()
	{
		ThemeManager instance = Singleton<ThemeManager>.instance;
		for (int i = 0; i < m_Themes.Length; i++)
		{
			m_NameTxt[i].text = instance.GetThemeData((ThemeName)i).m_ShopName;
		}
	}

	private void OnEnable()
	{
		Init();
	}

	public void SetListMenuUnder(List<GameObject> listMenuUnder)
	{
		m_MenuUnderList.Clear();
		m_MenuUnderList = listMenuUnder;
	}

	public override void Show(Action afterShowCallback = null)
	{
		Init();
		ReloadData();
		if (!m_IsAnimatingReward)
		{
			for (int i = 0; i < m_MenuUnderList.Count; i++)
			{
				m_MenuUnderList[i].SetActive(value: false);
			}
			m_IsAnimatingReward = true;
			m_AfterShowCallback = afterShowCallback;
			m_AnimObj_RT.anchoredPosition = m_StartPos;
			m_AnimObj_RT.ZKanchoredPositionTo(m_DesPos).setEaseType(EaseType.BackOut).setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterShowCallback != null)
				{
					m_AfterShowCallback();
				}
			})
				.start();
			if (m_IsOpenEvent)
			{
				m_ContentRect.anchoredPosition = m_ContentPos;
			}
		}
	}

	public override void Hide(Action afterHideCallback = null)
	{
		Init();
		if (!m_IsAnimatingReward)
		{
			for (int i = 0; i < m_MenuUnderList.Count; i++)
			{
				m_MenuUnderList[i].SetActive(value: true);
			}
			m_IsAnimatingReward = true;
			m_AfterHideCallback = afterHideCallback;
			m_AnimObj_RT.anchoredPosition = m_DesPos;
			m_AnimObj_RT.ZKanchoredPositionTo(m_StartPos).setEaseType(EaseType.BackIn).setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterHideCallback != null)
				{
					m_AfterHideCallback();
				}
			})
				.start();
		}
	}

	private void ResetContentPos()
	{
		m_ContentObj.transform.localPosition = m_ContentBeginPos;
	}

	public void ReloadData()
	{
		int money = GameData.Instance().GetMoney();
		ThemeManager instance = Singleton<ThemeManager>.instance;
		m_MoneyTxt.text = money.ToString();
		bool flag = false;
		Singleton<Achievement>.instance.CheckUnlockPlay(GameData.Instance().GetPlayCount());
		Singleton<Achievement>.instance.CheckUnlockScore(GameData.Instance().GetHighScore(BoardType.Triangle, isBomb: false));
		for (int i = 0; i < m_Themes.Length; i++)
		{
			flag = GameData.Instance().IsUnlockTheme(i);
			m_UseBtnsObj[i].SetActive(flag);
			m_BuyBtnsObj[i].SetActive(!flag);
			m_VideoUnlockBtn[i].SetActive(value: false);
			m_ConditionTxt[i].gameObject.SetActive(value: false);
		}
		List<ThemeData> themesWithCondition = instance.GetThemesWithCondition(TypeUnlock.Video);
		for (int j = 0; j < themesWithCondition.Count; j++)
		{
			int name = (int)themesWithCondition[j].Name;
			m_VideoUnlockBtn[name].SetActive(value: true);
			int countVideUnlock = GameData.Instance().GetCountVideUnlock(themesWithCondition[j].Name);
			int videoNeedToUnlock = themesWithCondition[j].VideoNeedToUnlock;
			int num = videoNeedToUnlock - countVideUnlock;
			m_ConditionTxt[name].gameObject.SetActive(value: false);
			m_VideoCountNeed[name].text = (videoNeedToUnlock - countVideUnlock).ToString();
			if (num < 10)
			{
				m_VideoImg[name].anchoredPosition = new Vector2(-93f, 0f);
				m_MultiText[name].anchoredPosition = new Vector2(76f, 2.5f);
				m_VideoCountNeed[name].GetComponent<RectTransform>().anchoredPosition = new Vector2(88f, 2.5f);
			}
			else
			{
				m_VideoImg[name].anchoredPosition = new Vector2(-102f, 0f);
				m_MultiText[name].anchoredPosition = new Vector2(66f, 2.5f);
				m_VideoCountNeed[name].GetComponent<RectTransform>().anchoredPosition = new Vector2(78f, 2.5f);
			}
		}
		List<ThemeData> themesWithCondition2 = instance.GetThemesWithCondition(TypeUnlock.Play);
		for (int k = 0; k < themesWithCondition2.Count; k++)
		{
			int name2 = (int)themesWithCondition2[k].Name;
			int playCount = GameData.Instance().GetPlayCount();
			int num2 = 50;
			m_ConditionTxt[name2].gameObject.SetActive(value: true);
			m_ConditionTxt[name2].text = "or play " + num2 + " times\n(" + playCount + "/" + num2 + ")";
		}
		List<ThemeData> themesWithCondition3 = instance.GetThemesWithCondition(TypeUnlock.Score);
		for (int l = 0; l < themesWithCondition3.Count; l++)
		{
			int name3 = (int)themesWithCondition3[l].Name;
			m_ConditionTxt[name3].gameObject.SetActive(value: true);
			m_ConditionTxt[name3].text = "or reach score " + 500 + "\n(" + GameData.Instance().GetHighScore(BoardType.Triangle, isBomb: false) + "/" + 500 + ")";
		}
		List<ThemeData> themesWithCondition4 = instance.GetThemesWithCondition(TypeUnlock.Combo);
		for (int m = 0; m < themesWithCondition4.Count; m++)
		{
			int name4 = (int)themesWithCondition4[m].Name;
			m_ConditionTxt[name4].gameObject.SetActive(value: true);
			m_ConditionTxt[name4].text = "or 5-line combo\n(" + GameData.Instance().GetCountCombo(5) + "/" + 1 + ")";
		}
	}

	public void SetUIForLongScreen()
	{
		for (int i = 0; i < m_Themes.Length; i++)
		{
			m_Preview[i].anchoredPosition = m_PosPreview;
			m_Preview[i].transform.localScale = m_PreviewScale;
			m_Name[i].anchoredPosition = m_PosName;
			m_BuyBtnRect[i].anchoredPosition = m_PosBtn;
			m_BuyBtnRect[i].localScale = m_ButtonScale;
			m_UseBtnRect[i].anchoredPosition = m_PosBtn;
			m_UseBtnRect[i].localScale = m_ButtonScale;
			m_NameTxt[i].resizeTextMaxSize = 45;
		}
		m_MoneyPanelRect.anchoredPosition = m_PosMoney;
	}

	public void OpenEvent(bool isOpen)
	{
		bool flag = GameData.Instance().IsUnlockTheme(ThemeName.Xmas);
		bool active = isOpen || flag;
		m_ThemeXmas.SetActive(active);
		m_IsOpenEvent = isOpen;
	}
}
