using GoogleMobileAds.Api;
using System;

namespace Root
{
	public class RichadxModule : IAdsService
	{
		private Action m_CloseCallback;

		private Action m_LoadedCallback;

		private InterstitialAd m_RichadxInterstitial;

		private string androidInterID;

		private string iosInterID;

		public void Init(AdsConfig adsConfig)
		{
			androidInterID = adsConfig.AndroidInterstitialID;
			iosInterID = adsConfig.IosInterstitialID;
		}

		public void ShowFullAds(Action closeCallback = null)
		{
			m_CloseCallback = closeCallback;
			if (IsFullAdsLoaded())
			{
				m_RichadxInterstitial.Show();
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
			m_RichadxInterstitial = new InterstitialAd(adUnitId);
			m_RichadxInterstitial.OnAdClosed += HandleOnAdClosed;
			AdRequest request = new AdRequest.Builder().Build();
			m_RichadxInterstitial.LoadAd(request);
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
			if (m_RichadxInterstitial == null)
			{
				return false;
			}
			return m_RichadxInterstitial.IsLoaded();
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
		}

		public void HideBanner()
		{
		}

		public void RefreshBanner()
		{
		}

		public void ShutDown()
		{
		}
	}
}
