using System;
using System.Collections.Generic;

namespace SA.Fitness
{
	public class DataPoint
	{
		private long startTime;

		private long endTime;

		private DataType dataType;

		private Dictionary<string, object> fields = new Dictionary<string, object>();

		public long StartTime => startTime;

		public long EndTime => endTime;

		public DataType DataType => dataType;

		public Dictionary<string, object> Fields => fields;

		public DataPoint(DataType type, string[] bundle, string key)
		{
			dataType = type;
			startTime = long.Parse(bundle[1]);
			endTime = long.Parse(bundle[2]);
			for (int i = 3; i < bundle.Length; i++)
			{
				if (!bundle[i].Equals(string.Empty))
				{
					string[] array = bundle[i].Split(new string[1]
					{
						key
					}, StringSplitOptions.None);
					fields.Add(array[0], array[1]);
				}
			}
		}
	}
}
