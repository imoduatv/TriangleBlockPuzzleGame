using System;
using UnityEngine;
using UnityEngine.Events;

public class KochavaConfiguration : MonoBehaviour
{
	[Serializable]
	public class ConsentStatusChangeEvent : UnityEvent
	{
	}

	[Serializable]
	public class AttributionChangeEvent : UnityEvent<string>
	{
	}

	public string appGUID_UnityEditor = string.Empty;

	public string appGUID_iOS = string.Empty;

	public string appGUID_Android = string.Empty;

	public string appGUID_WindowsPC = string.Empty;

	public string appGUID_MacOS = string.Empty;

	public string appGUID_Linux = string.Empty;

	public string appGUID_WebGL = string.Empty;

	public string kochavaPartnerName = string.Empty;

	public bool appAdLimitTracking;

	public Kochava.DebugLogLevel logLevel;

	public bool requestAttributionCallback;

	public AttributionChangeEvent attributionChangeEvent;

	public bool intelligentConsentManagement;

	public ConsentStatusChangeEvent consentStatusChangeEvent;

	private void Awake()
	{
		string appGuid = appGUID_UnityEditor;
		if (appGUID_Android.Length > 0)
		{
			appGuid = appGUID_Android;
		}
		Kochava.Tracker.Config.SetLogLevel(logLevel);
		Kochava.Tracker.Config.SetAppGuid(appGuid);
		Kochava.Tracker.Config.SetRetrieveAttribution(requestAttributionCallback);
		Kochava.Tracker.Config.SetAppLimitAdTracking(appAdLimitTracking);
		Kochava.Tracker.Config.SetIntelligentConsentManagement(intelligentConsentManagement);
		Kochava.Tracker.Config.SetPartnerName(kochavaPartnerName);
		Kochava.Tracker.Initialize();
		if (requestAttributionCallback && attributionChangeEvent != null)
		{
			Kochava.Tracker.SetAttributionHandler(delegate(string attribution)
			{
				attributionChangeEvent.Invoke(attribution);
			});
		}
		if (intelligentConsentManagement && consentStatusChangeEvent != null)
		{
			Kochava.Tracker.SetConsentStatusChangeHandler(delegate
			{
				consentStatusChangeEvent.Invoke();
			});
		}
	}
}
