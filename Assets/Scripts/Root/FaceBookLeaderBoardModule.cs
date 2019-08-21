using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Root
{
	public class FaceBookLeaderBoardModule : ILeaderBoardService
	{
		public class ScoreData
		{
			public string board_id;

			public string fb_id;

			public int score;
		}

		private Dictionary<string, int> m_Scores = new Dictionary<string, int>();

		private PlayersScrollController m_ScrollController;

		private IScoreService m_ScoreService;

		private ILeaderBoardService m_UMLeaderBoard;

		private const string GetScoreUrl = "http://api.dtamobile.com/score/get_score";

		private const string SubmitScoreUrl = "http://api.dtamobile.com/score/submit_score";

		private UTF8Encoding m_Utf8Encoding = new UTF8Encoding();

		private MD5CryptoServiceProvider m_MD5 = new MD5CryptoServiceProvider();

		private string m_DtaAppId;

		private string m_SecretKey;

		private IJsonService m_Json;

		private string m_BoardId;

		private Dictionary<string, WWW> m_RunningWww = new Dictionary<string, WWW>();

		private string m_LoginUserFbId;

		public FaceBookLeaderBoardModule(PlayersScrollController viewer, string dtaAppId, string secretKey, string leaderBoardId, IJsonService json)
		{
			m_ScrollController = viewer;
			m_DtaAppId = dtaAppId;
			m_SecretKey = secretKey;
			m_Json = json;
			m_BoardId = leaderBoardId;
		}

		public void SetScoreService(IScoreService scoreService)
		{
			m_ScoreService = scoreService;
		}

		public void SetUMLeaderBoard(ILeaderBoardService lbService)
		{
			m_UMLeaderBoard = lbService;
		}

		public void ConnectService()
		{
			m_UMLeaderBoard.ConnectService();
		}

		private void Login(string leaderBoardId)
		{
			if (!FB.IsInitialized)
			{
				return;
			}
			if (!FB.IsLoggedIn)
			{
				FBLogin.PromptForLogin(delegate
				{
					m_LoginUserFbId = AccessToken.CurrentAccessToken.UserId;
					if (!m_ScoreService.SubmitBestHighScore(leaderBoardId))
					{
						RequestPlayerScore();
					}
				});
				return;
			}
			m_LoginUserFbId = AccessToken.CurrentAccessToken.UserId;
			if (!m_ScoreService.SubmitBestHighScore(leaderBoardId))
			{
				RequestPlayerScore();
			}
		}

		private IEnumerator IERequestPlayerScore(string playerId)
		{
			WWWForm form = new WWWForm();
			form.AddField("fbId", playerId);
			form.AddField("appId", m_DtaAppId);
			form.AddField("boardId", m_BoardId);
			WWW www = new WWW("http://api.dtamobile.com/score/get_score", form);
			m_RunningWww[playerId] = www;
			yield return www;
			if (www.error == null)
			{
				UnityEngine.Debug.Log("response: " + www.text);
				ScoreData scoreData;
				try
				{
					scoreData = m_Json.FromJson<ScoreData>(www.text);
				}
				catch
				{
					scoreData = null;
				}
				if (scoreData != null && scoreData.board_id == m_BoardId && scoreData.fb_id == playerId && scoreData.score > 0)
				{
					m_Scores[playerId] = scoreData.score;
				}
			}
			else
			{
				UnityEngine.Debug.LogError(www.error);
			}
			m_RunningWww.Remove(playerId);
			if (m_RunningWww.Count == 0)
			{
				m_ScrollController.Show(m_Scores);
			}
		}

		private void RequestPlayerScore()
		{
			FBGraph.GetFriends(delegate(IGraphResult result)
			{
				if (result.Error != null)
				{
					UnityEngine.Debug.LogError(result.Error);
				}
				else
				{
					UnityEngine.Debug.Log("GetFriends respone: " + result.RawResult);
					List<object> list = new List<object>();
					if (result.ResultDictionary.TryGetValue("data", out object value))
					{
						list = (List<object>)value;
					}
					foreach (object item in list)
					{
						Dictionary<string, object> dictionary = (Dictionary<string, object>)item;
						string text = (string)dictionary["id"];
						if (!m_RunningWww.ContainsKey(text))
						{
							RunCoroutine(IERequestPlayerScore(text));
						}
					}
					if (!m_RunningWww.ContainsKey(m_LoginUserFbId))
					{
						RunCoroutine(IERequestPlayerScore(m_LoginUserFbId));
					}
				}
			});
		}

		private void RunCoroutine(IEnumerator ienum)
		{
			m_ScrollController.StartCoroutine(ienum);
		}

		public int GetPlayerScore(string leaderBoardID, int defaultValue)
		{
			if (!FB.IsLoggedIn)
			{
				return m_UMLeaderBoard.GetPlayerScore(leaderBoardID, defaultValue);
			}
			m_LoginUserFbId = AccessToken.CurrentAccessToken.UserId;
			if (string.IsNullOrEmpty(m_LoginUserFbId))
			{
				return defaultValue;
			}
			if (m_Scores.ContainsKey(m_LoginUserFbId))
			{
				return m_Scores[m_LoginUserFbId];
			}
			return defaultValue;
		}

		public void ShowLeaderBoard()
		{
			m_UMLeaderBoard.ShowLeaderBoard();
		}

		public void ShowLeaderBoard(string leaderBoardID)
		{
			if (FB.IsLoggedIn)
			{
				m_LoginUserFbId = AccessToken.CurrentAccessToken.UserId;
				if (m_Scores.Count > 0)
				{
					m_ScrollController.Show(m_Scores);
					m_ScoreService.SubmitBestHighScore(leaderBoardID);
				}
				else
				{
					RequestPlayerScore();
				}
			}
			else
			{
				Login(leaderBoardID);
			}
		}

		public void SubmitPlayerScore(string leaderBoardID, int value)
		{
			if (FB.IsLoggedIn)
			{
				m_LoginUserFbId = AccessToken.CurrentAccessToken.UserId;
				RunCoroutine(IESubmitPlayerScore(leaderBoardID, value));
			}
			else
			{
				m_UMLeaderBoard.SubmitPlayerScore(leaderBoardID, value);
			}
		}

		private IEnumerator IESubmitPlayerScore(string leaderBoardID, int value)
		{
			WWWForm form = new WWWForm();
			form.AddField("fbId", m_LoginUserFbId);
			form.AddField("appId", m_DtaAppId);
			form.AddField("boardId", m_BoardId);
			form.AddField("score", value);
			string key = Md5(m_SecretKey + value);
			form.AddField("key", key);
			WWW www = new WWW("http://api.dtamobile.com/score/submit_score", form);
			yield return www;
			if (www.error == null)
			{
				UnityEngine.Debug.Log("response: " + www.text);
			}
			else
			{
				UnityEngine.Debug.LogError(www.error);
			}
			RequestPlayerScore();
		}

		public string Md5(string strToEncrypt)
		{
			byte[] bytes = m_Utf8Encoding.GetBytes(strToEncrypt);
			byte[] array = m_MD5.ComputeHash(bytes);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text += Convert.ToString(array[i], 16).PadLeft(2, '0');
			}
			return text.PadLeft(32, '0');
		}
	}
}
