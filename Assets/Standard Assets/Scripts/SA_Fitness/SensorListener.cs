using SA.Common.Util;
using System;
using System.Threading;

namespace SA.Fitness
{
	public class SensorListener
	{
		public class Builder
		{
			private SensorListener listener = new SensorListener();

			public Builder SetDataType(DataType dataType)
			{
				listener.dataType = dataType;
				return this;
			}

			public Builder SetSamplingRate(long amount, TimeUnit unit)
			{
				listener.rateAmount = amount;
				listener.rateTimeUnit = unit;
				return this;
			}

			public SensorListener Build()
			{
				return listener;
			}
		}

		private int id;

		private DataType dataType;

		private long rateAmount;

		private TimeUnit rateTimeUnit;

		public int Id => id;

		public DataType DataType => dataType;

		public long RateAmount => rateAmount;

		public TimeUnit RateTimeUnit => rateTimeUnit;

		public event Action<int> OnRegisterSuccess;

		public event Action<int> OnRegisterFail;

		public event Action<int, DataPoint> OnDataPointReceived;

		private SensorListener()
		{
			this.OnRegisterSuccess = delegate
			{
			};
			this.OnRegisterFail = delegate
			{
			};
			this.OnDataPointReceived = delegate
			{
			};
			//base._002Ector();
			id = IdFactory.NextId;
		}

		public void DispatchRegisterSuccess()
		{
			this.OnRegisterSuccess(id);
		}

		public void DispatchRegisterFail()
		{
			this.OnRegisterFail(id);
		}

		public void DispatchDataPointEvent(string[] bundle)
		{
			this.OnDataPointReceived(id, new DataPoint(dataType, bundle, "|"));
		}
	}
}
