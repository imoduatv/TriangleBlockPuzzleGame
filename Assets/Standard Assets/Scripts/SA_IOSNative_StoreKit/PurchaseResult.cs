using SA.Common.Models;

namespace SA.IOSNative.StoreKit
{
	public class PurchaseResult : Result
	{
		private string _ProductIdentifier = string.Empty;

		private PurchaseState _State = PurchaseState.Failed;

		private string _Receipt = string.Empty;

		private string _TransactionIdentifier = string.Empty;

		private string _ApplicationUsername = string.Empty;

		public TransactionErrorCode TransactionErrorCode
		{
			get
			{
				if (_Error != null)
				{
					return (TransactionErrorCode)_Error.Code;
				}
				return TransactionErrorCode.SKErrorNone;
			}
		}

		public PurchaseState State => _State;

		public string ProductIdentifier => _ProductIdentifier;

		public string ApplicationUsername => _ApplicationUsername;

		public string Receipt => _Receipt;

		public string TransactionIdentifier => _TransactionIdentifier;

		public PurchaseResult(string productIdentifier, Error e)
			: base(e)
		{
			_ProductIdentifier = productIdentifier;
			_State = PurchaseState.Failed;
		}

		public PurchaseResult(string productIdentifier, PurchaseState state, string applicationUsername = "", string receipt = "", string transactionIdentifier = "")
		{
			_ProductIdentifier = productIdentifier;
			_State = state;
			_Receipt = receipt;
			_TransactionIdentifier = transactionIdentifier;
			_ApplicationUsername = applicationUsername;
		}
	}
}
