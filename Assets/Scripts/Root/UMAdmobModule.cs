using System;
using UnityEngine;

namespace Root
{
	public class UMAdmobModule : IAdsService
	{
		private Action LoadedCallBack;

		private Action CloseCallBack;

		private bool isLoadedFullAd;

		private bool isLoadedBannerAd;

		private int bannerId;

		public void Init(AdsConfig adsConfig)
		{
			UM_AdManager.Init();
		}

		public void ShowFullAds(Action closeCallback = null)
		{
			if (closeCallback != null)
			{
				CloseCallBack = closeCallback;
			}
			if (isLoadedFullAd)
			{
				UM_AdManager.ShowInterstitialAd();
			}
			else
			{
				closeCallback?.Invoke();
			}
		}

		public void LoadFullAds(Action loadedCallback = null)
		{
			if (loadedCallback != null)
			{
				LoadedCallBack = loadedCallback;
			}
			InitializeActions();
			UM_AdManager.LoadInterstitialAd();
			isLoadedFullAd = false;
		}

		public bool IsFullAdsLoaded()
		{
			return isLoadedFullAd;
		}

		public void LoadAndShowBannerBot()
		{
			LoadBanner(TextAnchor.LowerCenter, isAutoShow: true);
		}

		public void LoadAndShowBannerTop()
		{
			LoadBanner(TextAnchor.LowerCenter, isAutoShow: true);
		}

		public void LoadBannerBot()
		{
			LoadBanner(TextAnchor.LowerCenter, isAutoShow: false);
		}

		public void LoadBannerTop()
		{
			LoadBanner(TextAnchor.UpperCenter, isAutoShow: false);
		}

		private void LoadBanner(TextAnchor position, bool isAutoShow)
		{
			if (bannerId != 0)
			{
				UM_AdManager.DestroyBanner(bannerId);
				bannerId = 0;
			}
			bannerId = UM_AdManager.CreateAdBanner(position, GADBannerSize.SMART_BANNER);
			GoogleMobileAdBanner banner = GetBanner(bannerId);
			if (banner != null)
			{
				banner.ShowOnLoad = isAutoShow;
			}
		}

		public void ShowBanner(bool isRefreshBanner = false)
		{
			if (bannerId != 0)
			{
				UM_AdManager.ShowBanner(bannerId);
				if (isRefreshBanner)
				{
					RefreshBanner();
				}
			}
		}

		public void HideBanner()
		{
			if (bannerId != 0)
			{
				UM_AdManager.HideBanner(bannerId);
			}
		}

		public void RefreshBanner()
		{
			if (bannerId != 0)
			{
				UM_AdManager.RefreshBanner(bannerId);
			}
		}

		public void ShutDown()
		{
			if (bannerId != 0)
			{
				UM_AdManager.DestroyBanner(bannerId);
				bannerId = 0;
			}
		}

		private GoogleMobileAdBanner GetBanner(int id)
		{
			return GoogleMobileAd.GetBanner(id);
		}

		private void InitializeActions()
		{
			UM_AdManager.ResetActions();
			UM_AdManager.OnInterstitialLoaded += HandleOnInterstitialLoaded;
			UM_AdManager.OnInterstitialLoadFail += HandleOnInterstitialLoadFail;
			UM_AdManager.OnInterstitialClosed += HandleOnInterstitialClosed;
		}

		private void HandleOnInterstitialClosed()
		{
			UnityEngine.Debug.Log("Interstitial Ad was closed");
			UM_AdManager.OnInterstitialClosed -= HandleOnInterstitialClosed;
			LoadFullAds();
			if (CloseCallBack != null)
			{
				CloseCallBack();
			}
		}

		private void HandleOnInterstitialLoadFail(int code)
		{
			UnityEngine.Debug.Log("Interstitial is failed to load");
			UM_AdManager.OnInterstitialLoaded -= HandleOnInterstitialLoaded;
			UM_AdManager.OnInterstitialLoadFail -= HandleOnInterstitialLoadFail;
			UM_AdManager.OnInterstitialClosed -= HandleOnInterstitialClosed;
		}

		private void HandleOnInterstitialLoaded()
		{
			UnityEngine.Debug.Log("Interstitial ad content ready");
			UM_AdManager.OnInterstitialLoaded -= HandleOnInterstitialLoaded;
			UM_AdManager.OnInterstitialLoadFail -= HandleOnInterstitialLoadFail;
			isLoadedFullAd = true;
			if (LoadedCallBack != null)
			{
				LoadedCallBack();
			}
		}
	}
}
