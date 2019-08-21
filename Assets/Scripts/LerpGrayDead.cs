using UnityEngine;

public class LerpGrayDead : MonoBehaviour
{
	private Material m_grayMat;

	private SpriteRenderer m_sprRenderer;

	[SerializeField]
	private Color m_GrayColor;

	private Color m_CurrentColor;

	private bool m_IsLerp;

	private bool m_IsSolidColorTheme = true;

	private float m_LerpTime = 0.2f;

	private float m_TimeTmp = 3f;

	private void Update()
	{
		if (m_TimeTmp < m_LerpTime)
		{
			m_TimeTmp += Time.deltaTime;
			if (m_IsSolidColorTheme)
			{
				m_sprRenderer.color = Color.Lerp(m_CurrentColor, m_GrayColor, m_TimeTmp / m_LerpTime);
			}
			else
			{
				m_grayMat.SetFloat("_EffectAmount", m_TimeTmp / m_LerpTime);
			}
		}
	}

	public void StartLerpGray(bool isSolidColorTheme, float lerpTime = 0.2f)
	{
		m_sprRenderer = GetComponent<SpriteRenderer>();
		m_IsSolidColorTheme = isSolidColorTheme;
		m_LerpTime = lerpTime;
		if (isSolidColorTheme)
		{
			m_TimeTmp = 0f;
			m_CurrentColor = m_sprRenderer.color;
		}
		else
		{
			m_grayMat = m_sprRenderer.material;
			m_TimeTmp = 0f;
			m_grayMat.SetFloat("_EffectAmount", m_TimeTmp);
		}
	}

	public void ResetColor()
	{
		if (m_IsSolidColorTheme)
		{
			m_sprRenderer.color = m_CurrentColor;
		}
		else
		{
			m_grayMat.SetFloat("_EffectAmount", 0f);
		}
	}
}
