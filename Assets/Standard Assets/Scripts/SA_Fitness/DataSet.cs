using System.Collections.Generic;

namespace SA.Fitness
{
	public class DataSet
	{
		private DataType dataType;

		private List<DataPoint> dataPoints = new List<DataPoint>();

		public DataType DataType => dataType;

		public List<DataPoint> DataPoints => dataPoints;

		public DataSet(DataType dataType)
		{
			this.dataType = dataType;
		}

		internal void AddDataPoint(DataPoint dataPoint)
		{
			dataPoints.Add(dataPoint);
		}
	}
}
