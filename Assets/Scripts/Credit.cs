using Prime31.ZestKit;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
	[Header("UI Elements")]
	[SerializeField]
	private Image m_BackGround;

	[SerializeField]
	private RectTransform m_CreditTxt;

	[Header("Config Credit")]
	[SerializeField]
	private float m_StartAnchorY;

	[SerializeField]
	private float m_TimeFade;

	[SerializeField]
	private float m_TimeRun;

	private bool m_IsInit;

	private float m_CreditLength;

	private float m_CanvasLength = 1280f;

	private Vector2 m_StartPos;

	private Vector2 m_DesPos;

	private ITween<float> m_FadeTween;

	private ITween<Vector2> m_RunTween;

	private void Init()
	{
		if (!m_IsInit)
		{
			m_CreditLength = m_CreditTxt.rect.height;
			m_StartPos = new Vector2(0f, m_StartAnchorY);
			m_DesPos = new Vector2(0f, m_CanvasLength / 2f + m_CreditLength / 2f + 100f);
		}
	}

	public void ShowCredit()
	{
		Init();
		if (m_FadeTween != null)
		{
			m_FadeTween.stop();
			m_FadeTween = null;
		}
		if (m_RunTween != null)
		{
			m_RunTween.stop();
			m_RunTween = null;
		}
		base.gameObject.SetActive(value: true);
		Color color = m_BackGround.color;
		color.a = 0f;
		m_BackGround.color = color;
		m_CreditTxt.anchoredPosition = m_StartPos;
		m_FadeTween = m_BackGround.ZKalphaTo(1f, m_TimeFade).setEaseType(EaseType.Linear);
		m_FadeTween.start();
		m_RunTween = m_CreditTxt.ZKanchoredPositionTo(m_DesPos, m_TimeRun).setEaseType(EaseType.Linear).setCompletionHandler(delegate
		{
			HideCredit();
		});
		m_RunTween.start();
	}

	public void HideCredit()
	{
		Init();
		m_BackGround.ZKalphaTo(0f, m_TimeFade).setEaseType(EaseType.Linear).setCompletionHandler(delegate
		{
			base.gameObject.SetActive(value: false);
		})
			.start();
	}
}
