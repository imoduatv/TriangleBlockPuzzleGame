using Prime31.ZestKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuUI
{
	[Header("Animation Elements")]
	[SerializeField]
	private GameObject m_AnimationUI;

	[SerializeField]
	private Image m_BackgroundImg;

	[Header("UI Elements")]
	[SerializeField]
	private Image m_HomeBtnImg;

	[SerializeField]
	private Image m_RestartBtnImg;

	[SerializeField]
	private Image m_ShopBtnImg;

	[SerializeField]
	private Image m_ResumeBtnImg;

	[SerializeField]
	private Image m_NoAdsBtnImg;

	[SerializeField]
	private Image m_SoundBtnImg;

	private Image m_HomeIcon;

	private Image m_RestartIcon;

	private Image m_ShopIcon;

	private Image m_ResumeIcon;

	private RectTransform m_AnimationUI_RT;

	private Action m_AfterShowCallback;

	private Action m_AfterHideCallBack;

	private UIManager m_UIManager;

	private TweenParty m_TweenPartyShow;

	private TweenParty m_TweenPartyHide;

	private Vector3 m_StartPos;

	private Vector3 m_DesPos;

	private float alphaBGMax;

	private Color m_BackgroundColor;

	private Color m_SoundColor;

	private ThemeElement m_SoundOn;

	private ThemeElement m_SoundOff;

	public override void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			m_HomeIcon = m_HomeBtnImg.transform.GetChild(0).GetComponent<Image>();
			m_RestartIcon = m_RestartBtnImg.transform.GetChild(0).GetComponent<Image>();
			m_ResumeIcon = m_ResumeBtnImg.transform.GetChild(0).GetComponent<Image>();
			m_ShopIcon = m_ShopBtnImg.transform.GetChild(0).GetComponent<Image>();
			m_UIManager = Singleton<UIManager>.instance;
			m_AnimationUI_RT = m_AnimationUI.GetComponent<RectTransform>();
			m_DesPos = Vector3.zero;
			m_StartPos = new Vector3(0f, -970f);
			m_AnimationUI_RT.anchoredPosition = m_StartPos;
		}
	}

	private void OnEnable()
	{
		Init();
	}

	public override void Show(Action afterShowCallback = null)
	{
		if (!m_IsAnimatingReward)
		{
			Init();
			m_AfterShowCallback = afterShowCallback;
			m_IsAnimatingReward = true;
			m_AnimationUI_RT.anchoredPosition = m_StartPos;
			Color backgroundColor = m_BackgroundColor;
			backgroundColor.a = 0f;
			m_BackgroundImg.color = backgroundColor;
			m_TweenPartyShow = new TweenParty(0.3f);
			m_TweenPartyShow.addTween(m_AnimationUI_RT.ZKanchoredPositionTo(m_DesPos).setEaseType(EaseType.BackOut));
			m_TweenPartyShow.addTween(m_BackgroundImg.ZKalphaTo(alphaBGMax).setEaseType(EaseType.BackOut));
			m_TweenPartyShow.setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterShowCallback != null)
				{
					m_AfterShowCallback();
				}
			});
			m_TweenPartyShow.resetTweenState();
			m_TweenPartyShow.start();
		}
	}

	public override void Hide(Action afterHideCallback = null)
	{
		if (!m_IsAnimatingReward)
		{
			Init();
			m_AfterHideCallBack = afterHideCallback;
			m_IsAnimatingReward = true;
			m_AnimationUI_RT.anchoredPosition = m_DesPos;
			m_BackgroundImg.color = m_BackgroundColor;
			m_TweenPartyHide = new TweenParty(0.3f);
			m_TweenPartyHide.addTween(m_AnimationUI_RT.ZKanchoredPositionTo(m_StartPos).setEaseType(EaseType.BackIn));
			m_TweenPartyHide.addTween(m_BackgroundImg.ZKalphaTo(0f).setEaseType(EaseType.BackIn));
			m_TweenPartyHide.resetTweenState();
			m_TweenPartyHide.setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterHideCallBack != null)
				{
					m_AfterHideCallBack();
				}
			});
			m_TweenPartyHide.start();
		}
	}

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
		SetUI(m_HomeBtnImg, dictThemeElement["HomeBtn"]);
		SetUI(m_RestartBtnImg, dictThemeElement["RestartBtn"]);
		SetUI(m_ResumeBtnImg, dictThemeElement["ResumeBtn"]);
		SetUI(m_ShopBtnImg, dictThemeElement["ShopBtn"]);
		SetUI(m_NoAdsBtnImg, dictThemeElement["NoAdsBtn"]);
		SetUI(m_BackgroundImg, dictThemeElement["Background"]);
		SetUI(m_RestartIcon, dictThemeElement["RestartIcon"]);
		SetUI(m_ResumeIcon, dictThemeElement["ResumeIcon"]);
		SetUI(m_HomeIcon, dictThemeElement["HomeIcon"]);
		SetUI(m_ShopIcon, dictThemeElement["ShopIcon"]);
		m_BackgroundColor = dictThemeElement["Background"].ColorUI;
		alphaBGMax = m_BackgroundColor.a;
		m_SoundColor = dictThemeElement["SettingBtn"].ColorUI;
		m_SoundOn = dictThemeElement["SoundOn"];
		m_SoundOff = dictThemeElement["SoundOff"];
		m_SoundBtnImg.color = m_SoundColor;
	}

	public void SetImgSound(bool isSoundOn)
	{
		SetUI(m_SoundBtnImg, (!isSoundOn) ? m_SoundOff : m_SoundOn);
		m_SoundBtnImg.SetNativeSize();
		m_SoundBtnImg.color = m_SoundColor;
	}
}
