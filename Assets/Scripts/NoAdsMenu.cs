using Prime31.ZestKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsMenu : MenuUI
{
	[Header("Animation Object")]
	[SerializeField]
	private GameObject m_AnimationObj;

	[Header("UI Elements")]
	[SerializeField]
	private Image m_BackgroundImg;

	[SerializeField]
	private Image m_RemoveAdsPanel;

	[SerializeField]
	private Image m_PremiumPanel;

	[SerializeField]
	private Image m_Pack1kPanel;

	[SerializeField]
	private Image m_Pack5kPanel;

	[SerializeField]
	private Image m_BackBtn;

	[SerializeField]
	private Text[] m_TextArr;

	[SerializeField]
	private Image[] m_ImageArr;

	[SerializeField]
	private Text m_TxtRemoveAds;

	[Header("Local Price")]
	[SerializeField]
	private Text m_NoAdsPrice;

	[SerializeField]
	private Text m_PremiumPrice;

	[SerializeField]
	private Text m_Pack1kPrice;

	[SerializeField]
	private Text m_Pack5kPrice;

	private Image m_BackIcon;

	private RectTransform m_AnimationRT;

	private Vector3 m_DesPos;

	private Vector3 m_StartPos;

	private Action m_AfterShowCallback;

	private Action m_AfterHideCallback;

	private List<GameObject> m_ListMenuUnder;

	public override void Init()
	{
		if (!m_IsInit)
		{
			m_ListMenuUnder = new List<GameObject>();
			m_BackIcon = m_BackBtn.transform.GetChild(0).GetComponent<Image>();
			m_IsInit = true;
			m_NoAdsPrice.text = InAppPurchaseController.Instance().GetLocalizedNoAdsIAP();
			m_Pack1kPrice.text = InAppPurchaseController.Instance().GetLocalizedPack1k();
			m_Pack5kPrice.text = InAppPurchaseController.Instance().GetLocalizedPack5k();
			m_PremiumPrice.text = InAppPurchaseController.Instance().GetLocalizedPremiumIAP();
			m_AnimationRT = m_AnimationObj.GetComponent<RectTransform>();
			m_DesPos = Vector3.zero;
			m_StartPos = new Vector3(0f, -970f);
			m_AnimationRT.anchoredPosition = m_StartPos;
		}
	}

	private void OnEnable()
	{
		Init();
	}

	public void SetListMenuUnder(List<GameObject> listMenuUnder)
	{
		m_ListMenuUnder.Clear();
		m_ListMenuUnder = listMenuUnder;
	}

	public override void Show(Action afterShowCallback = null)
	{
		Init();
		if (!m_IsAnimatingReward)
		{
			for (int i = 0; i < m_ListMenuUnder.Count; i++)
			{
				m_ListMenuUnder[i].SetActive(value: false);
			}
			m_AfterShowCallback = afterShowCallback;
			m_IsAnimatingReward = true;
			base.gameObject.SetActive(value: true);
			m_AnimationRT.ZKanchoredPositionTo(m_DesPos).setEaseType(EaseType.BackOut).setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterShowCallback != null)
				{
					m_AfterShowCallback();
				}
			})
				.start();
		}
	}

	public override void Hide(Action afterHideCallback = null)
	{
		Init();
		if (!m_IsAnimatingReward)
		{
			m_IsAnimatingReward = true;
			m_AfterHideCallback = afterHideCallback;
			m_AnimationRT.ZKanchoredPositionTo(m_StartPos).setEaseType(EaseType.BackIn).setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				base.gameObject.SetActive(value: false);
				for (int i = 0; i < m_ListMenuUnder.Count; i++)
				{
					m_ListMenuUnder[i].SetActive(value: true);
				}
				if (m_AfterHideCallback != null)
				{
					m_AfterHideCallback();
				}
			})
				.start();
		}
	}

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
		SetUI(m_BackgroundImg, dictThemeElement["Background"]);
		m_BackgroundImg.sprite = dictThemeElement["Background"].SpriteUI;
        //SetUI(m_BackBtn, dictThemeElement["BackBtn"]);
        SetUI(m_RemoveAdsPanel, dictThemeElement["RemoveAdsPanel"]);
        SetUI(m_PremiumPanel, dictThemeElement["PremiumPanel"]);
        //SetUI(m_BackIcon, dictThemeElement["BackIcon"]);
        SetUI(m_Pack1kPanel, dictThemeElement["Pack1kPanel"]);
        SetUI(m_Pack5kPanel, dictThemeElement["Pack5kPanel"]);
        SetUI(m_TxtRemoveAds, dictThemeElement["BackBtn"]);
        Text[] textArr = m_TextArr;
		foreach (Text text in textArr)
		{
			SetUI(text, dictThemeElement["Text"]);
		}
		Image[] imageArr = m_ImageArr;
		foreach (Image image in imageArr)
		{
			SetUI(image, dictThemeElement["Text"]);
		}
	}
}
