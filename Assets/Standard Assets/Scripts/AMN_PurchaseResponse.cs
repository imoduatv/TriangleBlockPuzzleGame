public class AMN_PurchaseResponse : AMN_Result
{
	private string _requestId = string.Empty;

	private string _userId = string.Empty;

	private string _marketplace = string.Empty;

	private string _receiptId = string.Empty;

	private long _cancelDate;

	private long _purchaseDate;

	private string _sku = string.Empty;

	private string _productType = string.Empty;

	private string _status = string.Empty;

	public string RequestId => _requestId;

	public string UserId => _userId;

	public string Marketplace => _marketplace;

	public string ReceiptId => _receiptId;

	public long CancelDate => _cancelDate;

	public long PurchaseDatee => _purchaseDate;

	public string Sku => _sku;

	public string ProductType => _productType;

	public string Status => _status;

	public AMN_PurchaseResponse()
		: base(success: true)
	{
	}
}
