using System.Collections.Generic;

namespace SA.Fitness
{
	public sealed class Bucket
	{
		public enum Type
		{
			ActivitySegment = 4,
			ActivityType = 3,
			Session = 2,
			Time = 1
		}

		private List<DataSet> dataSets = new List<DataSet>();

		private Type type = Type.Time;

		private long startTime;

		private long endTime = 10L;

		public Type Bucketing => type;

		public long StartTime => startTime;

		public long EndTime => endTime;

		public List<DataSet> DataSets => dataSets;

		public Bucket(Type type)
		{
			this.type = type;
		}

		public void SetTimeRange(long startTime, long endTime)
		{
			this.startTime = startTime;
			this.endTime = endTime;
		}

		public void AddDataSet(DataSet dataSet)
		{
			dataSets.Add(dataSet);
		}
	}
}
