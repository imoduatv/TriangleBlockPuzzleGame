namespace Root
{
	public class ScoreModule : IScoreService
	{
		private IDataService dataService;

		private ILeaderBoardService leaderBoardService;

		public ScoreModule(IDataService dataService, ILeaderBoardService lbService)
		{
			this.dataService = dataService;
			leaderBoardService = lbService;
		}

		public int GetBestHighScore(string leaderBoardID)
		{
			bool isServerScore;
			return GetBestHighScore(leaderBoardID, out isServerScore);
		}

		public int GetBestHighScore(string leaderBoardID, out bool isServerScore)
		{
			int num = GetLocalScore(leaderBoardID);
			int serverScore = GetServerScore(leaderBoardID);
			isServerScore = false;
			if (num < serverScore)
			{
				num = serverScore;
				dataService.SetInt(leaderBoardID, serverScore);
				isServerScore = true;
			}
			return num;
		}

		public int GetBestLowScore(string leaderBoardID)
		{
			bool isServerScore;
			return GetBestLowScore(leaderBoardID, out isServerScore);
		}

		public int GetBestLowScore(string leaderBoardID, out bool isServerScore)
		{
			int num = GetLocalScore(leaderBoardID);
			int serverScore = GetServerScore(leaderBoardID);
			isServerScore = false;
			if (num > serverScore && serverScore > 0)
			{
				num = serverScore;
				dataService.SetInt(leaderBoardID, serverScore);
				isServerScore = true;
			}
			return num;
		}

		public bool SetNewBestHighScore(int score, string leaderBoardID)
		{
			bool isServerScore;
			int bestHighScore = GetBestHighScore(leaderBoardID, out isServerScore);
			if (score > bestHighScore)
			{
				dataService.SetInt(leaderBoardID, score);
				return true;
			}
			return false;
		}

		public bool SetNewBestLowScore(int score, string leaderBoardID)
		{
			bool isServerScore;
			int bestLowScore = GetBestLowScore(leaderBoardID, out isServerScore);
			if (score <= 0)
			{
				return false;
			}
			if (bestLowScore <= 0)
			{
				dataService.SetInt(leaderBoardID, score);
				return true;
			}
			if (score < bestLowScore)
			{
				dataService.SetInt(leaderBoardID, score);
				return true;
			}
			return false;
		}

		public bool SubmitBestHighScore(string leaderBoardID)
		{
			int localScore = GetLocalScore(leaderBoardID);
			int serverScore = GetServerScore(leaderBoardID);
			if (localScore > serverScore)
			{
				leaderBoardService.SubmitPlayerScore(leaderBoardID, localScore);
				return true;
			}
			return false;
		}

		public bool SubmitBestLowScore(string leaderBoardID)
		{
			int localScore = GetLocalScore(leaderBoardID);
			int serverScore = GetServerScore(leaderBoardID);
			if (localScore <= 0)
			{
				return false;
			}
			if (serverScore <= 0)
			{
				leaderBoardService.SubmitPlayerScore(leaderBoardID, localScore);
				return true;
			}
			if (localScore < serverScore)
			{
				leaderBoardService.SubmitPlayerScore(leaderBoardID, localScore);
				return true;
			}
			return false;
		}

		private int GetServerScore(string leaderBoardID)
		{
			return leaderBoardService.GetPlayerScore(leaderBoardID, 0);
		}

		private int GetLocalScore(string leaderBoardID)
		{
			return dataService.GetInt(leaderBoardID, 0);
		}
	}
}
