using Prime31.ZestKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectCombo : MonoBehaviour
{
	[Header("Animation Object")]
	[SerializeField]
	private Image m_BG;

	[SerializeField]
	private Text m_TextEffect;

	private string[][] m_EffectTextArr;

	private RectTransform m_Text_RT;

	private Action m_AfterShowCallback;

	private TweenChain m_Tween;

	private bool m_IsInit;

	private Vector3 m_StartPos;

	private Vector3 m_DesPos;

	private Vector3 m_StartScale;

	public float m_TimeScale;

	public float m_TimeFly;

	public float m_TimeDelay;

	public void Init()
	{
		if (!m_IsInit)
		{
			m_IsInit = true;
			base.gameObject.SetActive(value: false);
			m_Text_RT = m_TextEffect.GetComponent<RectTransform>();
			m_StartPos = new Vector3(1000f, 0f, 0f);
			m_DesPos = new Vector3(-1000f, 0f, 0f);
			m_StartScale = new Vector3(1f, 0f, 1f);
			m_EffectTextArr = new string[5][];
			m_EffectTextArr[0] = new string[3]
			{
				"Good Job!",
				"Great!",
				"Well done!"
			};
			m_EffectTextArr[1] = new string[3]
			{
				"Awesome!",
				"Amazing!",
				"Perfect!"
			};
			m_EffectTextArr[2] = new string[2]
			{
				"Incredible!",
				"Astonishing!"
			};
			m_EffectTextArr[3] = new string[2]
			{
				"Unbelievable!",
				"Great job!"
			};
			m_EffectTextArr[4] = new string[1]
			{
				"Trigon amazing!"
			};
		}
	}

	public void Show(string text, Action afterShowCallback = null)
	{
		Init();
		if (m_Tween != null)
		{
			m_Tween.stop();
			m_Tween = null;
		}
		base.gameObject.SetActive(value: true);
		m_TextEffect.text = text;
		m_Text_RT.anchoredPosition = m_StartPos;
		m_BG.transform.localScale = m_StartScale;
		m_Tween = new TweenChain();
		m_Tween.appendTween(m_BG.transform.ZKlocalScaleTo(Vector3.one, m_TimeScale).setEaseType(EaseType.ElasticIn));
		m_Tween.appendTween(m_Text_RT.ZKanchoredPositionTo(Vector3.zero, m_TimeFly).setEaseType(EaseType.BackOut));
		m_Tween.appendTween(m_Text_RT.ZKanchoredPositionTo(m_DesPos, m_TimeFly).setEaseType(EaseType.BackIn).setDelay(m_TimeDelay));
		m_Tween.appendTween(m_BG.transform.ZKlocalScaleTo(m_StartScale, m_TimeScale).setEaseType(EaseType.ElasticOut));
		m_Tween.setCompletionHandler(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		m_Tween.start();
	}

	public void Show(int clearCount, Action afterShowCallback = null)
	{
		Init();
		if (m_Tween != null)
		{
			m_Tween.stop();
			m_Tween = null;
		}
		base.gameObject.SetActive(value: true);
		if (clearCount > 5)
		{
			clearCount = 5;
		}
		string[] array = m_EffectTextArr[clearCount - 2];
		int num = UnityEngine.Random.Range(0, array.Length);
		string text = array[num];
		m_TextEffect.text = text;
		m_Text_RT.anchoredPosition = m_StartPos;
		m_BG.transform.localScale = m_StartScale;
		m_Tween = new TweenChain();
		m_Tween.appendTween(m_BG.transform.ZKlocalScaleTo(Vector3.one, m_TimeScale).setEaseType(EaseType.ElasticIn));
		m_Tween.appendTween(m_Text_RT.ZKanchoredPositionTo(Vector3.zero, m_TimeFly).setEaseType(EaseType.BackOut));
		m_Tween.appendTween(m_Text_RT.ZKanchoredPositionTo(m_DesPos, m_TimeFly).setEaseType(EaseType.BackIn).setDelay(m_TimeDelay));
		m_Tween.appendTween(m_BG.transform.ZKlocalScaleTo(m_StartScale, m_TimeScale).setEaseType(EaseType.ElasticOut));
		m_Tween.setCompletionHandler(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		m_Tween.start();
	}

	public void SetThemeUI(Dictionary<string, ThemeElement> ingameThemeDict)
	{
		SetUI(m_BG, ingameThemeDict["ComboBG"]);
		SetUI(m_TextEffect, ingameThemeDict["ComboTxt"]);
	}

	public void SetColorBGCombo(Color color)
	{
		Color color2 = new Color(color.r, color.g, color.b, 0.7f);
		m_BG.color = color2;
	}

	private void SetUI(Image image, ThemeElement themeElement)
	{
		image.sprite = themeElement.SpriteUI;
		image.color = themeElement.ColorUI;
	}

	private void SetUI(Text text, ThemeElement themeElement)
	{
		text.color = themeElement.ColorUI;
	}
}
