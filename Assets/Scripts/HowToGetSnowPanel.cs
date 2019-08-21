using Prime31.ZestKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HowToGetSnowPanel : MonoBehaviour
{
	private Button m_OkBtn;

	private RectTransform m_Panel;

	private Vector2 m_StartPos = new Vector2(0f, -1200f);

	private Vector2 m_DesPos = new Vector2(0f, 10f);

	public void Init()
	{
		m_Panel = base.transform.GetChild(0).GetComponent<RectTransform>();
		m_OkBtn = m_Panel.GetChild(1).GetComponent<Button>();
		m_OkBtn.onClick.AddListener(delegate
		{
			Singleton<UIManager>.instance.HideHowToGet();
		});
	}

	public void Show(bool isEffect, Action m_AfterShowCallback)
	{
		if (isEffect)
		{
			base.gameObject.SetActive(value: true);
			m_Panel.anchoredPosition = m_StartPos;
			m_Panel.ZKanchoredPositionTo(m_DesPos).setEaseType(EaseType.BackOut).setCompletionHandler(delegate
			{
				if (m_AfterShowCallback != null)
				{
					m_AfterShowCallback();
				}
			})
				.start();
			return;
		}
		base.gameObject.SetActive(value: true);
		if (m_AfterShowCallback != null)
		{
			m_AfterShowCallback();
		}
	}

	public void Hide(bool isEffect, Action m_AfterHideCallback)
	{
		if (isEffect)
		{
			m_Panel.anchoredPosition = m_DesPos;
			m_Panel.ZKanchoredPositionTo(m_StartPos).setEaseType(EaseType.BackIn).setCompletionHandler(delegate
			{
				base.gameObject.SetActive(value: false);
				if (m_AfterHideCallback != null)
				{
					m_AfterHideCallback();
				}
			})
				.start();
			return;
		}
		base.gameObject.SetActive(value: false);
		if (m_AfterHideCallback != null)
		{
			m_AfterHideCallback();
		}
	}
}
