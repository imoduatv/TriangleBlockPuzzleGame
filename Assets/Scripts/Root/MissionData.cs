namespace Root
{
	public class MissionData
	{
		public string KeyName;

		public int Value;

		public bool IsClaimed;

		public MissionData(string keyName)
		{
			KeyName = keyName;
			Value = 0;
			IsClaimed = false;
		}
	}
}
