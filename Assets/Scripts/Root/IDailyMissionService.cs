namespace Root
{
	public interface IDailyMissionService
	{
		void SetConfig(DailyMissionConfig[] configs);

		void AddValue(string keyName, int value);

		void SetValue(string keyName, int value);

		void SetValueIfHigher(string keyName, int value);

		int GetValue(string keyName);

		int GetConditionValue(string keyName);

		bool IsProgressDone(string keyName);

		bool IsClaimedReward(string keyName);

		void ClaimReward(string keyName);
	}
}
