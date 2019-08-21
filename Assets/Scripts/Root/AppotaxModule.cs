using GoogleMobileAds.Api;
using System;
using UnityEngine;

namespace Root
{
	public class AppotaxModule : IAdsService
	{
		private Action m_CloseCallback;

		private Action m_LoadedCallback;

		private InterstitialAd m_AppotaxInterstitial;

		private BannerView bannerView;

		private BannerView bannerViewPause;

		private string androidInterID;

		private string iosInterID;

		private string androidBannerID;

		private string iosBannerID;

		private bool isAcceptConsent = true;

		public void Init(AdsConfig adsConfig)
		{
			androidInterID = adsConfig.AndroidInterstitialID;
			iosInterID = adsConfig.IosInterstitialID;
			androidBannerID = adsConfig.AndroidBannerID;
			iosBannerID = adsConfig.IosBannerID;
		}

		public void Init(AdsConfig adsConfig, bool isAcceptConsent)
		{
			androidInterID = adsConfig.AndroidInterstitialID;
			iosInterID = adsConfig.IosInterstitialID;
			androidBannerID = adsConfig.AndroidBannerID;
			iosBannerID = adsConfig.IosBannerID;
			this.isAcceptConsent = isAcceptConsent;
		}

		public void SetConsent(bool isAcceptConsent)
		{
			this.isAcceptConsent = isAcceptConsent;
		}

		public void ShowFullAds(Action closeCallback = null)
		{
			m_CloseCallback = closeCallback;
			if (IsFullAdsLoaded())
			{
				m_AppotaxInterstitial.Show();
			}
			else if (m_CloseCallback != null)
			{
				m_CloseCallback();
			}
			LoadFullAds();
		}

		public void LoadFullAds(Action loadedCallback = null)
		{
			string adUnitId = androidInterID;
			m_AppotaxInterstitial = new InterstitialAd(adUnitId);
			m_AppotaxInterstitial.OnAdClosed += HandleOnAdClosed;
			AdRequest request = (!isAcceptConsent) ? new AdRequest.Builder().AddExtra("npa", "1").Build() : new AdRequest.Builder().Build();
			m_AppotaxInterstitial.LoadAd(request);
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
			if (m_AppotaxInterstitial == null)
			{
				return false;
			}
			return m_AppotaxInterstitial.IsLoaded();
		}

		public void LoadAndShowBannerBot()
		{
		}

		public void LoadAndShowBannerTop()
		{
		}

		public void LoadBannerBot()
		{
			string adUnitId = androidBannerID;
			bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
			AdRequest request = (!isAcceptConsent) ? new AdRequest.Builder().AddExtra("npa", "1").Build() : new AdRequest.Builder().Build();
			bannerView.LoadAd(request);
			UnityEngine.Debug.Log("Load banner appotax");
			HideBanner();
		}

		public void LoadBannerTop()
		{
		}

		public void LoadBannerTopForPause()
		{
			string adUnitId = androidBannerID;
			bannerViewPause = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
			AdRequest request = (!isAcceptConsent) ? new AdRequest.Builder().AddExtra("npa", "1").Build() : new AdRequest.Builder().Build();
			bannerViewPause.LoadAd(request);
			UnityEngine.Debug.Log("Load banner appotax");
			HideBannerPause();
		}

		public void ShowBanner(bool isRefreshBanner = false)
		{
			if (bannerView != null)
			{
				bannerView.Show();
			}
		}

		public void ShowBannerPause()
		{
			if (bannerViewPause != null)
			{
				bannerViewPause.Show();
			}
		}

		public void HideBanner()
		{
			if (bannerView != null)
			{
				bannerView.Hide();
			}
		}

		public void HideBannerPause()
		{
			if (bannerViewPause != null)
			{
				bannerViewPause.Hide();
			}
		}

		public void RefreshBanner()
		{
			AdRequest request = new AdRequest.Builder().Build();
			if (bannerView != null)
			{
				bannerView.LoadAd(request);
			}
			HideBanner();
		}

		public void RefreshBannerPause()
		{
			if (bannerViewPause != null)
			{
				AdRequest request = new AdRequest.Builder().Build();
				bannerViewPause.LoadAd(request);
				HideBannerPause();
			}
			else
			{
				LoadBannerTopForPause();
			}
		}

		public void ShutDown()
		{
		}
	}
}
