using System;
using System.Collections.Generic;
using UnityEngine;

namespace Root
{
	public class UMInAppPurchaseModule : IInAppPurchaseService
	{
		private IDataService dataService;

		private Dictionary<string, Action<bool>> listCallBacks;

		public UMInAppPurchaseModule(IDataService dataService)
		{
			this.dataService = dataService;
		}

		private Dictionary<string, Action<bool>> GetListCallBacks()
		{
			if (listCallBacks == null)
			{
				listCallBacks = new Dictionary<string, Action<bool>>();
			}
			return listCallBacks;
		}

		public void ConnectInit()
		{
			if (!UM_InAppPurchaseManager.Client.IsConnected)
			{
				UnityEngine.Debug.Log("Init IAP");
				UM_InAppPurchaseManager.Client.OnServiceConnected += OnBillingConnectFinishedAction;
				UM_InAppPurchaseManager.Client.Connect();
			}
		}

		public bool IsProductPurchased(string productID)
		{
			if (!UM_InAppPurchaseManager.Client.IsConnected)
			{
				return dataService.GetBool(productID, defaultValue: false);
			}
			return UM_InAppPurchaseManager.Client.IsProductPurchased(productID);
		}

		public void PurchaseProduct(string productID, Action<bool> callBack)
		{
			if (!UM_InAppPurchaseManager.Client.IsConnected)
			{
				UnityEngine.Debug.Log("IAP not init");
				return;
			}
			if (UM_InAppPurchaseManager.Client.IsProductPurchased(productID))
			{
				dataService.SetBool(productID, value: true);
				callBack(obj: true);
				ShowPopup("Purchases Restored", "Your previously purchased products have been restored.");
				return;
			}
			listCallBacks = GetListCallBacks();
			if (listCallBacks.ContainsKey(productID))
			{
				listCallBacks[productID] = callBack;
			}
			else
			{
				listCallBacks.Add(productID, callBack);
			}
			ShowPurchasingPreloader();
			UM_InAppPurchaseManager.Client.OnPurchaseFinished += OnPurchaseFlowFinishedAction;
			UM_InAppPurchaseManager.Client.Purchase(productID);
		}

		public void RestorePurchase(Dictionary<string, Action<bool>> callbacks)
		{
			listCallBacks = callbacks;
			UnityEngine.Debug.Log("RestoreButtonOnClick");
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				UM_InAppPurchaseManager.Client.RestorePurchases();
				foreach (string key in listCallBacks.Keys)
				{
					PurchaseProduct(key, listCallBacks[key]);
				}
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				CheckRestoreAndroid();
			}
		}

		private void CheckRestoreAndroid()
		{
			if (UM_InAppPurchaseManager.Client.IsConnected && Application.platform == RuntimePlatform.Android)
			{
				listCallBacks = GetListCallBacks();
				bool flag = false;
				foreach (string key in listCallBacks.Keys)
				{
					if (UM_InAppPurchaseManager.Client.IsProductPurchased(key))
					{
						UnityEngine.Debug.Log(key + " IsProductPurchased");
						dataService.SetBool(key, value: true);
						flag = true;
						listCallBacks[key](obj: true);
					}
					else
					{
						UnityEngine.Debug.Log(key + " Is Not Purchased");
						dataService.SetBool(key, value: false);
						listCallBacks[key](obj: false);
					}
				}
				listCallBacks.Clear();
				if (flag)
				{
					ShowPopup("Purchases Restored", "Your previously purchased products have been restored.");
				}
				else
				{
					ShowPopup("Failed", "There are no items available to restore at this time.");
				}
			}
			else
			{
				ShowPopup("Connection Error!", "Please check your internet connection and try again.");
			}
		}

		private void OnBillingConnectFinishedAction(UM_BillingConnectionResult result)
		{
			UM_InAppPurchaseManager.Client.OnServiceConnected -= OnBillingConnectFinishedAction;
			if (result.isSuccess)
			{
				UnityEngine.Debug.Log("Connected UM_InAppPurchaseManager");
			}
			else
			{
				UnityEngine.Debug.Log("Failed to connect UM_InAppPurchaseManager");
			}
		}

		private void OnPurchaseFlowFinishedAction(UM_PurchaseResult result)
		{
			if (result.isSuccess)
			{
				UnityEngine.Debug.Log("Product " + result.product.id + " purchase Success");
				ShowPopup("Thank you!", "Product " + result.product.id + " purchase Success");
				string id = result.product.id;
				dataService.SetBool(id, value: true);
				listCallBacks = GetListCallBacks();
				if (listCallBacks.ContainsKey(id))
				{
					listCallBacks[id](obj: true);
					listCallBacks.Remove(id);
				}
			}
			else
			{
				UnityEngine.Debug.Log("Product " + result.product.id + " purchase Failed");
				ShowPopup("Fail!", "Product " + result.product.id + " purchase Failed");
				string id2 = result.product.id;
				dataService.SetBool(id2, value: false);
				listCallBacks = GetListCallBacks();
				if (listCallBacks.ContainsKey(id2))
				{
					listCallBacks[id2](obj: false);
					listCallBacks.Remove(id2);
				}
			}
			HidePurchasingPreloader();
			UM_InAppPurchaseManager.Client.OnPurchaseFinished -= OnPurchaseFlowFinishedAction;
		}

		private void ShowPurchasingPreloader()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				MNP.ShowPreloader("Purchasing...", string.Empty);
			}
		}

		private void HidePurchasingPreloader()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				MNP.HidePreloader();
			}
		}

		private void ShowPopup(string title, string message)
		{
			MNPopup mNPopup = new MNPopup(title, message);
			mNPopup.AddAction("OK", null);
			mNPopup.Show();
		}

		private void OnRestoreFinished(UM_BaseResult result)
		{
			if (!result.IsSucceeded)
			{
			}
		}

		public string GetLocalizedPrice(string id)
		{
			if (UM_InAppPurchaseManager.GetProductById(id) == null)
			{
				UnityEngine.Debug.LogError("ID isn't correct or product isn't in UM_Setting");
				return string.Empty;
			}
			return UM_InAppPurchaseManager.GetProductById(id).LocalizedPrice;
		}
	}
}
