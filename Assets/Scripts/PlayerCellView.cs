using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCellView : EnhancedScrollerCellView
{
	private PlayerCellData m_PlayerData;

	[SerializeField]
	private RawImage m_Image;

	[SerializeField]
	private Text m_ScoreTxt;

	[SerializeField]
	private Text m_NameTxt;

	private string m_PlayFabName;

	private string m_PlayFabAvatarUrl;

	public void SetData(PlayerCellData data)
	{
		m_PlayerData = data;
		data.CurrentView = this;
		UpdateView();
	}

	public void UpdateView()
	{
		m_ScoreTxt.text = m_PlayerData.ScoreTxt;
		m_NameTxt.text = m_PlayerData.NameTxt;
		m_Image.texture = m_PlayerData.AvatarTexture;
	}

	private static string WithMaxLength(string value, int maxLength)
	{
		return value?.Substring(0, Mathf.Min(value.Length, maxLength));
	}
}
