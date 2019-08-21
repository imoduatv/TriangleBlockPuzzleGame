using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdMobBanner : MonoBehaviour
{
	public GADBannerSize size = GADBannerSize.SMART_BANNER;

	public TextAnchor anchor = TextAnchor.LowerCenter;

	private static Dictionary<string, GoogleMobileAdBanner> _registerdBanners;

	public static Dictionary<string, GoogleMobileAdBanner> registerdBanners
	{
		get
		{
			if (_registerdBanners == null)
			{
				_registerdBanners = new Dictionary<string, GoogleMobileAdBanner>();
			}
			return _registerdBanners;
		}
	}

	public string sceneBannerId => SceneManager.GetActiveScene().name + "_" + base.gameObject.name;

	private void Awake()
	{
		if (!GoogleMobileAd.IsInited)
		{
			GoogleMobileAd.Init();
		}
	}

	private void Start()
	{
		ShowBanner();
	}

	private void OnDestroy()
	{
		HideBanner();
	}

	public void ShowBanner()
	{
		GoogleMobileAdBanner googleMobileAdBanner;
		if (registerdBanners.ContainsKey(sceneBannerId))
		{
			googleMobileAdBanner = registerdBanners[sceneBannerId];
		}
		else
		{
			googleMobileAdBanner = GoogleMobileAd.CreateAdBanner(anchor, size);
			registerdBanners.Add(sceneBannerId, googleMobileAdBanner);
		}
		if (googleMobileAdBanner.IsLoaded && !googleMobileAdBanner.IsOnScreen)
		{
			googleMobileAdBanner.Show();
		}
	}

	public void HideBanner()
	{
		if (!registerdBanners.ContainsKey(sceneBannerId))
		{
			return;
		}
		GoogleMobileAdBanner googleMobileAdBanner = registerdBanners[sceneBannerId];
		if (googleMobileAdBanner.IsLoaded)
		{
			if (googleMobileAdBanner.IsOnScreen)
			{
				googleMobileAdBanner.Hide();
			}
		}
		else
		{
			googleMobileAdBanner.ShowOnLoad = false;
		}
	}
}
