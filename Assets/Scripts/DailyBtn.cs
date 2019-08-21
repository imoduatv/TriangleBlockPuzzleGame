using UnityEngine;
using UnityEngine.UI;

public class DailyBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject m_ShowReward;

	[SerializeField]
	private GameObject m_Collected;

	[SerializeField]
	private GameObject m_NotCollected;

	[SerializeField]
	private Text m_DayTxt;

	[SerializeField]
	private GameObject m_GoldBorder;

	[SerializeField]
	private Image m_DailyBtnImg;

	private Text m_MoneyRewardTxt;

	public void ShowReward(bool isShow)
	{
	}

	public void SetDailyText(bool isToday, int day)
	{
		m_DayTxt.text = day.ToString();
	}

	public void SetStatDaily(DailyStat stat, int day)
	{
		switch (stat)
		{
		case DailyStat.Available:
			SetDailyText(isToday: true, day);
			m_Collected.SetActive(value: false);
			m_NotCollected.SetActive(value: false);
			m_GoldBorder.SetActive(value: true);
			break;
		case DailyStat.NotAvailable:
			SetDailyText(isToday: false, day);
			m_Collected.SetActive(value: false);
			m_NotCollected.SetActive(value: false);
			m_GoldBorder.SetActive(value: true);
			break;
		case DailyStat.Collected:
			SetDailyText(isToday: false, day);
			m_Collected.SetActive(value: true);
			m_NotCollected.SetActive(value: false);
			m_ShowReward.SetActive(value: false);
			m_GoldBorder.SetActive(value: false);
			break;
		case DailyStat.NotCollected:
			ShowReward(isShow: false);
			SetDailyText(isToday: false, day);
			m_Collected.SetActive(value: false);
			m_NotCollected.SetActive(value: true);
			m_ShowReward.SetActive(value: false);
			m_GoldBorder.SetActive(value: false);
			break;
		}
	}

	public void SetMoneyRewardText(int value)
	{
		if (m_MoneyRewardTxt == null)
		{
			m_MoneyRewardTxt = m_ShowReward.transform.GetChild(1).GetComponent<Text>();
		}
		m_MoneyRewardTxt.text = value.ToString();
	}
}
