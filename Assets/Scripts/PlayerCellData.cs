using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCellData
{
	public const string FB_NAME_REQUEST = "first_name";

	public string FbUserId;

	public int Score;

	public string NameTxt;

	public string ScoreTxt;

	public Texture AvatarTexture;

	public PlayerCellView CurrentView;

	private string m_PlayFabName;

	private string m_PlayFabAvatarUrl;

	private static Dictionary<string, Texture> s_UsersAvatar = new Dictionary<string, Texture>();

	private static Dictionary<string, string> s_UsersName = new Dictionary<string, string>();

	public PlayerCellData()
	{
		FbUserId = null;
		Score = 0;
	}

	public PlayerCellData(string fbUserId, int score)
	{
		FbUserId = fbUserId;
		Score = score;
		ScoreTxt = score.ToString();
		NameTxt = string.Empty;
		AvatarTexture = null;
		UpdateView();
		GetFaceBookAccountInfo(fbUserId);
	}

	private void GetFaceBookAccountInfo(string fbId)
	{
		if (!string.IsNullOrEmpty(fbId))
		{
			if (s_UsersAvatar.ContainsKey(fbId))
			{
				Texture pictureTexture = s_UsersAvatar[fbId];
				SetPlayerCellAvatar(fbId, pictureTexture);
			}
			else
			{
				FBGraph.GetPlayerAvatar(fbId, delegate(IGraphResult result)
				{
					if (result.Error != null)
					{
						UnityEngine.Debug.LogError(result.Error);
					}
					else
					{
						string avatarUrl = GraphUtil.DeserializePictureURL(result.ResultDictionary);
						SetPlayerCellAvatar(fbId, avatarUrl);
					}
				});
			}
			if (s_UsersName.ContainsKey(fbId))
			{
				SetPlayerCellName(fbId, s_UsersName[fbId]);
			}
			else
			{
				FBGraph.GetPlayerInfo(fbId, delegate(IGraphResult result)
				{
					if (result.Error != null)
					{
						UnityEngine.Debug.LogError(result.Error);
					}
					else
					{
						if (!result.ResultDictionary.TryGetValue("first_name", out string value))
						{
							value = string.Empty;
						}
						SetPlayerCellName(fbId, value);
					}
				});
			}
		}
	}

	private void SetPlayerCellAvatar(string fbId, string avatarUrl)
	{
		GraphUtil.LoadImgFromURL(avatarUrl, delegate(Texture pictureTexture)
		{
			SetPlayerCellAvatar(fbId, pictureTexture);
		}, Singleton<GameManager>.Instance);
	}

	private void SetPlayerCellAvatar(string fbId, Texture pictureTexture)
	{
		if (pictureTexture != null)
		{
			s_UsersAvatar[fbId] = pictureTexture;
			AvatarTexture = pictureTexture;
			UpdateView();
		}
	}

	private void SetPlayerCellName(string fbId, string name)
	{
		s_UsersName[fbId] = name;
		NameTxt = name;
		UpdateView();
	}

	private void UpdateView()
	{
		if (CurrentView != null)
		{
			CurrentView.UpdateView();
		}
	}
}
