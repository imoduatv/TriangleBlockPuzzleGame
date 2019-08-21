using SA.Common.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SA.Fitness
{
	public class SensorRequest
	{
		public class Builder
		{
			private SensorRequest request = new SensorRequest();

			public Builder AddDataType(DataType dataType)
			{
				if (!request.dataTypes.Contains(dataType))
				{
					request.dataTypes.Add(dataType);
				}
				return this;
			}

			public Builder AddDataSourceType(DataSourceType dataSourceType)
			{
				if (!request.dataSourceTypes.Contains(dataSourceType))
				{
					request.DataSourceTypes.Add(dataSourceType);
				}
				return this;
			}

			public SensorRequest Build()
			{
				return request;
			}
		}

		private int id;

		private List<DataType> dataTypes;

		private List<DataSourceType> dataSourceTypes;

		public int Id => id;

		public List<DataType> DataTypes => dataTypes;

		public List<DataSourceType> DataSourceTypes => dataSourceTypes;

		public event Action<SensorRequestResult> OnRequestFinished;

		private SensorRequest()
		{
			this.OnRequestFinished = delegate
			{
			};
			dataTypes = new List<DataType>();
			dataSourceTypes = new List<DataSourceType>();
			//base._002Ector();
			id = IdFactory.NextId;
		}

		public void DispatchResult(string[] bundle)
		{
			int num = int.Parse(bundle[1]);
			string message = bundle[2];
			SensorRequestResult sensorRequestResult = (num != 0) ? new SensorRequestResult(id, num, message) : new SensorRequestResult(id);
			for (int i = 3; i < bundle.Length; i++)
			{
				string[] bundle2 = bundle[i].Split(new string[1]
				{
					"|"
				}, StringSplitOptions.None);
				DataSource source = new DataSource(bundle2);
				sensorRequestResult.AddDataSource(source);
			}
			this.OnRequestFinished(sensorRequestResult);
		}
	}
}
