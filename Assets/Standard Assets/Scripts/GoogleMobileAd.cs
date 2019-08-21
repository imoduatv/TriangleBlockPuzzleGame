using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class GoogleMobileAd
{
	public static GoogleMobileAdInterface controller;

	private static bool _IsInited = false;

	private static bool _IsInterstitialReady = false;

	private static bool _IsRewardedVideoReady = false;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static Action<int> _003C_003Ef__mg_0024cache1;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cache2;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cache3;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cache4;

	[CompilerGenerated]
	private static Action<string> _003C_003Ef__mg_0024cache5;

	[CompilerGenerated]
	private static Action<string, int> _003C_003Ef__mg_0024cache6;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cache7;

	[CompilerGenerated]
	private static Action<int> _003C_003Ef__mg_0024cache8;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cache9;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cacheA;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cacheB;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cacheC;

	public static bool IsInited => _IsInited;

	public static string BannersUunitId => controller.BannersUunitId;

	public static string InterstisialUnitId => controller.InterstisialUnitId;

	public static bool IsInterstitialReady => _IsInterstitialReady;

	public static string RewardedVideoUnitId => controller.RewardedVideoAdUnitId;

	public static bool IsRewardedVideoReady => _IsRewardedVideoReady;

	public bool IsEditorTestingEnabled => SA_EditorTesting.IsInsideEditor && GoogleMobileAdSettings.Instance.IsEditorTestingEnabled;

	public static event Action OnInterstitialLoaded;

	public static event Action<int> OnInterstitialFailedLoading;

	public static event Action OnInterstitialOpened;

	public static event Action OnInterstitialClosed;

	public static event Action OnInterstitialLeftApplication;

	public static event Action<string> OnAdInAppRequest;

	public static event Action<string, int> OnRewarded;

	public static event Action OnRewardedVideoAdClosed;

	public static event Action<int> OnRewardedVideoAdFailedToLoad;

	public static event Action OnRewardedVideoAdLeftApplication;

	public static event Action OnRewardedVideoLoaded;

	public static event Action OnRewardedVideoAdOpened;

	public static event Action OnRewardedVideoStarted;

	public static void Init()
	{
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.IPhonePlayer)
		{
			controller = Singleton<IOSAdMobController>.Instance;
			controller.Init(GoogleMobileAdSettings.Instance.IOS_BannersUnitId);
			if (!GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId.Equals(string.Empty))
			{
				controller.SetInterstisialsUnitID(GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId);
			}
			if (!GoogleMobileAdSettings.Instance.IOS_RewardedVideoAdUnitId.Equals(string.Empty))
			{
				controller.SetRewardedVideoAdUnitID(GoogleMobileAdSettings.Instance.IOS_RewardedVideoAdUnitId);
			}
		}
		else
		{
			controller = Singleton<AndroidAdMobController>.Instance;
			controller.Init(GoogleMobileAdSettings.Instance.Android_BannersUnitId);
			if (!GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId.Equals(string.Empty))
			{
				controller.SetInterstisialsUnitID(GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId);
			}
			if (!GoogleMobileAdSettings.Instance.Android_RewardedVideoAdUnitId.Equals(string.Empty))
			{
				controller.SetRewardedVideoAdUnitID(GoogleMobileAdSettings.Instance.Android_RewardedVideoAdUnitId);
			}
		}
		controller.InitEditorTesting(GoogleMobileAdSettings.Instance.IsEditorTestingEnabled, GoogleMobileAdSettings.Instance.EditorFillRate);
		controller.OnInterstitialLoaded += OnInterstitialLoadedListner;
		controller.OnInterstitialFailedLoading += OnInterstitialFailedLoadingListner;
		controller.OnInterstitialOpened += OnInterstitialOpenedListner;
		controller.OnInterstitialClosed += OnInterstitialClosedListner;
		controller.OnInterstitialLeftApplication += OnInterstitialLeftApplicationListner;
		controller.OnAdInAppRequest += OnAdInAppRequestListner;
		controller.OnRewarded += OnRewardedListner;
		controller.OnRewardedVideoAdClosed += OnRewardedVideoAdClosedListner;
		controller.OnRewardedVideoAdFailedToLoad += OnRewardedVideoAdFailedToLoadListner;
		controller.OnRewardedVideoAdLeftApplication += OnRewardedVideoAdLeftApplicationListner;
		controller.OnRewardedVideoLoaded += OnRewardedVideoLoadedListner;
		controller.OnRewardedVideoAdOpened += OnRewardedVideoAdOpenedListner;
		controller.OnRewardedVideoStarted += OnRewardedVideoStartedListner;
		_IsInited = true;
		if (GoogleMobileAdSettings.Instance.testDevices.Count > 0)
		{
			List<string> list = new List<string>();
			foreach (GADTestDevice testDevice in GoogleMobileAdSettings.Instance.testDevices)
			{
				list.Add(testDevice.ID);
			}
			AddTestDevices(list.ToArray());
		}
		TagForChildDirectedTreatment(GoogleMobileAdSettings.Instance.TagForChildDirectedTreatment);
		foreach (string defaultKeyword in GoogleMobileAdSettings.Instance.DefaultKeywords)
		{
			AddKeyword(defaultKeyword);
		}
	}

	public static void SetBannersUnitID(string android_unit_id, string ios_unit_id, string wp8_unit_id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("ChangeBannersUnitID shoudl be called only after Init function. Call ignored");
			return;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			controller.SetBannersUnitID(ios_unit_id);
			break;
		case RuntimePlatform.Android:
			controller.SetBannersUnitID(android_unit_id);
			break;
		default:
			controller.SetBannersUnitID(wp8_unit_id);
			break;
		}
	}

	public static void SetInterstisialsUnitID(string android_unit_id, string ios_unit_id, string wp8_unit_id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("ChangeInterstisialsUnitID shoudl be called only after Init function. Call ignored");
			return;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			controller.SetInterstisialsUnitID(ios_unit_id);
			break;
		case RuntimePlatform.Android:
			controller.SetInterstisialsUnitID(android_unit_id);
			break;
		default:
			controller.SetInterstisialsUnitID(wp8_unit_id);
			break;
		}
	}

	public static GoogleMobileAdBanner CreateAdBanner(TextAnchor anchor, GADBannerSize size)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		return controller.CreateAdBanner(anchor, size);
	}

	public static GoogleMobileAdBanner CreateAdBanner(int x, int y, GADBannerSize size)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		return controller.CreateAdBanner(x, y, size);
	}

	public static GoogleMobileAdBanner GetBanner(int id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("GetBanner shoudl be called only after Init function. Call ignored");
			return null;
		}
		return controller.GetBanner(id);
	}

	public static void DestroyBanner(int id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.DestroyBanner(id);
		}
	}

	public static void AddKeyword(string keyword)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("AddKeyword shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.AddKeyword(keyword);
		}
	}

	public static void SetBirthday(int year, AndroidMonth month, int day)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("SetBirthday shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.SetBirthday(year, month, day);
		}
	}

	public static void TagForChildDirectedTreatment(bool tagForChildDirectedTreatment)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("TagForChildDirectedTreatment shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.TagForChildDirectedTreatment(tagForChildDirectedTreatment);
		}
	}

	public static void AddTestDevice(string deviceId)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("AddTestDevice shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.AddTestDevice(deviceId);
		}
	}

	public static void AddTestDevices(params string[] ids)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("AddTestDevice shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.AddTestDevices(ids);
		}
	}

	public static void SetGender(GoogleGender gender)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("SetGender shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.SetGender(gender);
		}
	}

	public static void StartInterstitialAd()
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("StartInterstitialAd shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.StartInterstitialAd();
		}
	}

	public static void LoadInterstitialAd()
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("LoadInterstitialAd shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.LoadInterstitialAd();
		}
	}

	public static void ShowInterstitialAd()
	{
		if (_IsInterstitialReady)
		{
			_IsInterstitialReady = false;
			if (!_IsInited)
			{
				UnityEngine.Debug.LogWarning("ShowInterstitialAd shoudl be called only after Init function. Call ignored");
			}
			else
			{
				controller.ShowInterstitialAd();
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("ShowInterstitialAd shoudl be called only what  Interstitial Ad is Ready ");
		}
	}

	public static void RecordInAppResolution(GADInAppResolution resolution)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("RecordInAppResolution shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.RecordInAppResolution(resolution);
		}
	}

	public static void StartRewardedVideo()
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("StartRewardedVideo shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.StartRewardedVideo();
		}
	}

	public static void LoadRewardedVideo()
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("LoadRewardedVideo shoudl be called only after Init function. Call ignored");
		}
		else
		{
			controller.LoadRewardedVideo();
		}
	}

	public static void ShowRewardedVideo()
	{
		if (_IsRewardedVideoReady)
		{
			_IsRewardedVideoReady = false;
			if (!_IsInited)
			{
				UnityEngine.Debug.LogWarning("ShowRewardedVideo shoudl be called only after Init function. Call ignored");
			}
			else
			{
				controller.ShowRewardedVideo();
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("ShowRewardedVideo shoudl be called only what  Interstitial Ad is Ready ");
		}
	}

	private static void OnInterstitialLoadedListner()
	{
		_IsInterstitialReady = true;
		GoogleMobileAd.OnInterstitialLoaded();
	}

	private static void OnInterstitialFailedLoadingListner(int errorCode)
	{
		_IsInterstitialReady = false;
		GoogleMobileAd.OnInterstitialFailedLoading(errorCode);
	}

	private static void OnInterstitialOpenedListner()
	{
		_IsInterstitialReady = false;
		GoogleMobileAd.OnInterstitialOpened();
	}

	private static void OnInterstitialClosedListner()
	{
		_IsInterstitialReady = false;
		GoogleMobileAd.OnInterstitialClosed();
	}

	private static void OnInterstitialLeftApplicationListner()
	{
		GoogleMobileAd.OnInterstitialLeftApplication();
	}

	private static void OnAdInAppRequestListner(string itemId)
	{
		GoogleMobileAd.OnAdInAppRequest(itemId);
	}

	private static void OnRewardedVideoLoadedListner()
	{
		_IsRewardedVideoReady = true;
		GoogleMobileAd.OnRewardedVideoLoaded();
	}

	private static void OnRewardedVideoAdFailedToLoadListner(int errorCode)
	{
		_IsRewardedVideoReady = false;
		GoogleMobileAd.OnRewardedVideoAdFailedToLoad(errorCode);
	}

	private static void OnRewardedVideoStartedListner()
	{
		_IsRewardedVideoReady = false;
		GoogleMobileAd.OnRewardedVideoStarted();
	}

	private static void OnRewardedVideoAdOpenedListner()
	{
		_IsRewardedVideoReady = false;
		GoogleMobileAd.OnRewardedVideoAdOpened();
	}

	private static void OnRewardedVideoAdClosedListner()
	{
		_IsRewardedVideoReady = false;
		GoogleMobileAd.OnRewardedVideoAdClosed();
	}

	private static void OnRewardedVideoAdLeftApplicationListner()
	{
		_IsRewardedVideoReady = false;
		GoogleMobileAd.OnRewardedVideoAdLeftApplication();
	}

	private static void OnRewardedListner(string itemId, int count)
	{
		_IsRewardedVideoReady = false;
		GoogleMobileAd.OnRewarded(itemId, count);
	}

	static GoogleMobileAd()
	{
		GoogleMobileAd.OnInterstitialLoaded = delegate
		{
		};
		GoogleMobileAd.OnInterstitialFailedLoading = delegate
		{
		};
		GoogleMobileAd.OnInterstitialOpened = delegate
		{
		};
		GoogleMobileAd.OnInterstitialClosed = delegate
		{
		};
		GoogleMobileAd.OnInterstitialLeftApplication = delegate
		{
		};
		GoogleMobileAd.OnAdInAppRequest = delegate
		{
		};
		GoogleMobileAd.OnRewarded = delegate
		{
		};
		GoogleMobileAd.OnRewardedVideoAdClosed = delegate
		{
		};
		GoogleMobileAd.OnRewardedVideoAdFailedToLoad = delegate
		{
		};
		GoogleMobileAd.OnRewardedVideoAdLeftApplication = delegate
		{
		};
		GoogleMobileAd.OnRewardedVideoLoaded = delegate
		{
		};
		GoogleMobileAd.OnRewardedVideoAdOpened = delegate
		{
		};
		GoogleMobileAd.OnRewardedVideoStarted = delegate
		{
		};
	}
}
