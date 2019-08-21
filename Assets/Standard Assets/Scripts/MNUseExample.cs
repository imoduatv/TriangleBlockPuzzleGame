using UnityEngine;

public class MNUseExample : MNFeaturePreview
{
	public string appleId = "itms-apps://itunes.apple.com/id375380948?mt=8";

	public string androidAppUrl = "market://details?id=com.google.earth";

	private void Awake()
	{
	}

	private void OnGUI()
	{
		UpdateToStartPos();
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40f), "Native Pop Ups", style);
		StartY += YLableStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Rate PopUp with events"))
		{
			MNRateUsPopup mNRateUsPopup = new MNRateUsPopup("rate us", "rate us, please", "Rate Us", "No, Thanks", "Later");
			mNRateUsPopup.SetAppleId(appleId);
			mNRateUsPopup.SetAndroidAppUrl(androidAppUrl);
			mNRateUsPopup.AddDeclineListener(delegate
			{
				UnityEngine.Debug.Log("rate us declined");
			});
			mNRateUsPopup.AddRemindListener(delegate
			{
				UnityEngine.Debug.Log("remind me later");
			});
			mNRateUsPopup.AddRateUsListener(delegate
			{
				UnityEngine.Debug.Log("rate us!!!");
			});
			mNRateUsPopup.AddDismissListener(delegate
			{
				UnityEngine.Debug.Log("rate us dialog dismissed :(");
			});
			mNRateUsPopup.Show();
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Dialog PopUp"))
		{
			MNPopup mNPopup = new MNPopup("title", "dialog message");
			mNPopup.AddAction("action1", delegate
			{
				UnityEngine.Debug.Log("action 1 action callback");
			});
			mNPopup.AddAction("action2", delegate
			{
				UnityEngine.Debug.Log("action 2 action callback");
			});
			mNPopup.AddDismissListener(delegate
			{
				UnityEngine.Debug.Log("dismiss listener");
			});
			mNPopup.Show();
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Message PopUp"))
		{
			MNPopup mNPopup2 = new MNPopup("title", "dialog message");
			mNPopup2.AddAction("Ok", delegate
			{
				UnityEngine.Debug.Log("Ok action callback");
			});
			mNPopup2.AddDismissListener(delegate
			{
				UnityEngine.Debug.Log("dismiss listener");
			});
			mNPopup2.Show();
		}
		StartY += YButtonStep;
		StartX = XStartPos;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Prealoder"))
		{
			MNP.ShowPreloader("Title", "Message");
			Invoke("OnPreloaderTimeOut", 3f);
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Hide Prealoder"))
		{
			MNP.HidePreloader();
		}
	}

	private void OnPreloaderTimeOut()
	{
		MNP.HidePreloader();
	}
}
