using System.Collections.Generic;
using UnityEngine;

public class AMN_PlayerData : AMN_Singleton<AMN_PlayerData>
{
	private const string ENTITLEMENTS = "ENTITLEMENTS";

	public const string DATA_SPLITTER = "|";

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void AddNewSKU(string SKU)
	{
		string @string;
		if (PlayerPrefs.HasKey("ENTITLEMENTS"))
		{
			@string = PlayerPrefs.GetString("ENTITLEMENTS");
			@string = @string + SKU + "|";
		}
		else
		{
			@string = SKU + "|";
		}
		PlayerPrefs.SetString("ENTITLEMENTS", @string);
	}

	public static List<string> GetAvailableSKUs()
	{
		List<string> list = new List<string>();
		if (PlayerPrefs.HasKey("ENTITLEMENTS"))
		{
			string @string = PlayerPrefs.GetString("ENTITLEMENTS");
			string[] array = @string.Split("|"[0]);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
		}
		return list;
	}
}
