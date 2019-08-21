using Prime31.ZestKit;
using UnityEngine;
using UnityEngine.UI;

public class TextNumberIncreaseFX : MonoBehaviour
{
	[SerializeField]
	private float m_TimeFX = 1f;

	private Text m_Text;

	private int m_TargetNumber;

	private float m_CurrentNumber;

	private float m_SpeedIncre;

	private Vector3 m_OriginScale;

	private ITween<Vector3> m_Tween;

	private RectTransform m_Transform;

	private void Start()
	{
		m_Transform = GetComponent<RectTransform>();
		m_OriginScale = m_Transform.localScale;
	}

	public void Reset()
	{
		m_CurrentNumber = 0f;
		m_TargetNumber = 0;
		m_SpeedIncre = 0f;
		SetText("0");
	}

	public void Reset(int currentNumber)
	{
		m_CurrentNumber = currentNumber;
		m_TargetNumber = currentNumber;
		m_SpeedIncre = 0f;
		SetText(m_CurrentNumber.ToString());
	}

	public void IncreaseBigNumber(int target)
	{
		if (target > m_TargetNumber)
		{
			m_TargetNumber = target;
			m_SpeedIncre = ((float)m_TargetNumber - m_CurrentNumber) / m_TimeFX;
			if (m_Tween != null && m_Tween.isRunning())
			{
				m_Tween.stop();
			}
			m_Transform.localScale = m_OriginScale * 1.2f;
			m_Tween = m_Transform.ZKlocalScaleTo(m_OriginScale, m_TimeFX).setEaseType(EaseType.QuadIn);
			m_Tween.start();
		}
	}

	public void IncreaseSmallNumber(int target)
	{
		if (target > m_TargetNumber)
		{
			if (m_CurrentNumber == (float)m_TargetNumber)
			{
				m_TargetNumber = target;
				m_CurrentNumber = m_TargetNumber;
				SetText(m_TargetNumber.ToString());
			}
			else
			{
				m_TargetNumber = target;
				m_SpeedIncre = ((float)m_TargetNumber - m_CurrentNumber) / m_TimeFX;
			}
		}
	}

	public void IncreaseToNumber(int target)
	{
		if (target > m_TargetNumber)
		{
			if (target == Mathf.FloorToInt(m_CurrentNumber) + 1)
			{
				Reset(target);
				return;
			}
			m_TargetNumber = target;
			m_SpeedIncre = ((float)m_TargetNumber - m_CurrentNumber) / m_TimeFX;
		}
	}

	private void FixedUpdate()
	{
		if (m_CurrentNumber < (float)m_TargetNumber)
		{
			float num = m_SpeedIncre * Time.fixedDeltaTime;
			m_CurrentNumber += num;
			m_CurrentNumber = Mathf.Min(m_CurrentNumber, m_TargetNumber);
			SetText(Mathf.FloorToInt(m_CurrentNumber).ToString());
		}
	}

	private void SetText(string text)
	{
		if (m_Text == null)
		{
			m_Text = GetComponent<Text>();
		}
		if (m_Text != null)
		{
			m_Text.text = text;
		}
	}
}
