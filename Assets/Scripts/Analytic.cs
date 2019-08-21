using Facebook.Unity;
using Firebase.Analytics;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Analytic : MonoBehaviour
{
	private static Analytic _instance;

	private const string MODE_KEY = "Mode";

	public const string FIRST_DATE_KEY = "FirstDate";

	private const string m_EventXmasKey = "Xmas";

	private DateTime m_FirstOpenAppDate;

	private Dictionary<string, object> parameter = new Dictionary<string, object>();

	private const string RESTART_EVENT = "Restart Click";

	private const string SHOP_EVENT = "Shop Click";

	private const string REWARD_EVENT = "Show Reward ads - diamond";

	private const string PLAY_EVENT = "Play Click";

	private const string BUY_THEME_EVENT = "Buy Theme";

	private const string USE_THEME_EVENT = "Use Theme";

	private const string SHOW_INTER_EVENT = "Show Interstitial";

	private const string AD_REQUEST = "Ad Request";

	private const string TIME_PLAY = "Time_Play";

	private const string CONTINUE_EVENT = "Continue Click";

	private const string IAP_CLICK_EVENT = "IAP Click";

	private const string GAME_OVER_EVENT = "Game Over";

	private const string WATCH_VIDEO_UNLOCK = "Watch Video Unlock ";

	private const string SHOW_INTER_KEY = "Inter50times";

	private const string SHOW_REWARD_KEY = "Reward15times";

	private const string MOVE_COUNT = "Move_Count";

	public static Analytic Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<Analytic>();
			}
			return _instance;
		}
	}

	public void InitFB()
	{
		UnityEngine.Debug.Log("Init facebook");
		if (!FB.IsInitialized)
		{
			FB.Init(InitCallback, OnHideUnity);
		}
		else
		{
			FB.ActivateApp();
		}
		if (!PlayerPrefs.HasKey("Version 5") && !PlayerPrefs.HasKey("Version 6"))
		{
			PlayerPrefs.SetInt("Version 5", 1);
			PlayerPrefs.SetInt("Version 6", 1);
			PlayerPrefs.SetString("FirstDateOpen", DateTime.Now.ToString());
		}
	}

	private void InitCallback()
	{
		if (FB.IsInitialized)
		{
			FB.ActivateApp();
			ServicesManager.AnalyticABTest().SetLogAnalytic(delegate(string s)
			{
				UnityEngine.Debug.Log(s);
				LogAppEvent(s);
			});
			if (!PlayerPrefs.HasKey("Xmas"))
			{
				PlayerPrefs.SetInt("Xmas", 1);
				LogXmasEvent();
			}
			if (PlayerPrefs.HasKey("Version 6") && !PlayerPrefs.HasKey("LogFirstDate"))
			{
				PlayerPrefs.SetInt("LogFirstDate", 1);
				string @string = PlayerPrefs.GetString("FirstDateOpen");
				DateTime dateTime = DateTime.Parse(@string);
				string text = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
				UnityEngine.Debug.Log(text);
				LogFirstDate(text);
			}
		}
		else
		{
			UnityEngine.Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void LogAppEvent(string name, float value = 1f, Dictionary<string, object> dict = null)
	{
		if (FB.IsInitialized)
		{
			FB.LogAppEvent(name, value, dict);
		}
		string text = name.Replace(' ', '_').Replace('-', '_');
		UnityEngine.Debug.Log("Name after convert:" + text);
		LogFirebase(text);
	}

	public void LogFirebase(string name)
	{
		if (Singleton<FirebaseManager>.instance.IsFirebaseInitDone())
		{
			FirebaseAnalytics.LogEvent(name);
		}
	}

	public void LogRestartEvent(string restartPos, float value = 1f)
	{
		parameter.Clear();
		parameter["Restart Position"] = restartPos;
		LogAppEvent("Restart Click", value, parameter);
	}

	public void LogAdRequest(string adBrandName, float value = 1f)
	{
		parameter.Clear();
		parameter["Ad Brand"] = adBrandName;
		LogAppEvent("Ad Request", value, parameter);
	}

	public void LogIAPClickEvent(string iapPos, float value = 1f)
	{
		parameter.Clear();
		parameter["Position"] = iapPos;
		LogAppEvent("IAP Click", value, parameter);
	}

	public void LogShopEvent(string shopPos, float value = 1f)
	{
		parameter.Clear();
		parameter["Shop Position"] = shopPos;
		LogAppEvent("Shop Click", value, parameter);
	}

	public void LogBuyThemeEvent(ThemeName themeName, float value = 1f)
	{
		parameter.Clear();
		string name = "Buy Theme" + Enum.GetName(typeof(ThemeName), themeName);
		LogAppEvent(name, value);
	}

	public void LogUseThemeEvent(ThemeName themeName, float value = 1f)
	{
		parameter.Clear();
		string name = "Use Theme" + Enum.GetName(typeof(ThemeName), themeName);
		LogAppEvent(name, value);
	}

	public void LogRewardEvent(string rewardPos, float value = 1f)
	{
		parameter.Clear();
		parameter["Reward Position"] = rewardPos;
		LogAppEvent("Show Reward ads - diamond", value, parameter);
	}

	public void LogPlayEvent(bool isBomb, float value = 1f)
	{
		parameter.Clear();
		if (isBomb)
		{
			parameter["Mode"] = "Bomb Mode";
		}
		else
		{
			parameter["Mode"] = "Classic Mode";
		}
		LogAppEvent("Play Click", value, parameter);
	}

	public void LogShowInterEvent(string adBrandName, float value = 1f)
	{
		parameter.Clear();
		parameter["Ad Brand"] = adBrandName;
		LogAppEvent("Show Interstitial", value);
		LogAppEvent("fb_mobile_achievement_unlocked");
		Kochava.Event kochavaEvent = new Kochava.Event(Kochava.EventType.AdView);
		Kochava.Tracker.SendEvent(kochavaEvent);
		int @int = ServicesManager.DataNormal().GetInt("Inter50times", 0);
		@int++;
		UnityEngine.Debug.Log("Inter count:" + @int);
		if (@int == 50)
		{
			UnityEngine.Debug.Log("Log inter 50 times");
			LogAppEvent("fb_mobile_level_achieved");
			LogFirebase("Inter_50");
		}
		ServicesManager.DataNormal().SetInt("Inter50times", @int);
	}

	public void LogTimePlay(float value)
	{
		parameter.Clear();
		if (PlayerPrefs.HasKey("Mode"))
		{
			switch (PlayerPrefs.GetInt("Mode"))
			{
			case 0:
				parameter["Mode"] = "Three_Old_Ratio";
				break;
			case 1:
				parameter["Mode"] = "NoThree_Old_Ratio";
				break;
			case 2:
				parameter["Mode"] = "NoThree_New_Ratio";
				break;
			}
		}
		LogAppEvent("Time_Play", value, parameter);
	}

	public void LogContinueClick(float value = 1f)
	{
		LogAppEvent("Continue Click", value);
	}

	public void LogGameOver(float score)
	{
		LogAppEvent("Game Over", score);
	}

	public void LogClickVideoUnlock(ThemeName name)
	{
		string name2 = Enum.GetName(typeof(ThemeName), name);
		string name3 = "Watch Video Unlock " + name2;
		LogAppEvent(name3);
	}

	public void LogWatchVideoUnlock(ThemeName name)
	{
		string name2 = Enum.GetName(typeof(ThemeName), name);
		string name3 = "Watch Video Unlock " + name2;
		LogAppEvent(name3);
	}

	public void LogUnlockByVideo(ThemeName name)
	{
		string name2 = Enum.GetName(typeof(ThemeName), name);
		string text = "Unlock By Video:" + name2;
		UnityEngine.Debug.Log(text);
		LogAppEvent(text);
	}

	public void LogUnlockTheme50Plays()
	{
		LogAppEvent("Achievement 50 times play");
	}

	public void LogUnlockCombo5()
	{
		LogAppEvent("Achievement combo 5");
	}

	public void LogUnlock500Score()
	{
		LogAppEvent("Achievement highscore 500");
	}

	public void LogLBNative()
	{
		LogAppEvent("LB Native Click");
	}

	public void LogLBFacebook()
	{
		LogAppEvent("LB Facebook Click");
	}

	public void LogShowRewardGeneral()
	{
		LogAppEvent("Show Reward ads");
		LogAppEvent("fb_mobile_spent_credits");
		int @int = ServicesManager.DataNormal().GetInt("Reward15times", 0);
		@int++;
		UnityEngine.Debug.Log("Reward Count:" + @int);
		if (@int == 15)
		{
			UnityEngine.Debug.Log("Log Reward 15 times");
			LogAppEvent("fb_mobile_content_view");
			LogFirebase("Reward_15");
		}
		ServicesManager.DataNormal().SetInt("Reward15times", @int);
	}

	public void LogShowRewardTheme()
	{
		LogAppEvent("Show Reward ads - unlock theme");
	}

	public void LogShowRewardDouble()
	{
		LogAppEvent("Show Reward ads - double");
	}

	public void LogShowRewardSpin()
	{
		LogAppEvent("Show Reward ads - spin");
	}

	public void LogShowRewardDoubleSpin()
	{
		LogAppEvent("Show Reward ads - double spin");
	}

	public void LogShowAdsResume()
	{
		LogAppEvent("Show Ads Resume");
	}

	public void LogShowRewardContinue()
	{
		LogAppEvent("Show Reward ads - Continue");
	}

	public void LogShowSpin()
	{
		LogAppEvent("Show Spin");
	}

	public void LogClickNoThankSpin()
	{
		LogAppEvent("No Thanks Spin Click");
	}

	public void LogClickSpin()
	{
		LogAppEvent("Spin Click");
	}

	public void LogFirstDate(string dateFormated)
	{
		parameter.Clear();
		parameter["Date"] = dateFormated;
		LogAppEvent("First Date", 1f, parameter);
	}

	public void LogUseSkillNew3(int cost)
	{
		LogAppEvent("Use skill new shape", cost);
	}

	public void LogUseSkillUndo(int cost)
	{
		LogAppEvent("Use skill undo", cost);
	}

	public void LogAcceptGDPR()
	{
		LogAppEvent("Accept GDPR");
	}

	public void LogShowGDPR()
	{
		LogAppEvent("Show GDPR");
	}

	public void LogKochavaAttribution(string attributionData)
	{
		parameter.Clear();
		parameter["Data"] = attributionData;
		LogAppEvent("Kochava Attribution", 1f, parameter);
	}

	public void LogXmasEvent()
	{
		LogAppEvent("Event Xmas");
	}

	public void TestLogAchieveAd()
	{
		ServicesManager.DataNormal().SetInt("Reward15times", 14);
		ServicesManager.DataNormal().SetInt("Inter50times", 49);
	}

	public void LogMoveCount(int moveCount)
	{
		UnityEngine.Debug.Log("Move count:" + moveCount);
		LogAppEvent("Move_Count", moveCount);
	}
}
