using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using System;
using UnityEngine;

namespace Root
{
	public class AppodealModule : IAdsService, IRewardedVideoAdListener, IInterstitialAdListener, IBannerAdListener
	{
		private Action m_CloseCallback;

		private Action m_CloseRewardCallback;

		private Action m_SkipRewardCallback;

		private bool isNeedToShowBannerAds;

		private bool isFirstLoaded = true;

		public void Init(AdsConfig adsConfig)
		{
		}

		public void InitAppodeal(AdsConfig adsConfig, bool isAcceptConsent, int gender = 3, bool isInitInmobi = false)
		{
			if (!isInitInmobi)
			{
				UnityEngine.Debug.Log("Disable Inmobi");
				Appodeal.disableNetwork("inmobi");
			}
			else
			{
				UnityEngine.Debug.Log("Enable Inmobi");
			}
			string androidAppId = adsConfig.AndroidAppId;
			Appodeal.disableNetwork("chartboost");
			Appodeal.disableNetwork("startapp");
			Appodeal.disableNetwork("mintegral");
			Appodeal.setAutoCache(3, autoCache: true);
			Appodeal.setAutoCache(4, autoCache: true);
			Appodeal.setAutoCache(128, autoCache: true);
			Appodeal.disableLocationPermissionCheck();
			Appodeal.initialize(androidAppId, 135, isAcceptConsent);
			Appodeal.setInterstitialCallbacks(this);
			Appodeal.setRewardedVideoCallbacks(this);
			Appodeal.setBannerCallbacks(this);
		}

		public void ShowFullAds(Action closeCallback = null)
		{
			m_CloseCallback = closeCallback;
			if (Appodeal.isLoaded(3))
			{
				Appodeal.show(3);
			}
		}

		public bool ShowFullAdsWithEcpm(string placement, Action closeCallback = null)
		{
			m_CloseCallback = closeCallback;
			bool flag = Appodeal.canShow(3, placement);
			if (flag)
			{
				Appodeal.show(3, placement);
			}
			return flag;
		}

		public void ShowRewardAds(Action closeRewardCallback = null, Action skipRewardCallback = null)
		{
			m_CloseRewardCallback = closeRewardCallback;
			m_SkipRewardCallback = skipRewardCallback;
			if (Appodeal.isLoaded(128))
			{
				UnityEngine.Debug.Log("Show appodeal");
				Appodeal.show(128);
			}
		}

		public void LoadFullAds(Action loadedCallback = null)
		{
		}

		private void HandleOnAdClosed(object sender, EventArgs args)
		{
			if (m_CloseCallback != null)
			{
				m_CloseCallback();
			}
		}

		public bool IsFullAdsLoaded()
		{
			return Appodeal.isLoaded(3);
		}

		public bool IsRewardAdsLoaded()
		{
			return Appodeal.isLoaded(128);
		}

		public void LoadAndShowBannerBot()
		{
		}

		public void LoadAndShowBannerTop()
		{
		}

		public void LoadBannerBot()
		{
		}

		public void LoadBannerTop()
		{
		}

		public void ShowBanner(bool isRefreshBanner = false)
		{
			isNeedToShowBannerAds = true;
			UnityEngine.Debug.Log("Show appodeal banner");
			Appodeal.show(8, "bannerbot");
		}

		public void ShowBannerTop(bool isRefreshBanner = false)
		{
			isNeedToShowBannerAds = true;
			UnityEngine.Debug.Log("Show appodeal banner top");
			Appodeal.show(16, "bannertop");
		}

		public void HideBannerTop()
		{
			isNeedToShowBannerAds = false;
			Appodeal.hide(16);
			UnityEngine.Debug.Log("Hide appodeal banner");
		}

		public void HideBanner()
		{
			isNeedToShowBannerAds = false;
			Appodeal.hide(8);
			UnityEngine.Debug.Log("Hide appodeal banner");
		}

		public void RefreshBanner()
		{
		}

		public void ShutDown()
		{
		}

		public void onInterstitialLoaded(bool isPrecache)
		{
			UnityEngine.Debug.Log("Interstitial loaded");
		}

		public void onInterstitialFailedToLoad()
		{
			UnityEngine.Debug.Log("Interstitial failed");
		}

		public void onInterstitialShown()
		{
			UnityEngine.Debug.Log("Interstitial opened");
		}

		public void onInterstitialClosed()
		{
			UnityEngine.Debug.Log("Interstitial closed");
			if (m_CloseCallback != null)
			{
				m_CloseCallback();
			}
		}

		public void onInterstitialClicked()
		{
			UnityEngine.Debug.Log("Interstitial clicked");
		}

		public void onRewardedVideoLoaded()
		{
			UnityEngine.Debug.Log("Video loaded");
		}

		public void onRewardedVideoFailedToLoad()
		{
			UnityEngine.Debug.Log("Video failed");
		}

		public void onRewardedVideoShown()
		{
			UnityEngine.Debug.Log("Video shown");
		}

		public void onRewardedVideoClosed(bool finished)
		{
			if (finished)
			{
				if (m_CloseRewardCallback != null)
				{
					m_CloseRewardCallback();
				}
			}
			else if (m_SkipRewardCallback != null)
			{
				m_SkipRewardCallback();
			}
		}

		public void onRewardedVideoFinished(int amount, string name)
		{
			UnityEngine.Debug.Log("Reward: " + amount + " " + name);
		}

		public void onBannerLoaded(bool isPrecache)
		{
			if (isFirstLoaded && isNeedToShowBannerAds)
			{
				Appodeal.show(8);
				isFirstLoaded = false;
			}
		}

		public void onBannerFailedToLoad()
		{
		}

		public void onBannerShown()
		{
		}

		public void onBannerClicked()
		{
		}

		public void onRewardedVideoLoaded(bool precache)
		{
		}

		public void onRewardedVideoFinished(double amount, string name)
		{
		}

		public void onRewardedVideoExpired()
		{
		}

		public void onInterstitialExpired()
		{
		}

		public void onBannerExpired()
		{
		}

		public void onRewardedVideoClicked()
		{
		}
	}
}
