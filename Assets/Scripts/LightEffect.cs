using Prime31.ZestKit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightEffect : MonoBehaviour
{
	public Image m_Circle1;

	public Image m_Circle2;

	public Image m_Light1;

	public Image m_Light2;

	public float m_Time = 1f;

	private List<ITweenable> m_ListTween;

	private Color m_StartColor = new Color(1f, 1f, 1f, 0f);

	private Vector3 m_DesScale = Vector3.one * 0.5f;

	public void StartEffect()
	{
		ClearAllTween();
		StartEffectC(m_Circle1);
		StartEffectL(m_Light1);
		StartEffectC(m_Circle2, m_Time / 2f);
		StartEffectL(m_Light2, m_Time / 2f);
	}

	public void StartEffectC(Image image, float delay = 0f)
	{
		image.transform.localScale = Vector3.one;
		image.color = m_StartColor;
		ITween<float> tween = image.ZKalphaTo(1f, m_Time).setEaseType(EaseType.Linear).setLoops(LoopType.RestartFromBeginning, 100)
			.setDelay(delay);
		ITween<Vector3> tween2 = image.transform.ZKlocalScaleTo(m_DesScale, m_Time).setEaseType(EaseType.Linear).setLoops(LoopType.RestartFromBeginning, 100)
			.setDelay(delay);
		m_ListTween.Add(tween);
		m_ListTween.Add(tween2);
		tween2.start();
		tween.start();
	}

	public void StartEffectL(Image image, float delay = 0f)
	{
		image.transform.eulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360));
		image.transform.localScale = Vector3.one;
		image.color = m_StartColor;
		ITween<float> tween = image.ZKalphaTo(1f, m_Time / 2f).setEaseType(EaseType.Linear).setLoops(LoopType.PingPong, 100)
			.setDelay(delay)
			.setCompletionHandler(delegate
			{
				StartEffectL(image);
			});
		m_ListTween.Add(tween);
		tween.start();
	}

	public void ClearAllTween()
	{
		if (m_ListTween == null)
		{
			m_ListTween = new List<ITweenable>();
		}
		while (m_ListTween.Count > 0)
		{
			ITweenable tweenable = m_ListTween[0];
			m_ListTween.RemoveAt(0);
			if (tweenable != null && tweenable.isRunning())
			{
				tweenable.stop();
			}
			tweenable = null;
		}
		m_ListTween.Clear();
	}
}
