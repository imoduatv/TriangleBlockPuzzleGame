using Dta.TenTen;
using Prime31.ZestKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuUI
{
	[Header("Animation Object")]
	[SerializeField]
	private RectTransform[] m_RewardMoney;

	[Header("UI Elements")]
	[SerializeField]
	private Image m_BackgroundImg;

	[SerializeField]
	private Image m_TrophyImg;

	[SerializeField]
	private Text m_HighScoreTxt;

	[SerializeField]
	private Text m_MoneyTxt;

	[SerializeField]
	private Image m_PlayBtnImg;

	[SerializeField]
	private Image m_ShopBtnImg;

	[SerializeField]
	private Image m_VideoAdsBtnImg;

	[SerializeField]
	private Image m_NoAdsBtnImg;

	[SerializeField]
	private Image m_SettingBtnImg;

	[SerializeField]
	private Image m_MoneyPanelImg;

	[SerializeField]
	private Image m_LeaderBoardBtnImg;

	[SerializeField]
	private Image m_LBNativeBtnImg;

	[SerializeField]
	private Image m_RateBtnImg;

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

	[Header("SnowBtn")]
	[SerializeField]
	private GameObject[] m_SnowBtns;

	[Header("Event Button")]
	[SerializeField]
	private EventButton m_EventBtn;

	private Color m_EnableColor;

	private Color m_DisableColor;

	private Color m_EnableIconColor;

	private Color m_DisableIconColor;

	private Action m_AfterAnimCallback;

	private Vector3 m_DesPos;

	private Vector3 m_StartPos;

	private Spline m_TweenPosSpline;

	private Spline m_TweenScaleSpline;

	private Image m_PlayIcon;

	private Image m_LBIcon;

	private Image m_ShopIcon;

	private Image m_LBNativeIcon;

	private Image m_RateIcon;

	private Coroutine m_CheckLoadCorou;

	private int[] moneyPlusArr;

	private WaitForSeconds m_WaitAnim = new WaitForSeconds(0.5f);

	private WaitForSeconds oneSec = new WaitForSeconds(1f);

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

	public override void Init()
	{
		if (m_IsInit)
		{
			ReloadData();
			return;
		}
		m_IsInit = true;
		m_RateBtnImg.gameObject.SetActive(value: true);
		m_LBNativeBtnImg.gameObject.SetActive(value: false);
		m_DesPos = m_MoneyPanelImg.GetComponent<RectTransform>().anchoredPosition;
		m_StartPos = m_VideoAdsBtnImg.GetComponent<RectTransform>().anchoredPosition;
		m_PlayIcon = m_PlayBtnImg.transform.GetChild(0).GetComponent<Image>();
		m_LBIcon = m_LeaderBoardBtnImg.transform.GetChild(0).GetComponent<Image>();
		m_ShopIcon = m_ShopBtnImg.transform.GetChild(0).GetComponent<Image>();
		m_LBNativeIcon = m_LBNativeBtnImg.transform.GetChild(0).GetComponent<Image>();
		m_RateIcon = m_RateBtnImg.transform.GetChild(0).GetComponent<Image>();
		Vector3[] nodes = new Vector3[3]
		{
			m_StartPos,
			new Vector3(-240f, 0f, 0f),
			m_DesPos
		};
		Vector3[] nodes2 = new Vector3[3]
		{
			Vector3.one * 0.5f,
			Vector3.one * 1.2f,
			Vector3.one * 0.5f
		};
		m_TweenPosSpline = new Spline(nodes);
		m_TweenPosSpline.closePath();
		m_TweenScaleSpline = new Spline(nodes2);
		m_TweenScaleSpline.closePath();
		m_EventBtn.Init();
		m_EventBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			Singleton<UIManager>.instance.ShowExchangePanel();
		});
		moneyPlusArr = new int[m_RewardMoney.Length];
		int num = 0;
		int num2 = moneyPlusArr.Length;
		for (int i = 0; i < num2; i++)
		{
			moneyPlusArr[i] = (20 - num) / (num2 - i);
			num += moneyPlusArr[i];
		}
		ReloadData();
	}

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
		SetUI(m_BackgroundImg, dictThemeElement["Background"]);
		m_BackgroundImg.sprite = dictThemeElement["Background"].SpriteUI;
		SetUI(m_TrophyImg, dictThemeElement["Trophy"]);
		SetUI(m_HighScoreTxt, dictThemeElement["HighScoreTxt"]);
		SetUI(m_MoneyTxt, dictThemeElement["MoneyTxt"]);
		SetUI(m_PlayBtnImg, dictThemeElement["PlayBtn"]);
		SetUI(m_ShopBtnImg, dictThemeElement["ShopBtn"]);
		SetUI(m_VideoAdsBtnImg, dictThemeElement["VideoAdsBtn"]);
		SetUI(m_NoAdsBtnImg, dictThemeElement["NoAdsBtn"]);
		SetUI(m_SettingBtnImg, dictThemeElement["SettingBtn"]);
		SetUI(m_MoneyPanelImg, dictThemeElement["MoneyPanel"]);
		SetUI(m_LeaderBoardBtnImg, dictThemeElement["LeaderBoardBtn"]);
		SetUI(m_PlayIcon, dictThemeElement["PlayIcon"]);
		SetUI(m_LBIcon, dictThemeElement["LBIcon"]);
		SetUI(m_ShopIcon, dictThemeElement["ShopIcon"]);
		SetUI(m_LBNativeBtnImg, dictThemeElement["LBNativeBtn"]);
		SetUI(m_LBNativeIcon, dictThemeElement["LBNativeIcon"]);
		SetUI(m_RateBtnImg, dictThemeElement["LBNativeBtn"]);
		SetUI(m_RateIcon, dictThemeElement["LBNativeIcon"]);
		m_EnableColor = dictThemeElement["VideoAdsBtn"].ColorUI;
		m_DisableColor = dictThemeElement["NoVideoBtn"].ColorUI;
		m_EnableIconColor = dictThemeElement["VideoAdsIcon"].ColorUI;
		m_DisableIconColor = dictThemeElement["NoVideoIcon"].ColorUI;
		m_VideoAvai.color = m_EnableIconColor;
		for (int i = 0; i < m_VideoNotAvai.Length; i++)
		{
			m_VideoNotAvai[i].color = m_DisableIconColor;
		}
	}

	public void ReloadData()
	{
		int money = GameData.Instance().GetMoney();
		int highScore = GameData.Instance().GetHighScore(BoardType.Triangle, isBomb: false);
		m_HighScoreTxt.text = highScore.ToString();
		m_MoneyTxt.text = money.ToString();
		m_EventBtn.ReloadSnowFlakeCount();
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

	public void StartAnimReward(Action afterAnimCallback = null)
	{
		if (!m_IsAnimatingReward)
		{
			m_IsAnimatingReward = true;
			m_AfterAnimCallback = afterAnimCallback;
			StartCoroutine(IE_Anim());
		}
	}

	private IEnumerator IE_Anim()
	{
		int i = 0;
		int len = m_RewardMoney.Length;
		int index = 0;
		int preMoney = GameData.Instance().GetMoney() - 20;
		while (i < len)
		{
			m_RewardMoney[i].anchoredPosition = m_StartPos;
			m_RewardMoney[i].gameObject.SetActive(value: true);
			m_RewardMoney[i].transform.localScale = Vector3.one * 0.5f;
			TweenChain tweenChain = new TweenChain();
			TweenParty tweenParty = new TweenParty(1f);
			ITween<Vector3> splinePosTween = new SplineRectTrans(m_RewardMoney[i], m_TweenPosSpline, 1f).setEaseType(EaseType.Linear);
			ITween<Vector3> splineScaleTween = new SplineScale(m_RewardMoney[i].transform, m_TweenScaleSpline, 1f).setEaseType(EaseType.Linear);
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
		for (int i = 0; i < m_SnowBtns.Length; i++)
		{
			m_SnowBtns[i].SetActive(isOpen);
		}
	}
}
