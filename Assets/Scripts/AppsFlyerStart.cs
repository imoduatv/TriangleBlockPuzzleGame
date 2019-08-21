using UnityEngine;

public class AppsFlyerStart : MonoBehaviour
{
	public string DEV_KEY = "am2vzS5aCkTtE7TwGKmDxF";

	public string ANDROID_PACKAGE_NAME = "com.DTA.sudoku";

	public string IOS_APP_ID = "1144877573";

	private void Start()
	{
		AppsFlyer.init(DEV_KEY);
		AppsFlyer.setAppID(ANDROID_PACKAGE_NAME);
		AppsFlyer.loadConversionData("AppsFlyerTrackerCallbacks", "didReceiveConversionData", "didReceiveConversionDataWithError");
	}
}
