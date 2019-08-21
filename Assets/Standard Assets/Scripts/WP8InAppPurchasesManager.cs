using System;
using System.Collections.Generic;
using System.Threading;

public class WP8InAppPurchasesManager
{
	private static WP8InAppPurchasesManager _instance = null;

	private bool _IsInitialized;

	public const string SPLITTER1 = "|";

	public const string SPLITTER2 = "|%|";

	public const int RESULT_OK = 0;

	public const int RESULT_ERROR = 1;

	private List<WP8ProductTemplate> _products = new List<WP8ProductTemplate>();

	[Obsolete("This property is obsolete. Use 'Instance' property instead")]
	public static WP8InAppPurchasesManager instance => Instance;

	public static WP8InAppPurchasesManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new WP8InAppPurchasesManager();
			}
			return _instance;
		}
	}

	[Obsolete("This property is obsolete. Use 'Products' property instead")]
	public List<WP8ProductTemplate> products => Products;

	public List<WP8ProductTemplate> Products => _products;

	public bool IsInitialized => _IsInitialized;

	public static event Action<WP8_InAppsInitResult> OnInitComplete;

	public static event Action<WP8PurchseResponce> OnPurchaseFinished;

	public static event Action<WP8ConsumeResponse> OnConsumeFinished;

	[Obsolete("This method is obsolete. Use 'Init' method instead")]
	public void init()
	{
		Init();
	}

	public void Init()
	{
	}

	[Obsolete("This method is obsolete. Use 'Purchase' method instead")]
	public void purchase(string productId)
	{
		Purchase(productId);
	}

	public void Purchase(string productID)
	{
	}

	public WP8ProductTemplate GetProductById(string id)
	{
		foreach (WP8ProductTemplate product in _products)
		{
			if (product.ProductId.Equals(id))
			{
				return product;
			}
		}
		return null;
	}

	private void InitCallback(string res)
	{
		string[] array = res.Split(new string[1]
		{
			"|%|"
		}, StringSplitOptions.None);
		int statusCode = int.Parse(array[0]);
		WP8_InAppsInitResult wP8_InAppsInitResult = new WP8_InAppsInitResult(statusCode);
		if (wP8_InAppsInitResult.IsSucceeded)
		{
			for (int i = 1; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new string[1]
				{
					"|"
				}, StringSplitOptions.None);
				WP8ProductTemplate wP8ProductTemplate = new WP8ProductTemplate();
				wP8ProductTemplate.ImgURI = array2[0];
				wP8ProductTemplate.Name = array2[1];
				wP8ProductTemplate.ProductId = array2[2];
				wP8ProductTemplate.Price = array2[3];
				wP8ProductTemplate.Type = (WP8PurchaseProductType)Enum.Parse(typeof(WP8PurchaseProductType), array2[4]);
				wP8ProductTemplate.Description = array2[5];
				wP8ProductTemplate.IsPurchased = bool.Parse(array2[6]);
				wP8ProductTemplate.TransactionID = array2[7];
				_products.Add(wP8ProductTemplate);
			}
			_IsInitialized = true;
		}
		WP8InAppPurchasesManager.OnInitComplete(wP8_InAppsInitResult);
	}

	private void PurchaseCallback(string result)
	{
		string[] array = result.Split(new string[1]
		{
			"|"
		}, StringSplitOptions.None);
		int num = int.Parse(array[0]);
		WP8PurchseResponce wP8PurchseResponce = null;
		if (num == 0)
		{
			wP8PurchseResponce = new WP8PurchseResponce((WP8PurchaseCodes)Enum.Parse(typeof(WP8PurchaseCodes), array[1]), array[2]);
			wP8PurchseResponce.TransactionId = array[3];
		}
		else
		{
			wP8PurchseResponce = new WP8PurchseResponce((WP8PurchaseCodes)Enum.Parse(typeof(WP8PurchaseCodes), array[2]), array[1]);
		}
		WP8InAppPurchasesManager.OnPurchaseFinished(wP8PurchseResponce);
	}

	private void ConsumeCallback(string result)
	{
		string[] array = result.Split(new string[1]
		{
			"|"
		}, StringSplitOptions.None);
		int num = int.Parse(array[0]);
		WP8ConsumeResponse wP8ConsumeResponse = null;
		if (num == 0)
		{
			wP8ConsumeResponse = new WP8ConsumeResponse((WP8ConsumeCodes)Enum.Parse(typeof(WP8ConsumeCodes), array[1]), array[2]);
			wP8ConsumeResponse.TransactionId = array[3];
		}
		else
		{
			wP8ConsumeResponse = new WP8ConsumeResponse((WP8ConsumeCodes)Enum.Parse(typeof(WP8ConsumeCodes), array[2]), array[1]);
		}
		WP8InAppPurchasesManager.OnConsumeFinished(wP8ConsumeResponse);
	}

	private void DefferedPurchaseCallback(string result)
	{
		string[] array = result.Split(new string[1]
		{
			"|"
		}, StringSplitOptions.None);
		int num = int.Parse(array[0]);
		WP8PurchseResponce wP8PurchseResponce = null;
		if (num == 0)
		{
			wP8PurchseResponce = new WP8PurchseResponce((WP8PurchaseCodes)Enum.Parse(typeof(WP8PurchaseCodes), array[1]), array[2]);
			wP8PurchseResponce.TransactionId = array[3];
		}
		else
		{
			wP8PurchseResponce = new WP8PurchseResponce((WP8PurchaseCodes)Enum.Parse(typeof(WP8PurchaseCodes), array[2]), array[1]);
		}
	}

	static WP8InAppPurchasesManager()
	{
		WP8InAppPurchasesManager.OnInitComplete = delegate
		{
		};
		WP8InAppPurchasesManager.OnPurchaseFinished = delegate
		{
		};
		WP8InAppPurchasesManager.OnConsumeFinished = delegate
		{
		};
	}
}
