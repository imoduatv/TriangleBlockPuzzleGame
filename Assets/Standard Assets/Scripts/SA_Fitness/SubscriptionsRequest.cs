using SA.Common.Util;
using System;
using System.Threading;

namespace SA.Fitness
{
	public class SubscriptionsRequest
	{
		public class Builder
		{
			private SubscriptionsRequest request = new SubscriptionsRequest();

			public Builder SetDataType(DataType dataType)
			{
				request.dataType = dataType;
				return this;
			}

			public SubscriptionsRequest Build()
			{
				return request;
			}
		}

		private int id;

		private DataType dataType;

		public int Id => id;

		public DataType DataType => dataType;

		public event Action<SubscriptionsRequestResult> OnRequestFinished;

		private SubscriptionsRequest()
		{
			this.OnRequestFinished = delegate
			{
			};
			id = IdFactory.NextId;
			//base._002Ector();
		}

		public void DispatchRequestResult(string[] bundle)
		{
			int num = int.Parse(bundle[1]);
			string message = bundle[2];
			if (num == 0)
			{
				SubscriptionsRequestResult subscriptionsRequestResult = new SubscriptionsRequestResult(id);
				for (int i = 3; i < bundle.Length; i++)
				{
					if (!bundle[i].Equals(string.Empty))
					{
						Subscription subscription = new Subscription(new DataType(bundle[i]));
						subscriptionsRequestResult.AddSubscription(subscription);
					}
				}
			}
			else
			{
				SubscriptionsRequestResult subscriptionsRequestResult = new SubscriptionsRequestResult(id, num, message);
			}
			this.OnRequestFinished(new SubscriptionsRequestResult(id));
		}
	}
}
