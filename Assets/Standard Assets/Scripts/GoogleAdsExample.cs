using System;
using UnityEngine;
using UnityEngine.UI;

public class GoogleAdsExample : MonoBehaviour
{
	public Button ShowInterstitialButton;

	public Button ShowVideoButton;

	public Button[] banner1DependableButtons;

	public Button[] banner2DependableButtons;

	private GUIStyle style;

	private GUIStyle style2;

	private GoogleMobileAdBanner banner1;

	private GoogleMobileAdBanner banner2;

	private void Start()
	{
		GoogleMobileAd.Init();
		GoogleMobileAd.SetGender(GoogleGender.Male);
		GoogleMobileAd.AddKeyword("game");
		GoogleMobileAd.SetBirthday(1989, AndroidMonth.MARCH, 18);
		GoogleMobileAd.TagForChildDirectedTreatment(tagForChildDirectedTreatment: false);
		GoogleMobileAd.AddTestDevice("733770c317dcbf4675fe870d3df9ca42");
		GoogleMobileAd.OnAdInAppRequest += OnInAppRequest;
		InitStyles();
	}

	private void InitStyles()
	{
		style = new GUIStyle();
		style.normal.textColor = Color.white;
		style.fontSize = 16;
		style.fontStyle = FontStyle.BoldAndItalic;
		style.alignment = TextAnchor.UpperLeft;
		style.wordWrap = true;
		style2 = new GUIStyle();
		style2.normal.textColor = Color.white;
		style2.fontSize = 12;
		style2.fontStyle = FontStyle.Italic;
		style2.alignment = TextAnchor.UpperLeft;
		style2.wordWrap = true;
	}

	public void StartInterstitial()
	{
		GoogleMobileAd.StartInterstitialAd();
	}

	public void LoadInterstitial()
	{
		GoogleMobileAd.LoadInterstitialAd();
	}

	public void ShowInterstitial()
	{
		GoogleMobileAd.ShowInterstitialAd();
	}

	public void StartVideo()
	{
		GoogleMobileAd.StartRewardedVideo();
	}

	public void LoadVideo()
	{
		GoogleMobileAd.LoadRewardedVideo();
	}

	public void ShowVideo()
	{
		GoogleMobileAd.ShowRewardedVideo();
	}

	public void BannerCustomPos()
	{
		banner1 = GoogleMobileAd.CreateAdBanner(300, 100, GADBannerSize.BANNER);
	}

	public void BannerTopLeft()
	{
		banner1 = GoogleMobileAd.CreateAdBanner(TextAnchor.UpperLeft, GADBannerSize.BANNER);
	}

	public void BannerTopCenter()
	{
		banner1 = GoogleMobileAd.CreateAdBanner(TextAnchor.UpperCenter, GADBannerSize.BANNER);
	}

	public void BannerTopRight()
	{
		banner1 = GoogleMobileAd.CreateAdBanner(TextAnchor.UpperRight, GADBannerSize.BANNER);
	}

	public void BannerBottomLeft()
	{
		banner1 = GoogleMobileAd.CreateAdBanner(TextAnchor.LowerLeft, GADBannerSize.BANNER);
	}

	public void BannerBottomCenter()
	{
		banner1 = GoogleMobileAd.CreateAdBanner(TextAnchor.LowerCenter, GADBannerSize.BANNER);
	}

	public void BannerBottomRight()
	{
		banner1 = GoogleMobileAd.CreateAdBanner(TextAnchor.LowerRight, GADBannerSize.BANNER);
	}

	public void Refresh1()
	{
		banner1.Refresh();
	}

	public void MoveToCenter()
	{
		banner1.SetBannerPosition(TextAnchor.MiddleCenter);
	}

	public void ToRandomCoords()
	{
		banner1.SetBannerPosition(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height));
	}

	public void Hide1()
	{
		banner1.Hide();
	}

	public void Show1()
	{
		banner1.Show();
	}

	public void Destroy1()
	{
		GoogleMobileAd.DestroyBanner(banner1.id);
		banner1 = null;
	}

	public void SmartBanner()
	{
		banner2 = GoogleMobileAd.CreateAdBanner(TextAnchor.LowerLeft, GADBannerSize.SMART_BANNER);
		GoogleMobileAdBanner googleMobileAdBanner = banner2;
		googleMobileAdBanner.OnLoadedAction = (Action<GoogleMobileAdBanner>)Delegate.Combine(googleMobileAdBanner.OnLoadedAction, new Action<GoogleMobileAdBanner>(OnBannerLoadedAction));
		banner2.ShowOnLoad = false;
	}

	public void Refresh2()
	{
		banner2.Refresh();
	}

	public void Hide2()
	{
		banner2.Hide();
	}

	public void Show2()
	{
		banner2.Show();
	}

	public void Destroy2()
	{
		GoogleMobileAd.DestroyBanner(banner2.id);
		banner2 = null;
	}

	private void Update()
	{
		ShowInterstitialButton.interactable = GoogleMobileAd.IsInterstitialReady;
		ShowVideoButton.interactable = GoogleMobileAd.IsRewardedVideoReady;
		Button[] array = banner1DependableButtons;
		foreach (Button button in array)
		{
			button.interactable = (banner1 != null);
		}
		Button[] array2 = banner2DependableButtons;
		foreach (Button button2 in array2)
		{
			button2.interactable = (banner2 != null);
		}
	}

	private void OnInAppRequest(string productId)
	{
		UnityEngine.Debug.Log("In App Request for product Id: " + productId + " received");
		GoogleMobileAd.RecordInAppResolution(GADInAppResolution.RESOLUTION_SUCCESS);
	}

	private void OnInterstitialLoaded()
	{
		UnityEngine.Debug.Log("OnInterstitialLoaded catched with C# Actions usage");
	}

	private void OnOpenedAction(GoogleMobileAdBanner banner)
	{
		banner.OnOpenedAction = (Action<GoogleMobileAdBanner>)Delegate.Remove(banner.OnOpenedAction, new Action<GoogleMobileAdBanner>(OnOpenedAction));
		UnityEngine.Debug.Log("Banner was just clicked");
	}

	private void OnBannerLoadedAction(GoogleMobileAdBanner banner)
	{
		banner.OnLoadedAction = (Action<GoogleMobileAdBanner>)Delegate.Remove(banner.OnLoadedAction, new Action<GoogleMobileAdBanner>(OnBannerLoadedAction));
		banner.Show();
	}
}
