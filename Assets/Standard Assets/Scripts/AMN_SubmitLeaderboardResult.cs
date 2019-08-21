public class AMN_SubmitLeaderboardResult : AMN_Result
{
	private string error;

	private string leaderboardID;

	public string Error => error;

	public string LeaderboardID => leaderboardID;

	public AMN_SubmitLeaderboardResult(bool success)
		: base(success)
	{
	}

	public AMN_SubmitLeaderboardResult(string id, string err)
		: base(success: false)
	{
		leaderboardID = id;
		error = err;
	}

	public AMN_SubmitLeaderboardResult(string id)
		: base(success: true)
	{
		leaderboardID = id;
	}
}
