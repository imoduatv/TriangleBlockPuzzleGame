using UnityEngine;

public class LongScreenConfig : MonoBehaviour
{
	[SerializeField]
	private GameObject m_LogoInMain;

	[SerializeField]
	private GameObject m_LogoInGameOver;

	[SerializeField]
	private GameObject m_LogoInGame;

	[SerializeField]
	private GameObject m_InfoPanelGameOver;

	[SerializeField]
	private ShopMenu m_Shop;

	[SerializeField]
	private DailyMenu m_DailyMenu;

	[SerializeField]
	private RectTransform priceTxt;

	[SerializeField]
	private GameObject m_ExchangePanel;

	[SerializeField]
	private GameObject m_HowToGetPanel;

	[SerializeField]
	private Transform m_BGBoardXmas;

	public float m_BoardScreenShotScale = 65f;

	public void SetUIForLongScreen()
	{
		float num = (float)Screen.height * 1f / (float)Screen.width;
		if (num > 1.882353f)
		{
			m_LogoInGameOver.SetActive(value: false);
			m_LogoInMain.SetActive(value: false);
			m_LogoInGame.SetActive(value: false);
			m_InfoPanelGameOver.transform.localScale = Vector3.one * 0.8f;
			m_ExchangePanel.transform.localScale = Vector3.one * 0.8f;
			m_HowToGetPanel.transform.localScale = Vector3.one * 0.8f;
			m_Shop.SetUIForLongScreen();
			m_BoardScreenShotScale = 76f;
			m_DailyMenu.ScaleLongScreen();
			if (num > 1.97802186f && num < 2.142857f)
			{
				m_BGBoardXmas.transform.localScale = Vector3.one * 0.48f;
			}
			if (num > 2.142857f)
			{
				m_BGBoardXmas.transform.localScale = Vector3.one * 0.45f;
			}
		}
		else
		{
			m_BoardScreenShotScale = 65f;
		}
	}
}
