namespace Root
{
	public interface ILeaderBoardService
	{
		void ConnectService();

		int GetPlayerScore(string leaderBoardID, int defaultValue);

		void SubmitPlayerScore(string leaderBoardID, int value);

		void ShowLeaderBoard();

		void ShowLeaderBoard(string leaderBoardID);
	}
}
