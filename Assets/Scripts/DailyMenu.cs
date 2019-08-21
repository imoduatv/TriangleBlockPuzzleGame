using Prime31.ZestKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyMenu : MenuUI
{
	[SerializeField]
	private DailyRewardData m_RewardData;

	[SerializeField]
	private RectTransform[] m_RewardMoney;

	[SerializeField]
	private Image m_MoneyPanelImg;

	[SerializeField]
	private Text m_MoneyTxt;

	[SerializeField]
	private Text m_TodayReward;

	[SerializeField]
	private Text m_TodayTxt;

	[SerializeField]
	private RectTransform m_StartPosRect;

	[SerializeField]
	private DailyBtn[] m_DailyBtns;

	[SerializeField]
	private GameObject m_DailyObj;

	[SerializeField]
	private GameObject m_GetX1Btn;

	[SerializeField]
	private GameObject m_GetX2Btn;

	[SerializeField]
	private GameObject m_GetX1NoInternet;

	private Vector2 m_DesPos;

	private Action m_AfterAnimCallback;

	private Spline m_TweenPosSpline;

	private Spline m_TweenScaleSpline;

	private int[] moneyPlusArr;

	private WaitForSeconds m_WaitAnim = new WaitForSeconds(0.3f);

	public DailyRewardData RewardData => m_RewardData;

	public override void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			m_DesPos = m_MoneyPanelImg.transform.position;
			Vector3[] nodes = new Vector3[3]
			{
				Vector3.one * 0.5f,
				Vector3.one * 1.2f,
				Vector3.one * 0.5f
			};
			m_MoneyPanelImg.gameObject.SetActive(value: false);
			m_TweenScaleSpline = new Spline(nodes);
			m_TweenScaleSpline.closePath();
			moneyPlusArr = new int[m_RewardMoney.Length];
			ShowGetRewardBtn(isShow: true);
			SetRewardText();
		}
	}

	public void StartAnimReward(int day, bool isDouble, Action afterAnimCallback = null)
	{
		Init();
		m_AfterAnimCallback = afterAnimCallback;
		StartCoroutine(IE_Anim(day, isDouble));
	}

	private IEnumerator IE_Anim(int day, bool isDouble)
	{
		if (m_TweenPosSpline != null)
		{
			m_TweenPosSpline = null;
		}
		m_MoneyPanelImg.gameObject.SetActive(value: true);
		Vector3 m_StartPos = m_StartPosRect.transform.position;
		float m_PosY = (m_StartPos.y + m_DesPos.y) / 2f;
		Vector3[] tweenPosPoints = new Vector3[3]
		{
			m_StartPos,
			new Vector3(-2f, m_PosY, 0f),
			m_DesPos
		};
		m_TweenPosSpline = new Spline(tweenPosPoints);
		m_TweenPosSpline.closePath();
		int i = 0;
		int len = m_RewardMoney.Length;
		int index = 0;
		int plusMoney = 0;
		int rewardMoneyThisDay = m_RewardData.MoneyRewards[day - 1];
		if (isDouble)
		{
			rewardMoneyThisDay *= 2;
		}
		for (int j = 0; j < len; j++)
		{
			moneyPlusArr[j] = (rewardMoneyThisDay - plusMoney) / (len - j);
			plusMoney += moneyPlusArr[j];
		}
		int preMoney = GameData.Instance().GetMoney() - rewardMoneyThisDay;
		while (i < len)
		{
			m_RewardMoney[i].transform.position = m_StartPos;
			m_RewardMoney[i].gameObject.SetActive(value: true);
			m_RewardMoney[i].transform.localScale = Vector3.one * 0.5f;
			TweenChain tweenChain = new TweenChain();
			TweenParty tweenParty = new TweenParty(1f);
			SplineTween splinePosTween = new SplineTween(m_RewardMoney[i], m_TweenPosSpline, 1f);
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
						m_MoneyPanelImg.gameObject.SetActive(value: false);
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

	public void ShowMoney()
	{
		m_MoneyTxt.text = GameData.Instance().GetMoney().ToString();
	}

	private void ShowMoney(int value)
	{
		m_MoneyTxt.text = value.ToString();
	}

	public void SetStatDailyBtns(DailyStat[] dailyStats)
	{
		for (int i = 0; i < m_DailyBtns.Length; i++)
		{
			m_DailyBtns[i].SetStatDaily(dailyStats[i], i + 1);
		}
	}

	public void SetRewardText()
	{
		int[] moneyRewards = m_RewardData.MoneyRewards;
		for (int i = 0; i < m_DailyBtns.Length; i++)
		{
			m_DailyBtns[i].SetMoneyRewardText(moneyRewards[i]);
		}
	}

	public void SetTodayReward(int dayElapse)
	{
		m_TodayTxt.text = "Day " + (dayElapse + 1).ToString();
		m_TodayReward.text = m_RewardData.MoneyRewards[dayElapse].ToString();
	}

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
		SetUI(m_MoneyPanelImg, dictThemeElement["MoneyPanel"]);
		SetUI(m_MoneyTxt, dictThemeElement["MoneyTxt"]);
	}

	public void SetDoubleStat(VideoAdsStat stat)
	{
		if (stat == VideoAdsStat.Available)
		{
			m_GetX2Btn.SetActive(value: true);
			m_GetX1Btn.SetActive(value: true);
			m_GetX1NoInternet.SetActive(value: false);
		}
		else
		{
			m_GetX2Btn.SetActive(value: false);
			m_GetX1Btn.SetActive(value: false);
			m_GetX1NoInternet.SetActive(value: true);
		}
	}

	public void ShowGetRewardBtn(bool isShow)
	{
		m_GetX1Btn.SetActive(isShow);
		m_GetX2Btn.SetActive(isShow);
	}

	public void ScaleLongScreen()
	{
		m_DailyObj.transform.localScale = Vector3.one * 0.8f;
		m_GetX1Btn.transform.localScale = Vector3.one * 0.8f;
		m_GetX2Btn.transform.localScale = Vector3.one * 0.8f;
		m_GetX1NoInternet.transform.localScale = Vector3.one * 0.8f;
	}
}
