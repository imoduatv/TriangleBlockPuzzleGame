public class AMN_GetPurchaseProductsUpdateResponse : AMN_Result
{
	private string _requestId = string.Empty;

	private string _userId = string.Empty;

	private string _marketplace = string.Empty;

	private string _status = string.Empty;

	private bool _hasMore;

	public string RequestId => _requestId;

	public string UserId => _userId;

	public string Marketplace => _marketplace;

	public string Status => _status;

	public bool HasMore => _hasMore;

	public AMN_GetPurchaseProductsUpdateResponse()
		: base(success: true)
	{
	}
}
