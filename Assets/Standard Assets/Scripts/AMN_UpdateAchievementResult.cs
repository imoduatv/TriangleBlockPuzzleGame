public class AMN_UpdateAchievementResult : AMN_Result
{
	private string error;

	private string achievementID;

	public string Error => error;

	public string AchievementID => achievementID;

	public AMN_UpdateAchievementResult(bool success)
		: base(success)
	{
	}

	public AMN_UpdateAchievementResult(string id, string err)
		: base(success: false)
	{
		achievementID = id;
		error = err;
	}

	public AMN_UpdateAchievementResult(string id)
		: base(success: true)
	{
		achievementID = id;
	}
}
