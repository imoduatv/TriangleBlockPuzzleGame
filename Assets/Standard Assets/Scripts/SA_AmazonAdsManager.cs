using SA.Common.Util;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SA_AmazonAdsManager : AMN_Singleton<SA_AmazonAdsManager>
{
	public const string DATA_SPLITTER = "|";

	private bool _isInitialized;

	private Dictionary<int, AmazonAdBanner> _banners;

	public bool IsInitialized
	{
		get
		{
			return _isInitialized;
		}
		set
		{
			_isInitialized = value;
		}
	}

	public Dictionary<int, AmazonAdBanner> Banners => _banners;

	public event Action<AMN_InterstitialDataResult> OnInterstitialDataReceived;

	public event Action<AMN_InterstitialDismissedResult> OnInterstitialDismissed;

	public SA_AmazonAdsManager()
	{
		this.OnInterstitialDataReceived = delegate
		{
		};
		this.OnInterstitialDismissed = delegate
		{
		};
		_banners = new Dictionary<int, AmazonAdBanner>();
		//base._002Ector();
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Create()
	{
		AMN_AdvertisingProxy.GetInstance();
		UnityEngine.Debug.Log("UNITY SA_AmazonAdsManager was created");
	}

	public void Init(string api_key, bool isTestMode)
	{
		AMN_AdvertisingProxy.Init(api_key, isTestMode);
		IsInitialized = true;
	}

	public int CreateBanner(AmazonAdBanner.BannerAligns position)
	{
		AmazonAdBanner amazonAdBanner = new AmazonAdBanner(position, IdFactory.NextId);
		_banners.Add(amazonAdBanner.Id, amazonAdBanner);
		return amazonAdBanner.Id;
	}

	public bool IsBannerLoaded(int id)
	{
		if (_banners.ContainsKey(id))
		{
			return _banners[id].IsLoaded;
		}
		UnityEngine.Debug.Log("There is NO Ad Banner with such an ID " + id);
		return false;
	}

	public bool IsBannerOnScreen(int id)
	{
		if (_banners.ContainsKey(id))
		{
			return _banners[id].IsOnScreen;
		}
		UnityEngine.Debug.Log("There is NO Ad Banner with such an ID " + id);
		return false;
	}

	public void RefreshBanner(int id)
	{
		if (_banners.ContainsKey(id))
		{
			_banners[id].Refresh();
		}
	}

	public void DestroyBanner(int id)
	{
		if (_banners.ContainsKey(id))
		{
			_banners[id].Destroy();
		}
	}

	public void HideBanner(bool hide, int id)
	{
		if (_banners.ContainsKey(id))
		{
			_banners[id].Hide(hide);
		}
	}

	public void LoadInterstitial()
	{
		AMN_AdvertisingProxy.LoadInterstitial();
	}

	public void ShowInterstitial()
	{
		AMN_AdvertisingProxy.ShowInterstitial();
	}

	private void OnAdLoaded(string data)
	{
		UnityEngine.Debug.Log("OnAdLoaded data: " + data);
		string[] array = data.Split(new string[1]
		{
			"|"
		}, StringSplitOptions.None);
		int key = int.Parse(array[0]);
		int width = int.Parse(array[1]);
		int height = int.Parse(array[2]);
		bool canExpand = bool.Parse(array[3]);
		bool canPlayAudio = bool.Parse(array[4]);
		bool canPlayVideo = bool.Parse(array[5]);
		string adtype = array[6];
		AMN_AdProperties props = new AMN_AdProperties(canExpand, canPlayAudio, canPlayVideo, adtype);
		if (_banners.ContainsKey(key))
		{
			_banners[key].SetProperties(width, height, props);
			_banners[key].HandleOnBannerAdLoaded();
		}
	}

	private void OnAdFailedToLoad(string data)
	{
		UnityEngine.Debug.Log("OnBannerFailed with error " + data);
		string[] array = data.Split(new string[1]
		{
			"|"
		}, StringSplitOptions.None);
		int key = int.Parse(array[0]);
		if (_banners.ContainsKey(key))
		{
			_banners[key].HandleOnBannerAdFailedToLoad();
		}
	}

	private void onAdCollapsed(string data)
	{
		UnityEngine.Debug.Log("onAdCollapsed warning " + data);
		int key = int.Parse(data);
		if (_banners.ContainsKey(key))
		{
			_banners[key].HandleOnBannerAdCollapsed();
		}
	}

	private void onAdDismissed(string data)
	{
		UnityEngine.Debug.Log("onAdDismissed warning " + data);
		int key = int.Parse(data);
		if (_banners.ContainsKey(key))
		{
			_banners[key].HandleOnBannerAdDismissed();
		}
	}

	private void onAdExpanded(string data)
	{
		UnityEngine.Debug.Log("onAdExpanded warning " + data);
		int key = int.Parse(data);
		if (_banners.ContainsKey(key))
		{
			_banners[key].HandleOnBannerAdExpanded();
		}
	}

	private void OnInterstitialsLoaded(string adProperties)
	{
		SA_AmazonAdsExample.isInterstitialLoaded = true;
		string[] data = adProperties.Split("|"[0]);
		AMN_InterstitialDataResult obj = new AMN_InterstitialDataResult(data);
		this.OnInterstitialDataReceived(obj);
	}

	private void OnInterstitialsFailed(string error_message)
	{
		SA_AmazonAdsExample.isInterstitialLoaded = false;
		UnityEngine.Debug.Log("OnInterstitialsFailed with error " + error_message);
		AMN_InterstitialDataResult obj = new AMN_InterstitialDataResult(error_message);
		this.OnInterstitialDataReceived(obj);
	}

	private void OnInterstitialsDismissed(string warning_message)
	{
		SA_AmazonAdsExample.isInterstitialLoaded = false;
		UnityEngine.Debug.Log("OnInterstitialsDismissed warning " + warning_message);
		AMN_InterstitialDismissedResult obj = new AMN_InterstitialDismissedResult(warning_message);
		this.OnInterstitialDismissed(obj);
	}
}
