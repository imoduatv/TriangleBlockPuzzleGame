public class AMN_LocalPlayerScoreLoadedResult : AMN_Result
{
	private string _LeaderboardId = string.Empty;

	private GC_ScoreTimeSpan _TimeSpan = GC_ScoreTimeSpan.ALL_TIME;

	private int _Rank;

	private long _Score;

	private string _Error = string.Empty;

	public string LeaderboardId => _LeaderboardId;

	public GC_ScoreTimeSpan TimeSpan => _TimeSpan;

	public int Rank => _Rank;

	public long Score => _Score;

	public string Error => _Error;

	public AMN_LocalPlayerScoreLoadedResult(string leaderboardId, GC_ScoreTimeSpan timeSpan, int rank, long score)
		: base(success: true)
	{
		_LeaderboardId = leaderboardId;
		_TimeSpan = timeSpan;
		_Rank = rank;
		_Score = score;
	}

	public AMN_LocalPlayerScoreLoadedResult(string leaderboardId, GC_ScoreTimeSpan timeSpan, string error)
		: base(success: false)
	{
		_LeaderboardId = leaderboardId;
		_TimeSpan = timeSpan;
		_Error = error;
	}
}
