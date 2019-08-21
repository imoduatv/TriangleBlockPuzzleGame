namespace SA.Fitness
{
	public class Subscription
	{
		private DataType dataType;

		public DataType DataType => dataType;

		public Subscription(DataType dataType)
		{
			this.dataType = dataType;
		}
	}
}
