namespace Root
{
	public class TimeModule : ITimeService
	{
		private long totalSeconds;

		private int hours;

		private int minutes;

		private int seconds;

		private string format = "00";

		private string split = ":";

		private string split2 = " : ";

		public void SetTotalSeconds(int seconds)
		{
			totalSeconds = seconds;
			this.seconds = seconds % 60;
			minutes = seconds % 3600 / 60;
			hours = seconds / 3600;
		}

		public int GetSeconds()
		{
			return seconds;
		}

		public int GetMinutes()
		{
			return minutes;
		}

		public int GetHours()
		{
			return hours;
		}

		public string GetTimeString(bool withSpace = false)
		{
			if (withSpace)
			{
				return hours.ToString(format) + split2 + minutes.ToString(format) + split2 + seconds.ToString(format);
			}
			return hours.ToString(format) + split + minutes.ToString(format) + split + seconds.ToString(format);
		}
	}
}
