public class SA_AmazonReceipt
{
	private string _sku = string.Empty;

	private string _productType = string.Empty;

	private string _receiptId = string.Empty;

	private long _purchaseDate;

	private long _cancelDate;

	public string Sku => _sku;

	public string ProductType => _productType;

	public string ReceiptId => _receiptId;

	public long PurchaseDate => _purchaseDate;

	public long CancelDate => _cancelDate;
}
