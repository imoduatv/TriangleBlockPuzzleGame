namespace Root
{
	public interface IScoreService
	{
		int GetBestHighScore(string leaderBoardID);

		int GetBestHighScore(string leaderBoardID, out bool isServerScore);

		int GetBestLowScore(string leaderBoardID);

		int GetBestLowScore(string leaderBoardID, out bool isServerScore);

		bool SetNewBestHighScore(int score, string leaderBoardID);

		bool SetNewBestLowScore(int score, string leaderBoardID);

		bool SubmitBestHighScore(string leaderBoardID);

		bool SubmitBestLowScore(string leaderBoardID);
	}
}
