namespace SA.Fitness
{
	public class DataSource
	{
		private DataType dataType;

		private DataSourceType dataSourceType;

		private string appPackageName = string.Empty;

		private string device = string.Empty;

		private string name = string.Empty;

		private string streamId = string.Empty;

		private string streamName = string.Empty;

		public DataType DataType => dataType;

		public DataSourceType DataSourceType => dataSourceType;

		public string AppPackageName => appPackageName;

		public string Device => device;

		public string Name => name;

		public string StreamId => streamId;

		public string StreamName => streamName;

		public DataSource(string[] bundle)
		{
			appPackageName = bundle[0];
			dataType = new DataType(bundle[1]);
			device = bundle[2];
			name = bundle[3];
			streamId = bundle[4];
			streamName = bundle[5];
			dataSourceType = (DataSourceType)int.Parse(bundle[6]);
		}
	}
}
