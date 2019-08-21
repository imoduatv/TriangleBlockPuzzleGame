using SA.Common.Models;

public class GK_TBM_MatchTurnResult : Result
{
	private GK_TBM_Match _Match;

	public GK_TBM_Match Match => _Match;

	public GK_TBM_MatchTurnResult(GK_TBM_Match match)
	{
		_Match = match;
	}

	public GK_TBM_MatchTurnResult(string errorData)
		: base(new Error(errorData))
	{
	}
}
