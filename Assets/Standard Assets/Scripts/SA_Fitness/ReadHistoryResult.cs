using SA.Common.Models;
using System.Collections.Generic;

namespace SA.Fitness
{
	public class ReadHistoryResult : Result
	{
		private List<Bucket> buckets = new List<Bucket>();

		private List<DataSet> dataSets = new List<DataSet>();

		private int id;

		private bool isAggregated;

		public int Id => id;

		public bool IsAggregated => isAggregated;

		public List<Bucket> Buckets => buckets;

		public List<DataSet> DataSets => dataSets;

		public ReadHistoryResult(int id, bool isAggregated)
		{
			this.id = id;
			this.isAggregated = isAggregated;
		}

		public ReadHistoryResult(int id, int resultCode, string message)
			: base(new Error(resultCode, message))
		{
			this.id = id;
		}

		public void AddDataSet(DataSet dataSet)
		{
			dataSets.Add(dataSet);
		}

		public void AddBucket(Bucket bucket)
		{
			buckets.Add(bucket);
		}
	}
}
