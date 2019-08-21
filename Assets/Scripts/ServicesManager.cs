using Proyecto26;
using Root;
using System;
using System.Collections;
using UnityEngine;

public class ServicesManager : Singleton<ServicesManager>
{
	public TestRewardLoad RewardLoaded;

	private IDataService m_DataNormal;

	private IDataService m_DataSecure;

	private IJsonService m_Json;

	private ITimeService m_Time;

	private ILocalNotiService m_LocalNoti;

	private IUserLocation m_UserLocation;

	private ITimeServer m_TimeServer;

	[Header("Ad config")]
	[SerializeField]
	private AdsConfig m_AppotaxAdsConfig;

	[SerializeField]
	private AdsConfig m_AppodealAdsConfig;

	[SerializeField]
	private AdsConfig m_AppmaticAdsConfig;

	[Header("Appotax price Ad config")]
	[SerializeField]
	private AdsConfig m_Appotax3;

	[SerializeField]
	private AdsConfig m_Appotax5;

	[SerializeField]
	private AdsConfig m_Appotax7;

	[SerializeField]
	private AdsConfig m_Appotax10;

	[Header("Appmatic price Ad config")]
	[SerializeField]
	private AdsConfig m_Appmatic3;

	[SerializeField]
	private AdsConfig m_Appmatic5;

	[SerializeField]
	private AdsConfig m_Appmatic7;

	[SerializeField]
	private AdsConfig m_Appmatic10;

	[SerializeField]
	private AdsConfig m_Appmatic15;

	[SerializeField]
	private AdsConfig m_Appmatic20;

	[Header("Tenjin Config")]
	public TenjinConfig TenjinConfig;

	private const string m_AndroidKochavaID = "kotrigon-11h";

	private const string m_IOSKochavaID = "kotrigon-ios-iflm";

	private float lastFullAdsTime = -1f;

	private AppodealModule m_AppodealAds;

	private AppotaxModule m_AppotaxAds;

	private AppotaxModule m_ResumeAds;

	private AppotaxModule m_Appmatic;

	private AppotaxModule m_AppotaxAds3;

	private AppotaxModule m_AppotaxAds5;

	private AppotaxModule m_AppotaxAds7;

	private AppotaxModule m_AppotaxAds10;

	private AppotaxModule m_AppmaticAds3;

	private AppotaxModule m_AppmaticAds5;

	private AppotaxModule m_AppmaticAds7;

	private AppotaxModule m_AppmaticAds10;

	private AppotaxModule m_AppmaticAds15;

	private AppotaxModule m_AppmaticAds20;

	private TenjinModule m_TenjinModule;

	private const string ECPM20 = "eCPM20";

	private const string ECPM15 = "eCPM15";

	private const string ECPM10 = "eCPM10";

	private const string ECPM7 = "eCPM7";

	private const string ECPM5 = "eCPM5";

	private const string ECPM3 = "eCPM3";

	public const string GDPR_KEY = "GDPR";

	private IInAppPurchaseService m_IAP;

	private ILeaderBoardService m_LeaderBoard;

	private IScoreService m_Score;

	private IAdsService m_BannerAdsService;

	private IAnalyticABTestService m_AnalyticABTestService;

	private bool isInit;

	private bool isInitAllServices;

	[Header("LeaderBoard Config")]
	public LeaderBoardData leaderBoardData;

	[Header("AB Testing Config")]
	public ServicesConfig abConfigAnd;

	public ServicesConfig abConfigsIOS;

	private bool isFirstTimeShowAds = true;

	private const string m_AdCountKey = "Ad counter";

	private const string APPOTAX = "Appotax";

	private const string APPODEAL = "Appodeal";

	private const string RICHADX = "Richadx";

	private DateTime m_LastTimePause = DateTime.Now;

	public static IDataService DataNormal()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_DataNormal;
	}

	public static IDataService DataSecure()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_DataSecure;
	}

	public static IJsonService Json()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_Json;
	}

	public static ITimeService Time()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_Time;
	}

	public static ILocalNotiService LocalNoti()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_LocalNoti;
	}

	public static IUserLocation UserLocation()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_UserLocation;
	}

	public static ITimeServer TimeServer()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_TimeServer;
	}

	public static IInAppPurchaseService IAP()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_IAP;
	}

	public static ILeaderBoardService LeaderBoard()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_LeaderBoard;
	}

	public static IScoreService Score()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_Score;
	}

	public static IAnalyticABTestService AnalyticABTest()
	{
		Singleton<ServicesManager>.Instance.Init();
		return Singleton<ServicesManager>.Instance.m_AnalyticABTestService;
	}

	public void InitServices(bool isShowAds, bool isHaveIAP)
	{
		Init();
		if (isInitAllServices)
		{
			return;
		}
		isInitAllServices = true;
		if (isShowAds)
		{
			m_AppodealAds = new AppodealModule();
			m_AppotaxAds = new AppotaxModule();
			m_AppotaxAds.Init(m_AppotaxAdsConfig);
			m_Appmatic = new AppotaxModule();
			m_Appmatic.Init(m_AppmaticAdsConfig);
			m_AppmaticAds3 = new AppotaxModule();
			m_AppmaticAds3.Init(m_Appmatic3);
			m_AppmaticAds5 = new AppotaxModule();
			m_AppmaticAds5.Init(m_Appmatic5);
			m_AppmaticAds7 = new AppotaxModule();
			m_AppmaticAds7.Init(m_Appmatic7);
			m_AppmaticAds10 = new AppotaxModule();
			m_AppmaticAds10.Init(m_Appmatic10);
			m_AppmaticAds15 = new AppotaxModule();
			m_AppmaticAds15.Init(m_Appmatic15);
			m_AppmaticAds20 = new AppotaxModule();
			m_AppmaticAds20.Init(m_Appmatic20);
			m_AppotaxAds3 = new AppotaxModule();
			m_AppotaxAds3.Init(m_Appotax3);
			m_AppotaxAds5 = new AppotaxModule();
			m_AppotaxAds5.Init(m_Appotax5);
			m_AppotaxAds7 = new AppotaxModule();
			m_AppotaxAds7.Init(m_Appotax7);
			m_AppotaxAds10 = new AppotaxModule();
			m_AppotaxAds10.Init(m_Appotax10);
			m_TenjinModule = new TenjinModule();
			m_TenjinModule.Init(TenjinConfig, m_DataNormal);
			m_TenjinModule.Connect();
			if (DataNormal().HasKey("GDPR"))
			{
				bool @bool = DataNormal().GetBool("GDPR", defaultValue: true);
				InitAd(@bool, isGDPR: true);
			}
			else
			{
				UserLocation().RequestInfo(delegate(bool result)
				{
					if (result)
					{
						if (UserLocation().IsRegionGDPR())
						{
							Singleton<UIManager>.instance.ShowGDPR();
						}
						else
						{
							DataNormal().SetBool("GDPR", value: true);
							InitAd(isAcceptConsent: true, isGDPR: false);
						}
					}
					else
					{
						InitAd(isAcceptConsent: false, isGDPR: false);
					}
				});
			}
		}
		if (isHaveIAP)
		{
			m_IAP = new UMInAppPurchaseModule(m_DataSecure);
			m_IAP.ConnectInit();
		}
		bool flag = false;
		if (Application.isEditor)
		{
			flag = leaderBoardData.IsEditorEnable;
		}
		else if (Application.isMobilePlatform)
		{
			flag = leaderBoardData.IsMobileEnable;
		}
		if (flag)
		{
			PlayersScrollController viewer = UnityEngine.Object.FindObjectOfType<PlayersScrollController>();
			FaceBookLeaderBoardModule faceBookLeaderBoardModule = new FaceBookLeaderBoardModule(viewer, leaderBoardData.DtaAppId, leaderBoardData.SecretKey, leaderBoardData.LeaderBoardId, m_Json);
			m_Score = new ScoreModule(m_DataSecure, faceBookLeaderBoardModule);
			faceBookLeaderBoardModule.SetScoreService(m_Score);
			if (Application.platform == RuntimePlatform.Android)
			{
				faceBookLeaderBoardModule.SetUMLeaderBoard(new LeaderBoardMockModule());
			}
			else
			{
				faceBookLeaderBoardModule.SetUMLeaderBoard(new UMLeaderBoardModule());
			}
			m_LeaderBoard = faceBookLeaderBoardModule;
			m_LeaderBoard.ConnectService();
		}
		else
		{
			m_LeaderBoard = new LeaderBoardMockModule();
			m_Score = new ScoreModule(m_DataSecure, m_LeaderBoard);
		}
		m_TimeServer = new TimeServerModule(this, Json());
		InitKochava();
		PushAdId();
	}

	private void InitKochava()
	{
		Kochava.Tracker.Config.SetAppGuid("kotrigon-11h");
		Kochava.Tracker.Config.SetLogLevel(Kochava.DebugLogLevel.info);
		Kochava.Tracker.Config.SetRetrieveAttribution(value: true);
		Kochava.Tracker.SetAttributionHandler(AttributionHandle);
		Kochava.Tracker.Initialize();
	}

	private void AttributionHandle(string attributionData)
	{
		StartCoroutine(IE_PushAttributionData(attributionData));
	}

	private IEnumerator IE_PushAttributionData(string attributionData)
	{
		yield return new WaitForSeconds(2f);
		Analytic.Instance.LogKochavaAttribution(attributionData);
	}

	public void InitAd(bool isAcceptConsent, bool isGDPR)
	{
		if (isGDPR)
		{
			StartCoroutine(IE_WaitRemote(m_AppodealAdsConfig, isAcceptConsent));
		}
		else
		{
			StartCoroutine(IE_WaitTenjin());
		}
		m_AppotaxAds.SetConsent(isAcceptConsent);
		m_Appmatic.SetConsent(isAcceptConsent);
		m_AppmaticAds3.SetConsent(isAcceptConsent);
		m_AppmaticAds5.SetConsent(isAcceptConsent);
		m_AppmaticAds7.SetConsent(isAcceptConsent);
		m_AppmaticAds10.SetConsent(isAcceptConsent);
		m_AppmaticAds15.SetConsent(isAcceptConsent);
		m_AppmaticAds20.SetConsent(isAcceptConsent);
		m_AppotaxAds3.SetConsent(isAcceptConsent);
		m_AppotaxAds5.SetConsent(isAcceptConsent);
		m_AppotaxAds7.SetConsent(isAcceptConsent);
		m_AppotaxAds10.SetConsent(isAcceptConsent);
		m_AppotaxAds.LoadFullAds();
		m_Appmatic.LoadFullAds();
		m_AppodealAds.LoadBannerBot();
		m_AppodealAds.LoadBannerTop();
		m_AppodealAds.LoadFullAds();
	}

	private void InitBindingInjection()
	{
		m_Json = new JsonDotNetModule();
		m_Time = new TimeModule();
		m_DataNormal = new PlayerPrefsModule(m_Json);
		m_DataSecure = new SecurePlayerPrefsModule(m_Json);
		m_AnalyticABTestService = new AnalyticABTestModule(m_DataNormal);
		m_UserLocation = new UserLocationIP(this, Json());
		ServicesConfig config = abConfigAnd;
		DoActionIfNotNull(config, delegate
		{
			m_AnalyticABTestService.SetUpTest(config.abTestConfigs, config.bundleVersion, () => !m_DataNormal.HasKey("Tut.Step.Key"));
		});
	}

	private void Init()
	{
		if (!isInit)
		{
			isInit = true;
			InitBindingInjection();
		}
	}

	private void DoActionIfNotNull(object obj, Action action)
	{
		if (obj != null)
		{
			action();
		}
	}

	private void Start()
	{
		Init();
	}

	private void OnApplicationPause(bool isPaused)
	{
		if (isPaused)
		{
			SaveData();
		}
	}

	private void OnApplicationQuit()
	{
		SaveData();
	}

	private void SaveData()
	{
		Init();
		m_DataNormal.Save();
		m_DataSecure.Save();
	}

	public void ClearData()
	{
		Init();
		PlayerPrefs.DeleteAll();
	}

	public void ShowFullAds(Action afterShowCallback = null)
	{
		if (!GameData.Instance().GetIsAds())
		{
			afterShowCallback?.Invoke();
			return;
		}
		float timeFullAd = Singleton<FirebaseManager>.instance.GetTimeFullAd();
		if (lastFullAdsTime < 0f || UnityEngine.Time.realtimeSinceStartup - lastFullAdsTime >= timeFullAd)
		{
			ShowAd(afterShowCallback);
		}
		else
		{
			afterShowCallback?.Invoke();
		}
	}

	public bool IsFullAdsLoaded()
	{
		return m_AppotaxAds.IsFullAdsLoaded();
	}

	public void ShowAd(Action afterShowCallback = null)
	{
		int adType = Singleton<UnityRemote>.instance.GetAdType();
		switch (adType)
		{
		case 1:
			ShowAppodeal_1_Appotax_2(afterShowCallback);
			break;
		case 2:
			ShowAppodeal_1_Appmatic_2(afterShowCallback);
			break;
		case 3:
			ShowAppotax_1_Appodeal_2(afterShowCallback);
			break;
		case 4:
			ShowAppmatic_1_Appodeal_2(afterShowCallback);
			break;
		case 5:
		case 6:
			if (!ShowAppotax_1_Appodeal_2_WithEcpm("eCPM10", afterShowCallback) && !ShowAppotax_1_Appodeal_2_WithEcpm("eCPM7", afterShowCallback) && !ShowAppotax_1_Appodeal_2_WithEcpm("eCPM5", afterShowCallback) && !ShowAppotax_1_Appodeal_2_WithEcpm("eCPM3", afterShowCallback))
			{
				if (adType == 5)
				{
					ShowAppotax_1_Appodeal_2(afterShowCallback);
				}
				else
				{
					ShowAppodeal_1_Appotax_2(afterShowCallback);
				}
			}
			LoadAdAppotaxEcpm();
			break;
		case 7:
		case 8:
			UnityEngine.Debug.Log("Show appmatic ecpm");
			if (!ShowAppmatic_1_Appodeal_2_WithEcpm("eCPM20", afterShowCallback) && !ShowAppmatic_1_Appodeal_2_WithEcpm("eCPM15", afterShowCallback) && !ShowAppmatic_1_Appodeal_2_WithEcpm("eCPM10", afterShowCallback) && !ShowAppmatic_1_Appodeal_2_WithEcpm("eCPM7", afterShowCallback) && !ShowAppmatic_1_Appodeal_2_WithEcpm("eCPM5", afterShowCallback) && !ShowAppmatic_1_Appodeal_2_WithEcpm("eCPM3", afterShowCallback))
			{
				if (adType == 7)
				{
					ShowAppmatic_1_Appodeal_2(afterShowCallback);
				}
				else
				{
					ShowAppodeal_1_Appmatic_2(afterShowCallback);
				}
			}
			LoadAdAppmaticEcpm();
			break;
		default:
			ShowAppodeal_1_Appotax_2(afterShowCallback);
			break;
		}
		m_AppodealAds.LoadFullAds();
	}

	public void ShowAppotax_1_Appodeal_2(Action afterShowCallback)
	{
		if (m_AppotaxAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appotax");
			m_AppotaxAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
			return;
		}
		m_AppotaxAds.LoadFullAds();
		if (m_AppodealAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appodeal");
			m_AppodealAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
		}
		else
		{
			afterShowCallback?.Invoke();
		}
	}

	public void ShowAppodeal_1_Appotax_2(Action afterShowCallback)
	{
		if (m_AppodealAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appodeal");
			m_AppodealAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
		}
		else if (m_AppotaxAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appotax");
			m_AppotaxAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
		}
		else
		{
			m_AppotaxAds.LoadFullAds();
			afterShowCallback?.Invoke();
		}
	}

	public void ShowAppodeal_1_Appmatic_2(Action afterShowCallback)
	{
		if (m_AppodealAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appodeal");
			m_AppodealAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
		}
		else if (m_Appmatic.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("APPMATIC");
			m_Appmatic.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
		}
		else
		{
			m_Appmatic.LoadFullAds();
			afterShowCallback?.Invoke();
		}
	}

	public void ShowAppmatic_1_Appodeal_2(Action afterShowCallback)
	{
		if (m_Appmatic.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("APPMATIC");
			m_Appmatic.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
			return;
		}
		m_Appmatic.LoadFullAds();
		if (m_AppodealAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appodeal");
			m_AppodealAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
		}
		else
		{
			afterShowCallback?.Invoke();
		}
	}

	public bool ShowAppmatic_1_Appodeal_2_WithEcpm(string ecpm, Action afterShowCallback)
	{
		AppotaxModule appotaxModule = (ecpm == "eCPM20") ? m_AppmaticAds20 : ((ecpm == "eCPM15") ? m_AppmaticAds15 : ((ecpm == "eCPM10") ? m_AppmaticAds10 : ((ecpm == "eCPM7") ? m_AppmaticAds7 : ((ecpm == "eCPM5") ? m_AppmaticAds5 : ((!(ecpm == "eCPM3")) ? m_Appmatic : m_AppmaticAds3)))));
		if (appotaxModule.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("APPMATIC_" + ecpm);
			appotaxModule.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
			return true;
		}
		if (m_AppodealAds.ShowFullAdsWithEcpm(ecpm, afterShowCallback))
		{
			Analytic.Instance.LogShowInterEvent("APPODEAL_" + ecpm);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
			return true;
		}
		return false;
	}

	public bool ShowAppotax_1_Appodeal_2_WithEcpm(string ecpm, Action afterShowCallback)
	{
		AppotaxModule appotaxModule = (ecpm == "eCPM10") ? m_AppotaxAds10 : ((ecpm == "eCPM7") ? m_AppotaxAds7 : ((ecpm == "eCPM5") ? m_AppotaxAds5 : ((!(ecpm == "eCPM3")) ? m_AppotaxAds : m_AppotaxAds3)));
		if (appotaxModule.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("APPOTAX_" + ecpm);
			appotaxModule.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
			return true;
		}
		if (m_AppodealAds.ShowFullAdsWithEcpm(ecpm, afterShowCallback))
		{
			Analytic.Instance.LogShowInterEvent("APPODEAL_" + ecpm);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
			lastFullAdsTime = UnityEngine.Time.realtimeSinceStartup;
			return true;
		}
		return false;
	}

	public void LoadAdAppotaxEcpm()
	{
		UnityEngine.Debug.Log("Load appotax ecpm");
		StartCoroutine(IE_InitAppotaxEcpm());
	}

	public void LoadAdAppmaticEcpm()
	{
		UnityEngine.Debug.Log("Load appmatic ecpm");
		StartCoroutine(IE_InitAppmaticEcpm());
	}

	private IEnumerator IE_InitAppotaxEcpm()
	{
		if (!m_AppotaxAds5.IsFullAdsLoaded())
		{
			m_AppotaxAds5.LoadFullAds();
		}
		if (!m_AppotaxAds7.IsFullAdsLoaded())
		{
			m_AppotaxAds7.LoadFullAds();
		}
		yield return new WaitForSecondsRealtime(10f);
		if (!m_AppotaxAds3.IsFullAdsLoaded())
		{
			m_AppotaxAds3.LoadFullAds();
		}
		if (!m_AppotaxAds10.IsFullAdsLoaded())
		{
			m_AppotaxAds10.LoadFullAds();
		}
	}

	private IEnumerator IE_InitAppmaticEcpm()
	{
		if (!m_AppmaticAds10.IsFullAdsLoaded())
		{
			m_AppmaticAds10.LoadFullAds();
		}
		if (!m_AppmaticAds15.IsFullAdsLoaded())
		{
			m_AppmaticAds15.LoadFullAds();
		}
		yield return new WaitForSecondsRealtime(10f);
		if (!m_AppmaticAds7.IsFullAdsLoaded())
		{
			m_AppmaticAds7.LoadFullAds();
		}
		if (!m_AppmaticAds20.IsFullAdsLoaded())
		{
			m_AppmaticAds20.LoadFullAds();
		}
		yield return new WaitForSecondsRealtime(10f);
		if (!m_AppmaticAds3.IsFullAdsLoaded())
		{
			m_AppmaticAds3.LoadFullAds();
		}
		if (!m_AppmaticAds5.IsFullAdsLoaded())
		{
			m_AppmaticAds5.LoadFullAds();
		}
	}

	public void ShowAdsOnlyAppodeal(Action afterShowCallback)
	{
		if (m_AppodealAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appodeal");
			m_AppodealAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
		}
		else
		{
			afterShowCallback?.Invoke();
		}
	}

	public void ShowAds2(Action afterShowCallback)
	{
		if (m_AppotaxAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appotax");
			m_AppotaxAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
		}
		else if (m_AppodealAds.IsFullAdsLoaded())
		{
			Analytic.Instance.LogShowInterEvent("Appodeal");
			m_AppodealAds.ShowFullAds(afterShowCallback);
			Singleton<UIManager>.instance.ResetTimeCheckShowAds();
		}
		else
		{
			afterShowCallback?.Invoke();
		}
	}

	public void SaveLastTimePause()
	{
		m_LastTimePause = DateTime.Now;
	}

	public void ShowResumeAds(Action afterShowCallback = null)
	{
		afterShowCallback?.Invoke();
	}

	public void ShowResumeAdsOld(Action afterShowCallback = null)
	{
		if (!GameData.Instance().GetIsAds())
		{
			afterShowCallback?.Invoke();
			return;
		}
		if (Singleton<GameManager>.instance.ShowAdResume == ShowAdResume.None)
		{
			afterShowCallback?.Invoke();
			return;
		}
		DateTime now = DateTime.Now;
		double totalSeconds = (now - m_LastTimePause).TotalSeconds;
		if (totalSeconds < 60.0)
		{
			afterShowCallback?.Invoke();
		}
		else if (m_ResumeAds.IsFullAdsLoaded())
		{
			m_ResumeAds.ShowFullAds(afterShowCallback);
			Analytic.Instance.LogShowAdsResume();
		}
		else
		{
			afterShowCallback?.Invoke();
		}
	}

	public void LoadFullAds()
	{
		StartCoroutine(IE_DelayLoadAds());
	}

	private IEnumerator IE_DelayLoadAds()
	{
		yield return new WaitForSeconds(10f);
	}

	private IEnumerator IE_WaitTenjin()
	{
		int timeOut = TenjinConfig.Timeout;
		while (timeOut > 0 && !m_TenjinModule.IsHaveGender())
		{
			yield return new WaitForSecondsRealtime(1f);
			timeOut--;
		}
		if (m_TenjinModule.IsHaveGender())
		{
			StartCoroutine(IE_WaitRemote(m_AppodealAdsConfig, isAcceptConsent: true, m_TenjinModule.GetGender()));
		}
		else
		{
			StartCoroutine(IE_WaitRemote(m_AppodealAdsConfig, isAcceptConsent: true));
		}
	}

	public void ShowBanner()
	{
		if (!GameData.Instance().GetIsAds())
		{
			return;
		}
		GameManager.ABBanner aB_Banner = Singleton<GameManager>.instance.AB_Banner;
		ThemeName currentTheme = GameData.Instance().GetCurrentTheme();
		bool flag = Singleton<ThemeManager>.instance.IsThemeBannerTop(currentTheme);
		if (m_AppodealAds != null)
		{
			if (aB_Banner == GameManager.ABBanner.Top && flag)
			{
				m_AppodealAds.ShowBannerTop();
			}
			else
			{
				m_AppodealAds.ShowBanner();
			}
		}
	}

	public void HideBanner()
	{
		if (m_AppodealAds != null)
		{
			m_AppodealAds.HideBanner();
			m_AppodealAds.HideBannerTop();
		}
	}

	public void ShowBannerPause()
	{
		if (GameData.Instance().GetIsAds() && m_AppodealAds != null)
		{
			m_AppodealAds.ShowBannerTop();
		}
	}

	public void HideBannerPause()
	{
		if (m_AppodealAds != null)
		{
			m_AppodealAds.HideBannerTop();
		}
	}

	public void RefreshBannerPause()
	{
		if (GameData.Instance().GetIsAds())
		{
			HideBannerPause();
		}
	}

	public void ShowRewardAds(Action afterShowRewardCallback = null, Action skipRewardCallback = null)
	{
		if (Application.isEditor)
		{
			afterShowRewardCallback?.Invoke();
		}
		else if (m_AppodealAds.IsRewardAdsLoaded())
		{
			m_AppodealAds.ShowRewardAds(afterShowRewardCallback, skipRewardCallback);
		}
	}

	public bool IsRewardAvailable()
	{
		if (Application.isEditor)
		{
			return RewardLoaded == TestRewardLoad.Loaded;
		}
		bool flag = Application.internetReachability != NetworkReachability.NotReachable;
		bool flag2 = m_AppodealAds.IsRewardAdsLoaded();
		return flag && flag2;
	}

	private IEnumerator IE_WaitRemote(AdsConfig appodealConifg, bool isAcceptConsent, int gender = 3)
	{
		yield return new WaitForSecondsRealtime(2f);
		bool isInitInmobi = Singleton<UnityRemote>.instance.GetIsInitInmobi();
		m_AppodealAds.InitAppodeal(appodealConifg, isAcceptConsent, gender, isInitInmobi);
	}

	public void PushAdId()
	{
		string pushApplovin = "applovin_key";
		string str = "2TyqUL7l6p-W_d_DwWUykpqe6vTZ6BoamR8ofBmM9hWT_mgTIm8ZyN_3SeeeELFpGVWhgHQQTDdq3OFvKLrZJ9";
		string str2 = "com.DTA.trigon";
		string url = "https://api.applovin.com/suppressionApps?api_key=" + str + "&package_name=" + str2;
		if (Application.isEditor)
		{
			string text = "abc";
			if (!string.IsNullOrEmpty(text))
			{
				m_DataNormal.SetBool(pushApplovin, value: true);
				RequestHelper requestHelper = new RequestHelper();
				requestHelper.Uri = url;
				requestHelper.Method = "POST";
				requestHelper.BodyString = text;
				RequestHelper options = requestHelper;
				RestClient.Request(options).Then(delegate(ResponseHelper res)
				{
					UnityEngine.Debug.Log("status code:" + res.StatusCode);
					UnityEngine.Debug.Log("text:" + res.Text);
				});
			}
		}
		Application.RequestAdvertisingIdentifierAsync(delegate(string advertisingId, bool trackingEnabled, string error)
		{
			UnityEngine.Debug.Log("ad id:" + advertisingId);
			if (!string.IsNullOrEmpty(advertisingId))
			{
				m_DataNormal.SetBool(pushApplovin, value: true);
				RequestHelper options2 = new RequestHelper
				{
					Uri = url,
					Method = "POST",
					BodyString = advertisingId
				};
				RestClient.Request(options2).Then(delegate(ResponseHelper res)
				{
					UnityEngine.Debug.Log("status code:" + res.StatusCode);
					UnityEngine.Debug.Log("text:" + res.Text);
				});
			}
		});
	}
}
