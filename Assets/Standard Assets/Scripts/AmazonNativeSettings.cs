using System.Collections.Generic;
using UnityEngine;

public class AmazonNativeSettings : ScriptableObject
{
	public const string VERSION_NUMBER = "2.8/24";

	public int ToolbarIndex;

	public bool ShowActions = true;

	public bool ShowStoreParams;

	public bool IsGameCircleEnabled;

	public bool IsBillingEnabled;

	public bool IsAdvertisingEnabled;

	public List<AmazonProductTemplate> InAppProducts = new List<AmazonProductTemplate>();

	public string AppAPIKey = string.Empty;

	public bool IsTestMode = true;

	public AMN_BannerAlign AdvertisingBannerAlign = AMN_BannerAlign.Bottom;

	public bool ShowLeaderboards = true;

	public List<GC_Leaderboard> Leaderboards = new List<GC_Leaderboard>();

	public bool ShowAchievementsParams = true;

	public List<GC_Achievement> Achievements = new List<GC_Achievement>();

	public string AmazonDeveloperConsoleLink = "https://goo.gl/EKAKSJ";

	public string GameCircleDownloadLink = "https://goo.gl/mMvNEM";

	public string BillingDownloadLink = "https://goo.gl/NyLUJG";

	public string AdvertisingDownloadLink = "https://goo.gl/3ZKs5t";

	private const string AMNSettingsAssetName = "AmazonNativeSettings";

	private const string AMNSettingsPath = "Plugins/StansAssets/Support/Settings/Resources/";

	private const string AMNSettingsAssetExtension = ".asset";

	private static AmazonNativeSettings instance;

	public static AmazonNativeSettings Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (Resources.Load("AmazonNativeSettings") as AmazonNativeSettings);
				if (instance == null)
				{
					instance = ScriptableObject.CreateInstance<AmazonNativeSettings>();
				}
			}
			return instance;
		}
	}
}
