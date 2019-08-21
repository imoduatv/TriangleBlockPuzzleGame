using SA.Common.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SA.Fitness
{
	public class DeleteDataRequest
	{
		public class Builder
		{
			private DeleteDataRequest request = new DeleteDataRequest();

			public Builder SetTimeInterval(long startTime, long endTime, TimeUnit unit)
			{
				request.startTime = startTime;
				request.endTime = endTime;
				request.timeUnit = unit;
				return this;
			}

			public Builder AddDataType(DataType dataType)
			{
				if (!request.dataTypes.Contains(dataType))
				{
					request.dataTypes.Add(dataType);
				}
				return this;
			}

			public Builder AddSession(string sessionId)
			{
				if (!request.sessions.Contains(sessionId))
				{
					request.sessions.Add(sessionId);
				}
				return this;
			}

			public Builder DeleteAllSessions()
			{
				request.sessions.Clear();
				return this;
			}

			public DeleteDataRequest Build()
			{
				return request;
			}
		}

		private int id;

		private long startTime;

		private long endTime;

		private TimeUnit timeUnit;

		private List<DataType> dataTypes;

		private List<string> sessions;

		public int Id => id;

		public long StartTime => startTime;

		public long EndTime => endTime;

		public TimeUnit TimeUnit => timeUnit;

		public List<DataType> DataTypes => dataTypes;

		public List<string> Sessions => sessions;

		public event Action OnRequestFinished;

		public DeleteDataRequest()
		{
			this.OnRequestFinished = delegate
			{
			};
			id = IdFactory.NextId;
			endTime = 1L;
			dataTypes = new List<DataType>();
			sessions = new List<string>();
			//base._002Ector();
		}

		public void DispatchRequestResult()
		{
			this.OnRequestFinished();
		}
	}
}
