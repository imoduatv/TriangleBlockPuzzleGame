using SA.Common.Pattern;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMobileAdTab : FeatureTab
{
	private const string MY_BANNERS_AD_UNIT_ID = "ca-app-pub-6101605888755494/1824764765";

	private const string MY_INTERSTISIALS_AD_UNIT_ID = "ca-app-pub-6101605888755494/3301497967";

	private const string MY_REWARDED_VIDEO_AD_UNIT_ID = "ca-app-pub-6101605888755494/5378283960";

	private GoogleMobileAdBanner Banner;

	private GoogleMobileAdBanner SmartBanner;

	private bool IsInterstisialsAdReady;

	private bool IsVideoAdReady;

	[SerializeField]
	private Button ShowInterstitialButton;

	[SerializeField]
	private Button ShowVideoButton;

	public Toggle CustomPosTgl;

	public Toggle UpperLeftTgl;

	public Toggle UpperCenterTgl;

	public Toggle UpperRightTgl;

	public Toggle BottomLeftTgl;

	public Toggle BottomCenterTgl;

	public Toggle BottomRightTgl;

	public Button BannerHideBtn;

	public Button BannerCreateBtn;

	public Button BannerRefreshBtn;

	public Button BannerChangePosRandomBtn;

	public Button BannerDestroyBtn;

	public Toggle SmartBotPosTgl;

	public Toggle SmartTopPosTgl;

	public Button SmartHide;

	public Button SmartCreate;

	public Button SmartRefresh;

	public Button SmartDestroy;

	private TextAnchor? BannerPosition;

	private TextAnchor SmartBannerPosition = TextAnchor.UpperCenter;

	private void Start()
	{
		CustomPosTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				BannerPosition = null;
			}
		});
		UpperLeftTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				BannerPosition = TextAnchor.UpperLeft;
			}
		});
		UpperCenterTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				BannerPosition = TextAnchor.UpperCenter;
			}
		});
		UpperRightTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				BannerPosition = TextAnchor.UpperRight;
			}
		});
		BottomLeftTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				BannerPosition = TextAnchor.LowerLeft;
			}
		});
		BottomCenterTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				BannerPosition = TextAnchor.LowerCenter;
			}
		});
		BottomRightTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				BannerPosition = TextAnchor.LowerRight;
			}
		});
		SmartTopPosTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				SmartBannerPosition = TextAnchor.UpperCenter;
			}
		});
		SmartBotPosTgl.onValueChanged.AddListener(delegate(bool b)
		{
			if (b)
			{
				SmartBannerPosition = TextAnchor.LowerCenter;
			}
		});
	}

	public void Init()
	{
		GoogleMobileAd.Init();
		GoogleMobileAd.SetGender(GoogleGender.Male);
		GoogleMobileAd.AddKeyword("game");
		GoogleMobileAd.SetBirthday(1989, AndroidMonth.MARCH, 18);
		GoogleMobileAd.TagForChildDirectedTreatment(tagForChildDirectedTreatment: false);
		GoogleMobileAd.OnInterstitialLoaded += OnInterstisialsLoaded;
		GoogleMobileAd.OnInterstitialOpened += OnInterstisialsOpen;
		Singleton<AndroidAdMobController>.Instance.OnRewardedVideoLoaded += HandleOnRewardedVideoLoaded;
		Singleton<AndroidAdMobController>.Instance.OnRewardedVideoAdClosed += HandleOnRewardedVideoAdClosed;
		GoogleMobileAd.OnAdInAppRequest += OnInAppRequest;
	}

	public void StartInterstitialAd()
	{
		GoogleMobileAd.StartInterstitialAd();
	}

	public void LoadInterstitialAd()
	{
		GoogleMobileAd.LoadInterstitialAd();
	}

	public void ShowInterstitialAd()
	{
		GoogleMobileAd.ShowInterstitialAd();
	}

	public void StartRewardedVideoAd()
	{
		Singleton<AndroidAdMobController>.Instance.StartRewardedVideo();
	}

	public void LoadRewardedVideoAd()
	{
		Singleton<AndroidAdMobController>.Instance.LoadRewardedVideo();
	}

	public void ShowRewardedVideoAd()
	{
		Singleton<AndroidAdMobController>.Instance.ShowRewardedVideo();
	}

	public void BannerHide()
	{
		Banner.Hide();
	}

	public void BannerCreate()
	{
		TextAnchor? bannerPosition = BannerPosition;
		Banner = ((!bannerPosition.HasValue) ? GoogleMobileAd.CreateAdBanner(300, 100, GADBannerSize.BANNER) : GoogleMobileAd.CreateAdBanner(BannerPosition.Value, GADBannerSize.BANNER));
	}

	public void BannerRefresh()
	{
		Banner.Refresh();
	}

	public void BannerDestroy()
	{
		GoogleMobileAd.DestroyBanner(Banner.id);
		Banner = null;
	}

	public void SmartBannerHide()
	{
		SmartBanner.Hide();
	}

	public void SmartBannerCreate()
	{
		SmartBanner = GoogleMobileAd.CreateAdBanner(SmartBannerPosition, GADBannerSize.SMART_BANNER);
	}

	public void SmartBannerRefresh()
	{
		SmartBanner.Refresh();
	}

	public void SmartBannerDestroy()
	{
		GoogleMobileAd.DestroyBanner(SmartBanner.id);
		SmartBanner = null;
	}

	public void ChangePosRandom()
	{
		Banner.SetBannerPosition(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
	}

	private void FixedUpdate()
	{
		if (IsInterstisialsAdReady)
		{
			ShowInterstitialButton.interactable = true;
		}
		else
		{
			ShowInterstitialButton.interactable = false;
		}
		if (IsVideoAdReady)
		{
			ShowVideoButton.interactable = true;
		}
		else
		{
			ShowVideoButton.interactable = false;
		}
		if (Banner != null)
		{
			BannerDestroyBtn.interactable = true;
			if (Banner.IsLoaded)
			{
				BannerRefreshBtn.interactable = true;
				BannerChangePosRandomBtn.interactable = true;
				if (Banner.IsOnScreen)
				{
					BannerHideBtn.interactable = true;
					BannerCreateBtn.interactable = false;
				}
				else
				{
					BannerHideBtn.interactable = false;
					BannerCreateBtn.interactable = true;
				}
			}
			else
			{
				BannerRefreshBtn.interactable = false;
				BannerChangePosRandomBtn.interactable = false;
				BannerHideBtn.interactable = false;
				BannerCreateBtn.interactable = false;
			}
		}
		else
		{
			BannerHideBtn.interactable = false;
			BannerCreateBtn.interactable = true;
			BannerRefreshBtn.interactable = false;
			BannerDestroyBtn.interactable = false;
			BannerChangePosRandomBtn.interactable = false;
		}
		if (SmartBanner != null)
		{
			SmartDestroy.interactable = true;
			if (SmartBanner.IsLoaded)
			{
				SmartRefresh.interactable = true;
				if (SmartBanner.IsOnScreen)
				{
					SmartHide.interactable = true;
					SmartCreate.interactable = false;
				}
				else
				{
					SmartHide.interactable = false;
					SmartCreate.interactable = true;
				}
			}
			else
			{
				SmartRefresh.interactable = false;
				SmartHide.interactable = false;
				SmartCreate.interactable = false;
			}
		}
		else
		{
			SmartHide.interactable = false;
			SmartCreate.interactable = true;
			SmartRefresh.interactable = false;
			SmartDestroy.interactable = false;
		}
	}

	private void OnInterstisialsLoaded()
	{
		IsInterstisialsAdReady = true;
	}

	private void OnInterstisialsOpen()
	{
		IsInterstisialsAdReady = false;
	}

	private void HandleOnRewardedVideoLoaded()
	{
		IsVideoAdReady = true;
	}

	private void HandleOnRewardedVideoAdClosed()
	{
		IsVideoAdReady = false;
	}

	private void OnInAppRequest(string productId)
	{
		UnityEngine.Debug.Log("In App Request for product Id: " + productId + " received");
		GoogleMobileAd.RecordInAppResolution(GADInAppResolution.RESOLUTION_SUCCESS);
	}
}
