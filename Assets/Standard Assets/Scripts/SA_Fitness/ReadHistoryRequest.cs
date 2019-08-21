using SA.Common.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SA.Fitness
{
	public class ReadHistoryRequest
	{
		public class Builder
		{
			private ReadHistoryRequest request = new ReadHistoryRequest();

			public Builder Aggregate(DataType inputDataType, DataType outputDataType)
			{
				request.isAggregated = true;
				request.dataType = inputDataType;
				request.aggregateType = outputDataType;
				return this;
			}

			public Builder Read(DataType dataType)
			{
				request.isAggregated = false;
				request.dataType = dataType;
				return this;
			}

			public Builder BucketByActivitySegment(int minDuration, TimeUnit timeUnit)
			{
				request.minDuration = minDuration;
				request.bucketUnit = timeUnit;
				request.bucketingType = Bucket.Type.ActivitySegment;
				return this;
			}

			public Builder BucketByActivityType(int minDuration, TimeUnit timeUnit)
			{
				request.minDuration = minDuration;
				request.bucketUnit = timeUnit;
				request.bucketingType = Bucket.Type.ActivityType;
				return this;
			}

			public Builder BucketBySession(int minDuration, TimeUnit timeUnit)
			{
				request.minDuration = minDuration;
				request.bucketUnit = timeUnit;
				request.bucketingType = Bucket.Type.Session;
				return this;
			}

			public Builder BucketByTime(int minDuration, TimeUnit timeUnit)
			{
				request.minDuration = minDuration;
				request.bucketUnit = timeUnit;
				request.bucketingType = Bucket.Type.Time;
				return this;
			}

			public Builder SetLimit(int limit)
			{
				request.limit = limit;
				return this;
			}

			public Builder SetTimeRange(long startTime, long endTime, TimeUnit unit)
			{
				request.startTime = startTime;
				request.endTime = endTime;
				request.timeUnit = unit;
				return this;
			}

			public ReadHistoryRequest Build()
			{
				return request;
			}
		}

		private int id;

		private long startTime;

		private long endTime;

		private TimeUnit timeUnit;

		private DataType dataType;

		private DataType aggregateType;

		private int limit;

		private bool isAggregated;

		private int minDuration;

		private TimeUnit bucketUnit;

		private Bucket.Type bucketingType;

		public bool IsAggregated => isAggregated;

		public Bucket.Type BucketingType => bucketingType;

		public TimeUnit BucketUnit => bucketUnit;

		public int MinDuration => minDuration;

		public int Limit => limit;

		public DataType AggregateType => aggregateType;

		public DataType DataType => dataType;

		public TimeUnit TimeUnit => timeUnit;

		public long EndTime => endTime;

		public long StartTime => startTime;

		public int Id => id;

		public event Action<ReadHistoryResult> OnReadFinished;

		private ReadHistoryRequest()
		{
			this.OnReadFinished = delegate
			{
			};
			id = IdFactory.NextId;
			endTime = 1L;
			timeUnit = TimeUnit.Milliseconds;
			bucketUnit = TimeUnit.Milliseconds;
			bucketingType = Bucket.Type.Time;
			//base._002Ector();
		}

		public void DispatchReadResult(string[] bundle)
		{
			int num = int.Parse(bundle[1]);
			string message = bundle[2];
			ReadHistoryResult readHistoryResult = (num != 0) ? new ReadHistoryResult(id, num, message) : new ReadHistoryResult(id, isAggregated);
			if (readHistoryResult.IsSucceeded)
			{
				for (int i = 3; i < bundle.Length; i++)
				{
					string[] array = bundle[i].Split(new string[1]
					{
						"~"
					}, StringSplitOptions.None);
					DataSet dataSet = new DataSet(new DataType(array[0]));
					if (array.Length > 1)
					{
						for (int j = 1; j < array.Length; j++)
						{
							string[] array2 = array[j].Split(new string[1]
							{
								"$"
							}, StringSplitOptions.None);
							List<string> list = new List<string>();
							list.Add(array2[0]);
							list.Add(array2[1]);
							list.Add(array2[2]);
							list.Add(array2[3]);
							DataPoint dataPoint = new DataPoint(new DataType(array2[0]), list.ToArray(), "%");
							dataSet.AddDataPoint(dataPoint);
						}
					}
					readHistoryResult.AddDataSet(dataSet);
				}
			}
			this.OnReadFinished(readHistoryResult);
		}

		public void DispatchAggregatedResult(string[] bundle)
		{
			int num = int.Parse(bundle[1]);
			string message = bundle[2];
			ReadHistoryResult readHistoryResult = (num != 0) ? new ReadHistoryResult(id, num, message) : new ReadHistoryResult(id, isAggregated);
			if (readHistoryResult.IsSucceeded)
			{
				for (int i = 3; i < bundle.Length; i++)
				{
					string[] array = bundle[i].Split(new string[1]
					{
						"~"
					}, StringSplitOptions.None);
					Bucket bucket = new Bucket((Bucket.Type)int.Parse(array[0]));
					bucket.SetTimeRange(long.Parse(array[1]), long.Parse(array[2]));
					if (array.Length > 3)
					{
						for (int j = 3; j < array.Length; j++)
						{
							string[] array2 = array[j].Split(new string[1]
							{
								"$"
							}, StringSplitOptions.None);
							DataSet dataSet = new DataSet(new DataType(array2[0]));
							if (array2.Length > 1)
							{
								for (int k = 1; k < array2.Length; k++)
								{
									string[] array3 = array2[k].Split(new string[1]
									{
										"%"
									}, StringSplitOptions.None);
									List<string> list = new List<string>();
									list.Add(array3[0]);
									list.Add(array3[1]);
									list.Add(array3[2]);
									list.Add(array3[3]);
									DataPoint dataPoint = new DataPoint(new DataType(array3[0]), list.ToArray(), "^");
									dataSet.AddDataPoint(dataPoint);
								}
							}
							bucket.AddDataSet(dataSet);
						}
					}
					readHistoryResult.AddBucket(bucket);
				}
			}
			this.OnReadFinished(readHistoryResult);
		}
	}
}
