using System.Collections.Generic;

public class FB_AppInviteResult : FB_Result
{
	private bool _cancelled = true;

	private IDictionary<string, object> _resultDictionary;

	public bool Cancelled => _cancelled;

	public IDictionary<string, object> ResultDictionary => _resultDictionary;

	public FB_AppInviteResult(bool cancelled, string RawData, string Error, IDictionary<string, object> dict)
		: base(RawData, Error)
	{
		_cancelled = cancelled;
		_resultDictionary = dict;
	}
}
