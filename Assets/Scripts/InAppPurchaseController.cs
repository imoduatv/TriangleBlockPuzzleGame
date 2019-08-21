using System;
using System.Collections.Generic;
using UnityEngine;

public class InAppPurchaseController : MonoBehaviour
{
	private bool isInit;

	private static InAppPurchaseController _instance;

	private static string IAP_GO_PRO = "Upgrade_Pro_VRxRacer";

	private static string IAP_NO_ADS2 = "no_ads_triangle_gridfit";

	private static string IAP_MULTI_COLOR = "Multi_Color_VRxRacer2";

	private static string IAP_PREMIUM = "premium_trigon";

	private static string IAP_DIAMOND_PACK_1k = "diamond_pack_1k";

	private static string IAP_DIAMOND_PACK_5k = "diamond_pack_5k";

	private string ErrorInternet = "Please check your internet connection and try again!";

	private bool isInited;

	public static InAppPurchaseController Instance()
	{
		if (_instance == null)
		{
			_instance = UnityEngine.Object.FindObjectOfType<InAppPurchaseController>();
			if (_instance == null)
			{
				_instance = new GameObject("InAppPurchaseController").AddComponent<InAppPurchaseController>();
			}
			UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
			if (!_instance.isInit)
			{
				_instance.isInit = true;
				_instance.Init();
			}
		}
		return _instance;
	}

	private void Init()
	{
	}

	public void BuyPremium()
	{
		ServicesManager.IAP().PurchaseProduct(IAP_PREMIUM, delegate(bool result)
		{
			if (result)
			{
				Singleton<GameManager>.instance.SetNoAds();
				Singleton<GameManager>.instance.SetPremium();
				Singleton<UIManager>.instance.SetNoAdsUI();
				Singleton<UIManager>.instance.SetPremiumUI();
				if (Singleton<UIManager>.instance.IsNoAdsPanelShow())
				{
					Singleton<UIManager>.instance.HideNoAdsMenu();
				}
			}
		});
	}

	public void BuyNoAds2ButtonClick()
	{
		ServicesManager.IAP().PurchaseProduct(IAP_NO_ADS2, delegate(bool result)
		{
			if (result)
			{
				Singleton<GameManager>.Instance.SetNoAds();
				Singleton<UIManager>.instance.SetNoAdsUI();
			}
		});
	}

	public void Buy1kDiamonds()
	{
		ServicesManager.IAP().PurchaseProduct(IAP_DIAMOND_PACK_1k, delegate(bool result)
		{
			if (result)
			{
				GameData.Instance().AddMoney(1000);
				Singleton<UIManager>.instance.ReloadCostSkill();
			}
		});
	}

	public void Buy5kDiamonds()
	{
		ServicesManager.IAP().PurchaseProduct(IAP_DIAMOND_PACK_5k, delegate(bool result)
		{
			if (result)
			{
				GameData.Instance().AddMoney(5000);
				Singleton<UIManager>.instance.ReloadCostSkill();
			}
		});
	}

	public void RestoreButtonOnClick()
	{
		UnityEngine.Debug.Log("RestoreButtonOnClick");
		Dictionary<string, Action<bool>> dictionary = new Dictionary<string, Action<bool>>();
		dictionary[IAP_NO_ADS2] = delegate(bool result)
		{
			if (result)
			{
				Singleton<GameManager>.Instance.SetNoAds();
				Singleton<UIManager>.instance.SetNoAdsUI();
			}
		};
		dictionary[IAP_PREMIUM] = delegate(bool result)
		{
			if (result)
			{
				Singleton<GameManager>.instance.SetNoAds();
				Singleton<GameManager>.instance.SetPremium();
				Singleton<UIManager>.instance.SetNoAdsUI();
				if (Singleton<UIManager>.instance.IsNoAdsPanelShow())
				{
					Singleton<UIManager>.instance.HideNoAdsMenu();
				}
			}
		};
		ServicesManager.IAP().RestorePurchase(dictionary);
	}

	public void ActiveNoAds()
	{
		GameData.Instance().SetIsAds(value: false);
		GameData.Instance().SaveData();
	}

	public string GetLocalizedNoAdsIAP()
	{
		return ServicesManager.IAP().GetLocalizedPrice(IAP_NO_ADS2);
	}

	public string GetLocalizedPremiumIAP()
	{
		return ServicesManager.IAP().GetLocalizedPrice(IAP_PREMIUM);
	}

	public string GetLocalizedPack1k()
	{
		return ServicesManager.IAP().GetLocalizedPrice(IAP_DIAMOND_PACK_1k);
	}

	public string GetLocalizedPack5k()
	{
		return ServicesManager.IAP().GetLocalizedPrice(IAP_DIAMOND_PACK_5k);
	}
}
