using System.Collections;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
	public float m_Speed;

	public Direction m_Direction;

	private Coroutine m_CloudCorou;

	private RectTransform m_CloudRect;

	public void OnEnable()
	{
		if (m_CloudCorou != null)
		{
			StopCoroutine(m_CloudCorou);
			m_CloudCorou = null;
		}
		m_CloudCorou = StartCoroutine(IE_CloudMove());
	}

	private IEnumerator IE_CloudMove()
	{
		if (m_CloudRect == null)
		{
			m_CloudRect = GetComponent<RectTransform>();
		}
		while (true)
		{
			if (m_Direction == Direction.Left)
			{
				m_CloudRect.anchoredPosition -= new Vector2(Time.deltaTime * m_Speed, 0f);
				Vector2 anchoredPosition = m_CloudRect.anchoredPosition;
				if (anchoredPosition.x < -600f)
				{
					RectTransform cloudRect = m_CloudRect;
					Vector2 anchoredPosition2 = m_CloudRect.anchoredPosition;
					cloudRect.anchoredPosition = new Vector2(600f, anchoredPosition2.y);
				}
			}
			else
			{
				m_CloudRect.anchoredPosition += new Vector2(Time.deltaTime * m_Speed, 0f);
				Vector2 anchoredPosition3 = m_CloudRect.anchoredPosition;
				if (anchoredPosition3.x > 600f)
				{
					RectTransform cloudRect2 = m_CloudRect;
					Vector2 anchoredPosition4 = m_CloudRect.anchoredPosition;
					cloudRect2.anchoredPosition = new Vector2(-600f, anchoredPosition4.y);
				}
			}
			yield return null;
		}
	}

	private void OnDisable()
	{
		if (m_CloudCorou != null)
		{
			StopCoroutine(m_CloudCorou);
			m_CloudCorou = null;
		}
	}
}
