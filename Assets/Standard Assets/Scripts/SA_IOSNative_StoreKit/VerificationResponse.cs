using System;

namespace SA.IOSNative.StoreKit
{
	public class VerificationResponse
	{
		private int _Status;

		private string _Receipt;

		private string _ProductIdentifier;

		private string _OriginalJSON;

		public int Status => _Status;

		public string Receipt => _Receipt;

		public string ProductIdentifier => _ProductIdentifier;

		public string OriginalJSON => _OriginalJSON;

		public VerificationResponse(string productIdentifier, string dataArray)
		{
			string[] array = dataArray.Split('|');
			_Status = Convert.ToInt32(array[0]);
			_OriginalJSON = array[1];
			_Receipt = array[2];
			_ProductIdentifier = productIdentifier;
		}
	}
}
