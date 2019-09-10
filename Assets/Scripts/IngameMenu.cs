using Prime31.ZestKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenu : MenuUI
{
	[Header("Animation UI")]
	[SerializeField]
	private Image m_RewardMoney;

	[SerializeField]
	private ParticleSystem m_RewardParticle1;

	[SerializeField]
	private ParticleSystem m_RewardParticle2;

	[Header("UI Elements")]
	[SerializeField]
	private Image m_IngameTopBG;

	[SerializeField]
	private Image m_BackgroundImg;

	[SerializeField]
	private Text m_ScoreTxt;

	[SerializeField]
	private Image m_TrophyImg;

	[SerializeField]
	private Text m_HighScoreTxt;

	[SerializeField]
	private Image m_PauseBtnImg;

	[SerializeField]
	private Text m_MoneyTxt1;

	[SerializeField]
	private Text m_MoneyTxt2;

	[SerializeField]
	private Image m_MoneyPanel1;

	[SerializeField]
	private Image m_MoneyPanel2;

	[SerializeField]
	private Image m_ThemeIconImg;

	[Header("Spin Elements")]
	[SerializeField]
	private Image m_GiftTop;

	[SerializeField]
	private Image m_GiftBot;

	[SerializeField]
	private Image m_GiftEvent;

	[Header("Reward Array")]
	[SerializeField]
	private RectTransform[] m_RewardMoneyArr;

	[Header("Logo")]
	[SerializeField]
	private GameObject m_LogoBot;

	[SerializeField]
	private GameObject m_LogoTop;

	[Header("Cost Skill Text")]
	[SerializeField]
	private Text m_CostSkill3;

	[SerializeField]
	private Text m_CostSkillUndo;

	[Header("Snow Flake")]
	[SerializeField]
	private RectTransform m_SnowFlakePanel;

	[SerializeField]
	private RectTransform m_SnowFlakeEffect;

	[Header("Xmas board BG")]
	[SerializeField]
	private GameObject m_XmasBoardBG;

	private Text m_SnowFlakeCountTxt;

	private ParticleSystem m_SnowParticle;

	private Action m_AfterRewardAnimCallback;

	private Action m_AfterFadeCallback;

	private Action m_AfterAnimCallback;

	private Action m_AfterSnowEffectCallback;

	private Vector3 m_DesPosReward;

	private Vector3 m_DesPosSnow;

	private Button m_MoneyPanelBtn;

	private Image m_MoneyPanel;

	private Text m_MoneyTxt;

	private ParticleSystem m_RewardParticle;

	private TextNumberIncreaseFX m_ScoreTextFX;

	private TextNumberIncreaseFX m_HighScoreTextFX;

	private Spline m_TweenScaleSpline;

	private int[] moneyPlusArr;

	private bool isLongScreen;

	private bool m_IsAnimatingSnow;

	private WaitForSeconds m_WaitAnim = new WaitForSeconds(0.3f);

	private void OnEnable()
	{
		Init();
		ShowMoney();
		ShowSnowFlake();
		if (GameData.Instance().GetCurrentTheme() == ThemeName.Xmas)
		{
			m_XmasBoardBG.SetActive(value: true);
		}
		else
		{
			m_XmasBoardBG.SetActive(value: false);
		}
	}

	private void OnDisable()
	{
		if (m_XmasBoardBG != null)
		{
			m_XmasBoardBG.SetActive(value: false);
		}
	}

	public override void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			Vector3[] nodes = new Vector3[3]
			{
				Vector3.one * 0.5f,
				Vector3.one * 1.2f,
				Vector3.one * 0.5f
			};
			if (Singleton<GameManager>.instance.AB_Skill == GameManager.ABSkill.No)
			{
				m_MoneyPanel1.gameObject.SetActive(value: true);
				m_MoneyPanel2.gameObject.SetActive(value: false);
				m_RewardParticle = m_RewardParticle1;
				m_MoneyPanel = m_MoneyPanel1;
				m_MoneyTxt = m_MoneyTxt1;
			}
			else
			{
				m_MoneyPanel1.gameObject.SetActive(value: false);
				m_MoneyPanel2.gameObject.SetActive(value: true);
				m_RewardParticle = m_RewardParticle2;
				m_MoneyPanel = m_MoneyPanel2;
				m_MoneyTxt = m_MoneyTxt2;
			}
			m_SnowFlakeCountTxt = m_SnowFlakePanel.transform.GetChild(0).GetComponent<Text>();
			m_SnowParticle = m_SnowFlakePanel.transform.GetChild(1).GetComponent<ParticleSystem>();
			m_DesPosReward = m_MoneyPanel.transform.position;
			m_DesPosSnow = m_SnowFlakePanel.transform.position;
			m_TweenScaleSpline = new Spline(nodes);
			m_TweenScaleSpline.closePath();
			moneyPlusArr = new int[m_RewardMoneyArr.Length];
			m_RewardMoney.gameObject.SetActive(value: false);
			for (int i = 0; i < m_RewardMoneyArr.Length; i++)
			{
				m_RewardMoneyArr[i].gameObject.SetActive(value: false);
			}
			float num = (float)Screen.height * 1f / (float)Screen.width;
			if (num > 1.882353f)
			{
				isLongScreen = true;
			}
			else
			{
				isLongScreen = false;
			}
		}
	}

	private TextNumberIncreaseFX GetScoreTextFX()
	{
		if (m_ScoreTextFX == null)
		{
			m_ScoreTextFX = m_ScoreTxt.GetComponent<TextNumberIncreaseFX>();
		}
		return m_ScoreTextFX;
	}

	private TextNumberIncreaseFX GetHighScoreTextFX()
	{
		if (m_HighScoreTextFX == null)
		{
			m_HighScoreTextFX = m_HighScoreTxt.GetComponent<TextNumberIncreaseFX>();
		}
		return m_HighScoreTextFX;
	}

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
        SetUI(m_BackgroundImg, dictThemeElement["Background"]);
        m_BackgroundImg.sprite = dictThemeElement["Background"].SpriteUI;
        SetUI(m_TrophyImg, dictThemeElement["Trophy"]);
        SetUI(m_HighScoreTxt, dictThemeElement["HighScoreTxt"]);
        SetUI(m_MoneyTxt1, dictThemeElement["MoneyTxt"]);
        SetUI(m_MoneyTxt2, dictThemeElement["MoneyTxt"]);
        SetUI(m_ScoreTxt, dictThemeElement["ScoreTxt"]);
        SetUI(m_PauseBtnImg, dictThemeElement["PauseBtn"]);
        SetUI(m_MoneyPanel1, dictThemeElement["MoneyPanel"]);
        SetUI(m_MoneyPanel2, dictThemeElement["MoneyPanel2"]);
        m_MoneyPanel2.color = m_MoneyPanel.color;
        SetUI(m_ThemeIconImg, dictThemeElement["ThemeIcon"]);
        SetUI(m_GiftBot, dictThemeElement["GiftBot"]);
        SetUI(m_GiftTop, dictThemeElement["GiftTop"]);
        SetUI(m_CostSkill3, dictThemeElement["CostSkillTxt"]);
        SetUI(m_CostSkillUndo, dictThemeElement["CostSkillTxt"]);
        if (dictThemeElement.ContainsKey("TopBG"))
		{
			m_IngameTopBG.gameObject.SetActive(value: true);
			SetUI(m_IngameTopBG, dictThemeElement["TopBG"]);
		}
		else
		{
			m_IngameTopBG.gameObject.SetActive(value: false);
		}
		m_ThemeIconImg.SetNativeSize();
		if (isLongScreen)
		{
			m_LogoBot.SetActive(value: false);
			m_LogoTop.SetActive(value: false);
			return;
		}
		ThemeName currentTheme = GameData.Instance().GetCurrentTheme();
		if (Singleton<GameManager>.instance.AB_Banner == GameManager.ABBanner.Top && Singleton<ThemeManager>.instance.IsThemeBannerTop(currentTheme))
		{
			m_LogoBot.SetActive(value: false);
			m_LogoTop.SetActive(value: true);
		}
		else
		{
			m_LogoBot.SetActive(value: true);
			m_LogoTop.SetActive(value: false);
		}
	}

	public void StartAnimReward(Vector3 startPos, Action afterAnimCallback = null)
	{
		if (!m_IsAnimatingReward)
		{
			Init();
			m_IsAnimatingReward = true;
			m_RewardMoney.transform.localScale = Vector3.one * 0.6f;
			m_RewardMoney.gameObject.SetActive(value: true);
			m_RewardMoney.transform.position = startPos;
			TweenChain tweenChain = new TweenChain();
			Vector2 v = CalculateSplinePoint(startPos, m_DesPosReward);
			Vector3[] nodes = new Vector3[3]
			{
				startPos,
				v,
				m_DesPosReward
			};
			Spline spline = new Spline(nodes);
			spline.closePath();
			m_AfterRewardAnimCallback = afterAnimCallback;
			SplineTween splineTween = new SplineTween(m_RewardMoney.transform, spline, 1f);
			splineTween.setCompletionHandler(delegate
			{
				m_RewardMoney.gameObject.SetActive(value: false);
				m_MoneyTxt.text = GameData.Instance().GetMoney().ToString();
				m_RewardParticle.Play();
				if (m_AfterRewardAnimCallback != null)
				{
					m_AfterRewardAnimCallback();
				}
				m_IsAnimatingReward = false;
			});
			tweenChain.appendTween(splineTween);
			tweenChain.appendTween(m_MoneyPanel.transform.ZKlocalScaleTo(Vector3.one * 1.2f, 0.05f).setLoops(LoopType.PingPong).setEaseType(EaseType.Linear));
			tweenChain.start();
		}
	}

	public void StartAnimSnow(Vector3 startPos, Action afterAnimCallback = null)
	{
		if (!m_IsAnimatingSnow)
		{
			Init();
			m_IsAnimatingSnow = true;
			m_SnowFlakeEffect.transform.localScale = Vector3.one * 0.6f;
			m_SnowFlakeEffect.gameObject.SetActive(value: true);
			m_SnowFlakeEffect.transform.position = startPos;
			TweenChain tweenChain = new TweenChain();
			Vector2 v = CalculateSplinePoint(startPos, m_DesPosSnow);
			Vector3[] nodes = new Vector3[3]
			{
				startPos,
				v,
				m_DesPosSnow
			};
			Spline spline = new Spline(nodes);
			spline.closePath();
			m_AfterSnowEffectCallback = afterAnimCallback;
			SplineTween splineTween = new SplineTween(m_SnowFlakeEffect.transform, spline, 1f);
			splineTween.setCompletionHandler(delegate
			{
				m_SnowFlakeEffect.gameObject.SetActive(value: false);
				m_SnowFlakeCountTxt.text = GameData.Instance().GetCurrentSnowFlake().ToString();
				m_SnowParticle.Play();
				if (m_AfterSnowEffectCallback != null)
				{
					m_AfterSnowEffectCallback();
				}
				m_IsAnimatingSnow = false;
			});
			tweenChain.appendTween(splineTween);
			tweenChain.appendTween(m_SnowFlakePanel.transform.ZKlocalScaleTo(Vector3.one * 1.2f, 0.05f).setLoops(LoopType.PingPong).setEaseType(EaseType.Linear));
			tweenChain.start();
		}
	}

	public void UpdateScore(int currentScore, int highScore, bool isStartGame)
	{
		Init();
		if (Singleton<GameManager>.instance.ScoreEffect == ScoreEffect.FX)
		{
			if (isStartGame)
			{
				GetScoreTextFX().Reset(currentScore);
				GetHighScoreTextFX().Reset(highScore);
			}
			else
			{
				GetScoreTextFX().IncreaseToNumber(currentScore);
				GetHighScoreTextFX().IncreaseToNumber(highScore);
			}
		}
		else
		{
			m_ScoreTxt.text = currentScore.ToString();
			m_HighScoreTxt.text = highScore.ToString();
		}
	}

	private Vector2 CalculateSplinePoint(Vector2 startPos, Vector2 desPos)
	{
		Vector2 a = (startPos + desPos) / 2f;
		return a + Vector2.one * 0.5f;
	}

	public void ReloadData()
	{
		ShowMoney();
	}

	public void StartAnimReward(bool isDouble, int reward, Action afterAnimCallback = null)
	{
		Init();
		m_AfterAnimCallback = afterAnimCallback;
		StartCoroutine(IE_Anim(isDouble, reward));
	}

	private IEnumerator IE_Anim(bool isDouble, int reward)
	{
		Vector3 m_StartPos = Vector3.zero;
		int i = 0;
		int len = m_RewardMoneyArr.Length;
		int index = 0;
		int plusMoney = 0;
		int rewardMoneyThisDay = reward;
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
			m_RewardMoneyArr[i].position = m_StartPos;
			m_RewardMoneyArr[i].gameObject.SetActive(value: true);
			m_RewardMoneyArr[i].transform.localScale = Vector3.one * 0.5f;
			TweenChain tweenChain = new TweenChain();
			ITween<Vector3> tweenFly = m_RewardMoneyArr[i].ZKpositionTo(m_DesPosReward, 1f).setEaseType(EaseType.Linear).setCompletionHandler(delegate
			{
				m_RewardMoneyArr[index].gameObject.SetActive(value: false);
			});
			tweenChain.appendTween(tweenFly).appendTween(m_MoneyPanel.transform.ZKlocalScaleTo(Vector3.one * 1.2f, 0.05f).setLoops(LoopType.PingPong).setEaseType(EaseType.Linear));
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
		m_MoneyTxt2.text = GameData.Instance().GetMoney().ToString();
	}

	public void ShowSnowFlake()
	{
		m_SnowFlakeCountTxt.text = GameData.Instance().GetCurrentSnowFlake().ToString();
	}

	private void ShowMoney(int value)
	{
		m_MoneyTxt.text = value.ToString();
		m_MoneyTxt2.text = value.ToString();
	}

	public void OpenEvent(bool isOpen)
	{
		m_SnowFlakePanel.gameObject.SetActive(isOpen);
		m_GiftBot.gameObject.SetActive(!isOpen);
		m_GiftTop.gameObject.SetActive(!isOpen);
		m_GiftEvent.gameObject.SetActive(isOpen);
	}
}
