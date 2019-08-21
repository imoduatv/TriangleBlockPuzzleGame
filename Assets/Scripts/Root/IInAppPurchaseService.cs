using System;
using System.Collections.Generic;

namespace Root
{
	public interface IInAppPurchaseService
	{
		void ConnectInit();

		bool IsProductPurchased(string productID);

		void PurchaseProduct(string productID, Action<bool> callBack);

		void RestorePurchase(Dictionary<string, Action<bool>> callbacks);

		string GetLocalizedPrice(string productId);
	}
}
