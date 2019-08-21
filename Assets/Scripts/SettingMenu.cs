using Prime31.ZestKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MenuUI
{
	[SerializeField]
	private GameObject m_AnimObj;

	[Header("UI Elements")]
	[SerializeField]
	private Image m_BackgroundImg;

	[SerializeField]
	private Image m_SoundImg;

	[SerializeField]
	private Image m_RestoreBtnImg;

	[SerializeField]
	private Image m_RateBtnImg;

	[SerializeField]
	private Image m_CloseBtn;

	[SerializeField]
	private Image m_LeaderBoardImg;

	private RectTransform m_AnimObj_RT;

	private Vector3 m_StartPos;

	private Vector3 m_DesPos;

	private ThemeElement m_SoundOn;

	private ThemeElement m_SoundOff;

	private Action m_AfterShowCallback;

	private Action m_AfterHideCallback;

	public override void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			m_AnimObj_RT = m_AnimObj.GetComponent<RectTransform>();
			m_DesPos = m_AnimObj_RT.anchoredPosition;
			Vector2 anchoredPosition = m_AnimObj_RT.anchoredPosition;
			float y = anchoredPosition.y - m_AnimObj_RT.rect.height - 10f;
			m_StartPos = new Vector3(0f, y);
			m_AnimObj_RT.anchoredPosition = m_StartPos;
		}
	}

	private void OnEnable()
	{
		Init();
	}

	public override void Show(Action afterShowCallback = null)
	{
		Init();
		if (!m_IsAnimatingReward)
		{
			m_AfterShowCallback = afterShowCallback;
			m_IsAnimatingReward = true;
			m_AnimObj_RT.anchoredPosition = m_StartPos;
			m_AnimObj_RT.ZKanchoredPositionTo(m_DesPos, 0.2f).setEaseType(EaseType.BackOut).setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterShowCallback != null)
				{
					m_AfterShowCallback();
				}
			})
				.start();
		}
	}

	public override void Hide(Action afterHideCallback = null)
	{
		Init();
		if (!m_IsAnimatingReward)
		{
			m_AfterHideCallback = afterHideCallback;
			m_IsAnimatingReward = true;
			m_AnimObj_RT.anchoredPosition = m_DesPos;
			m_AnimObj_RT.ZKanchoredPositionTo(m_StartPos, 0.2f).setEaseType(EaseType.BackIn).setCompletionHandler(delegate
			{
				m_IsAnimatingReward = false;
				if (m_AfterHideCallback != null)
				{
					m_AfterHideCallback();
				}
			})
				.start();
		}
	}

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
		SetUI(m_BackgroundImg, dictThemeElement["Background"]);
		SetUI(m_RestoreBtnImg, dictThemeElement["RestoreBtn"]);
		SetUI(m_RateBtnImg, dictThemeElement["RateBtn"]);
		SetUI(m_CloseBtn, dictThemeElement["CloseBtn"]);
		m_SoundOff = dictThemeElement["SoundOff"];
		m_SoundOn = dictThemeElement["SoundOn"];
	}

	public void SetImgSound(bool isSoundOn)
	{
		SetUI(m_SoundImg, (!isSoundOn) ? m_SoundOff : m_SoundOn);
	}
}
