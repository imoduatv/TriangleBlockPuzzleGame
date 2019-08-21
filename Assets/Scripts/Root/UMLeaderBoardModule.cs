using SA.Common.Pattern;
using System;

namespace Root
{
	public class UMLeaderBoardModule : ILeaderBoardService
	{
		private Action m_Callback;

		public void ConnectService()
		{
			SA.Common.Pattern.Singleton<UM_GameServiceManager>.Instance.Connect();
		}

		private bool IsConnected()
		{
			return SA.Common.Pattern.Singleton<UM_GameServiceManager>.Instance.IsConnected;
		}

		private void ConnectThenRunCallback(Action callback)
		{
			SA.Common.Pattern.Singleton<UM_GameServiceManager>.Instance.Connect();
			UM_GameServiceManager.OnPlayerConnected += OnPlayerConnected;
			m_Callback = callback;
		}

		private void OnPlayerConnected()
		{
			UM_GameServiceManager.OnPlayerConnected -= OnPlayerConnected;
			if (m_Callback != null)
			{
				m_Callback();
				m_Callback = null;
			}
		}

		public int GetPlayerScore(string leaderBoardID, int defaultValue)
		{
			UM_Leaderboard lB = GetLB(leaderBoardID);
			int result = defaultValue;
			if (lB != null)
			{
				UM_Score currentPlayerScore = lB.GetCurrentPlayerScore(UM_TimeSpan.ALL_TIME, UM_CollectionType.GLOBAL);
				if (currentPlayerScore != null)
				{
					result = (int)currentPlayerScore.LongScore;
				}
			}
			return result;
		}

		public void ShowLeaderBoard()
		{
			if (IsConnected())
			{
				SA.Common.Pattern.Singleton<UM_GameServiceManager>.Instance.ShowLeaderBoardsUI();
			}
			else
			{
				ConnectThenRunCallback(delegate
				{
					SA.Common.Pattern.Singleton<UM_GameServiceManager>.Instance.ShowLeaderBoardsUI();
				});
			}
		}

		public void ShowLeaderBoard(string leaderBoardID)
		{
			ShowLeaderBoard();
		}

		public void SubmitPlayerScore(string leaderBoardID, int value)
		{
			if (IsConnected())
			{
				SA.Common.Pattern.Singleton<UM_GameServiceManager>.Instance.SubmitScore(leaderBoardID, value, 0L);
			}
			else
			{
				ConnectThenRunCallback(delegate
				{
					SA.Common.Pattern.Singleton<UM_GameServiceManager>.Instance.SubmitScore(leaderBoardID, value, 0L);
				});
			}
		}

		private UM_Leaderboard GetLB(string leaderBoardID)
		{
			return SA.Common.Pattern.Singleton<UM_GameServiceManager>.Instance.GetLeaderboard(leaderBoardID);
		}
	}
}
