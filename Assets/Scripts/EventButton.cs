using UnityEngine;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
	private Text m_Day;

	private Text m_Time;

	private Text m_SnowFlakeCount;

	private GameObject m_TimeCount;

	private GameObject m_NoConnection;

	private Button m_Btn;

	public void Init()
	{
		m_TimeCount = base.transform.GetChild(0).GetChild(0).gameObject;
		m_NoConnection = base.transform.GetChild(0).GetChild(1).gameObject;
		m_Btn = GetComponent<Button>();
		m_SnowFlakeCount = base.transform.GetChild(1).GetComponent<Text>();
		m_Day = m_TimeCount.transform.GetChild(0).GetComponent<Text>();
		m_Time = m_TimeCount.transform.GetChild(1).GetComponent<Text>();
	}

	public void SetTimeCount(int d, int h, int m, int s)
	{
		m_Day.text = d.ToString() + "d,";
		m_Time.text = h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
	}

	public void ReloadSnowFlakeCount()
	{
		m_SnowFlakeCount.text = "x" + GameData.Instance().GetCurrentSnowFlake().ToString();
	}

	public void SetEnable(bool isEnable)
	{
		m_TimeCount.SetActive(isEnable);
		m_NoConnection.SetActive(!isEnable);
		m_Btn.interactable = isEnable;
	}
}
