using SA.Common.Models;
using System.Collections.Generic;

namespace SA.Fitness
{
	public class SubscriptionsRequestResult : Result
	{
		private List<Subscription> subscriptions = new List<Subscription>();

		private int id;

		public int Id => id;

		public List<Subscription> Subscriptions => subscriptions;

		public SubscriptionsRequestResult(int id)
		{
			this.id = id;
		}

		public SubscriptionsRequestResult(int id, int resultCode, string message)
			: base(new Error(resultCode, message))
		{
			this.id = id;
		}

		public void AddSubscription(Subscription subscription)
		{
			subscriptions.Add(subscription);
		}
	}
}
