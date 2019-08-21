using System.Collections.Generic;

public class AMN_RequestAchievementsResult : AMN_Result
{
	private string error;

	private List<GC_Achievement> achievementList;

	public string Error => error;

	public List<GC_Achievement> AchievementList => achievementList;

	public AMN_RequestAchievementsResult(bool success)
		: base(success)
	{
	}

	public AMN_RequestAchievementsResult(string err)
		: base(success: false)
	{
		error = err;
	}

	public AMN_RequestAchievementsResult(List<GC_Achievement> list)
		: base(success: true)
	{
		achievementList = list;
	}
}
