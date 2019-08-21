using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GPaymnetManagerExample : MonoBehaviour
{
	private static bool _isInited;

	public const string ANDROID_TEST_PURCHASED = "android.test.Purchased";

	public const string ANDROID_TEST_CANCELED = "android.test.canceled";

	public const string ANDROID_TEST_REFUNDED = "android.test.refunded";

	public const string ANDROID_TEST_ITEM_UNAVAILABLE = "android.test.item_unavailable";

	[CompilerGenerated]
	private static Action<BillingResult> _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static Action<BillingResult> _003C_003Ef__mg_0024cache1;

	[CompilerGenerated]
	private static Action<BillingResult> _003C_003Ef__mg_0024cache2;

	[CompilerGenerated]
	private static Action<BillingResult> _003C_003Ef__mg_0024cache3;

	[CompilerGenerated]
	private static Action<BillingResult> _003C_003Ef__mg_0024cache4;

	[CompilerGenerated]
	private static Action<BillingResult> _003C_003Ef__mg_0024cache5;

	public static bool isInited => _isInited;

	public static void init()
	{
		AndroidInAppPurchaseManager.Client.AddProduct("android.test.Purchased");
		AndroidInAppPurchaseManager.Client.AddProduct("android.test.canceled");
		AndroidInAppPurchaseManager.Client.AddProduct("android.test.refunded");
		AndroidInAppPurchaseManager.Client.AddProduct("android.test.item_unavailable");
		string sKU = "my.prod.id";
		AndroidInAppPurchaseManager.Client.AddProduct(sKU);
		GoogleProductTemplate googleProductTemplate = new GoogleProductTemplate();
		googleProductTemplate.SKU = sKU;
		AndroidInAppPurchaseManager.Client.AddProduct(googleProductTemplate);
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
		AndroidInAppPurchaseManager.ActionProductConsumed += OnProductConsumed;
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		AndroidInAppPurchaseManager.Client.Connect();
	}

	public static void Purchase(string SKU)
	{
		AndroidInAppPurchaseManager.Client.Purchase(SKU);
	}

	public static void consume(string SKU)
	{
		AndroidInAppPurchaseManager.Client.Consume(SKU);
	}

	private static void OnProcessingPurchasedProduct(GooglePurchaseTemplate Purchase)
	{
	}

	private static void OnProcessingConsumeProduct(GooglePurchaseTemplate Purchase)
	{
	}

	private static void OnProductPurchased(BillingResult result)
	{
		if (result.IsSuccess)
		{
			AndroidMessage.Create("Product Purchased", result.Purchase.SKU + "\n Full Response: " + result.Purchase.OriginalJson);
			OnProcessingPurchasedProduct(result.Purchase);
		}
		else
		{
			AndroidMessage.Create("Product Purchase Failed", result.Response.ToString() + " " + result.Message);
		}
		UnityEngine.Debug.Log("Purchased Responce: " + result.Response.ToString() + " " + result.Message);
	}

	private static void OnProductConsumed(BillingResult result)
	{
		if (result.IsSuccess)
		{
			AndroidMessage.Create("Product Consumed", result.Purchase.SKU + "\n Full Response: " + result.Purchase.OriginalJson);
			OnProcessingConsumeProduct(result.Purchase);
		}
		else
		{
			AndroidMessage.Create("Product Cousume Failed", result.Response.ToString() + " " + result.Message);
		}
		UnityEngine.Debug.Log("Cousume Responce: " + result.Response.ToString() + " " + result.Message);
	}

	private static void OnBillingConnected(BillingResult result)
	{
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
		if (result.IsSuccess)
		{
			AndroidInAppPurchaseManager.Client.RetrieveProducDetails();
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
		}
		AndroidMessage.Create("Connection Responce", result.Response.ToString() + " " + result.Message);
		UnityEngine.Debug.Log("Connection Responce: " + result.Response.ToString() + " " + result.Message);
	}

	private static void OnRetrieveProductsFinised(BillingResult result)
	{
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
		if (result.IsSuccess)
		{
			_isInited = true;
			AndroidMessage.Create("Success", "Billing init complete inventory contains: " + AndroidInAppPurchaseManager.Client.Inventory.Purchases.Count + " products");
			foreach (GoogleProductTemplate product in AndroidInAppPurchaseManager.Client.Inventory.Products)
			{
				UnityEngine.Debug.Log(product.Title);
				UnityEngine.Debug.Log(product.OriginalJson);
			}
		}
		else
		{
			AndroidMessage.Create("Connection Response", result.Response.ToString() + " " + result.Message);
		}
		UnityEngine.Debug.Log("Connection Response: " + result.Response.ToString() + " " + result.Message);
	}
}
