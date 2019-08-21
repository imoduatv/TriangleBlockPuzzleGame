using System.Collections.Generic;

public class AMN_GetProductDataResponse : AMN_Result
{
	private string _requestId = string.Empty;

	private List<string> _unavailableSkus;

	private string _status = string.Empty;

	public string RequestId => _requestId;

	public List<string> UnavailableSkus => _unavailableSkus;

	public string Status => _status;

	public AMN_GetProductDataResponse()
		: base(success: true)
	{
	}
}
