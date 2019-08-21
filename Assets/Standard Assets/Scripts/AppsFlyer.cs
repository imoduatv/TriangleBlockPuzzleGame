using System;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyer : MonoBehaviour
{
	private static AndroidJavaClass obj = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");

	private static AndroidJavaObject cls_AppsFlyer = obj.CallStatic<AndroidJavaObject>("getInstance", new object[0]);

	private static AndroidJavaClass cls_AppsFlyerHelper = new AndroidJavaClass("com.appsflyer.AppsFlyerUnityHelper");

	private static string devKey;

	public static void trackEvent(string eventName, string eventValue)
	{
		MonoBehaviour.print("AF.cs this is deprecated method. please use trackRichEvent instead. nothing is sent.");
	}

	public static void setCurrencyCode(string currencyCode)
	{
		//cls_AppsFlyer.Call("setCurrencyCode", currencyCode);
	}

	public static void setCustomerUserID(string customerUserID)
	{
	}

	public static void loadConversionData(string callbackObject, string callbackMethod, string callbackFailedMethod)
	{
	}

	public static void setCollectIMEI(bool shouldCollect)
	{
	}

	public static void setCollectAndroidID(bool shouldCollect)
	{
	}

	public static void init(string key)
	{
		//MonoBehaviour.print("AF.cs init");
		//devKey = key;
		//using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		//{
		//	using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
		//	{
		//		androidJavaObject.Call("runOnUiThread", new AndroidJavaRunnable(init_cb));
		//	}
		//}
	}

	private static void init_cb()
	{
		//MonoBehaviour.print("AF.cs start tracking");
		//trackAppLaunch();
		//using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		//{
		//	using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
		//	{
		//		AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getApplication", new object[0]);
		//		cls_AppsFlyer.Call("startTracking", androidJavaObject2, devKey);
		//	}
		//}
	}

	public static void setAppsFlyerKey(string key)
	{
		//MonoBehaviour.print("AF.cs setAppsFlyerKey");
		//init(key);
	}

	public static void trackAppLaunch()
	{
	}

	public static void setAppID(string packageName)
	{
	}

	public static void createValidateInAppListener(string aObject, string callbackMethod, string callbackFailedMethod)
	{
	}

	public static void validateReceipt(string publicKey, string purchaseData, string signature, string price, string currency, Dictionary<string, string> extraParams)
	{
	}

	public static void trackRichEvent(string eventName, Dictionary<string, string> eventValues)
	{
	}


	public static void setImeiData(string imeiData)
	{
	}

	public static void setAndroidIdData(string androidIdData)
	{
	}

	public static void setIsDebug(bool isDebug)
	{
	}

	public static void setIsSandbox(bool isSandbox)
	{
	}

	public static void getConversionData()
	{
	}

	public static void handleOpenUrl(string url, string sourceApplication, string annotation)
	{
	}

	public static string getAppsFlyerId()
	{
        return string.Empty;
	}

	public static void setGCMProjectNumber(string googleGCMNumber)
	{
	}
}
