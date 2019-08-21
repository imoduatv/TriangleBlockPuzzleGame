using Prime31.ZestKit;
using UnityEngine;
using UnityEngine.UI;

public class ExchangePanel : MonoBehaviour
{
	private ExchangeUI[] m_ExchangeUIs;

	private Button m_CloseBtn;

	private Button m_HowToGetBtn;

	private RectTransform m_Panel;

	private Text m_Day;

	private Text m_Hour;

	private Text m_Min;

	private Text m_Sec;

	private Text m_SnowFlakeCount;

	private Vector2 m_StartPos = new Vector2(0f, -1200f);

	public void Init()
	{
		m_ExchangeUIs = GetComponentsInChildren<ExchangeUI>();
		m_Panel = base.transform.GetChild(0).GetComponent<RectTransform>();
		m_CloseBtn = m_Panel.GetChild(3).GetComponent<Button>();
		m_HowToGetBtn = m_Panel.GetChild(4).GetComponent<Button>();
		Transform child = m_Panel.transform.GetChild(1);
		Transform child2 = child.transform.GetChild(0);
		Transform child3 = child.transform.GetChild(1);
		m_Day = child2.GetChild(0).GetComponent<Text>();
		m_Hour = child2.GetChild(1).GetComponent<Text>();
		m_Min = child2.GetChild(2).GetComponent<Text>();
		m_Sec = child2.GetChild(3).GetComponent<Text>();
		m_SnowFlakeCount = child3.GetChild(0).GetComponent<Text>();
		for (int i = 0; i < m_ExchangeUIs.Length; i++)
		{
			m_ExchangeUIs[i].Init(this);
		}
		m_CloseBtn.onClick.AddListener(delegate
		{
			Singleton<UIManager>.instance.HideExchangePanel();
		});
		m_HowToGetBtn.onClick.AddListener(delegate
		{
			Singleton<UIManager>.instance.ShowHowToGet();
		});
	}

	public void Hide(bool isEffect)
	{
		if (isEffect)
		{
			m_Panel.ZKanchoredPositionTo(m_StartPos).setEaseType(EaseType.BackIn).setCompletionHandler(delegate
			{
				base.gameObject.SetActive(value: false);
			})
				.start();
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void Show(bool isEffect)
	{
		if (isEffect)
		{
			base.gameObject.SetActive(value: true);
			m_Panel.anchoredPosition = m_StartPos;
			m_Panel.ZKanchoredPositionTo(Vector2.zero).setEaseType(EaseType.BackOut).start();
		}
		else
		{
			base.gameObject.SetActive(value: true);
		}
		ReloadSnowFlakeCount();
	}

	public void SetTimeCount(int d, int h, int m, int s)
	{
		m_Day.text = d.ToString() + ((d <= 1) ? " day" : " days") + ",";
		m_Hour.text = h.ToString("00");
		m_Min.text = m.ToString("00");
		m_Sec.text = s.ToString("00");
	}

	public void ReloadSnowFlakeCount()
	{
		m_SnowFlakeCount.text = GameData.Instance().GetCurrentSnowFlake().ToString();
	}
}
