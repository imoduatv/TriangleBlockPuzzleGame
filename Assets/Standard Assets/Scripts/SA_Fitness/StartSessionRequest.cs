using SA.Common.Models;
using SA.Common.Util;
using System;
using System.Threading;

namespace SA.Fitness
{
	public class StartSessionRequest
	{
		public class Builder
		{
			private StartSessionRequest request = new StartSessionRequest();

			public Builder SetName(string name)
			{
				request.name = name;
				return this;
			}

			public Builder SetIdentifier(string id)
			{
				request.sessionId = id;
				return this;
			}

			public Builder SetDescription(string description)
			{
				request.description = description;
				return this;
			}

			public Builder SetStartTime(long startTime, TimeUnit timeUnit)
			{
				request.startTime = startTime;
				request.timeUnit = timeUnit;
				return this;
			}

			public Builder SetActivity(Activity activity)
			{
				request.activity = activity;
				return this;
			}

			public StartSessionRequest Build()
			{
				return request;
			}
		}

		private int id;

		private string name;

		private string sessionId;

		private string description;

		private long startTime;

		private TimeUnit timeUnit;

		private Activity activity;

		public int Id => id;

		public string Name => name;

		public string SessionId => sessionId;

		public string Description => description;

		public long StartTime => startTime;

		public TimeUnit TimeUnit => timeUnit;

		public Activity Activity => activity;

		public event Action<Result> OnSessionStarted;

		private StartSessionRequest()
		{
			this.OnSessionStarted = delegate
			{
			};
			id = IdFactory.NextId;
			name = string.Empty;
			sessionId = string.Empty;
			description = string.Empty;
			activity = Activity.UNKNOWN;
			//base._002Ector();
		}

		public void DispatchSessionStartResult(Result result)
		{
			this.OnSessionStarted(result);
		}
	}
}
