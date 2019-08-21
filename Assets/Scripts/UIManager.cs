using Archon.SwissArmyLib.Coroutines;
using Dta.TenTen;
using Prime31.ZestKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	private delegate void ActionCallBack();

	private const int SCORE_ADS = 0;

	private DateTime m_EndTimeEvent = new DateTime(2019, 1, 1, 0, 0, 0);

	[Header("Menus")]
	public GameObject HomeMenu;

	public GameObject SelectMode;

	public GameObject NoAdsMenu;

	public GameObject InGameMenu;

	public GameObject PauseMenu;

	public GameObject ShopMenu;

	public GameObject GameOver;

	public GameObject SettingMenu;

	public GameObject NotEnoughDiamond;

	public GameObject EffectCombo;

	public GameObject LeaderBoardMenu;

	public GameObject LeaderBoardTrailer;

	public GameObject Credit;

	public GameObject ContinueMenu;

	public GameObject DailyMenu;

	public GameObject ComingSoonMenu;

	public GameObject ConfirmMenu;

	public GameObject SpinMenuObj;

	public GameObject SpinConfirm;

	public GameObject SpinMenuEventObj;

	public GameObject SpinConfirmEventObj;

	public GameObject GDPRObj;

	[Header("Event Menus")]
	public ExchangePanel ExchangePanel;

	public HowToGetSnowPanel HowToGetSnowPanel;

	public NotEnoughMenu NotEnoughSnowFlake;

	public GameObject ParticleXmasTheme;

	[Header("Background ingame")]
	public GameObject BackgroundFarm;

	[Header("Board Screenshot")]
	public GameObject BoardScreenshot;

	[Header("Board BG Theme Baby")]
	public GameObject BoardBG;

	[Header("Score Screenshot")]
	public GameObject ShareBtn;

	public GameObject DemoScoreScreenshot;

	public RawImage ScreenshotImg;

	public GameObject LogoScreenshot;

	[Header("Buy No-Ads")]
	public GameObject NoAdsInIAP;

	public GameObject BuyNoAdsAtMenu;

	public GameObject BuyNoAdsAtGameOver;

	public GameObject BuyNoAdsAtPause;

	[Header("Review Active Area")]
	public GameObject ReviewActiveArea;

	private QuitOnEscape quitOnEscape;

	[Header("Spin")]
	public bool m_IsTestSpin;

	public Vector2 m_TimeGetSpinVec = new Vector2(180f, 300f);

	public GameObject m_SpinBtn;

	private GameObject m_GiftAnimatorObj;

	private GameObject m_GiftLightObj;

	private LightEffect m_LightEffect;

	private Animator m_GiftAnimator;

	[SerializeField]
	private bool m_IsShowSpin;

	[SerializeField]
	private int m_TimeGetSpin;

	[SerializeField]
	private int m_TimeSpinCount;

	private Coroutine m_TimeCountSpinCorou;

	[Header("Ice theme diamond")]
	[SerializeField]
	private GameObject[] m_IceDiamonds;

	private bool isSound = true;

	private bool isShowWatchAds = true;

	private GameObject originRoot;

	private GameObject cloneRoot;

	private bool isRequestNoti;

	[Header("Continue")]
	public Text m_CountTimeTxt;

	[Header("App Info")]
	public string packageNameRateShare = "com.DTA.trigon";

	public string appIdRateShare = "1347129450";

	[Header("Skill cost")]
	[SerializeField]
	private Button m_Skill3Btn;

	[SerializeField]
	private Button m_SkillUndoBtn;

	private Text m_Skill3CostTxt;

	private Text m_SkillUndoCostTxt;

	private RectTransform m_Skill3CostObj;

	private RectTransform m_SkillUndoObj;

	private Vector3 lower10Pos = Vector3.zero;

	private Vector3 lower10Scale = Vector3.one;

	private Vector3 lower100Pos = new Vector3(5f, 0f, 0f);

	private Vector3 lower100Scale = Vector3.one;

	private Vector3 lower1000Pos = new Vector3(17f, 0f);

	private Vector3 lower1000Scale = Vector3.one;

	private Vector3 higher1000Pos = new Vector3(28f, 0f);

	private Vector3 higher1000Scale = Vector3.one;

	private ThemeManager m_ThemeManger;

	private DateTime m_TimeStart;

	private DateTime m_TimeStartCountAd;

	private DateTime m_TimeSpin;

	private float m_TimePlay;

	private float m_TimeCheckReward;

	private float m_TimeCheckShowAds;

	private float m_TimeCheckSpin;

	private bool isFirstGameOver = true;

	private bool isShowFullAdContinue;

	private LongScreenConfig longScreenConfig;

	private PauseMenu m_PauseMenu;

	private SettingMenu m_SettingMenu;

	private ShopMenu m_ShopMenu;

	private GameOverMenu m_GameOverMenu;

	private MainMenu m_MainMenu;

	private IngameMenu m_IngameMenu;

	private NotEnoughMenu m_NotEnoughMenu;

	private NoAdsMenu m_NoAdsMenu;

	private EffectCombo m_EffectCombo;

	private LeaderBoardMenu m_LeaderBoardMenu;

	private LeaderBoardMenu m_LeaderBoardTrailer;

	private ContinueMenu m_ContinueMenu;

	private DailyMenu m_DailyMenu;

	private Credit m_Credit;

	private ComingSoon m_ComingSoon;

	private ConfirmMenu m_ConfirmMenu;

	private SpinMenu m_SpinMenuOld;

	private SpinMenu m_SpinMenuEvent;

	private SpinMenu m_SpinMenu;

	private SpinConfirm m_SpinConfirmOld;

	private SpinConfirm m_SpinConfirmEvent;

	private SpinConfirm m_SpinConfirm;

	private GameObject m_ShadowParent;

	private GameObject m_ShapeParent;

	[SerializeField]
	private MenuState m_MenuState;

	[SerializeField]
	private MenuState m_PreMenuState;

	private bool m_IsInit;

	private bool m_IsOpenEvent;

	private Coroutine m_EventCountCorou;

	private Coroutine m_RecheckTimeCorou;

	private int dayReward;

	private const string KEY_300_SCORE = "Key300";

	private const string KEY_400_SCORE = "Key400";

	private const string KEY_500_SCORE = "Key500";

	private ThemeName themeBuyChoose;

	private float m_TimeStartPlay;

	public float LastPlayTime;

	private DateTime m_TimePlayStart;

	private WaitForSeconds m_DelayClear = new WaitForSeconds(0.72f);

	public RectTransform handTutorial;

	private ITween<Vector2> tweenTutorial1;

	private ITween<Vector3> tweenTutorial2;

	private Action onShapeRotate;

	private Texture2D screenTexture;

	private Coroutine m_CounTimeCorou;

	private bool m_IsShowAdsBeforeContinue;

	private WaitForSeconds oneSec = new WaitForSeconds(1f);

	private WaitForSeconds showTime = new WaitForSeconds(5f);

	private WaitForSeconds hideTime = new WaitForSeconds(0.8f);

	private Coroutine hideGiftCorou;

	private WaitForSeconds waitSpinAnim = new WaitForSeconds(0.5f);

	public bool IsCanPause
	{
		get;
		set;
	}

	public void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			m_MenuState = MenuState.Home;
			m_PreMenuState = MenuState.Home;
			m_GiftAnimatorObj = m_SpinBtn.transform.GetChild(1).gameObject;
			m_GiftLightObj = m_SpinBtn.transform.GetChild(0).gameObject;
			m_SkillUndoObj = m_SkillUndoBtn.transform.GetChild(0).GetComponent<RectTransform>();
			m_Skill3CostObj = m_Skill3Btn.transform.GetChild(0).GetComponent<RectTransform>();
			m_Skill3CostTxt = m_Skill3CostObj.transform.GetChild(0).GetComponent<Text>();
			m_SkillUndoCostTxt = m_SkillUndoObj.transform.GetChild(0).GetComponent<Text>();
			if (Singleton<GameManager>.instance.AB_Skill == GameManager.ABSkill.No)
			{
				m_Skill3Btn.gameObject.SetActive(value: false);
				m_SkillUndoBtn.gameObject.SetActive(value: false);
			}
			else
			{
				m_Skill3Btn.gameObject.SetActive(value: true);
				m_SkillUndoBtn.gameObject.SetActive(value: true);
			}
			m_LightEffect = m_GiftLightObj.GetComponent<LightEffect>();
			m_GiftAnimator = m_GiftAnimatorObj.GetComponent<Animator>();
			ExchangePanel.Init();
			HowToGetSnowPanel.Init();
			NotEnoughSnowFlake.Init();
			isSound = GameData.Instance().GetIsSound();
			IsCanPause = true;
			quitOnEscape = UnityEngine.Object.FindObjectOfType<QuitOnEscape>();
			quitOnEscape.gameObject.SetActive(value: false);
			EscapeController.Instance().SetCallback(quitOnEscape.ShowDialogQuit);
			ZestKit.cacheVector3Tweens = false;
			ZestKit.cacheVector2Tweens = false;
			ZestKit.cacheFloatTweens = false;
			m_ThemeManger = Singleton<ThemeManager>.instance;
			StartCheckTimeAd();
			m_PauseMenu = PauseMenu.GetComponent<PauseMenu>();
			m_SettingMenu = SettingMenu.GetComponent<SettingMenu>();
			m_ShopMenu = ShopMenu.GetComponent<ShopMenu>();
			m_GameOverMenu = GameOver.GetComponent<GameOverMenu>();
			m_MainMenu = HomeMenu.GetComponent<MainMenu>();
			m_IngameMenu = InGameMenu.GetComponent<IngameMenu>();
			m_NoAdsMenu = NoAdsMenu.GetComponent<NoAdsMenu>();
			m_NotEnoughMenu = NotEnoughDiamond.GetComponent<NotEnoughMenu>();
			m_EffectCombo = EffectCombo.GetComponent<EffectCombo>();
			m_LeaderBoardMenu = LeaderBoardMenu.GetComponent<LeaderBoardMenu>();
			m_LeaderBoardTrailer = LeaderBoardTrailer.GetComponent<LeaderBoardMenu>();
			m_ContinueMenu = ContinueMenu.GetComponent<ContinueMenu>();
			m_Credit = Credit.GetComponent<Credit>();
			m_DailyMenu = DailyMenu.GetComponent<DailyMenu>();
			m_ComingSoon = ComingSoonMenu.GetComponent<ComingSoon>();
			m_ConfirmMenu = ConfirmMenu.GetComponent<ConfirmMenu>();
			m_SpinMenuOld = SpinMenuObj.GetComponent<SpinMenu>();
			m_SpinConfirmOld = SpinConfirm.GetComponent<SpinConfirm>();
			m_SpinMenuEvent = SpinMenuEventObj.GetComponent<SpinMenu>();
			m_SpinConfirmEvent = SpinConfirmEventObj.GetComponent<SpinConfirm>();
			m_PauseMenu.Init();
			m_SettingMenu.Init();
			m_ShopMenu.Init();
			m_GameOverMenu.Init();
			m_MainMenu.Init();
			m_IngameMenu.Init();
			m_NoAdsMenu.Init();
			m_NotEnoughMenu.Init();
			m_EffectCombo.Init();
			m_LeaderBoardMenu.Init();
			m_ContinueMenu.Init();
			m_DailyMenu.Init();
			m_ComingSoon.Init();
			m_ConfirmMenu.Init();
			m_SpinMenuOld.Init();
			m_SpinMenuEvent.Init();
			CheckEventOffline();
			longScreenConfig = GetComponent<LongScreenConfig>();
			longScreenConfig.SetUIForLongScreen();
			originRoot = GameObject.Find("Root");
			m_ShadowParent = GameObject.Find("Shadows");
			m_ShapeParent = GameObject.Find("Shapes");
			ThemeName currentTheme = GameData.Instance().GetCurrentTheme();
			if (currentTheme == ThemeName.Soft && !GameData.Instance().IsUnlockTheme(ThemeName.Soft))
			{
				SetTheme(ThemeName.Night);
			}
			else
			{
				SetTheme(currentTheme);
			}
			SetSound();
			bool isAds = GameData.Instance().GetIsAds();
			if (GameData.Instance().GetIsPremium())
			{
				SetNoAdsUI();
			}
			if (!isAds)
			{
				SetNoAdsUI();
			}
			CheckTimeEvent();
		}
	}

	public void SetPremiumUI()
	{
		BuyNoAdsAtMenu.SetActive(value: false);
		BuyNoAdsAtGameOver.SetActive(value: false);
		BuyNoAdsAtPause.SetActive(value: false);
	}

	public void SetNoAdsUI()
	{
		NoAdsInIAP.SetActive(value: false);
	}

	private void SetSound()
	{
		GameData.Instance().SetIsSound(isSound);
		m_SettingMenu.SetImgSound(isSound);
		m_PauseMenu.SetImgSound(isSound);
		if (isSound)
		{
			Singleton<SoundManager>.Instance.SoundOn();
		}
		else
		{
			Singleton<SoundManager>.Instance.SoundOff();
		}
	}

	public void PlayNormal()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		Singleton<GameManager>.Instance.gameType = BoardType.Normal;
		SelectMode.SetActive(value: true);
		EscapeController.Instance().SetCallback(ShowHomeMenu);
	}

	public void PlayHexagon()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		Singleton<GameManager>.Instance.gameType = BoardType.Hexagonal;
		SelectMode.SetActive(value: true);
		EscapeController.Instance().SetCallback(ShowHomeMenu);
	}

	public void PlayTriangle()
	{
		Singleton<GameManager>.instance.LoadSaveGame();
		Singleton<GameManager>.Instance.gameType = BoardType.Triangle;
		EscapeController.Instance().SetCallback(ShowHomeMenu);
		if (Singleton<GameManager>.Instance.IsRunTutorial())
		{
			Singleton<GameManager>.Instance.isBomb = false;
			SwitchToInGame();
		}
		else
		{
			PlayClassic();
		}
	}

	public void PlayClassic()
	{
		Singleton<GameManager>.Instance.isBomb = false;
		SwitchToInGame();
	}

	public void PlayBomb()
	{
		Analytic.Instance.LogPlayEvent(isBomb: true);
		Singleton<ServicesManager>.Instance.ShowBanner();
		Singleton<SoundManager>.Instance.PlayButton();
		Singleton<GameManager>.Instance.isBomb = true;
		SwitchToInGame();
	}

	public void BuyNoAdsHome()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		ShowNoAdsMenu();
	}

	public void OnClickShopIngame()
	{
		if (!Singleton<GameManager>.instance.IsRunTutorial())
		{
			OnClickShopIAP("Ingame");
		}
	}

	public void OnClickShopEvent()
	{
		OnClickShopIAP("Xmas");
	}

	public void OnClickShopIAP(string iapPos)
	{
		List<GameObject> list;
		if (m_MenuState == MenuState.Event && m_PreMenuState == MenuState.Shop)
		{
			UnityEngine.Debug.Log("Go iap from event from shop");
			list = new List<GameObject>();
			list.Add(ShopMenu);
			list.Add(ExchangePanel.gameObject);
		}
		else if (m_MenuState != MenuState.Shop)
		{
			UnityEngine.Debug.Log("Go iap from event home, game over");
			list = new List<GameObject>();
			if (m_MenuState == MenuState.Event)
			{
				list.Add(ExchangePanel.gameObject);
				if (m_PreMenuState == MenuState.Home)
				{
					list.Add(HomeMenu);
				}
				else
				{
					list.Add(GameOver);
				}
			}
			else
			{
				m_PreMenuState = m_MenuState;
				m_MenuState = MenuState.IAP;
				list = GetListMenuUnder();
			}
		}
		else
		{
			UnityEngine.Debug.Log("Go iap from shop");
			list = new List<GameObject>();
			list.Add(ShopMenu);
		}
		Singleton<SoundManager>.Instance.PlayButton();
		Analytic.Instance.LogIAPClickEvent(iapPos);
		m_NotEnoughMenu.gameObject.SetActive(value: false);
		NotEnoughDiamond.SetActive(value: false);
		m_NoAdsMenu.SetListMenuUnder(list);
		m_NoAdsMenu.Show();
	}

	public void OnClickVideoAdsHome()
	{
		Singleton<ServicesManager>.Instance.ShowRewardAds(delegate
		{
			Analytic.Instance.LogShowRewardGeneral();
			Analytic.Instance.LogRewardEvent("Home Reward");
			isShowWatchAds = false;
			GameData.Instance().AddMoney(20);
			m_MainMenu.CheckLoadVideo();
			m_MainMenu.StartAnimReward();
		}, delegate
		{
			isShowWatchAds = false;
			VideoAdsStat videoAdsStat = CheckStatVideo();
			m_MainMenu.CheckLoadVideo();
		});
	}

	public void OnClickVideoAdsOver()
	{
		Singleton<ServicesManager>.Instance.ShowRewardAds(delegate
		{
			Analytic.Instance.LogShowRewardGeneral();
			Analytic.Instance.LogRewardEvent("Game Over Reward");
			isShowWatchAds = false;
			GameData.Instance().AddMoney(20);
			m_GameOverMenu.CheckLoadVideo();
			m_GameOverMenu.StartAnimReward(20);
		}, delegate
		{
			isShowWatchAds = false;
			m_GameOverMenu.CheckLoadVideo();
		});
	}

	public void HideNoAdsMenu()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		if (m_MenuState != MenuState.Shop && m_MenuState != MenuState.Event)
		{
			m_MenuState = m_PreMenuState;
			m_PreMenuState = MenuState.IAP;
		}
		m_NoAdsMenu.Hide();
	}

	public void BuyNoAds()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		InAppPurchaseController.Instance().BuyNoAds2ButtonClick();
	}

	public void Buy1kDiamonds()
	{
		Singleton<SoundManager>.instance.PlayButton();
		InAppPurchaseController.Instance().Buy1kDiamonds();
	}

	public void Buy5kDiamonds()
	{
		Singleton<SoundManager>.instance.PlayButton();
		InAppPurchaseController.Instance().Buy5kDiamonds();
	}

	public void BuyPremium()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		InAppPurchaseController.Instance().BuyPremium();
	}

	public void RestorePurchase()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		InAppPurchaseController.Instance().RestoreButtonOnClick();
	}

	public void ShowLeaderboard()
	{
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.LeaderBoard;
		SettingMenu.SetActive(value: false);
		HideSetting();
		Singleton<SoundManager>.Instance.PlayButton();
		Analytic.Instance.LogLBFacebook();
		if (!Singleton<TrailerTest>.instance.IsTrailer)
		{
			string leaderBoardName = GameData.Instance().GetLeaderBoardName(BoardType.Triangle, isBomb: false);
			ServicesManager.LeaderBoard().ShowLeaderBoard(leaderBoardName);
			LeaderBoardMenu.SetActive(value: true);
			m_LeaderBoardMenu.Show();
		}
		else
		{
			LeaderBoardTrailer.SetActive(value: true);
			m_LeaderBoardTrailer.Show();
		}
	}

	public void ShowNativeLeaderBoard()
	{
		Analytic.Instance.LogLBNative();
		Singleton<SoundManager>.Instance.PlayButton();
		ComingSoonMenu.SetActive(value: true);
		m_ComingSoon.Show();
	}

	public void HideComingSoon()
	{
		m_ComingSoon.Hide(delegate
		{
			ComingSoonMenu.SetActive(value: false);
		});
	}

	public void HideLeaderBoard()
	{
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.Home;
		Singleton<SoundManager>.Instance.PlayButton();
		if (!Singleton<TrailerTest>.instance.IsTrailer)
		{
			m_LeaderBoardMenu.Hide();
		}
		else
		{
			m_LeaderBoardTrailer.Hide();
		}
	}

	private void DelaySendAllLocalHighScore()
	{
		GameData.Instance().CheckSendAllLocalHighScore();
	}

	public void Rate()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		Application.OpenURL("market://details?id=" + packageNameRateShare);
	}

	public void Sound()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		Singleton<CheatManager>.instance.RaiseCount();
		isSound = !isSound;
		SetSound();
	}

	public void ChangeSkin(int i)
	{
		Singleton<SoundManager>.Instance.PlayButton();
	}

	public void Pause()
	{
		if (IsCanPause && Singleton<GameManager>.instance.State == GameState.Playing)
		{
			m_PreMenuState = m_MenuState;
			m_MenuState = MenuState.Pause;
			Singleton<ServicesManager>.Instance.HideBanner();
			Singleton<ServicesManager>.instance.ShowBannerPause();
			Singleton<SoundManager>.Instance.PlayButton();
			ShowPauseMenu();
			GameData.Instance().SubmitHighScore(Singleton<GameManager>.Instance.gameType, Singleton<GameManager>.Instance.isBomb);
		}
	}

	public void Home()
	{
		ReviewActiveArea.SetActive(value: false);
		Singleton<GameManager>.instance.State = GameState.Home;
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.Home;
		Singleton<GameManager>.Instance.ClearBoard();
		ShowHomeMenu();
	}

	public void ResetGame()
	{
		Singleton<GameManager>.Instance.ClearBoard();
		SwitchToInGame();
	}

	public void ResetGameOver()
	{
		Singleton<GameManager>.Instance.ClearBoard();
		SwitchToInGame();
	}

	public void Undo()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		Singleton<GameManager>.Instance.Undo();
	}

	public void Shop()
	{
		Singleton<SoundManager>.Instance.PlayButton();
		if (Singleton<GameManager>.Instance.State == GameState.Home)
		{
			Analytic.Instance.LogShopEvent("Home Shop");
		}
		else if (Singleton<GameManager>.Instance.State == GameState.Pause)
		{
			Analytic.Instance.LogShopEvent("Pause Shop");
			Singleton<ServicesManager>.instance.HideBannerPause();
		}
		else if (Singleton<GameManager>.Instance.State == GameState.GameOver)
		{
			Analytic.Instance.LogShopEvent("Game Over Shop");
		}
		ShowShopMenu();
	}

	public void ShopLeft()
	{
		LogUtils.Log("ShopLeft");
		Singleton<SoundManager>.Instance.PlayButton();
	}

	public void ShopRight()
	{
		LogUtils.Log("ShopRight");
		Singleton<SoundManager>.Instance.PlayButton();
	}

	public void Buy()
	{
		LogUtils.Log("Buy");
		Singleton<SoundManager>.Instance.PlayButton();
	}

	private void SwitchToInGame()
	{
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.InGame;
		Singleton<ServicesManager>.instance.ShowBanner();
		HideAll();
		LeaderBoardMenu.SetActive(value: false);
		ReviewActiveArea.SetActive(value: false);
		ThemeName currentTheme = GameData.Instance().GetCurrentTheme();
		Analytic.Instance.LogUseThemeEvent(currentTheme);
		InGameMenu.SetActive(value: true);
		SetShowBoardAndShape(isShow: true);
		StartCheckTime();
		ResetTimePlay();
		StartCountTimePlay();
		StartGame();
		m_TimeStartPlay = Time.time;
		EscapeController.Instance().SetCallback(ShowPauseMenu);
		if (m_TimeCountSpinCorou == null && !m_IsShowSpin)
		{
			m_TimeCountSpinCorou = StartCoroutine(IE_CountSpin());
		}
		CheckShowSpin();
	}

	private void HideAll()
	{
		HomeMenu.SetActive(value: false);
		SelectMode.SetActive(value: false);
		NoAdsMenu.SetActive(value: false);
		PauseMenu.SetActive(value: false);
		ShopMenu.SetActive(value: false);
		InGameMenu.SetActive(value: false);
		SetShowBoardAndShape(isShow: false);
		GameOver.SetActive(value: false);
		SettingMenu.SetActive(value: false);
	}

	public void ShowHomeMenu()
	{
		HideAll();
		HomeMenu.SetActive(value: true);
		EscapeController.Instance().SetCallback(quitOnEscape.ShowDialogQuit);
	}

	public void ShowSelectMode()
	{
		HideAll();
		HomeMenu.SetActive(value: true);
		SelectMode.SetActive(value: true);
	}

	public void ShowNoAdsMenu()
	{
		NoAdsMenu.SetActive(value: true);
	}

	public void ShowInGame()
	{
		Singleton<SoundManager>.instance.PlayButton();
		Singleton<ServicesManager>.instance.RefreshBannerPause();
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.InGame;
		m_PauseMenu.Hide(delegate
		{
			InGameMenu.SetActive(value: true);
			SetShowBoardAndShape(isShow: true);
			Singleton<GameManager>.Instance.ResumeGame();
			Singleton<ServicesManager>.Instance.ShowBanner();
		});
		StartCountTimePlay();
		EscapeController.Instance().SetCallback(ShowPauseMenu);
	}

	public void ShowPauseMenu()
	{
		InGameMenu.SetActive(value: true);
		SetShowBoardAndShape(isShow: true);
		PauseMenu.SetActive(value: true);
		Singleton<GameManager>.Instance.PauseGame();
		m_PauseMenu.Show();
		EscapeController.Instance().SetCallback(ShowInGame);
	}

	public void ShowShopMenu()
	{
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.Shop;
		if (m_PreMenuState == MenuState.Pause)
		{
			SetShowBoardAndShape(isShow: false);
		}
		ShopMenu.SetActive(value: true);
		m_ShopMenu.SetListMenuUnder(GetListMenuUnder());
		m_ShopMenu.Show();
	}

	public void HideShopMenu()
	{
		m_MenuState = m_PreMenuState;
		m_PreMenuState = MenuState.Shop;
		Singleton<SoundManager>.instance.PlayButton();
		if (m_MenuState == MenuState.Pause)
		{
			UnityEngine.Debug.Log("Show board and shape");
			SetShowBoardAndShape(isShow: true);
		}
		m_ShopMenu.Hide(delegate
		{
			ShopMenu.SetActive(value: false);
			if (Singleton<GameManager>.instance.State == GameState.Pause)
			{
				Singleton<ServicesManager>.instance.ShowBannerPause();
			}
		});
	}

	public void ShowGameOver()
	{
		HideAll();
		ReviewActiveArea.SetActive(value: true);
		Analytic.Instance.LogTimePlay(GetTimePlay());
		Analytic.Instance.LogMoveCount(Singleton<GameManager>.instance.MoveCount);
		ResetTimePlay();
		Singleton<ServicesManager>.Instance.HideBanner();
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.GameOver;
		m_DailyMenu.ShowMoney();
		GameOver.SetActive(value: true);
		m_GameOverMenu.CheckLoadVideo();
		m_GameOverMenu.Show(delegate
		{
			if (!isShowFullAdContinue)
			{
				Singleton<ServicesManager>.instance.ShowFullAds(delegate
				{
					CheckShowRateUs();
				});
			}
			else
			{
				CheckShowRateUs();
				isShowFullAdContinue = false;
			}
			Singleton<GameManager>.instance.ResetBoardColor();
			DateTime now = DateTime.Now;
			string @string = PlayerPrefs.GetString("FirstDate");
			DateTime d = DateTime.Parse(@string);
			int num = (now - d).Days;
			if (num < 0)
			{
				num = 0;
			}
			if (num <= 27)
			{
				DailyStat[] dailyStat = GameData.Instance().GetDailyStat();
				for (int i = 0; i < dailyStat.Length; i++)
				{
					if (i < num && dailyStat[i] != 0)
					{
						dailyStat[i] = DailyStat.NotCollected;
					}
				}
				if (dailyStat[num] == DailyStat.NotAvailable || dailyStat[num] == DailyStat.Available)
				{
					m_DailyMenu.ShowMoney();
					dailyStat[num] = DailyStat.Available;
					GameData.Instance().SetDailyStat(dailyStat);
					m_DailyMenu.SetRewardText();
					m_DailyMenu.SetTodayReward(num);
					m_DailyMenu.SetStatDailyBtns(dailyStat);
					m_DailyMenu.ShowGetRewardBtn(isShow: true);
					dayReward = num;
					VideoAdsStat doubleStat = CheckStatVideo();
					m_DailyMenu.SetDoubleStat(doubleStat);
					DailyMenu.SetActive(value: true);
					Singleton<ServicesManager>.instance.ShowBannerPause();
				}
			}
		});
	}

	public void CheckShowRateUs()
	{
		int highScore = GameData.Instance().GetHighScore(BoardType.Triangle, isBomb: false);
		if (highScore >= 300)
		{
			if (highScore < 400)
			{
				RateApp("Key300");
			}
			else if (highScore < 500)
			{
				RateApp("Key400");
			}
			else
			{
				RateApp("Key500");
			}
		}
	}

	private void RateApp(string keyScore)
	{
		if (!PlayerPrefs.HasKey(keyScore))
		{
			PlayerPrefs.SetInt(keyScore, 1);
			UnityEngine.Debug.Log("Show rate us");
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				iOSReviewRequest.Request();
			}
			else if (!GameData.Instance().IsRate())
			{
				NativeToolkit.RateApp("Enjoying Trigon?", "Rate this game so we can keep more updates coming.", "Rate now", "Later", "No thanks", string.Empty, RateAppCallback);
			}
		}
	}

	private void RateAppCallback(string result)
	{
		if (result == "2")
		{
			Analytic.Instance.LogAppEvent("Rate: Rate now");
			GameData.Instance().SetRate(value: true);
		}
		else if (result == "0")
		{
			Analytic.Instance.LogAppEvent("Rate: No thanks");
			GameData.Instance().SetRate(value: true);
		}
		else
		{
			Analytic.Instance.LogAppEvent("Rate: Later");
			GameData.Instance().SetRate(value: false);
		}
	}

	public void OnClickRestartOver()
	{
		Analytic.Instance.LogRestartEvent("Game Over");
		SettingMenu.SetActive(value: false);
		m_GameOverMenu.Hide(delegate
		{
			GameOver.SetActive(value: false);
			ResetGameOver();
		});
	}

	public void OnClickHomeOver()
	{
		Singleton<SoundManager>.instance.PlayButton();
		SettingMenu.SetActive(value: false);
		m_GameOverMenu.Hide(delegate
		{
			GameOver.SetActive(value: false);
			Home();
		});
	}

	public void OnClickHomePause()
	{
		Singleton<SoundManager>.instance.PlayButton();
		Singleton<ServicesManager>.instance.RefreshBannerPause();
		GameData.Instance().SaveMoveCount(Singleton<GameManager>.instance.MoveCount);
		GameData.Instance().SaveTimePlayLast(GetTimePlay());
		SettingMenu.SetActive(value: false);
		HideTutorial();
		if (!Singleton<GameManager>.instance.IsRunTutorial())
		{
			Singleton<GameManager>.instance.SaveGame();
		}
		m_PauseMenu.Hide(delegate
		{
			PauseMenu.SetActive(value: false);
			Home();
		});
	}

	public void OnClickRestartPause()
	{
		Analytic.Instance.LogRestartEvent("Pause");
		Singleton<ServicesManager>.instance.RefreshBannerPause();
		SettingMenu.SetActive(value: false);
		m_PauseMenu.Hide(delegate
		{
			PauseMenu.SetActive(value: false);
			Singleton<ServicesManager>.Instance.ShowFullAds();
			ResetGame();
		});
	}

	public void FadeScreenNoSlot()
	{
	}

	public void ShowSetting()
	{
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.Setting;
		Singleton<SoundManager>.instance.PlayButton();
		SettingMenu.SetActive(value: true);
		m_SettingMenu.Show();
	}

	public void HideSetting()
	{
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.Home;
		Singleton<SoundManager>.instance.PlayButton();
		Singleton<CheatManager>.instance.CheckOpenCheat();
		m_SettingMenu.Hide(delegate
		{
			SettingMenu.SetActive(value: false);
		});
	}

	private void StartGame()
	{
		Singleton<ServicesManager>.Instance.ShowBanner();
		Singleton<GameManager>.Instance.StartMainGame();
	}

	private void SetTheme(ThemeName themeName)
	{
		ThemeData themeData = m_ThemeManger.GetThemeData(themeName);
		GameData.Instance().SetCurrentTheme(themeName);
		m_IngameMenu.SetThemeUI(themeData.DictIngameUI);
		m_MainMenu.SetThemeUI(themeData.DictMainUI);
		m_PauseMenu.SetThemeUI(themeData.DictPauseUI);
		m_GameOverMenu.SetThemeUI(themeData.DictGameOverUI);
		m_SettingMenu.SetThemeUI(themeData.DictSettingUI);
		m_NoAdsMenu.SetThemeUI(themeData.DictNoAdsUI);
		m_EffectCombo.SetThemeUI(themeData.DictIngameUI);
		m_LeaderBoardMenu.SetThemeUI(themeData.DictSettingUI);
		m_LeaderBoardTrailer.SetThemeUI(themeData.DictSettingUI);
		m_DailyMenu.SetThemeUI(themeData.DictGameOverUI);
		m_ContinueMenu.SetThemeUI(themeData.DictContinueUI);
		m_SpinMenuOld.SetThemeUI(themeData.DictIngameUI);
		m_SpinMenuEvent.SetThemeUI(themeData.DictIngameUI);
		m_SpinMenu.SetThemeUI(themeData.DictIngameUI);
		Singleton<AssetManagers>.instance.ChangeTheme(themeName);
		Singleton<GameManager>.instance.SetBorderShape();
		if (Singleton<GameManager>.instance.State == GameState.Pause && themeData.IsHaveBorder)
		{
			Singleton<GameManager>.instance.CheckDeadPiece();
		}
		if (themeName == ThemeName.Ice)
		{
			SetDiamond(enable: true);
		}
		else
		{
			SetDiamond(enable: false);
		}
		if (themeName == ThemeName.Farm)
		{
			BackgroundFarm.SetActive(value: true);
		}
		else
		{
			BackgroundFarm.SetActive(value: false);
		}
		if (themeName == ThemeName.Xmas)
		{
			ParticleXmasTheme.SetActive(value: true);
		}
		else
		{
			ParticleXmasTheme.SetActive(value: false);
		}
	}

	private void SetDiamond(bool enable)
	{
		for (int i = 0; i < m_IceDiamonds.Length; i++)
		{
			m_IceDiamonds[i].SetActive(enable);
		}
	}

	public void OnClickUseTheme(int id)
	{
		ThemeName currentTheme = GameData.Instance().GetCurrentTheme();
		if (id != (int)currentTheme)
		{
			ThemeData themeData = m_ThemeManger.GetThemeData(currentTheme);
			BinaryRawImage.ClearCache(themeData.BackGroundBytes);
		}
		SetTheme((ThemeName)id);
		HideShopMenu();
	}

	public void OnClickBuyTheme(int id)
	{
		ThemeName themeName = themeBuyChoose = (ThemeName)id;
		ThemeData themeData = m_ThemeManger.GetThemeData(themeName);
		int money = GameData.Instance().GetMoney();
		if (money >= themeData.ThemePrice)
		{
			OpenConfirmDialog(themeName);
			return;
		}
		Singleton<SoundManager>.instance.PlayButton();
		ShowNotEnough();
	}

	public void OpenConfirmDialog(ThemeName themeName)
	{
		ConfirmMenu.SetActive(value: true);
		m_ConfirmMenu.Show(themeName);
	}

	public void OnClickConfirmOK()
	{
		ThemeData themeData = Singleton<ThemeManager>.instance.GetThemeData(themeBuyChoose);
		Analytic.Instance.LogBuyThemeEvent(themeData.Name);
		GameData.Instance().AddMoney(-themeData.ThemePrice);
		GameData.Instance().SetUnlockTheme(themeData.Name, value: true);
		ReloadData();
		OnClickUseTheme((int)themeData.Name);
		OnClickConfirmCancel();
	}

	public void OnClickConfirmCancel()
	{
		m_ConfirmMenu.Hide(delegate
		{
			ConfirmMenu.SetActive(value: false);
		});
	}

	public void ReloadData()
	{
		m_MainMenu.ReloadData();
		m_ShopMenu.ReloadData();
		m_IngameMenu.ReloadData();
		m_GameOverMenu.ReloadData();
	}

	public void TakeBoardScreenshot()
	{
		if (cloneRoot != null)
		{
			UnityEngine.Object.Destroy(cloneRoot);
		}
		cloneRoot = UnityEngine.Object.Instantiate(originRoot);
		cloneRoot.transform.SetParent(BoardScreenshot.transform);
		cloneRoot.transform.localPosition = -Vector3.forward;
		cloneRoot.transform.localScale = Vector3.one * longScreenConfig.m_BoardScreenShotScale;
		SetSortingLayer(cloneRoot, "UI");
	}

	public void SetSortingLayer(GameObject obj, string layer)
	{
		SpriteRenderer[] componentsInChildren = obj.GetComponentsInChildren<SpriteRenderer>();
		SpriteRenderer[] array = componentsInChildren;
		foreach (SpriteRenderer spriteRenderer in array)
		{
			spriteRenderer.sortingLayerName = layer;
		}
	}

	public VideoAdsStat CheckStatVideo()
	{
		if (Singleton<ServicesManager>.Instance.IsRewardAvailable())
		{
			return VideoAdsStat.Available;
		}
		return VideoAdsStat.NotAvailable;
	}

	public float GetTimePlay()
	{
		float time = Time.time;
		float num = time - m_TimeStartPlay;
		num += LastPlayTime;
		UnityEngine.Debug.Log("timePlay:" + num);
		return num;
	}

	public void StartCountTimePlay()
	{
		m_TimePlayStart = DateTime.Now;
	}

	public void StartCheckTimeAd()
	{
		m_TimeStartCountAd = DateTime.Now;
	}

	public void StartCheckTime()
	{
		m_TimeStart = DateTime.Now;
	}

	public float GetTimeCheckShowAds()
	{
		DateTime now = DateTime.Now;
		float num = (float)(now - m_TimeStartCountAd).TotalSeconds;
		m_TimeCheckShowAds += num;
		return m_TimeCheckShowAds;
	}

	public float GetTimeCheckReward()
	{
		DateTime now = DateTime.Now;
		float num = (float)(now - m_TimeStart).TotalSeconds;
		m_TimeCheckReward += num;
		return m_TimeCheckReward;
	}

	public void ResetTimeCheckReward()
	{
		m_TimeCheckReward = 0f;
	}

	public void ResetTimeCheckShowAds()
	{
		m_TimeCheckShowAds = 0f;
		m_TimeStartCountAd = DateTime.Now;
	}

	public void ResetTimePlay()
	{
		m_TimePlay = 0f;
	}

	public void UpdateScoreInGame(int currentScore, int highScore, bool isStartGame)
	{
		m_IngameMenu.UpdateScore(currentScore, highScore, isStartGame);
		Singleton<Achievement>.instance.CheckUnlockScore(highScore);
	}

	public void StartAnimRewardIngame(int numCount, Vector3 shapePos)
	{
		if (Singleton<GameManager>.Instance.IsRunTutorial())
		{
			numCount = 2;
		}
		GameData.Instance().AddMoney(numCount - 1);
		m_IngameMenu.StartAnimReward(shapePos);
		if (Singleton<GameManager>.instance.CurrentTut > GameManager.TutStep.Tut3 && Singleton<GameManager>.Instance.IsShowEffectCombo)
		{
			StartCoroutine(IE_StartEffectCombo(numCount));
		}
	}

	private IEnumerator IE_StartEffectCombo(int numCount)
	{
		yield return m_DelayClear;
		m_EffectCombo.Show(numCount);
	}

	public void StartAnimRewardIngame(Vector3 shapePos)
	{
		m_IngameMenu.StartAnimReward(shapePos);
	}

	public void StartAnimSnow(Vector3 shapePos)
	{
		m_IngameMenu.StartAnimSnow(shapePos);
	}

	public IEnumerator IEShowTutorial1()
	{
		HideTutorial(tweenTutorial1);
		HideTutorial(tweenTutorial2);
		yield return new WaitForSeconds(0.3f);
		handTutorial.gameObject.SetActive(value: true);
		handTutorial.anchoredPosition = new Vector2(-9f, -514f);
		if (tweenTutorial1 != null && tweenTutorial1.isRunning())
		{
			tweenTutorial1.stop();
			tweenTutorial1 = null;
		}
		tweenTutorial1 = handTutorial.ZKanchoredPositionTo(new Vector2(-47f, -92f), 2f).setLoops(LoopType.RestartFromBeginning, int.MaxValue, 0.5f).setEaseType(EaseType.QuadOut)
			.setLoopCompletionHandler(delegate
			{
				handTutorial.anchoredPosition = new Vector2(-9f, -514f);
			});
		tweenTutorial1.start();
	}

	public IEnumerator IEShowTutorial2(bool isSpin)
	{
		HideTutorial(tweenTutorial1);
		HideTutorial(tweenTutorial2);
		yield return new WaitForSeconds(0.3f);
		if (isSpin)
		{
			handTutorial.gameObject.SetActive(value: true);
			handTutorial.anchoredPosition = new Vector2(-9f, -514f);
			handTutorial.localScale = new Vector3(0.85f, 0.85f, 1f);
			tweenTutorial2 = handTutorial.ZKlocalScaleTo(new Vector3(0.75f, 0.75f, 1f), 1f).setLoops(LoopType.RestartFromBeginning, int.MaxValue, 0.2f).setEaseType(EaseType.QuadOut)
				.setLoopCompletionHandler(delegate
				{
					handTutorial.localScale = new Vector3(0.85f, 0.85f, 1f);
				});
			tweenTutorial2.start();
			onShapeRotate = delegate
			{
				StartCoroutine(IEShowTutorial2_Drag());
			};
		}
		else
		{
			StartCoroutine(IEShowTutorial2_Drag());
		}
	}

	private IEnumerator IEShowTutorial2_Drag()
	{
		HideTutorial(tweenTutorial1);
		HideTutorial(tweenTutorial2);
		yield return null;
		handTutorial.gameObject.SetActive(value: true);
		handTutorial.anchoredPosition = new Vector2(-9f, -514f);
		if (tweenTutorial1 != null && tweenTutorial1.isRunning())
		{
			tweenTutorial1.stop();
			tweenTutorial1 = null;
		}
		tweenTutorial1 = handTutorial.ZKanchoredPositionTo(new Vector2(-85f, -160f), 2f).setLoops(LoopType.RestartFromBeginning, int.MaxValue, 0.5f).setEaseType(EaseType.QuadOut)
			.setLoopCompletionHandler(delegate
			{
				handTutorial.anchoredPosition = new Vector2(-9f, -514f);
			});
		tweenTutorial1.start();
	}

	public void OnShapeRotate()
	{
		if (onShapeRotate != null)
		{
			onShapeRotate();
			onShapeRotate = null;
		}
	}

	private void HideTutorial<T>(ITween<T> tween) where T : struct
	{
		handTutorial.gameObject.SetActive(value: false);
	}

	public void HideTutorial()
	{
		HideTutorial(tweenTutorial1);
		HideTutorial(tweenTutorial2);
	}

	public void OnShowReview(bool isHold)
	{
		if (isHold)
		{
			Singleton<SoundManager>.instance.PlayButton();
			GameOver.SetActive(value: false);
			cloneRoot.SetActive(value: false);
			InGameMenu.SetActive(value: true);
			SetShowBoardAndShape(isShow: true);
		}
		else
		{
			GameOver.SetActive(value: true);
			cloneRoot.SetActive(value: true);
			InGameMenu.SetActive(value: false);
			SetShowBoardAndShape(isShow: false);
		}
	}

	public void ShowNotEnough()
	{
		Singleton<SoundManager>.instance.PlayButton();
		NotEnoughDiamond.SetActive(value: true);
		m_NotEnoughMenu.Show();
	}

	public void HideNotEnough()
	{
		Singleton<SoundManager>.instance.PlayButton();
		m_NotEnoughMenu.Hide(delegate
		{
			NotEnoughDiamond.SetActive(value: false);
		});
	}

	public void TestAnimationReward()
	{
		m_IngameMenu.StartAnimReward(Vector3.zero);
	}

	private IEnumerator TakeScoreScreenshot()
	{
		yield return new WaitForSeconds(0.2f);
		ShareBtn.SetActive(value: false);
		LogoScreenshot.SetActive(value: true);
		yield return new WaitForEndOfFrame();
		float imageheight = 352f;
		float imagewidth = 720f;
		float ratio = imagewidth / imageheight;
		float canvasheight = 1280f;
		int widthTexture = (int)((float)Screen.height / (canvasheight / imageheight) * ratio);
		int heightTexture = (int)((float)Screen.height / (canvasheight / imageheight));
		Texture2D screenTexture = new Texture2D(widthTexture, heightTexture, TextureFormat.RGB24,  true);
		screenTexture.ReadPixels(new Rect((float)Screen.width / 2f - (float)widthTexture / 2f, (float)Screen.height / 2f - (float)heightTexture / 2f + 151f / 640f * (float)Screen.height, widthTexture, heightTexture), 0, 0);
		screenTexture.Apply();
		UM_ShareUtility.ShareMedia("#Trigon", "Best triangle game", screenTexture);
		LogoScreenshot.SetActive(value: false);
		ShareBtn.SetActive(value: true);
		MNP.HidePreloader();
	}

	public void ShowScoreScreenshot()
	{
		StartCoroutine(TakeScoreScreenshot());
	}

	public void HideScoreScreenshot()
	{
		ShareBtn.SetActive(value: true);
		LogoScreenshot.SetActive(value: false);
		DemoScoreScreenshot.SetActive(value: false);
	}

	public void ShareScore()
	{
		Singleton<SoundManager>.instance.PlayButton();
		MNP.ShowPreloader(string.Empty, string.Empty);
		StartCoroutine(TakeScoreScreenshot());
	}

	public void ShowContinuePanel()
	{
		Singleton<ServicesManager>.instance.HideBanner();
		if (Singleton<ServicesManager>.instance.IsFullAdsLoaded())
		{
			Singleton<ServicesManager>.instance.ShowFullAds(delegate
			{
				isShowFullAdContinue = true;
				m_ContinueMenu.ShowContinue();
			});
			return;
		}
		isShowFullAdContinue = false;
		m_ContinueMenu.ShowContinue();
	}

	private IEnumerator IE_CountTimeGO()
	{
		int time = 10;
		m_CountTimeTxt.text = time.ToString();
		m_CountTimeTxt.transform.localScale = Vector3.one;
		while (time > 0)
		{
			yield return oneSec;
			time--;
			TweenChain tweenChain = new TweenChain();
			tweenChain.appendTween(m_CountTimeTxt.transform.ZKlocalScaleTo(Vector3.one * 1.1f, 0.1f).setEaseType(EaseType.Linear).setCompletionHandler(delegate
			{
				m_CountTimeTxt.text = time.ToString();
			}));
			tweenChain.appendTween(m_CountTimeTxt.transform.ZKlocalScaleTo(Vector3.one, 0.1f).setEaseType(EaseType.Linear));
			tweenChain.start();
		}
		yield return oneSec;
		OnClickNoContinue();
	}

	public void OnClickContinueWithAds()
	{
		Singleton<ServicesManager>.instance.HideBannerPause();
		m_ContinueMenu.StopAllCorou();
		Singleton<ServicesManager>.instance.ShowRewardAds(delegate
		{
			Analytic.Instance.LogShowRewardContinue();
			Analytic.Instance.LogShowRewardGeneral();
			Singleton<GameManager>.instance.ContinueAfterAds();
			Singleton<ServicesManager>.instance.ShowBanner();
			HideAll();
			ContinueMenu.SetActive(value: false);
			InGameMenu.SetActive(value: true);
			SetShowBoardAndShape(isShow: true);
		}, delegate
		{
			OnClickNoContinue();
		});
	}

	public void OnClickNoContinue()
	{
		Singleton<ServicesManager>.instance.HideBannerPause();
		m_ContinueMenu.HideContinue();
		ContinueMenu.SetActive(value: false);
		ShowGameOver();
	}

	public void SetColorBGCombo(Color color)
	{
		m_EffectCombo.SetColorBGCombo(color);
	}

	public bool IsNoAdsPanelShow()
	{
		return NoAdsMenu.activeSelf;
	}

	public void OnClickSendMailSupport()
	{
		Singleton<SoundManager>.instance.PlayButton();
		Analytic.Instance.LogAppEvent("Send email");
		if (Application.platform == RuntimePlatform.Android)
		{
			UM_ShareUtility.SendMail("Trigon Android support", string.Empty, "support@ieccorp.vn");
		}
		else
		{
			UM_ShareUtility.SendMail("Trigon IOS support", string.Empty, "support@ieccorp.vn");
		}
	}

	public void OnClickShowCredit()
	{
		Singleton<SoundManager>.instance.PlayButton();
		m_Credit.ShowCredit();
	}

	public void OnClickHideCredit()
	{
		m_Credit.HideCredit();
	}

	public void OnClickDailyReward(int day)
	{
		GameData.Instance().AddMoney(m_DailyMenu.RewardData.MoneyRewards[day - 1]);
		m_GameOverMenu.ReloadData();
		DailyStat[] dailyStat = GameData.Instance().GetDailyStat();
		dailyStat[day - 1] = DailyStat.Collected;
		GameData.Instance().SetDailyStat(dailyStat);
		m_DailyMenu.SetStatDailyBtns(dailyStat);
		m_DailyMenu.StartAnimReward(day, isDouble: false, delegate
		{
			DailyMenu.SetActive(value: false);
		});
	}

	public void ClickDailyGet(bool isDouble)
	{
		int num = dayReward + 1;
		int num2 = m_DailyMenu.RewardData.MoneyRewards[num - 1];
		if (isDouble)
		{
			num2 *= 2;
		}
		GameData.Instance().AddMoney(num2);
		DailyStat[] dailyStat = GameData.Instance().GetDailyStat();
		dailyStat[num - 1] = DailyStat.Collected;
		GameData.Instance().SetDailyStat(dailyStat);
		m_DailyMenu.SetStatDailyBtns(dailyStat);
		DailyMenu.SetActive(value: false);
		m_GameOverMenu.StartAnimReward(num2, isDaily: true);
	}

	public void OnClickGetX1()
	{
		Singleton<ServicesManager>.instance.HideBannerPause();
		ClickDailyGet(isDouble: false);
	}

	public void OnClickGetX2()
	{
		Singleton<ServicesManager>.instance.HideBannerPause();
		Singleton<ServicesManager>.instance.ShowRewardAds(delegate
		{
			Analytic.Instance.LogShowRewardGeneral();
			Analytic.Instance.LogShowRewardDouble();
			ClickDailyGet(isDouble: true);
		});
	}

	public void WatchVideoUnlock(int index)
	{
		int videoCounted = GameData.Instance().GetCountVideUnlock((ThemeName)index);
		Singleton<ServicesManager>.instance.ShowRewardAds(delegate
		{
			Analytic.Instance.LogShowRewardGeneral();
			Analytic.Instance.LogShowRewardTheme();
			Analytic.Instance.LogWatchVideoUnlock((ThemeName)index);
			videoCounted++;
			GameData.Instance().SetCountVideoUnlock((ThemeName)index, videoCounted);
			if (videoCounted >= Singleton<ThemeManager>.instance.GetThemeData((ThemeName)index).VideoNeedToUnlock)
			{
				GameData.Instance().SetUnlockTheme(index, value: true);
				Analytic.Instance.LogUnlockByVideo((ThemeName)index);
			}
			m_ShopMenu.ReloadData();
		});
	}

	public void OnClickMoneyPanelShop()
	{
		OnClickShopIAP("ShopPanel");
	}

	public void OnClickNotEnoughDiamond()
	{
		OnClickShopIAP("NotEnough");
	}

	private IEnumerator IE_CountSpin()
	{
		ResetCountSpin();
		while (m_TimeSpinCount < m_TimeGetSpin)
		{
			yield return oneSec;
			m_TimeSpinCount++;
		}
		m_IsShowSpin = true;
		CheckShowSpin();
	}

	private void CheckShowSpin()
	{
		if (Singleton<ServicesManager>.instance.IsRewardAvailable() && m_IsShowSpin && Singleton<GameManager>.instance.State == GameState.Playing)
		{
			m_SpinBtn.SetActive(value: true);
			Analytic.Instance.LogShowSpin();
			ShowGift();
		}
	}

	public void ShowGift()
	{
		if (!m_GiftAnimatorObj.activeSelf)
		{
			m_GiftAnimatorObj.SetActive(value: true);
			if (Singleton<GameManager>.instance.AB_GiftEffect == GameManager.ABGiftEffect.Light)
			{
				m_GiftLightObj.SetActive(value: true);
				m_LightEffect.StartEffect();
			}
			else
			{
				m_GiftLightObj.SetActive(value: false);
			}
			m_GiftAnimator.Play("Start");
			StartCountHideGift();
		}
		else
		{
			StartCountHideGift();
		}
	}

	private void StartCountHideGift()
	{
		if (hideGiftCorou != null)
		{
			StopCoroutine(hideGiftCorou);
			hideGiftCorou = null;
		}
		hideGiftCorou = StartCoroutine(IE_HideGift());
	}

	private IEnumerator IE_HideGift()
	{
		yield return showTime;
		m_GiftAnimator.SetTrigger("ScaleBack");
		yield return hideTime;
		m_SpinBtn.SetActive(value: false);
		m_GiftAnimatorObj.SetActive(value: false);
		m_GiftAnimatorObj.transform.localScale = Vector3.one;
		m_LightEffect.ClearAllTween();
		m_GiftLightObj.SetActive(value: false);
		StartCountAfterHideGift();
	}

	public void OnClickSpinBtn()
	{
		if (Singleton<GameManager>.instance.State == GameState.Playing)
		{
			if (hideGiftCorou != null)
			{
				StopCoroutine(hideGiftCorou);
				hideGiftCorou = null;
			}
			Analytic.Instance.LogClickSpin();
			m_SpinBtn.SetActive(value: false);
			m_SpinMenu.RandomReward();
			m_GiftAnimatorObj.SetActive(value: false);
			m_IsShowSpin = false;
			m_SpinConfirm.gameObject.SetActive(value: true);
			m_SpinConfirm.ShowReward();
		}
	}

	private void ResetCountSpin()
	{
		m_TimeSpinCount = 0;
		switch (Singleton<GameManager>.instance.GiftTime)
		{
		case GiftTime.Three:
			m_TimeGetSpin = 180;
			break;
		case GiftTime.Five:
			m_TimeGetSpin = 300;
			break;
		default:
			m_TimeGetSpin = 420;
			break;
		}
		if (m_IsTestSpin)
		{
			m_TimeGetSpin = 5;
		}
		m_IsShowSpin = false;
	}

	public void OnClickNoSpin()
	{
		Analytic.Instance.LogClickNoThankSpin();
		StartCountAfterHideGift();
	}

	public void StartCountAfterHideGift()
	{
		m_SpinConfirm.gameObject.SetActive(value: false);
		if (m_TimeCountSpinCorou != null)
		{
			StopCoroutine(m_TimeCountSpinCorou);
			m_TimeCountSpinCorou = null;
		}
		m_TimeCountSpinCorou = StartCoroutine(IE_CountSpin());
	}

	public void OnClickSpin()
	{
		Singleton<ServicesManager>.Instance.ShowRewardAds(delegate
		{
			Analytic.Instance.LogShowRewardSpin();
			Analytic.Instance.LogShowRewardGeneral();
			m_SpinConfirm.gameObject.SetActive(value: false);
			m_SpinMenu.gameObject.SetActive(value: true);
			m_SpinMenu.Spin();
		});
	}

	public void OnClickGetX1Spin()
	{
		m_SpinMenu.gameObject.SetActive(value: false);
		int reward = m_SpinMenu.GetReward();
		GameData.Instance().AddMoney(reward);
		if (m_IsOpenEvent)
		{
			GameData.Instance().AddSnowFlake(1);
			m_IngameMenu.StartAnimSnow(Vector3.zero);
		}
		m_IngameMenu.StartAnimReward(isDouble: false, reward, delegate
		{
			StartCoroutine(IE_WaitAfterAnim());
			ReloadCostSkill();
		});
	}

	public void OnClickGetX2Spin()
	{
		Singleton<ServicesManager>.instance.ShowRewardAds(delegate
		{
			m_SpinMenu.gameObject.SetActive(value: false);
			Analytic.Instance.LogShowRewardDoubleSpin();
			Analytic.Instance.LogShowRewardGeneral();
			int reward = m_SpinMenu.GetReward();
			GameData.Instance().AddMoney(reward * 2);
			if (m_IsOpenEvent)
			{
				GameData.Instance().AddSnowFlake(2);
				m_IngameMenu.StartAnimSnow(Vector3.zero);
			}
			m_IngameMenu.StartAnimReward(isDouble: true, reward, delegate
			{
				StartCoroutine(IE_WaitAfterAnim());
				ReloadCostSkill();
			});
		});
	}

	private IEnumerator IE_WaitAfterAnim()
	{
		yield return waitSpinAnim;
		m_SpinMenu.gameObject.SetActive(value: false);
		if (m_TimeCountSpinCorou != null)
		{
			StopCoroutine(m_TimeCountSpinCorou);
			m_TimeCountSpinCorou = null;
		}
		m_TimeCountSpinCorou = StartCoroutine(IE_CountSpin());
	}

	public void SetShowBoardAndShape(bool isShow)
	{
		if (originRoot == null)
		{
			originRoot = GameObject.Find("Root");
		}
		if (m_ShadowParent == null)
		{
			m_ShadowParent = GameObject.Find("Shadows");
		}
		if (m_ShapeParent == null)
		{
			m_ShapeParent = GameObject.Find("Shapes");
		}
		originRoot.SetActive(isShow);
		m_ShadowParent.SetActive(isShow);
		m_ShapeParent.SetActive(isShow);
	}

	private List<GameObject> GetListMenuUnder()
	{
		List<GameObject> list = new List<GameObject>();
		switch (m_PreMenuState)
		{
		case MenuState.Home:
			list.Add(HomeMenu);
			break;
		case MenuState.InGame:
			list.Add(InGameMenu);
			list.Add(m_ShapeParent);
			list.Add(m_ShadowParent);
			list.Add(originRoot);
			break;
		case MenuState.LeaderBoard:
			list.Add(HomeMenu);
			list.Add(LeaderBoardMenu);
			break;
		case MenuState.Pause:
			list.Add(PauseMenu);
			list.Add(InGameMenu);
			break;
		case MenuState.Setting:
			list.Add(SettingMenu);
			list.Add(HomeMenu);
			break;
		case MenuState.Shop:
			list.Add(ShopMenu);
			break;
		case MenuState.GameOver:
			list.Add(GameOver);
			break;
		}
		return list;
	}

	public void ReloadCostSkill()
	{
		int costNew3Shapes = GameData.Instance().GetCostNew3Shapes();
		int costUndo = GameData.Instance().GetCostUndo();
		m_Skill3CostTxt.text = costNew3Shapes.ToString();
		m_SkillUndoCostTxt.text = costUndo.ToString();
		ScaleTextBaseOnCost(costNew3Shapes, costUndo);
		CheckCostHigherThanMoney(costNew3Shapes, costUndo);
		ReloadData();
	}

	public void ScaleTextBaseOnCost(int costSkill3, int costSkillUndo)
	{
		if (costSkill3 < 10)
		{
			m_Skill3CostObj.anchoredPosition = lower10Pos;
			m_Skill3CostObj.transform.localScale = lower10Scale;
		}
		else if (costSkill3 < 100)
		{
			m_Skill3CostObj.anchoredPosition = lower100Pos;
			m_Skill3CostObj.transform.localScale = lower100Scale;
		}
		else if (costSkill3 < 1000)
		{
			m_Skill3CostObj.anchoredPosition = lower1000Pos;
			m_Skill3CostObj.transform.localScale = lower1000Scale;
		}
		else
		{
			m_Skill3CostObj.anchoredPosition = higher1000Pos;
			m_Skill3CostObj.transform.localScale = higher1000Scale;
		}
		if (costSkillUndo < 10)
		{
			m_SkillUndoObj.anchoredPosition = lower10Pos;
			m_SkillUndoObj.transform.localScale = lower10Scale;
		}
		else if (costSkillUndo < 100)
		{
			m_SkillUndoObj.anchoredPosition = lower100Pos;
			m_SkillUndoObj.transform.localScale = lower100Scale;
		}
		else if (costSkillUndo < 1000)
		{
			m_SkillUndoObj.anchoredPosition = lower1000Pos;
			m_SkillUndoObj.transform.localScale = lower1000Scale;
		}
		else
		{
			m_SkillUndoObj.anchoredPosition = higher1000Pos;
			m_SkillUndoObj.transform.localScale = higher1000Scale;
		}
	}

	public void CheckCostHigherThanMoney(int costSkill3, int costSkillUndo)
	{
		int money = GameData.Instance().GetMoney();
		if (money < costSkill3)
		{
			m_Skill3Btn.interactable = false;
		}
		else
		{
			m_Skill3Btn.interactable = true;
		}
		if (money < costSkillUndo)
		{
			m_SkillUndoBtn.interactable = false;
		}
		else
		{
			m_SkillUndoBtn.interactable = true;
		}
	}

	public void ShowGDPR()
	{
		GDPRObj.SetActive(value: true);
		Analytic.Instance.LogShowGDPR();
	}

	public void OnClickAcceptConsent()
	{
		GDPRObj.SetActive(value: false);
		Analytic.Instance.LogAcceptGDPR();
		ServicesManager.DataNormal().SetBool("GDPR", value: true);
		Singleton<ServicesManager>.instance.InitAd(isAcceptConsent: true, isGDPR: true);
	}

	public void OnClickDenyConsent()
	{
		GDPRObj.SetActive(value: false);
		ServicesManager.DataNormal().SetBool("GDPR", value: false);
		Singleton<ServicesManager>.instance.InitAd(isAcceptConsent: false, isGDPR: true);
	}

	public void ShowExchangePanel()
	{
		ExchangePanel.Show(isEffect: true);
		m_PreMenuState = m_MenuState;
		m_MenuState = MenuState.Event;
	}

	public void ShowHowToGet()
	{
		ExchangePanel.Hide(isEffect: false);
		HowToGetSnowPanel.Show(isEffect: true, null);
	}

	public void HideHowToGet()
	{
		HowToGetSnowPanel.Hide(isEffect: true, delegate
		{
			ExchangePanel.Show(isEffect: false);
		});
	}

	public void HideExchangePanel()
	{
		m_MenuState = m_PreMenuState;
		m_PreMenuState = MenuState.Event;
		ExchangePanel.Hide(isEffect: true);
	}

	public void ShowNotEnoughSnowFlake()
	{
		NotEnoughSnowFlake.gameObject.SetActive(value: true);
		NotEnoughSnowFlake.Show();
	}

	public void HideNotEnoughSnowFlake()
	{
		NotEnoughSnowFlake.Hide(delegate
		{
			NotEnoughSnowFlake.gameObject.SetActive(value: false);
		});
	}

	private IEnumerator IE_CountTimeEvent(int seconds)
	{
		while (seconds > -1)
		{
			int oneDay = 86400;
			int day = seconds / oneDay;
			int hour = seconds % oneDay / 3600;
			int min = seconds % 3600 / 60;
			int sec = seconds % 60;
			m_MainMenu.SetTimeCount(day, hour, min, sec);
			m_GameOverMenu.SetTimeCount(day, hour, min, sec);
			ExchangePanel.SetTimeCount(day, hour, min, sec);
			yield return BetterCoroutines.WaitForSecondsRealtime(1f);
			seconds--;
		}
		OpenEvent(isOpen: false);
	}

	private IEnumerator IE_ReCheckTimeEvent()
	{
		CheckTimeEvent();
		yield return BetterCoroutines.WaitForSecondsRealtime(2f);
	}

	private void CheckTimeEvent()
	{
		CheckEventOffline();
		if (m_EventCountCorou != null)
		{
			StopCoroutine(m_EventCountCorou);
		}
		ServicesManager.TimeServer().RequestTimeNow(delegate(bool result)
		{
			OpenEvent(isOpen: false);
			if (result)
			{
				DateTime timeNow = ServicesManager.TimeServer().GetTimeNow();
				double totalSeconds = (m_EndTimeEvent - timeNow).TotalSeconds;
				if (totalSeconds > 0.0)
				{
					SetEnableEvent(isEnable: true);
					if (m_EventCountCorou != null)
					{
						StopCoroutine(m_EventCountCorou);
					}
					m_EventCountCorou = StartCoroutine(IE_CountTimeEvent((int)totalSeconds));
				}
				else
				{
					OpenEvent(isOpen: false);
				}
				if (m_RecheckTimeCorou != null)
				{
					StopCoroutine(m_RecheckTimeCorou);
				}
			}
			else
			{
				CheckEventOffline();
			}
		});
	}

	private void CheckEventOffline()
	{
		DateTime utcNow = DateTime.UtcNow;
		double totalSeconds = (m_EndTimeEvent - utcNow).TotalSeconds;
		UnityEngine.Debug.Log(totalSeconds);
		if (totalSeconds > 0.0)
		{
			OpenEvent(isOpen: true);
			SetEnableEvent(isEnable: false);
			return;
		}
		if (m_RecheckTimeCorou != null)
		{
			StopCoroutine(m_RecheckTimeCorou);
		}
		OpenEvent(isOpen: false);
	}

	public void OpenEvent(bool isOpen)
	{
		if (!Singleton<UnityRemote>.instance.GetIsOpenEvent())
		{
			isOpen = false;
		}
		isOpen = false;
		m_GameOverMenu.OpenEvent(isOpen);
		m_IngameMenu.OpenEvent(isOpen);
		m_MainMenu.OpenEvent(isOpen);
		m_ShopMenu.OpenEvent(isOpen);
		Singleton<GameManager>.instance.SetOpenEvent(isOpen);
		m_SpinMenu = ((!isOpen) ? m_SpinMenuOld : m_SpinMenuEvent);
		m_SpinConfirm = ((!isOpen) ? m_SpinConfirmOld : m_SpinConfirmEvent);
		m_IsOpenEvent = isOpen;
	}

	public void SetEnableEvent(bool isEnable)
	{
		m_GameOverMenu.SetEnableEvent(isEnable);
		m_MainMenu.SetEnableEvent(isEnable);
	}

	private void OnApplicationQuit()
	{
		if (Singleton<GameManager>.instance.State == GameState.Playing)
		{
			GameData.Instance().SaveMoveCount(Singleton<GameManager>.instance.MoveCount);
			GameData.Instance().SaveTimePlayLast(GetTimePlay());
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			if (Singleton<GameManager>.instance.State == GameState.Playing)
			{
				GameData.Instance().SaveMoveCount(Singleton<GameManager>.instance.MoveCount);
				GameData.Instance().SaveTimePlayLast(GetTimePlay());
			}
		}
		else if (m_IsInit)
		{
			if (m_EventCountCorou != null)
			{
				StopCoroutine(m_EventCountCorou);
			}
			if (m_RecheckTimeCorou != null)
			{
				StopCoroutine(m_RecheckTimeCorou);
			}
			m_RecheckTimeCorou = StartCoroutine(IE_ReCheckTimeEvent());
		}
	}
}
