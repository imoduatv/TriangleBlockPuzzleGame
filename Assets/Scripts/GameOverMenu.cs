using Prime31.ZestKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MenuUI
{
	[Header("Animation Obj")]
	[SerializeField]
	private GameObject m_InfoZone;

	[SerializeField]
	private GameObject m_ButtonsZone;

	[Header("Reward Animation")]
	[SerializeField]
	private RectTransform[] m_RewardMoney;

	[Header("UI Elements")]
	[SerializeField]
	private Image m_BackgroundImg;

	[SerializeField]
	private Image m_InfoPanel;

	[SerializeField]
	private Text m_ScoreTxt;

	[SerializeField]
	private Image m_TrophyImg;

	[SerializeField]
	private Text m_HighScoreTxt;

	[SerializeField]
	private Image m_HomeBtnImg;

	[SerializeField]
	private Image m_RestartBtnImg;

	[SerializeField]
	private Image m_RateBtnImg;

	[SerializeField]
	private Image m_LBNativeImg;

	[SerializeField]
	private Image m_ShopBtnImg;

	[SerializeField]
	private Image m_VideoAdsBtnImg;

	[SerializeField]
	private Text m_MoneyTxt;

	[SerializeField]
	private Image m_NoAdsBtnImg;

	[SerializeField]
	private Image m_SettingBtnImg;

	[SerializeField]
	private Image m_MoneyPanelImg;

	[SerializeField]
	private Image m_InfoBG;

	[SerializeField]
	private Image m_ShareBtnImg;

	[Header("VideoAdsStat")]
	[SerializeField]
	private Button m_VideoBtn;

	[SerializeField]
	private GameObject m_VideoAvaiObj;

	[SerializeField]
	private Image m_VideoAvai;

	[SerializeField]
	private GameObject m_VideoNotAvaiObj;

	[SerializeField]
	private Image[] m_VideoNotAvai;

	[Space(5f)]
	[SerializeField]
	private EventButton m_EventBtn;

	private Color m_EnableColor;

	private Color m_DisableColor;

	private Color m_EnableIconColor;

	private Color m_DisableIconColor;

	private Button m_VideoAdsBtn;

	private Image m_PreviewBG;

	private Image m_VideoImgStat;

	private RectTransform m_InfoZone_RT;

	private RectTransform m_ButtonsZone_RT;

	private Vector3 m_InfoStartPos;

	private Vector3 m_InfoDesPos;

	private Vector3 m_ButtonsStartPos;

	private Vector3 m_ButtonsDesPos;

	private Vector3 m_StartPos;

	private Vector3 m_DesPos;

	private Action m_AfterShowCallback;

	private Action m_AfterHideCallback;

	private Action m_AfterAnimCallback;

	private TweenParty m_TweenPartyShow;

	private TweenParty m_TweenPartyHide;

	private Spline m_TweenPosSpline;

	private Spline m_TweenScaleSpline;

	private Spline m_TweenPosSplineDaily;

	private Image m_HomeIcon;

	private Image m_RestartIcon;

	private Image m_RateIcon;

	private Image m_ShopIcon;

	private Image m_LBNativeIcon;

	private Coroutine m_CheckLoadCorou;

	private int[] moneyPlusArr;

	private WaitForSeconds m_WaitAnim = new WaitForSeconds(0.3f);

	private WaitForSeconds oneSec = new WaitForSeconds(1f);

	public override void Init()
	{
		if (m_IsInit)
		{
			ReloadData();
			return;
		}
		m_IsInit = true;
		m_RateBtnImg.gameObject.SetActive(value: true);
		m_LBNativeImg.gameObject.SetActive(value: false);
		m_HomeIcon = m_HomeBtnImg.transform.GetChild(0).GetComponent<Image>();
		m_RestartIcon = m_RestartBtnImg.transform.GetChild(0).GetComponent<Image>();
		m_RateIcon = m_RateBtnImg.transform.GetChild(0).GetComponent<Image>();
		m_ShopIcon = m_ShopBtnImg.transform.GetChild(0).GetComponent<Image>();
		m_LBNativeIcon = m_LBNativeImg.transform.GetChild(0).GetComponent<Image>();
		m_VideoAdsBtn = m_VideoAdsBtnImg.GetComponent<Button>();
		m_InfoZone_RT = m_InfoZone.GetComponent<RectTransform>();
		m_ButtonsZone_RT = m_ButtonsZone.GetComponent<RectTransform>();
		m_PreviewBG = m_InfoZone.transform.GetChild(1).GetChild(3).GetComponent<Image>();
		m_InfoDesPos = m_InfoZone_RT.anchoredPosition;
		Vector2 anchoredPosition = m_InfoZone_RT.anchoredPosition;
		float y = anchoredPosition.y + m_InfoZone_RT.rect.height;
		m_InfoStartPos = new Vector3(0f, y);
		m_InfoZone_RT.anchoredPosition = m_InfoStartPos;
		m_ButtonsDesPos = m_ButtonsZone_RT.anchoredPosition;
		Vector2 anchoredPosition2 = m_ButtonsZone_RT.anchoredPosition;
		float y2 = anchoredPosition2.y - m_ButtonsZone_RT.rect.height;
		m_ButtonsStartPos = new Vector3(0f, y2);
		m_ButtonsZone_RT.anchoredPosition = m_ButtonsStartPos;
		m_DesPos = m_MoneyPanelImg.GetComponent<RectTransform>().anchoredPosition;
		m_StartPos = m_VideoAdsBtnImg.GetComponent<RectTransform>().anchoredPosition;
		Vector3[] nodes = new Vector3[3]
		{
			m_StartPos,
			new Vector3(-100f, -107f, 0f),
			m_DesPos
		};
		Vector3[] nodes2 = new Vector3[3]
		{
			Vector3.zero,
			new Vector3(-100f, m_DesPos.y / 2f, 0f),
			m_DesPos
		};
		Vector3[] nodes3 = new Vector3[3]
		{
			Vector3.one * 0.5f,
			Vector3.one * 1.2f,
			Vector3.one * 0.5f
		};
		m_TweenPosSpline = new Spline(nodes);
		m_TweenPosSpline.closePath();
		m_TweenScaleSpline = new Spline(nodes3);
		m_TweenScaleSpline.closePath();
		m_TweenPosSplineDaily = new Spline(nodes2);
		m_TweenPosSplineDaily.closePath();
		moneyPlusArr = new int[m_RewardMoney.Length];
		m_EventBtn.Init();
		m_EventBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			Singleton<UIManager>.instance.ShowExchangePanel();
		});
		ReloadData();
	}

	private void OnEnable()
	{
		Init();
		CheckLoadVideo();
	}

	private void OnDisable()
	{
		if (m_CheckLoadCorou != null)
		{
			StopCoroutine(m_CheckLoadCorou);
			m_CheckLoadCorou = null;
		}
	}

	public void ReloadData()
	{
		ShowScore();
		ShowMoney();
		m_EventBtn.ReloadSnowFlakeCount();
	}

	public override void Show(Action afterShowCallback = null)
	{
		Init();
		if (!m_IsAnimatingReward)
		{
			m_IsAnimatingReward = true;
			m_AfterShowCallback = afterShowCallback;
			m_InfoZone_RT.anchoredPosition = m_InfoStartPos;
			m_ButtonsZone_RT.anchoredPosition = m_ButtonsStartPos;
			m_TweenPartyShow = new TweenParty(0.3f);
			m_TweenPartyShow.addTween(m_InfoZone_RT.ZKanchoredPositionTo(m_InfoDesPos).setEaseType(EaseType.BackOut));
			m_TweenPartyShow.addTween(m_ButtonsZone_RT.ZKanchoredPositionTo(m_ButtonsDesPos).setEaseType(EaseType.BackOut));
			m_TweenPartyShow.resetTweenState();
			m_TweenPartyShow.setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterShowCallback != null)
				{
					m_AfterShowCallback();
				}
			});
			m_TweenPartyShow.start();
		}
	}

	public override void Hide(Action afterHideCallback = null)
	{
		Init();
		if (!m_IsAnimatingReward)
		{
			m_IsAnimatingReward = true;
			m_AfterHideCallback = afterHideCallback;
			m_InfoZone_RT.anchoredPosition = m_InfoDesPos;
			m_ButtonsZone_RT.anchoredPosition = m_ButtonsDesPos;
			m_TweenPartyHide = new TweenParty(0.3f);
			m_TweenPartyHide.addTween(m_InfoZone_RT.ZKanchoredPositionTo(m_InfoStartPos).setEaseType(EaseType.BackIn));
			m_TweenPartyHide.addTween(m_ButtonsZone_RT.ZKanchoredPositionTo(m_ButtonsStartPos).setEaseType(EaseType.BackIn));
			m_TweenPartyHide.resetTweenState();
			m_TweenPartyHide.setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterHideCallback != null)
				{
					m_AfterHideCallback();
				}
			});
			m_TweenPartyHide.start();
		}
	}

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
		SetUI(m_BackgroundImg, dictThemeElement["Background"]);
		m_BackgroundImg.sprite = dictThemeElement["Background"].SpriteUI;
		SetUI(m_InfoPanel, dictThemeElement["InfoPanel"]);
		SetUI(m_ScoreTxt, dictThemeElement["ScoreTxt"]);
		SetUI(m_TrophyImg, dictThemeElement["Trophy"]);
		SetUI(m_HighScoreTxt, dictThemeElement["HighScoreTxt"]);
		SetUI(m_HomeBtnImg, dictThemeElement["HomeBtn"]);
		SetUI(m_RestartBtnImg, dictThemeElement["RestartBtn"]);
		SetUI(m_RateBtnImg, dictThemeElement["RateBtn"]);
		SetUI(m_ShopBtnImg, dictThemeElement["ShopBtn"]);
		SetUI(m_VideoAdsBtnImg, dictThemeElement["VideoAdsBtn"]);
		SetUI(m_MoneyTxt, dictThemeElement["MoneyTxt"]);
		SetUI(m_NoAdsBtnImg, dictThemeElement["NoAdsBtn"]);
		SetUI(m_SettingBtnImg, dictThemeElement["SettingBtn"]);
		SetUI(m_MoneyPanelImg, dictThemeElement["MoneyPanel"]);
		SetUI(m_ShareBtnImg, dictThemeElement["ShareBtn"]);
		SetUI(m_ShopIcon, dictThemeElement["ShopIcon"]);
		SetUI(m_HomeIcon, dictThemeElement["HomeIcon"]);
		SetUI(m_RestartIcon, dictThemeElement["RestartIcon"]);
		SetUI(m_RateIcon, dictThemeElement["RateIcon"]);
		SetUI(m_LBNativeImg, dictThemeElement["RateBtn"]);
		SetUI(m_LBNativeIcon, dictThemeElement["RateIcon"]);
		SetUI(m_PreviewBG, dictThemeElement["PreviewBG"]);
		m_EnableColor = dictThemeElement["VideoAdsBtn"].ColorUI;
		m_DisableColor = dictThemeElement["NoVideoBtn"].ColorUI;
		m_EnableIconColor = dictThemeElement["VideoAdsIcon"].ColorUI;
		m_DisableIconColor = dictThemeElement["NoVideoIcon"].ColorUI;
		m_VideoAvai.color = m_EnableIconColor;
		for (int i = 0; i < m_VideoNotAvai.Length; i++)
		{
			m_VideoNotAvai[i].color = m_DisableIconColor;
		}
		m_InfoBG.color = m_InfoPanel.color;
	}

	public void SetVideoAdsStat(VideoAdsStat stat)
	{
		switch (stat)
		{
		case VideoAdsStat.Available:
			m_VideoBtn.gameObject.SetActive(value: true);
			m_VideoAvaiObj.SetActive(value: true);
			m_VideoNotAvaiObj.SetActive(value: false);
			m_VideoBtn.interactable = true;
			break;
		case VideoAdsStat.Hide:
			m_VideoBtn.gameObject.SetActive(value: false);
			break;
		default:
			m_VideoBtn.gameObject.SetActive(value: true);
			m_VideoAvaiObj.SetActive(value: false);
			m_VideoNotAvaiObj.SetActive(value: true);
			m_VideoBtn.interactable = false;
			break;
		}
	}

	public void StartAnimReward(int money, bool isDaily = false, Action afterAnimCallback = null)
	{
		m_AfterAnimCallback = afterAnimCallback;
		StartCoroutine(IE_Anim(money, isDaily));
	}

	private IEnumerator IE_Anim(int money, bool isDaily)
	{
		int i = 0;
		int len = m_RewardMoney.Length;
		int index = 0;
		int preMoney = GameData.Instance().GetMoney() - money;
		int plusMoney = 0;
		int lenMoney = moneyPlusArr.Length;
		for (int j = 0; j < lenMoney; j++)
		{
			moneyPlusArr[j] = (money - plusMoney) / (lenMoney - j);
			plusMoney += moneyPlusArr[j];
		}
		while (i < len)
		{
			m_RewardMoney[i].anchoredPosition = m_StartPos;
			m_RewardMoney[i].gameObject.SetActive(value: true);
			m_RewardMoney[i].transform.localScale = Vector3.one * 0.5f;
			TweenChain tweenChain = new TweenChain();
			TweenParty tweenParty = new TweenParty(1f);
			SplineRectTrans splinePosTween = isDaily ? new SplineRectTrans(m_RewardMoney[i], m_TweenPosSplineDaily, 1f) : new SplineRectTrans(m_RewardMoney[i], m_TweenPosSpline, 1f);
			SplineScale splineScaleTween = new SplineScale(m_RewardMoney[i].transform, m_TweenScaleSpline, 1f);
			tweenParty.addTween(splinePosTween).addTween(splineScaleTween).setEaseType(EaseType.Linear)
				.setCompletionHandler(delegate
				{
					m_RewardMoney[index].gameObject.SetActive(value: false);
				});
			tweenParty.resetTweenState();
			tweenChain.appendTween(tweenParty).appendTween(m_MoneyPanelImg.transform.ZKlocalScaleTo(Vector3.one * 1.2f, 0.05f).setLoops(LoopType.PingPong).setEaseType(EaseType.Linear));
			tweenChain.setCompletionHandler(delegate
			{
				preMoney += moneyPlusArr[index];
				ShowMoney(preMoney);
				if (index == len - 1)
				{
					if (m_AfterAnimCallback != null)
					{
						m_AfterAnimCallback();
					}
					m_IsAnimatingReward = false;
				}
				index++;
			});
			tweenChain.start();
			i++;
			yield return m_WaitAnim;
		}
	}

	private void ShowScore()
	{
		GameManager instance = Singleton<GameManager>.instance;
		GameData gameData = GameData.Instance();
		m_ScoreTxt.text = gameData.GetCurrentScore().ToString();
		int highScore = gameData.GetHighScore(instance.gameType, instance.isBomb);
		m_HighScoreTxt.text = highScore.ToString();
		int num = 50;
		num = ((highScore >= 0 && highScore < 1000) ? 50 : ((highScore >= 10000) ? 40 : 45));
		m_ScoreTxt.fontSize = num;
		m_HighScoreTxt.fontSize = num;
	}

	private void ShowMoney()
	{
		m_MoneyTxt.text = GameData.Instance().GetMoney().ToString();
	}

	private void ShowMoney(int value)
	{
		m_MoneyTxt.text = value.ToString();
	}

	public void CheckLoadVideo()
	{
		if (m_CheckLoadCorou != null)
		{
			StopCoroutine(m_CheckLoadCorou);
			m_CheckLoadCorou = null;
		}
		if (!Singleton<ServicesManager>.instance.IsRewardAvailable())
		{
			SetVideoAdsStat(VideoAdsStat.NotAvailable);
			m_CheckLoadCorou = StartCoroutine(IE_CheckLoadVideo());
		}
		else
		{
			SetVideoAdsStat(VideoAdsStat.Available);
		}
	}

	private IEnumerator IE_CheckLoadVideo()
	{
		while (!Singleton<ServicesManager>.instance.IsRewardAvailable())
		{
			yield return oneSec;
		}
		SetVideoAdsStat(VideoAdsStat.Available);
	}

	public void SetTimeCount(int d, int h, int m, int s)
	{
		m_EventBtn.SetTimeCount(d, h, m, s);
	}

	public void SetEnableEvent(bool isEnable)
	{
		m_EventBtn.SetEnable(isEnable);
	}

	public void OpenEvent(bool isOpen)
	{
		m_EventBtn.gameObject.SetActive(isOpen);
	}
}
