using Prime31.ZestKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmMenu : MenuUI
{
	[Header("Animation Object")]
	public GameObject m_AnimationObj;

	[SerializeField]
	private Text m_ComfirmTxt;

	private RectTransform m_Animation_RT;

	private Action m_AfterShowCallback;

	private Action m_AfterHideCallback;

	private Vector3 m_DesPos;

	private Vector3 m_StartPos;

	private ThemeManager m_ThemeManager;

	private void OnEnable()
	{
		Init();
	}

	public override void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			m_DesPos = Vector3.zero;
			m_StartPos = new Vector3(0f, -800f, 0f);
			m_Animation_RT = m_AnimationObj.GetComponent<RectTransform>();
			m_Animation_RT.anchoredPosition = m_StartPos;
			m_ThemeManager = Singleton<ThemeManager>.instance;
		}
	}

	public void Show(ThemeName theme, Action afterShowCallback = null)
	{
		Init();
		ThemeData themeData = m_ThemeManager.GetThemeData(theme);
		string shopName = themeData.m_ShopName;
		int themePrice = themeData.ThemePrice;
		m_ComfirmTxt.text = "Buy theme " + shopName + " for \n" + themePrice;
		if (!m_IsAnimatingReward)
		{
			m_IsAnimatingReward = true;
			m_AfterShowCallback = afterShowCallback;
			m_Animation_RT.anchoredPosition = m_StartPos;
			m_Animation_RT.ZKanchoredPositionTo(m_DesPos).setEaseType(EaseType.BackOut).setCompletionHandler(delegate
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
			m_IsAnimatingReward = true;
			m_AfterHideCallback = afterHideCallback;
			m_Animation_RT.anchoredPosition = m_DesPos;
			m_Animation_RT.ZKanchoredPositionTo(m_StartPos).setEaseType(EaseType.BackIn).setCompletionHandler(delegate
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
}
