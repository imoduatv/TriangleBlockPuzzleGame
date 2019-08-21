using Prime31.ZestKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueMenu : MenuUI
{
	[Header("UI Elements")]
	[SerializeField]
	private Image m_BGImg;

	[SerializeField]
	private Text m_GOTxt;

	[SerializeField]
	private Text m_CurrentScore;

	[SerializeField]
	private Text m_CountTxt;

	[SerializeField]
	private Image m_CircleImg;

	[SerializeField]
	private Image m_CircleInImg;

	[SerializeField]
	private Text m_NoThanksTxt;

	[SerializeField]
	private Image m_VideoBtnImg;

	[SerializeField]
	private Image m_VideoIcon;

	[SerializeField]
	private Text m_ContinueTxt;

	[SerializeField]
	private Button m_HideContinueBtn;

	[SerializeField]
	private Button m_WatchVideoBtn;

	[SerializeField]
	private EaseType typeAnim;

	[SerializeField]
	public int m_TimeCount = 5;

	private Coroutine m_CountimeCorou;

	private Coroutine m_CircleCorou;

	private Coroutine m_NoThankCorou;

	private Coroutine m_AnimBtnCorou;

	private WaitForSeconds oneSec = new WaitForSeconds(1f);

	public override void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
		SetUI(m_BGImg, dictThemeElement["Background"]);
		SetUI(m_GOTxt, dictThemeElement["GOTxt"]);
		SetUI(m_CurrentScore, dictThemeElement["ScoreTxt"]);
		SetUI(m_CircleImg, dictThemeElement["CircleOut"]);
		SetUI(m_CircleInImg, dictThemeElement["CircleIn"]);
		SetUI(m_CountTxt, dictThemeElement["CountTxt"]);
		SetUI(m_VideoBtnImg, dictThemeElement["VideoBtn"]);
		SetUI(m_NoThanksTxt, dictThemeElement["NoThankTxt"]);
		SetUI(m_VideoIcon, dictThemeElement["IconVideo"]);
		SetUI(m_ContinueTxt, dictThemeElement["IconVideo"]);
		m_BGImg.sprite = dictThemeElement["Background"].SpriteUI;
	}

	public void StartCountTime()
	{
		m_NoThanksTxt.transform.localScale = Vector3.zero;
		m_HideContinueBtn.interactable = false;
		m_CircleImg.fillAmount = 1f;
		m_WatchVideoBtn.transform.localScale = Vector3.one;
		StartCountTimeNumber();
		StartCountCircle();
		DelayShowNoThank();
		StartAnimBtn();
	}

	private IEnumerator IE_AnimBtn()
	{
		for (int timeTmp = m_TimeCount; timeTmp > 0; timeTmp--)
		{
			AnimateWatchBtn();
			yield return oneSec;
		}
	}

	private void AnimateWatchBtn()
	{
		m_WatchVideoBtn.transform.ZKlocalScaleTo(Vector3.one * 1.1f, 0.15f).setEaseType(typeAnim).setLoops(LoopType.PingPong, 2)
			.start();
	}

	private void StartAnimBtn()
	{
		if (m_AnimBtnCorou != null)
		{
			StopCoroutine(m_AnimBtnCorou);
			m_AnimBtnCorou = null;
		}
		m_AnimBtnCorou = StartCoroutine(IE_AnimBtn());
	}

	private void StartCountTimeNumber()
	{
		if (m_CountimeCorou != null)
		{
			StopCoroutine(m_CountimeCorou);
			m_CountimeCorou = null;
		}
		m_CountimeCorou = StartCoroutine(IE_CountTime());
	}

	private void StartCountCircle()
	{
		if (m_CircleCorou != null)
		{
			StopCoroutine(m_CircleCorou);
			m_CircleCorou = null;
		}
		m_CircleCorou = StartCoroutine(IE_Circle());
	}

	private void DelayShowNoThank()
	{
		if (m_NoThankCorou != null)
		{
			StopCoroutine(m_NoThankCorou);
			m_NoThankCorou = null;
		}
		m_NoThankCorou = StartCoroutine(IE_ShowNoThank());
	}

	private IEnumerator IE_CountTime()
	{
		for (int m_Timetmp = m_TimeCount; m_Timetmp > 0; m_Timetmp--)
		{
			m_CountTxt.text = m_Timetmp.ToString();
			yield return oneSec;
		}
		Singleton<UIManager>.instance.OnClickNoContinue();
	}

	private IEnumerator IE_Circle()
	{
		for (float m_Timetmp = m_TimeCount; m_Timetmp > 0f; m_Timetmp -= Time.deltaTime)
		{
			m_CircleImg.fillAmount = m_Timetmp / (float)m_TimeCount;
			yield return null;
		}
	}

	private IEnumerator IE_ShowNoThank()
	{
		yield return oneSec;
		m_HideContinueBtn.interactable = true;
		m_NoThanksTxt.transform.ZKlocalScaleTo(Vector3.one, 0.2f).start();
	}

	public void ShowContinue()
	{
		Singleton<ServicesManager>.instance.ShowBannerPause();
		base.gameObject.SetActive(value: true);
		m_CurrentScore.text = GameData.Instance().GetCurrentScore().ToString();
		StartCountTime();
	}

	public void HideContinue()
	{
		Singleton<ServicesManager>.instance.HideBannerPause();
		StopAllCorou();
	}

	public void StopAllCorou()
	{
		if (m_CircleCorou != null)
		{
			StopCoroutine(m_CircleCorou);
			m_CircleCorou = null;
		}
		if (m_CountimeCorou != null)
		{
			StopCoroutine(m_CountimeCorou);
			m_CountimeCorou = null;
		}
		if (m_NoThankCorou != null)
		{
			StopCoroutine(m_NoThankCorou);
			m_NoThankCorou = null;
		}
		if (m_AnimBtnCorou != null)
		{
			StopCoroutine(m_AnimBtnCorou);
			m_AnimBtnCorou = null;
		}
	}
}
