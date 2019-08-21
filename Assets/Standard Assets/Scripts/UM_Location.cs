using SA.Common.Pattern;
using System;
using System.Threading;
using UnityEngine;

public class UM_Location : Singleton<UM_Location>
{
	public static event Action<UM_LocaleInfo> OnLocaleLoaded;

	public void GetLocale()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			IOSNativeUtility.OnLocaleLoaded += HandleOnLocaleLoaded_IOS;
			Singleton<IOSNativeUtility>.Instance.GetLocale();
			break;
		case RuntimePlatform.Android:
			AndroidNativeUtility.LocaleInfoLoaded += HandleLocaleInfoLoaded_Android;
			Singleton<AndroidNativeUtility>.Instance.LoadLocaleInfo();
			break;
		}
	}

	private void HandleLocaleInfoLoaded_Android(AN_Locale locale)
	{
		AndroidNativeUtility.LocaleInfoLoaded -= HandleLocaleInfoLoaded_Android;
		UM_Location.OnLocaleLoaded(new UM_LocaleInfo(locale));
	}

	private void HandleOnLocaleLoaded_IOS(ISN_Locale locale)
	{
		IOSNativeUtility.OnLocaleLoaded -= HandleOnLocaleLoaded_IOS;
		UM_Location.OnLocaleLoaded(new UM_LocaleInfo(locale));
	}

	static UM_Location()
	{
		UM_Location.OnLocaleLoaded = delegate
		{
		};
	}
}
