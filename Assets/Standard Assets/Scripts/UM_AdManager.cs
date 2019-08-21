using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public static class UM_AdManager
{
	private static bool _IsInited;

	private static bool _AmazonAdsShowOnLoad;

	[CompilerGenerated]
	private static Action<AMN_InterstitialDataResult> _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static Action<AMN_InterstitialDismissedResult> _003C_003Ef__mg_0024cache1;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cache2;

	[CompilerGenerated]
	private static Action<int> _003C_003Ef__mg_0024cache3;

	[CompilerGenerated]
	private static Action _003C_003Ef__mg_0024cache4;

	public static bool IsInited => _IsInited;

	public static event Action OnInterstitialLoaded;

	public static event Action<int> OnInterstitialLoadFail;

	public static event Action OnInterstitialClosed;

	public static void Init()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				GoogleMobileAd.Init();
				GoogleMobileAdInterstitialSubscribe();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				AMN_Singleton<SA_AmazonAdsManager>.Instance.Create();
				AMN_Singleton<SA_AmazonAdsManager>.Instance.Init(AmazonNativeSettings.Instance.AppAPIKey, AmazonNativeSettings.Instance.IsTestMode);
				AMN_Singleton<SA_AmazonAdsManager>.Instance.OnInterstitialDataReceived += AmazonInterstitialDataReceived;
				AMN_Singleton<SA_AmazonAdsManager>.Instance.OnInterstitialDismissed += AmazonInterstitialDismissed;
			}
			else
			{
				GoogleMobileAd.Init();
				GoogleMobileAdInterstitialSubscribe();
			}
			break;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			GoogleMobileAd.Init();
			GoogleMobileAdInterstitialSubscribe();
			break;
		}
		_IsInited = true;
	}

	public static void ResetActions()
	{
		UM_AdManager.OnInterstitialLoaded = delegate
		{
		};
		UM_AdManager.OnInterstitialLoadFail = delegate
		{
		};
		UM_AdManager.OnInterstitialClosed = delegate
		{
		};
	}

	public static int CreateAdBanner(TextAnchor anchor, GADBannerSize size = GADBannerSize.BANNER)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return 0;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				return GoogleMobileAd.CreateAdBanner(anchor, size).id;
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				return AMN_Singleton<SA_AmazonAdsManager>.Instance.CreateBanner(AmazonAdBanner.BannerAligns.Bottom);
			}
			return GoogleMobileAd.CreateAdBanner(anchor, size).id;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			return GoogleMobileAd.CreateAdBanner(anchor, size).id;
		}
		return 0;
	}

	public static bool IsBannerLoaded(int id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("IsBannerLoaded shoudl be called only after Init function. Call ignored");
			return false;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				return GoogleMobileAd.GetBanner(id).IsLoaded;
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				return AMN_Singleton<SA_AmazonAdsManager>.Instance.IsBannerLoaded(id);
			}
			return GoogleMobileAd.GetBanner(id).IsLoaded;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			return GoogleMobileAd.GetBanner(id).IsLoaded;
		}
		return false;
	}

	public static bool IsBannerOnScreen(int id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("IsBannerOnScreen shoudl be called only after Init function. Call ignored");
			return false;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				return GoogleMobileAd.GetBanner(id).IsOnScreen;
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				return AMN_Singleton<SA_AmazonAdsManager>.Instance.IsBannerOnScreen(id);
			}
			return GoogleMobileAd.GetBanner(id).IsOnScreen;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			return GoogleMobileAd.GetBanner(id).IsOnScreen;
		}
		return false;
	}

	public static void DestroyBanner(int id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
			return;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				GoogleMobileAd.DestroyBanner(id);
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				AMN_Singleton<SA_AmazonAdsManager>.Instance.DestroyBanner(id);
			}
			else
			{
				GoogleMobileAd.DestroyBanner(id);
			}
			break;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			GoogleMobileAd.DestroyBanner(id);
			break;
		}
	}

	public static void HideBanner(int id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
			return;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				GoogleMobileAd.GetBanner(id).Hide();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				AMN_Singleton<SA_AmazonAdsManager>.Instance.HideBanner(hide: true, id);
			}
			else
			{
				GoogleMobileAd.GetBanner(id).Hide();
			}
			break;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			GoogleMobileAd.GetBanner(id).Hide();
			break;
		}
	}

	public static void ShowBanner(int id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
			return;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				GoogleMobileAd.GetBanner(id).Show();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				AMN_Singleton<SA_AmazonAdsManager>.Instance.HideBanner(hide: false, id);
			}
			else
			{
				GoogleMobileAd.GetBanner(id).Show();
			}
			break;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			GoogleMobileAd.GetBanner(id).Show();
			break;
		}
	}

	public static void RefreshBanner(int id)
	{
		if (!_IsInited)
		{
			UnityEngine.Debug.LogWarning("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
			return;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				GoogleMobileAd.GetBanner(id).Refresh();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				AMN_Singleton<SA_AmazonAdsManager>.Instance.RefreshBanner(id);
			}
			else
			{
				GoogleMobileAd.GetBanner(id).Refresh();
			}
			break;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			GoogleMobileAd.GetBanner(id).Refresh();
			break;
		}
	}

	public static void StartInterstitialAd()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				GoogleMobileAd.StartInterstitialAd();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				_AmazonAdsShowOnLoad = true;
				AMN_Singleton<SA_AmazonAdsManager>.Instance.LoadInterstitial();
			}
			else
			{
				GoogleMobileAd.StartInterstitialAd();
			}
			break;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			GoogleMobileAd.StartInterstitialAd();
			break;
		}
	}

	public static void LoadInterstitialAd()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				GoogleMobileAd.LoadInterstitialAd();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				AMN_Singleton<SA_AmazonAdsManager>.Instance.LoadInterstitial();
			}
			else
			{
				GoogleMobileAd.LoadInterstitialAd();
			}
			break;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			GoogleMobileAd.LoadInterstitialAd();
			break;
		}
	}

	public static void ShowInterstitialAd()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)
			{
				GoogleMobileAd.ShowInterstitialAd();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				AMN_Singleton<SA_AmazonAdsManager>.Instance.ShowInterstitial();
			}
			else
			{
				GoogleMobileAd.ShowInterstitialAd();
			}
			break;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			GoogleMobileAd.ShowInterstitialAd();
			break;
		}
	}

	private static void GoogleMobileAdInterstitialSubscribe()
	{
		GoogleMobileAd.OnInterstitialLoaded += InterstitialLoadedHandler;
		GoogleMobileAd.OnInterstitialFailedLoading += InterstitialLoadFailHandler;
		GoogleMobileAd.OnInterstitialClosed += InterstitialClosedHandler;
	}

	private static void InterstitialLoadedHandler()
	{
		UM_AdManager.OnInterstitialLoaded();
	}

	private static void InterstitialLoadFailHandler(int errorCode)
	{
		UM_AdManager.OnInterstitialLoadFail(errorCode);
	}

	private static void InterstitialClosedHandler()
	{
		UM_AdManager.OnInterstitialClosed();
	}

	private static void AmazonInterstitialDismissed(AMN_InterstitialDismissedResult result)
	{
		UM_AdManager.OnInterstitialClosed();
	}

	private static void AmazonInterstitialDataReceived(AMN_InterstitialDataResult result)
	{
		if (result.isSuccess)
		{
			UM_AdManager.OnInterstitialLoaded();
			if (_AmazonAdsShowOnLoad)
			{
				AMN_Singleton<SA_AmazonAdsManager>.Instance.ShowInterstitial();
			}
		}
		else
		{
			UM_AdManager.OnInterstitialLoadFail(0);
		}
	}

	static UM_AdManager()
	{
		UM_AdManager.OnInterstitialLoaded = delegate
		{
		};
		UM_AdManager.OnInterstitialLoadFail = delegate
		{
		};
		UM_AdManager.OnInterstitialClosed = delegate
		{
		};
		_IsInited = false;
		_AmazonAdsShowOnLoad = false;
	}
}
