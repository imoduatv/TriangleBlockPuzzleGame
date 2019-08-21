using UnityEngine;

public class UM_AdExample : MonoBehaviour
{
	private GUIStyle style;

	private GUIStyle style2;

	private int bannerId1;

	private int bannerId2;

	private bool isLoadInt;

	private void Start()
	{
		InitializeActions();
		UM_AdManager.Init();
		UM_ExampleStatusBar.text = "Unified ad example scene loaded";
		InitStyles();
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
	}

	private void HandleOnInterstitialLoadFail(int errorCode)
	{
		UnityEngine.Debug.Log("Interstitial is failed to load. Error Code: " + errorCode);
		UM_AdManager.OnInterstitialLoaded -= HandleOnInterstitialLoaded;
		UM_AdManager.OnInterstitialLoadFail -= HandleOnInterstitialLoadFail;
		UM_AdManager.OnInterstitialClosed -= HandleOnInterstitialClosed;
	}

	private void HandleOnInterstitialLoaded()
	{
		UnityEngine.Debug.Log("Interstitial ad content ready");
		UM_AdManager.OnInterstitialLoaded -= HandleOnInterstitialLoaded;
		UM_AdManager.OnInterstitialLoadFail -= HandleOnInterstitialLoadFail;
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

	private void OnGUI()
	{
		float num = 20f;
		float num2 = 10f;
		GUI.Label(new Rect(num2, num, Screen.width, 40f), "Interstisal Example", style);
		num += 40f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Start Interstitial Ad"))
		{
			InitializeActions();
			UM_AdManager.StartInterstitialAd();
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Load Interstitial Ad"))
		{
			InitializeActions();
			UM_AdManager.LoadInterstitialAd();
			isLoadInt = true;
		}
		GUI.enabled = false;
		if (isLoadInt)
		{
			GUI.enabled = true;
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Show Interstitial Ad"))
		{
			UM_AdManager.ShowInterstitialAd();
			isLoadInt = false;
		}
		GUI.enabled = true;
		num += 80f;
		num2 = 10f;
		GUI.Label(new Rect(num2, num, Screen.width, 40f), "Banners Example", style);
		GUI.enabled = false;
		if (bannerId1 == 0)
		{
			GUI.enabled = true;
		}
		num += 40f;
		num2 = 10f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Banner Top Left"))
		{
			bannerId1 = UM_AdManager.CreateAdBanner(TextAnchor.UpperLeft);
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Banner Top Center"))
		{
			bannerId1 = UM_AdManager.CreateAdBanner(TextAnchor.UpperCenter);
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Banner Top Right"))
		{
			bannerId1 = UM_AdManager.CreateAdBanner(TextAnchor.UpperRight);
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Banner Bottom Left"))
		{
			bannerId1 = UM_AdManager.CreateAdBanner(TextAnchor.LowerLeft);
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Banner Bottom Center"))
		{
			bannerId1 = UM_AdManager.CreateAdBanner(TextAnchor.LowerCenter);
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Banner Bottom Right"))
		{
			bannerId1 = UM_AdManager.CreateAdBanner(TextAnchor.LowerRight);
		}
		GUI.enabled = false;
		if (bannerId1 != 0 && UM_AdManager.IsBannerLoaded(bannerId1))
		{
			GUI.enabled = true;
		}
		num += 80f;
		num2 = 10f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Refresh"))
		{
			UM_AdManager.RefreshBanner(bannerId1);
		}
		GUI.enabled = false;
		if (bannerId1 != 0 && UM_AdManager.IsBannerLoaded(bannerId1) && UM_AdManager.IsBannerOnScreen(bannerId1))
		{
			GUI.enabled = true;
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Hide"))
		{
			UM_AdManager.HideBanner(bannerId1);
		}
		GUI.enabled = false;
		if (bannerId1 != 0 && UM_AdManager.IsBannerLoaded(bannerId1) && !UM_AdManager.IsBannerOnScreen(bannerId1))
		{
			GUI.enabled = true;
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Show"))
		{
			UM_AdManager.ShowBanner(bannerId1);
		}
		GUI.enabled = false;
		if (bannerId1 != 0)
		{
			GUI.enabled = true;
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Destroy"))
		{
			UM_AdManager.DestroyBanner(bannerId1);
			bannerId1 = 0;
		}
		GUI.enabled = true;
		num += 80f;
		num2 = 10f;
		GUI.Label(new Rect(num2, num, Screen.width, 40f), "Banner 2", style);
		GUI.enabled = false;
		if (bannerId2 == 0)
		{
			GUI.enabled = true;
		}
		num += 40f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Create Banner 2"))
		{
			bannerId2 = UM_AdManager.CreateAdBanner(TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
		}
		GUI.enabled = false;
		if (bannerId2 != 0 && UM_AdManager.IsBannerLoaded(bannerId2))
		{
			GUI.enabled = true;
		}
		num += 80f;
		num2 = 10f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Refresh"))
		{
			UM_AdManager.RefreshBanner(bannerId2);
		}
		GUI.enabled = false;
		if (bannerId2 != 0 && UM_AdManager.IsBannerLoaded(bannerId2) && UM_AdManager.IsBannerOnScreen(bannerId2))
		{
			GUI.enabled = true;
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Hide"))
		{
			UM_AdManager.HideBanner(bannerId2);
		}
		GUI.enabled = false;
		if (bannerId2 != 0 && UM_AdManager.IsBannerLoaded(bannerId2) && !UM_AdManager.IsBannerOnScreen(bannerId2))
		{
			GUI.enabled = true;
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Show"))
		{
			UM_AdManager.ShowBanner(bannerId2);
		}
		GUI.enabled = false;
		if (bannerId2 != 0)
		{
			GUI.enabled = true;
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Destroy"))
		{
			UM_AdManager.DestroyBanner(bannerId2);
			bannerId2 = 0;
		}
		GUI.enabled = true;
	}
}
