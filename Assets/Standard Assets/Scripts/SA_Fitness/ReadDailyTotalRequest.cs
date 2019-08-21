using SA.Common.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SA.Fitness
{
	public class ReadDailyTotalRequest
	{
		public class Builder
		{
			private ReadDailyTotalRequest request = new ReadDailyTotalRequest();

			public Builder SetDataType(DataType dataType)
			{
				request.dataType = dataType;
				return this;
			}

			public ReadDailyTotalRequest Build()
			{
				return request;
			}
		}

		private int id;

		private DataType dataType;

		public int Id => id;

		public DataType DataType => dataType;

		public event Action<ReadDailyTotalResult> OnRequestFinished;

		private ReadDailyTotalRequest()
		{
			this.OnRequestFinished = delegate
			{
			};
			id = IdFactory.NextId;
			//base._002Ector();
		}

		public void DispatchResult(string[] bundle)
		{
			int num = int.Parse(bundle[1]);
			ReadDailyTotalResult readDailyTotalResult = (num != 0) ? new ReadDailyTotalResult(id, num, bundle[2]) : new ReadDailyTotalResult(id);
			if (readDailyTotalResult.IsSucceeded)
			{
				DataSet dataSet = new DataSet(dataType);
				if (bundle.Length > 3)
				{
					for (int i = 4; i < bundle.Length; i++)
					{
						string[] array = bundle[i].Split(new string[1]
						{
							"~"
						}, StringSplitOptions.None);
						List<string> list = new List<string>();
						list.Add(array[0]);
						list.Add(array[1]);
						list.Add(array[2]);
						list.Add(array[3]);
						DataPoint dataPoint = new DataPoint(new DataType(array[0]), list.ToArray(), "$");
						dataSet.AddDataPoint(dataPoint);
					}
				}
				readDailyTotalResult.SetData(dataSet);
			}
			this.OnRequestFinished(readDailyTotalResult);
		}
	}
}
