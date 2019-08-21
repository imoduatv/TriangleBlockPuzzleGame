public class AMN_GetUserDataResponse : AMN_Result
{
	private string _requestId = string.Empty;

	private string _userId = string.Empty;

	private string _marketplace = string.Empty;

	private string _status = string.Empty;

	public string RequestId => _requestId;

	public string UserId => _userId;

	public string Marketplace => _marketplace;

	public string Status => _status;

	public AMN_GetUserDataResponse()
		: base(success: true)
	{
	}
}
