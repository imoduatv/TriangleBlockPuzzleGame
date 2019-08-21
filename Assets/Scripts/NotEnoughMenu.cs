using Prime31.ZestKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NotEnoughMenu : MenuUI
{
	[Header("Animation Object")]
	public GameObject m_AnimationObj;

	protected RectTransform m_Animation_RT;

	protected Action m_AfterShowCallback;

	protected Action m_AfterHideCallback;

	protected Vector3 m_DesPos;

	protected Vector3 m_StartPos;

	protected Text m_NotEnoughTxt;

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
			m_NotEnoughTxt = m_AnimationObj.transform.GetChild(0).GetComponent<Text>();
			m_NotEnoughTxt.text = "Not enough diamonds, \nget more?";
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
