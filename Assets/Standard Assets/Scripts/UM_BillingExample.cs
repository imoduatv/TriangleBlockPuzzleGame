using UnityEngine;

public class UM_BillingExample : BaseIOSFeaturePreview
{
	public const string CONSUMABLE_PRODUCT_ID = "coins_bonus";

	public const string NON_CONSUMABLE_PRODUCT_ID = "coins_pack";

	private void Awake()
	{
		UM_ExampleStatusBar.text = "Unified billing exmple scene loaded";
		UM_InAppPurchaseManager.Client.OnPurchaseFinished += OnPurchaseFlowFinishedAction;
		UM_InAppPurchaseManager.Client.OnServiceConnected += OnConnectFinished;
	}

	private void OnGUI()
	{
		UpdateToStartPos();
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40f), "In-App Purchases", style);
		StartY += YLableStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Init"))
		{
			UM_InAppPurchaseManager.Client.OnServiceConnected += OnBillingConnectFinishedAction;
			UM_InAppPurchaseManager.Client.Connect();
			UM_ExampleStatusBar.text = "Initializing billing...";
		}
		if (UM_InAppPurchaseManager.Client.IsConnected)
		{
			GUI.enabled = true;
		}
		else
		{
			GUI.enabled = false;
		}
		StartX = XStartPos;
		StartY += YButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Buy Consumable Item"))
		{
			UM_InAppPurchaseManager.Client.Purchase("coins_bonus");
			UM_ExampleStatusBar.text = "Start purchsing coins_bonus product";
		}
		StartX += XButtonStep;
		bool enabled = GUI.enabled;
		string empty = string.Empty;
		if (UM_InAppPurchaseManager.Client.IsProductPurchased("coins_pack"))
		{
			empty = "Already purchased";
			GUI.enabled = false;
		}
		else
		{
			empty = "Not yet purchased";
		}
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Buy Non-Consumable Item \n" + empty))
		{
			UM_ExampleStatusBar.text = "Start purchsing coins_pack product";
			UM_InAppPurchaseManager.Client.Purchase("coins_pack");
		}
		GUI.enabled = enabled;
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Restore Purshases \n For IOS Only"))
		{
			UM_InAppPurchaseManager.Client.RestorePurchases();
		}
	}

	private void OnConnectFinished(UM_BillingConnectionResult result)
	{
		if (result.isSuccess)
		{
			UM_ExampleStatusBar.text = "Billing init Success";
		}
		else
		{
			UM_ExampleStatusBar.text = "Billing init Failed";
		}
	}

	private void OnPurchaseFlowFinishedAction(UM_PurchaseResult result)
	{
		UM_InAppPurchaseManager.Client.OnPurchaseFinished -= OnPurchaseFlowFinishedAction;
		if (result.isSuccess)
		{
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Success";
		}
		else
		{
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Failed";
		}
	}

	private void OnBillingConnectFinishedAction(UM_BillingConnectionResult result)
	{
		UM_InAppPurchaseManager.Client.OnServiceConnected -= OnBillingConnectFinishedAction;
		if (result.isSuccess)
		{
			UnityEngine.Debug.Log("Connected");
		}
		else
		{
			UnityEngine.Debug.Log("Failed to connect");
		}
	}
}
