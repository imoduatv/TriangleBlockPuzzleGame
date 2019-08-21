using System.Collections.Generic;

namespace Root
{
	public class LeaderBoardMockModule : ILeaderBoardService
	{
		private Dictionary<string, int> data;

		public void ConnectService()
		{
		}

		public int GetPlayerScore(string leaderBoardID, int defaultValue)
		{
			Dictionary<string, int> dictionary = GetData();
			if (dictionary.ContainsKey(leaderBoardID))
			{
				return dictionary[leaderBoardID];
			}
			return defaultValue;
		}

		public void ShowLeaderBoard()
		{
		}

		public void ShowLeaderBoard(string leaderBoardID)
		{
			ShowLeaderBoard();
		}

		public void SubmitPlayerScore(string leaderBoardID, int value)
		{
			Dictionary<string, int> dictionary = GetData();
			if (dictionary.ContainsKey(leaderBoardID))
			{
				dictionary[leaderBoardID] = value;
			}
			else
			{
				dictionary.Add(leaderBoardID, value);
			}
		}

		private Dictionary<string, int> GetData()
		{
			if (data == null)
			{
				data = new Dictionary<string, int>();
			}
			return data;
		}
	}
}
