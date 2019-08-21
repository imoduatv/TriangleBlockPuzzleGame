using System.Collections.Generic;

public class AMN_RequestLeaderboardsResult : AMN_Result
{
	private string error;

	private List<GC_Leaderboard> leaderboardsList;

	public string Error => error;

	public List<GC_Leaderboard> LeaderboardsList => leaderboardsList;

	public AMN_RequestLeaderboardsResult(bool success)
		: base(success)
	{
	}

	public AMN_RequestLeaderboardsResult(string err)
		: base(success: false)
	{
		error = err;
	}

	public AMN_RequestLeaderboardsResult(List<GC_Leaderboard> list)
		: base(success: true)
	{
		leaderboardsList = list;
	}
}
