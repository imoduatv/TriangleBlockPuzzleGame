using Prime31.ZestKit;
using System;
using UnityEngine;

public class ComingSoon : MenuUI
{
	[Header("Animation Object")]
	public GameObject m_AnimationObj;

	private RectTransform m_Animation_RT;

	private Action m_AfterShowCallback;

	private Action m_AfterHideCallback;

	private Vector3 m_DesPos;

	private Vector3 m_StartPos;

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
		}
	}

	public override void Show(Action afterShowCallback = null)
	{
		Init();
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
