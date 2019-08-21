using SA.Common.Models;
using SA.Common.Util;
using System;
using System.Threading;

namespace SA.Fitness
{
	public class SubscribeRequest
	{
		public class Builder
		{
			private SubscribeRequest request = new SubscribeRequest();

			public Builder SetDataType(DataType dataType)
			{
				request.dataType = dataType;
				return this;
			}

			public SubscribeRequest Build()
			{
				return request;
			}
		}

		private int id;

		private DataType dataType;

		public int Id => id;

		public DataType DataType => dataType;

		public event Action<Result> OnSubscribeFinished;

		private SubscribeRequest()
		{
			this.OnSubscribeFinished = delegate
			{
			};
			id = IdFactory.NextId;
			//base._002Ector();
		}

		public void DispatchResult(Result result)
		{
			this.OnSubscribeFinished(result);
		}
	}
}
