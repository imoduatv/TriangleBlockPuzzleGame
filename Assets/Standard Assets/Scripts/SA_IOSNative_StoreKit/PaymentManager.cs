using SA.Common.Models;
using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SA.IOSNative.StoreKit
{
	public class PaymentManager : Singleton<PaymentManager>
	{
		public const string APPLE_VERIFICATION_SERVER = "https://buy.itunes.apple.com/verifyReceipt";

		public const string SANDBOX_VERIFICATION_SERVER = "https://sandbox.itunes.apple.com/verifyReceipt";

		private bool _IsStoreLoaded;

		private bool _IsWaitingLoadResult;

		private static int _nextId;

		private Dictionary<int, StoreProductView> _productsView = new Dictionary<int, StoreProductView>();

		private static string lastPurchasedProduct;

		public List<Product> Products => IOSNativeSettings.Instance.InAppProducts;

		public bool IsStoreLoaded => _IsStoreLoaded;

		public bool IsInAppPurchasesEnabled => BillingNativeBridge.ISN_InAppSettingState();

		public bool IsWaitingLoadResult => _IsWaitingLoadResult;

		private static int NextId
		{
			get
			{
				_nextId++;
				return _nextId;
			}
		}

		public static event Action<Result> OnStoreKitInitComplete;

		public static event Action OnRestoreStarted;

		public static event Action<RestoreResult> OnRestoreComplete;

		public static event Action<string> OnTransactionStarted;

		public static event Action<PurchaseResult> OnTransactionComplete;

		public static event Action<string> OnProductPurchasedExternally;

		public static event Action<VerificationResponse> OnVerificationComplete;

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public void LoadStore(bool forceLoad = false)
		{
			if (_IsStoreLoaded && !forceLoad)
			{
				Invoke("FireSuccessInitEvent", 1f);
			}
			else
			{
				if (_IsWaitingLoadResult)
				{
					return;
				}
				_IsWaitingLoadResult = true;
				string text = string.Empty;
				int count = Products.Count;
				for (int i = 0; i < count; i++)
				{
					if (i != 0)
					{
						text += ",";
					}
					text += Products[i].Id;
				}
				ISN_SoomlaGrow.Init();
				if (!Application.isEditor)
				{
					BillingNativeBridge.LoadStore(text);
					if (IOSNativeSettings.Instance.TransactionsHandlingMode == TransactionsHandlingMode.Manual)
					{
						BillingNativeBridge.EnableManulaTransactionsMode();
					}
					if (!IOSNativeSettings.Instance.PromotedPurchaseSupport)
					{
						BillingNativeBridge.DisablePromotedPurchases();
					}
				}
				else if (IOSNativeSettings.Instance.InAppsEditorTesting)
				{
					Invoke("EditorFakeInitEvent", 1f);
				}
			}
		}

		public void BuyProduct(string productId)
		{
			if (!Application.isEditor)
			{
				PaymentManager.OnTransactionStarted(productId);
				if (!_IsStoreLoaded)
				{
					ISN_Logger.Log("buyProduct shouldn't be called before StoreKit is initialized");
					Error error = new Error(4, "StoreKit not yet initialized");
					SendTransactionFailEvent(productId, error);
				}
				else
				{
					BillingNativeBridge.BuyProduct(productId);
				}
			}
			else if (IOSNativeSettings.Instance.InAppsEditorTesting)
			{
				FireProductBoughtEvent(productId, string.Empty, string.Empty, string.Empty, IsRestored: false);
			}
		}

		public void FinishTransaction(string productId)
		{
			BillingNativeBridge.FinishTransaction(productId);
		}

		public void AddProductId(string productId)
		{
			Product product = new Product();
			product.Id = productId;
			AddProduct(product);
		}

		public void AddProduct(Product product)
		{
			bool flag = false;
			int index = 0;
			foreach (Product product2 in Products)
			{
				if (product2.Id.Equals(product.Id))
				{
					flag = true;
					index = Products.IndexOf(product2);
					break;
				}
			}
			if (flag)
			{
				Products[index] = product;
			}
			else
			{
				Products.Add(product);
			}
		}

		public Product GetProductById(string prodcutId)
		{
			foreach (Product product2 in Products)
			{
				if (product2.Id.Equals(prodcutId))
				{
					return product2;
				}
			}
			Product product = new Product();
			product.Id = prodcutId;
			Products.Add(product);
			return product;
		}

		public void RestorePurchases()
		{
			if (!_IsStoreLoaded)
			{
				Error e = new Error(7, "Store Kit Initilizations required");
				RestoreResult obj = new RestoreResult(e);
				PaymentManager.OnRestoreComplete(obj);
				return;
			}
			PaymentManager.OnRestoreStarted();
			if (!Application.isEditor)
			{
				BillingNativeBridge.RestorePurchases();
			}
			else if (IOSNativeSettings.Instance.InAppsEditorTesting)
			{
				foreach (Product product in Products)
				{
					if (product.Type == ProductType.NonConsumable)
					{
						ISN_Logger.Log("Restored: " + product.Id);
						FireProductBoughtEvent(product.Id, string.Empty, string.Empty, string.Empty, IsRestored: true);
					}
				}
				FireRestoreCompleteEvent();
			}
		}

		public void VerifyLastPurchase(string url)
		{
			BillingNativeBridge.VerifyLastPurchase(url);
		}

		public void RegisterProductView(StoreProductView view)
		{
			view.SetId(NextId);
			_productsView.Add(view.Id, view);
		}

		private void OnStoreKitInitFailed(string data)
		{
			Error error = new Error(data);
			_IsStoreLoaded = false;
			_IsWaitingLoadResult = false;
			Result obj = new Result(error);
			PaymentManager.OnStoreKitInitComplete(obj);
			if (!IOSNativeSettings.Instance.DisablePluginLogs)
			{
				ISN_Logger.Log("STORE_KIT_INIT_FAILED Error: " + error.Message);
			}
		}

		private void onStoreDataReceived(string data)
		{
			if (data.Equals(string.Empty))
			{
				ISN_Logger.Log("InAppPurchaseManager, no products avaiable");
				Result obj = new Result();
				PaymentManager.OnStoreKitInitComplete(obj);
				return;
			}
			string[] array = data.Split('|');
			for (int i = 0; i < array.Length; i += 7)
			{
				string prodcutId = array[i];
				Product productById = GetProductById(prodcutId);
				productById.DisplayName = array[i + 1];
				productById.Description = array[i + 2];
				productById.LocalizedPrice = array[i + 3];
				productById.Price = Convert.ToSingle(array[i + 4]);
				productById.CurrencyCode = array[i + 5];
				productById.CurrencySymbol = array[i + 6];
				productById.IsAvailable = true;
			}
			ISN_Logger.Log("InAppPurchaseManager, total products in settings: " + Products.Count.ToString());
			int num = 0;
			foreach (Product product in Products)
			{
				if (product.IsAvailable)
				{
					num++;
				}
			}
			ISN_Logger.Log("InAppPurchaseManager, total avaliable products" + num);
			FireSuccessInitEvent();
		}

		private void onProductBought(string array)
		{
			string[] array2 = array.Split("|"[0]);
			bool isRestored = false;
			if (array2[1].Equals("0"))
			{
				isRestored = true;
			}
			string productIdentifier = array2[0];
			FireProductBoughtEvent(productIdentifier, array2[2], array2[3], array2[4], isRestored);
		}

		private void onProductPurchasedExternally(string productIdentifier)
		{
			PaymentManager.OnProductPurchasedExternally(productIdentifier);
		}

		private void onProductStateDeferred(string productIdentifier)
		{
			PurchaseResult obj = new PurchaseResult(productIdentifier, PurchaseState.Deferred, string.Empty, string.Empty, string.Empty);
			PaymentManager.OnTransactionComplete(obj);
		}

		private void onTransactionFailed(string data)
		{
			string[] array = data.Split(new string[1]
			{
				"|%|".ToString()
			}, StringSplitOptions.None);
			string productIdentifier = array[0];
			Error error = new Error(array[1]);
			SendTransactionFailEvent(productIdentifier, error);
		}

		private void onVerificationResult(string data)
		{
			VerificationResponse obj = new VerificationResponse(lastPurchasedProduct, data);
			PaymentManager.OnVerificationComplete(obj);
		}

		public void onRestoreTransactionFailed(string array)
		{
			Error e = new Error(array);
			RestoreResult obj = new RestoreResult(e);
			PaymentManager.OnRestoreComplete(obj);
		}

		public void onRestoreTransactionComplete(string array)
		{
			FireRestoreCompleteEvent();
		}

		private void OnProductViewLoaded(string viewId)
		{
			int key = Convert.ToInt32(viewId);
			if (_productsView.ContainsKey(key))
			{
				_productsView[key].OnContentLoaded();
			}
		}

		private void OnProductViewLoadedFailed(string viewId)
		{
			int key = Convert.ToInt32(viewId);
			if (_productsView.ContainsKey(key))
			{
				_productsView[key].OnContentLoadFailed();
			}
		}

		private void OnProductViewDismissed(string viewId)
		{
			int key = Convert.ToInt32(viewId);
			if (_productsView.ContainsKey(key))
			{
				_productsView[key].OnProductViewDismissed();
			}
		}

		private void FireSuccessInitEvent()
		{
			_IsStoreLoaded = true;
			_IsWaitingLoadResult = false;
			Result obj = new Result();
			PaymentManager.OnStoreKitInitComplete(obj);
		}

		private void FireRestoreCompleteEvent()
		{
			RestoreResult obj = new RestoreResult();
			PaymentManager.OnRestoreComplete(obj);
		}

		private void FireProductBoughtEvent(string productIdentifier, string applicationUsername, string receipt, string transactionIdentifier, bool IsRestored)
		{
			PurchaseState state = IsRestored ? PurchaseState.Restored : PurchaseState.Purchased;
			PurchaseResult purchaseResult = new PurchaseResult(productIdentifier, state, applicationUsername, receipt, transactionIdentifier);
			lastPurchasedProduct = purchaseResult.ProductIdentifier;
			PaymentManager.OnTransactionComplete(purchaseResult);
		}

		private void SendTransactionFailEvent(string productIdentifier, Error error)
		{
			PurchaseResult obj = new PurchaseResult(productIdentifier, error);
			PaymentManager.OnTransactionComplete(obj);
		}

		private void EditorFakeInitEvent()
		{
			FireSuccessInitEvent();
		}

		static PaymentManager()
		{
			PaymentManager.OnStoreKitInitComplete = delegate
			{
			};
			PaymentManager.OnRestoreStarted = delegate
			{
			};
			PaymentManager.OnRestoreComplete = delegate
			{
			};
			PaymentManager.OnTransactionStarted = delegate
			{
			};
			PaymentManager.OnTransactionComplete = delegate
			{
			};
			PaymentManager.OnProductPurchasedExternally = delegate
			{
			};
			PaymentManager.OnVerificationComplete = delegate
			{
			};
			_nextId = 1;
		}
	}
}
