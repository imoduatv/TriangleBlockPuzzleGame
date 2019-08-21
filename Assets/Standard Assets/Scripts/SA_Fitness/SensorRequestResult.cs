using SA.Common.Models;
using System.Collections.Generic;

namespace SA.Fitness
{
	public class SensorRequestResult : Result
	{
		private int id;

		private List<DataSource> dataSources = new List<DataSource>();

		public int Id => id;

		public List<DataSource> DataSources => dataSources;

		public SensorRequestResult(int id)
		{
			this.id = id;
		}

		public SensorRequestResult(int id, int resultCode, string message)
			: base(new Error(resultCode, message))
		{
			this.id = id;
		}

		public void AddDataSource(DataSource source)
		{
			dataSources.Add(source);
		}
	}
}
