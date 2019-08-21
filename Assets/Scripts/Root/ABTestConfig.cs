using System;

namespace Root
{
	[Serializable]
	public class ABTestConfig
	{
		public string KeyName;

		public bool IsActive;

		public bool IsOnlyNewInstall;

		public bool IsOverride;

		public string[] ListEventsAB;

		public int[] ListRandomRatio;

		public int DefaultValue;
	}
}
