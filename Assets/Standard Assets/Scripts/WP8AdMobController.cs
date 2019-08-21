using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WP8AdMobController : Singleton<WP8AdMobController>, GoogleMobileAdInterface
{
	private bool _IsInited;

	private Dictionary<int, WP8ADBanner> _banners;

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
			foreach (KeyValuePair<int, WP8ADBanner> banner in _banners)
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

	public WP8AdMobController()
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
		_banners = new Dictionary<int, WP8ADBanner>();
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

	public void InitEditorTesting(bool isEditorTesting, int fillRate)
	{
	}

	public void SetOrientation(ScreenOrientation orientation)
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
		WP8ADBanner wP8ADBanner = new WP8ADBanner(anchor, size, GADBannerIdFactory.nextId);
		_banners.Add(wP8ADBanner.id, wP8ADBanner);
		return wP8ADBanner;
	}

	public GoogleMobileAdBanner CreateAdBanner(int x, int y, GADBannerSize size)
	{
		if (!IsInited)
		{
			UnityEngine.Debug.LogWarning("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		WP8ADBanner wP8ADBanner = new WP8ADBanner(TextAnchor.MiddleCenter, size, GADBannerIdFactory.nextId);
		_banners.Add(wP8ADBanner.id, wP8ADBanner);
		return wP8ADBanner;
	}

	public void DestroyBanner(int id)
	{
		if (_banners != null && _banners.ContainsKey(id))
		{
			WP8ADBanner wP8ADBanner = _banners[id];
			if (wP8ADBanner.IsLoaded)
			{
				_banners.Remove(id);
			}
			else
			{
				wP8ADBanner.DestroyAfterLoad();
			}
		}
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
		WP8ADBanner wP8ADBanner = GetBanner(id) as WP8ADBanner;
		if (wP8ADBanner != null)
		{
			wP8ADBanner.SetDimentions(w, h);
			wP8ADBanner.OnBannerAdLoaded();
		}
	}

	private void OnBannerAdFailedToLoad(string bannerID)
	{
		int id = Convert.ToInt32(bannerID);
		(GetBanner(id) as WP8ADBanner)?.OnBannerAdFailedToLoad();
	}

	private void OnBannerAdOpened(string bannerID)
	{
		int id = Convert.ToInt32(bannerID);
		(GetBanner(id) as WP8ADBanner)?.OnBannerAdOpened();
	}

	private void OnBannerAdClosed(string bannerID)
	{
		int id = Convert.ToInt32(bannerID);
		(GetBanner(id) as WP8ADBanner)?.OnBannerAdClosed();
	}

	private void OnBannerAdLeftApplication(string bannerID)
	{
		int id = Convert.ToInt32(bannerID);
		(GetBanner(id) as WP8ADBanner)?.OnBannerAdLeftApplication();
	}

	private void OnInterstitialAdLoaded(string data)
	{
		this.OnInterstitialLoaded();
	}

	private void OnInterstitialAdFailedToLoad(string data)
	{
		this.OnInterstitialFailedLoading(0);
	}

	private void OnInterstitialAdOpened(string data)
	{
		this.OnInterstitialOpened();
	}

	private void OnInterstitialAdClosed(string data)
	{
		this.OnInterstitialClosed();
	}

	private void OnInterstitialAdLeftApplication(string data)
	{
		this.OnInterstitialLeftApplication();
	}

	private void OnInAppPurchaseRequested(string productId)
	{
		this.OnAdInAppRequest(productId);
	}
}
