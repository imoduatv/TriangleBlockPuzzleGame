using UnityEngine;

public class CustomInterstisialExample : MonoBehaviour
{
	private void Start()
	{
		GoogleMobileAd.Init();
		GoogleMobileAd.OnInterstitialLoaded += HandleOnInterstitialLoaded;
		GoogleMobileAd.OnInterstitialClosed += OnInterstisialsClosed;
		GoogleMobileAd.OnInterstitialOpened += OnInterstisialsOpen;
		GoogleMobileAd.LoadInterstitialAd();
	}

	private void HandleOnInterstitialLoaded()
	{
		GoogleMobileAd.ShowInterstitialAd();
	}

	private void OnInterstisialsOpen()
	{
	}

	private void OnInterstisialsClosed()
	{
	}
}
