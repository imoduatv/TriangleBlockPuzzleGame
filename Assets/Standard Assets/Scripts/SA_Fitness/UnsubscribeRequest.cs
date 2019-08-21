using SA.Common.Models;
using SA.Common.Util;
using System;
using System.Threading;

namespace SA.Fitness
{
	public class UnsubscribeRequest
	{
		public class Builder
		{
			private UnsubscribeRequest request = new UnsubscribeRequest();

			public Builder SetDataType(DataType dataType)
			{
				request.dataType = dataType;
				return this;
			}

			public UnsubscribeRequest Build()
			{
				return request;
			}
		}

		private int id;

		private DataType dataType;

		public int Id => id;

		public DataType DataType => dataType;

		public event Action<Result> OnUnsubscribeFinished;

		private UnsubscribeRequest()
		{
			this.OnUnsubscribeFinished = delegate
			{
			};
			id = IdFactory.NextId;
			//base._002Ector();
		}

		public void DispatchUnsubscribeResult(Result result)
		{
			this.OnUnsubscribeFinished(result);
		}
	}
}
