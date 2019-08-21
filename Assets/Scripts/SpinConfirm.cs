using Prime31.ZestKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpinConfirm : MonoBehaviour
{
	[SerializeField]
	private RectTransform[] gifts;

	public float m_TimeShow;

	public float m_TimeWait;

	private Vector3 m_DesScale = new Vector3(1f, 1f, 1f);

	private Text[] giftsText;

	private Image[] giftsImg;

	private Color m_NormalColor;

	private Color m_SpecialColor;

	private bool m_IsInit;

	public void SetRewardText(SpinReward[] rewardData)
	{
		for (int i = 0; i < giftsText.Length; i++)
		{
			giftsText[i].text = rewardData[i].reward.ToString();
			if (rewardData[i].tier == TierSpin.T4)
			{
				giftsImg[i].color = m_SpecialColor;
			}
			else
			{
				giftsImg[i].color = m_NormalColor;
			}
		}
	}

	public void Init(Color normalColor, Color specialColor)
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			giftsText = new Text[gifts.Length];
			giftsImg = new Image[gifts.Length];
			for (int i = 0; i < giftsText.Length; i++)
			{
				giftsText[i] = gifts[i].transform.GetChild(1).GetComponent<Text>();
				giftsImg[i] = gifts[i].GetComponent<Image>();
			}
			m_NormalColor = normalColor;
			m_SpecialColor = specialColor;
		}
	}

	public void ShowReward()
	{
		for (int i = 0; i < gifts.Length; i++)
		{
			gifts[i].transform.localScale = Vector3.zero;
		}
		StartCoroutine(IE_ShowReward());
	}

	private IEnumerator IE_ShowReward()
	{
		WaitForSeconds wait = new WaitForSeconds(m_TimeWait);
		for (int i = 0; i < gifts.Length; i++)
		{
			Singleton<SoundManager>.instance.PlayItemSlotAppear();
			gifts[i].transform.ZKlocalScaleTo(m_DesScale, m_TimeShow).setEaseType(EaseType.BackOut).start();
			yield return wait;
		}
	}
}
