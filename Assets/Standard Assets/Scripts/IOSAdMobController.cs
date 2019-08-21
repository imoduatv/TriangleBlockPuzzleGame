using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IOSAdMobController : Singleton<IOSAdMobController>, GoogleMobileAdInterface
{
	private bool _IsInited;

	private Dictionary<int, IOSADBanner> _banners;

	private string _BannersUunitId = string.Empty;

	private string _InterstisialUnitId = string.Empty;

	private string _RewardedVideoAdUnitId = string.Empty;

	private const string DEVICES_SEPARATOR = ",";

	public List<GoogleMobileAdBanner> banners
	{
		get
		{
			List<GoogleMobileAdBanner> list = new List<GoogleMobileAdBanner>();
			if (_banners == null)
			{
				return list;
			}
			foreach (KeyValuePair<int, IOSADBanner> banner in _banners)
			{
				list.Add(banner.Value);
			}
			return list;
		}
	}

	public bool IsInited => _IsInited;

	public string BannersUunitId => _BannersUunitId;

	public string InterstisialUnitId => _InterstisialUnitId;

	public string RewardedVideoAdUnitId => _RewardedVideoAdUnitId;

	public event Action OnInterstitialLoaded;

	public event Action<int> OnInterstitialFailedLoading;

	public event Action OnInterstitialOpened;

	public event Action OnInterstitialClosed;

	public event Action OnInterstitialLeftApplication;

	public event Action<string> OnAdInAppRequest;

	public event Action<string, int> OnRewarded;

	public event Action OnRewardedVideoAdClosed;

	public event Action<int> OnRewardedVideoAdFailedToLoad;

	public event Action OnRewardedVideoAdLeftApplication;

	public event Action OnRewardedVideoLoaded;

	public event Action OnRewardedVideoAdOpened;

	public event Action OnRewardedVideoStarted;

	public IOSAdMobController()
	{
		this.OnInterstitialLoaded = delegate
		{
		};
		this.OnInterstitialFailedLoading = delegate
		{
		};
		this.OnInterstitialOpened = delegate
		{
		};
		this.OnInterstitialClosed = delegate
		{
		};
		this.OnInterstitialLeftApplication = delegate
		{
		};
		this.OnAdInAppRequest = delegate
		{
		};
		this.OnRewarded = delegate
		{
		};
		this.OnRewardedVideoAdClosed = delegate
		{
		};
		this.OnRewardedVideoAdFailedToLoad = delegate
		{
		};
		this.OnRewardedVideoAdLeftApplication = delegate
		{
		};
		this.OnRewardedVideoLoaded = delegate
		{
		};
		this.OnRewardedVideoAdOpened = delegate
		{
		};
		this.OnRewardedVideoStarted = delegate
		{
		};
		//base._002Ector();
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Init(string ad_unit_id)
	{
		if (_IsInited)
		{
			UnityEngine.Debug.LogWarning("Init shoudl be called only once. Call ignored");
			return;
		}
		_IsInited = true;
		_BannersUunitId = ad_unit_id;
		_InterstisialUnitId = ad_unit_id;
		_RewardedVideoAdUnitId = ad_unit_id;
		_banners = new Dictionary<int, IOSADBanner>();
	}

	public void Init(string banners_unit_id, string interstisial_unit_id)
	{
		if (_IsInited)
		{
			UnityEngine.Debug.LogWarning("Init shoudl be called only once. Call ignored");
			return;
		}
		Init(banners_unit_id);
		SetInterstisialsUnitID(interstisial_unit_id);
	}

	public void InitEditorTesting(bool isEditorTestingEnabled, int fillRate)
	{
	}

	public void SetBannersUnitID(string ad_unit_id)
	{
		_BannersUunitId = ad_unit_id;
	}

	public void SetInterstisialsUnitID(string ad_unit_id)
	{
		_InterstisialUnitId = ad_unit_id;
	}

	public void SetRewardedVideoAdUnitID(string id)
	{
		_RewardedVideoAdUnitId = id;
	}

	public GoogleMobileAdBanner CreateAdBanner(TextAnchor anchor, GADBannerSize size)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		IOSADBanner iOSADBanner = new IOSADBanner(anchor, size, GADBannerIdFactory.nextId);
		_banners.Add(iOSADBanner.id, iOSADBanner);
		return iOSADBanner;
	}

	public GoogleMobileAdBanner CreateAdBanner(int x, int y, GADBannerSize size)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		IOSADBanner iOSADBanner = new IOSADBanner(x, y, size, GADBannerIdFactory.nextId);
		_banners.Add(iOSADBanner.id, iOSADBanner);
		return iOSADBanner;
	}

	public void DestroyBanner(int id)
	{
		if (_banners != null && _banners.ContainsKey(id))
		{
			IOSADBanner iOSADBanner = _banners[id];
			if (iOSADBanner.IsLoaded)
			{
				_banners.Remove(id);
			}
			else
			{
				iOSADBanner.DestroyAfterLoad();
			}
		}
	}

	public void DirectBannerDestory(int id)
	{
	}

	public void RecordInAppResolution(GADInAppResolution resolution)
	{
	}

	public void AddKeyword(string keyword)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("AddKeyword shoudl be called only after Init function. Call ignored");
		}
	}

	public void AddTestDevice(string deviceId)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("AddTestDevice shoudl be called only after Init function. Call ignored");
		}
	}

	public void AddTestDevices(params string[] ids)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("AddTestDevice shoudl be called only after Init function. Call ignored");
		}
		else if (ids.Length != 0)
		{
		}
	}

	public void SetGender(GoogleGender gender)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("SetGender shoudl be called only after Init function. Call ignored");
		}
	}

	public void SetBirthday(int year, AndroidMonth month, int day)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("SetBirthday shoudl be called only after Init function. Call ignored");
		}
	}

	public void TagForChildDirectedTreatment(bool tagForChildDirectedTreatment)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("TagForChildDirectedTreatment shoudl be called only after Init function. Call ignored");
		}
	}

	public void StartInterstitialAd()
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("StartInterstitialAd shoudl be called only after Init function. Call ignored");
		}
	}

	public void LoadInterstitialAd()
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("LoadInterstitialAd shoudl be called only after Init function. Call ignored");
		}
	}

	public void ShowInterstitialAd()
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("ShowInterstitialAd shoudl be called only after Init function. Call ignored");
		}
	}

	public void StartRewardedVideo()
	{
	}

	public void LoadRewardedVideo()
	{
	}

	public void ShowRewardedVideo()
	{
	}

	public GoogleMobileAdBanner GetBanner(int id)
	{
		if (_banners.ContainsKey(id))
		{
			return _banners[id];
		}
		UnityEngine.Debug.LogWarning("Banner id: " + id.ToString() + " not found");
		return null;
	}

	private void OnBannerAdLoaded(string data)
	{
		string[] array = data.Split("|"[0]);
		int id = Convert.ToInt32(array[0]);
		int w = Convert.ToInt32(array[1]);
		int h = Convert.ToInt32(array[2]);
		IOSADBanner iOSADBanner = GetBanner(id) as IOSADBanner;
		if (iOSADBanner != null)
		{
			iOSADBanner.SetDimentions(w, h);
			iOSADBanner.OnBannerAdLoaded();
		}
	}

	private void OnBannerAdFailedToLoad(string bannerID)
	{
		int id = Convert.ToInt32(bannerID);
		(GetBanner(id) as IOSADBanner)?.OnBannerAdFailedToLoad();
	}

	private void OnBannerAdOpened(string bannerID)
	{
		int id = Convert.ToInt32(bannerID);
		(GetBanner(id) as IOSADBanner)?.OnBannerAdOpened();
	}

	private void OnBannerAdClosed(string bannerID)
	{
		int id = Convert.ToInt32(bannerID);
		(GetBanner(id) as IOSADBanner)?.OnBannerAdClosed();
	}

	private void OnBannerAdLeftApplication(string bannerID)
	{
		int id = Convert.ToInt32(bannerID);
		(GetBanner(id) as IOSADBanner)?.OnBannerAdLeftApplication();
	}

	private void OnInterstitialAdLoaded()
	{
		this.OnInterstitialLoaded();
	}

	private void OnInterstitialAdFailedToLoad()
	{
		this.OnInterstitialFailedLoading(0);
	}

	private void OnInterstitialAdOpened()
	{
		this.OnInterstitialOpened();
	}

	private void OnInterstitialAdClosed()
	{
		this.OnInterstitialClosed();
	}

	private void OnInterstitialAdLeftApplication()
	{
		this.OnInterstitialLeftApplication();
	}

	private void RewardBasedVideoAdDidReceiveAd()
	{
		this.OnRewardedVideoLoaded();
	}

	private void RewardBasedVideoAdDidOpen()
	{
		this.OnRewardedVideoAdOpened();
	}

	private void RewardBasedVideoAdDidStartPlaying()
	{
		this.OnRewardedVideoStarted();
	}

	private void RewardBasedVideoAdDidClose()
	{
		this.OnRewardedVideoAdClosed();
	}

	private void RewardBasedVideoAdDidFailToLoadWithError()
	{
		this.OnRewardedVideoAdFailedToLoad(0);
	}

	private void RewardBasedVideoAdWillLeaveApplication()
	{
		this.OnRewardedVideoAdLeftApplication();
	}

	private void RewardUserWithReward(string data)
	{
		string[] array = data.Split('|');
		string arg = array[0];
		int arg2 = Convert.ToInt32(array[1]);
		this.OnRewarded(arg, arg2);
	}

	private void OnInAppPurchaseRequested(string productId)
	{
		this.OnAdInAppRequest(productId);
	}
}
