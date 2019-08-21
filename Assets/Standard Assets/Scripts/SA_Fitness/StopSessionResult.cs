using SA.Common.Models;
using System.Collections.Generic;

namespace SA.Fitness
{
	public class StopSessionResult : Result
	{
		private List<Session> sessions = new List<Session>();

		private int id;

		public int Id => id;

		public List<Session> Sessions => sessions;

		public StopSessionResult(int id)
		{
			this.id = id;
		}

		public StopSessionResult(int id, int resultCode, string message)
			: base(new Error(resultCode, message))
		{
			this.id = id;
		}

		public void AddSession(Session session)
		{
			sessions.Add(session);
		}
	}
}
