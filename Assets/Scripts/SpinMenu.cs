using Prime31.ZestKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinMenu : MenuUI
{
	[SerializeField]
	private SpinRewardData m_DataReward;

	[SerializeField]
	private SpinConfirm m_SpinConfirm;

	[Header("UI Elements")]
	[SerializeField]
	private GameObject m_SpinPanel;

	[SerializeField]
	private Image m_MoneyPanelImg;

	[SerializeField]
	private Text m_MoneyTxt;

	[SerializeField]
	private RectTransform[] m_RewardMoney;

	[SerializeField]
	private GameObject m_GetX1Btn;

	[SerializeField]
	private GameObject m_GetX2Btn;

	[SerializeField]
	private GameObject m_GetX1NoVid;

	[Header("Color spin")]
	[SerializeField]
	private Color m_NormalColor;

	[SerializeField]
	private Color m_SpecialColor;

	[Header("Sprite Spin")]
	[SerializeField]
	private Sprite m_NormalSpr;

	[SerializeField]
	private Sprite m_HighlightSpr;

	private bool m_IsSpinning;

	public RectTransform[] giftsRect;

	public RectTransform spin;

	private RectTransform cloneSpin;

	private ITween<Vector2> tweenArrow1;

	private ITween<Vector2> tweenArrow2;

	private ITween<Vector3> tweenGift;

	private int index;

	private int lastIndex;

	public int randomIndex;

	[Header("Time delay config")]
	public float timeDelayStart;

	public float timeDelayMid;

	public float timeDelayEnd;

	public float timeDelayLerp;

	[Header("Time config")]
	public float timeStart;

	public float timeMid;

	public float timeEnd;

	private Text[] rewardTxt;

	private Image[] rewardImg;

	private Coroutine spinCorou;

	private Action m_AfterAnimCallback;

	private Vector3 m_DesPos;

	private Spline m_TweenScaleSpline;

	private int[] moneyPlusArr;

	private Dictionary<TierSpin, int> dictIndexSpin;

	private WaitForSeconds m_WaitAnim = new WaitForSeconds(0.3f);

	public override void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			m_SpinConfirm.Init(m_NormalColor, m_SpecialColor);
			dictIndexSpin = new Dictionary<TierSpin, int>();
			rewardTxt = new Text[giftsRect.Length];
			rewardImg = new Image[giftsRect.Length];
			for (int i = 0; i < rewardTxt.Length; i++)
			{
				rewardTxt[i] = giftsRect[i].transform.GetChild(1).GetComponent<Text>();
				rewardImg[i] = giftsRect[i].GetComponent<Image>();
			}
			ChangeMoneyPanelAnchor();
			m_DesPos = m_MoneyPanelImg.GetComponent<RectTransform>().anchoredPosition;
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
		}
	}

	public SpinReward[] ShuffleArray(SpinReward[] originArr)
	{
		for (int i = 0; i < originArr.Length; i++)
		{
			SpinReward spinReward = originArr[i];
			int num = UnityEngine.Random.Range(i, originArr.Length);
			originArr[i] = originArr[num];
			originArr[num] = spinReward;
		}
		return originArr;
	}

	public void RandomReward()
	{
		SpinReward[] array = ShuffleArray(m_DataReward.rewards);
		dictIndexSpin.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			dictIndexSpin.Add(array[i].tier, i);
		}
		SetRewardText(array);
	}

	public void Spin()
	{
		m_IsSpinning = true;
		randomIndex = RandomIndexByRatio();
		m_SpinPanel.SetActive(value: true);
		SetActiveBtn(value: false);
		StartCoroutine(IE_Spin());
		StartCoroutine(IE_UpdateScale());
	}

	private int RandomIndexByRatio()
	{
		if (ServicesManager.DataNormal().GetBool("FirstSpin", defaultValue: true))
		{
			ServicesManager.DataNormal().SetBool("FirstSpin", value: false);
			if (Singleton<GameManager>.instance.SpinFirst == SpinFirstReward.R_25)
			{
				return dictIndexSpin[TierSpin.T1];
			}
			if (Singleton<GameManager>.instance.SpinFirst == SpinFirstReward.R_50)
			{
				return dictIndexSpin[TierSpin.T2];
			}
			if (Singleton<GameManager>.instance.SpinFirst == SpinFirstReward.R_75)
			{
				return dictIndexSpin[TierSpin.T3];
			}
			return dictIndexSpin[TierSpin.T4];
		}
		int num = UnityEngine.Random.Range(0, 100);
		if (num < 60)
		{
			return dictIndexSpin[TierSpin.T1];
		}
		if (num < 90)
		{
			return dictIndexSpin[TierSpin.T2];
		}
		if (num < 99)
		{
			return dictIndexSpin[TierSpin.T3];
		}
		return dictIndexSpin[TierSpin.T4];
	}

	private void StopTween(ITween<Vector2> tween)
	{
		if (tween != null && tween.isRunning())
		{
			tween.stop();
		}
	}

	private void StopTween(ITween<Vector3> tween)
	{
		if (tween != null && tween.isRunning())
		{
			tween.stop();
		}
	}

	private IEnumerator IE_UpdateScale()
	{
		Vector3 normalScale = new Vector3(1f, 1f, 1f);
		Vector3 selectScale = new Vector3(1.1f, 1.1f, 1f);
		float speed = 1.5f;
		for (int i = 0; i < giftsRect.Length; i++)
		{
			giftsRect[i].localScale = normalScale;
		}
		if (m_NormalSpr == null || m_HighlightSpr == null)
		{
			spin.GetComponent<Image>().enabled = true;
		}
		else
		{
			spin.GetComponent<Image>().enabled = false;
		}
		if (cloneSpin != null)
		{
			UnityEngine.Object.Destroy(cloneSpin.gameObject);
		}
		StopTween(tweenGift);
		StopTween(tweenArrow1);
		StopTween(tweenArrow2);
		while (m_IsSpinning)
		{
			for (int j = 0; j < giftsRect.Length; j++)
			{
				if (j == lastIndex)
				{
					giftsRect[j].localScale = selectScale;
				}
				else
				{
					giftsRect[j].localScale = Vector3.MoveTowards(giftsRect[j].localScale, normalScale, speed * Time.deltaTime);
				}
			}
			yield return null;
		}
		for (int k = 0; k < giftsRect.Length; k++)
		{
			if (k == lastIndex)
			{
				giftsRect[k].localScale = selectScale;
			}
			else
			{
				giftsRect[k].localScale = normalScale;
			}
		}
		cloneSpin = UnityEngine.Object.Instantiate(spin);
		cloneSpin.GetChild(0).gameObject.SetActive(value: false);
		cloneSpin.GetChild(1).gameObject.SetActive(value: false);
		cloneSpin.SetParent(giftsRect[lastIndex]);
		cloneSpin.anchoredPosition3D = Vector3.zero;
		cloneSpin.localScale = Vector3.one;
		tweenGift = giftsRect[lastIndex].ZKlocalScaleTo(normalScale).setFrom(selectScale).setLoops(LoopType.PingPong, 9999)
			.setEaseType(EaseType.Linear);
		RectTransform rectArrowUnder = spin.GetChild(0).gameObject.GetComponent<RectTransform>();
		RectTransform rectArrowAbove = spin.GetChild(1).gameObject.GetComponent<RectTransform>();
		Vector2 tempPos = rectArrowUnder.anchoredPosition;
		Vector2 tempPos2 = rectArrowAbove.anchoredPosition;
		tempPos.y -= 10f;
		tempPos2.y += 10f;
		tweenArrow1 = rectArrowUnder.ZKanchoredPositionTo(tempPos).setLoops(LoopType.PingPong, 9999).setEaseType(EaseType.Linear);
		tweenArrow2 = rectArrowAbove.ZKanchoredPositionTo(tempPos2).setLoops(LoopType.PingPong, 9999).setEaseType(EaseType.Linear);
		tweenArrow1.start();
		tweenArrow2.start();
		tweenGift.start();
		spin.GetComponent<Image>().enabled = false;
	}

	private IEnumerator IE_SpinByTime(float timeDelay)
	{
		WaitForSeconds waitDelay = new WaitForSeconds(timeDelay);
		while (true)
		{
			if (index == giftsRect.Length)
			{
				index = 0;
			}
			spin.anchoredPosition = giftsRect[index].anchoredPosition;
			ChooseIndex(index);
			Singleton<SoundManager>.instance.PlayItemSlotRandom();
			lastIndex = index;
			index++;
			yield return waitDelay;
		}
	}

	private IEnumerator IE_SpinLerp(float timeDelay)
	{
		WaitForSeconds waitDelay = new WaitForSeconds(timeDelay);
		while (true)
		{
			if (index == giftsRect.Length)
			{
				index = 0;
			}
			lastIndex = index;
			spin.anchoredPosition = giftsRect[index].anchoredPosition;
			ChooseIndex(index);
			if (index == randomIndex)
			{
				break;
			}
			Singleton<SoundManager>.instance.PlayItemSlotRandom();
			index++;
			yield return waitDelay;
		}
		Singleton<SoundManager>.instance.PlayItemSlotGet();
		SetActiveBtn(value: true);
		m_IsSpinning = false;
	}

	private IEnumerator IE_Spin()
	{
		m_MoneyPanelImg.gameObject.SetActive(value: false);
		ResetCoroutine();
		spinCorou = StartCoroutine(IE_SpinByTime(timeDelayStart));
		yield return new WaitForSeconds(timeStart);
		ResetCoroutine();
		spinCorou = StartCoroutine(IE_SpinByTime(timeDelayMid));
		yield return new WaitForSeconds(timeMid);
		ResetCoroutine();
		spinCorou = StartCoroutine(IE_SpinByTime(timeDelayEnd));
		yield return new WaitForSeconds(timeEnd);
		int distanceIndex = (index > randomIndex) ? (randomIndex + giftsRect.Length - index) : (randomIndex - index);
		if (distanceIndex < 3)
		{
			ResetCoroutine();
			spinCorou = StartCoroutine(IE_SpinByTime(timeDelayEnd));
			yield return new WaitForSeconds(timeDelayEnd * (float)(distanceIndex + 1));
		}
		ResetCoroutine();
		spinCorou = StartCoroutine(IE_SpinLerp(timeDelayLerp));
	}

	private void ChooseIndex(int index)
	{
		if (!(m_NormalSpr == null) && !(m_HighlightSpr == null))
		{
			for (int i = 0; i < rewardImg.Length; i++)
			{
				rewardImg[i].sprite = m_NormalSpr;
			}
			rewardImg[index].sprite = m_HighlightSpr;
		}
	}

	private void ResetCoroutine()
	{
		index = lastIndex;
		if (spinCorou != null)
		{
			StopCoroutine(spinCorou);
			spinCorou = null;
		}
	}

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
		SetUI(m_MoneyPanelImg, dictThemeElement["MoneyPanel"]);
		SetUI(m_MoneyTxt, dictThemeElement["MoneyTxt"]);
	}

	public void StartAnimReward(bool isDouble, Action afterAnimCallback = null)
	{
		Init();
		m_AfterAnimCallback = afterAnimCallback;
		StartCoroutine(IE_Anim(isDouble));
	}

	private IEnumerator IE_Anim(bool isDouble)
	{
		m_MoneyPanelImg.gameObject.SetActive(value: true);
		m_SpinPanel.SetActive(value: false);
		SetActiveBtn(value: false);
		Vector2 m_StartPos = giftsRect[randomIndex].GetComponent<RectTransform>().anchoredPosition;
		float num = (m_StartPos.y + m_DesPos.y) / 2f;
		int i = 0;
		int len = m_RewardMoney.Length;
		int index = 0;
		int plusMoney = 0;
		int rewardMoneyThisDay = m_DataReward.rewards[randomIndex].reward;
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
			m_RewardMoney[i].anchoredPosition = m_StartPos;
			m_RewardMoney[i].gameObject.SetActive(value: true);
			m_RewardMoney[i].transform.localScale = Vector3.one * 0.5f;
			TweenChain tweenChain = new TweenChain();
			ITween<Vector2> tweenFly = m_RewardMoney[i].ZKanchoredPositionTo(m_DesPos, 1f).setEaseType(EaseType.Linear).setCompletionHandler(delegate
			{
				m_RewardMoney[index].gameObject.SetActive(value: false);
			});
			tweenChain.appendTween(tweenFly).appendTween(m_MoneyPanelImg.transform.ZKlocalScaleTo(Vector3.one * 1.2f, 0.05f).setLoops(LoopType.PingPong).setEaseType(EaseType.Linear));
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

	public void ShowMoney()
	{
		m_MoneyTxt.text = GameData.Instance().GetMoney().ToString();
	}

	private void ShowMoney(int value)
	{
		m_MoneyTxt.text = value.ToString();
	}

	public int GetReward()
	{
		return m_DataReward.rewards[randomIndex].reward;
	}

	public void ChangeMoneyPanelAnchor()
	{
		Vector3 position = m_MoneyPanelImg.transform.position;
		RectTransform component = m_MoneyPanelImg.GetComponent<RectTransform>();
		component.anchorMax = Vector2.one * 0.5f;
		component.anchorMin = Vector2.one * 0.5f;
		m_MoneyPanelImg.transform.position = position;
	}

	public void SetActiveBtn(bool value)
	{
		if (!value)
		{
			m_GetX1Btn.SetActive(value);
			m_GetX2Btn.SetActive(value);
			m_GetX1NoVid.SetActive(value);
		}
		else
		{
			bool flag = Singleton<ServicesManager>.instance.IsRewardAvailable();
			m_GetX1Btn.SetActive(flag);
			m_GetX2Btn.SetActive(flag);
			m_GetX1NoVid.SetActive(!flag);
		}
	}

	public void SetRewardText(SpinReward[] rewards)
	{
		m_SpinConfirm.SetRewardText(rewards);
		for (int i = 0; i < rewardTxt.Length; i++)
		{
			rewardTxt[i].text = rewards[i].reward.ToString();
			if (rewards[i].tier == TierSpin.T4)
			{
				rewardImg[i].color = m_SpecialColor;
			}
			else
			{
				rewardImg[i].color = m_NormalColor;
			}
		}
	}
}
