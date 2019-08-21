using System.Collections;
using UnityEngine;

public class GiftAnimation : MonoBehaviour
{
	[SerializeField]
	private Animator m_GiftAnimator;

	private WaitForSeconds showTime = new WaitForSeconds(5f);

	private WaitForSeconds hideTime = new WaitForSeconds(0.8f);

	public void ShowGift()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
			m_GiftAnimator.Play("Start");
			StartCoroutine(IE_HideGift());
		}
	}

	private IEnumerator IE_HideGift()
	{
		yield return showTime;
		m_GiftAnimator.SetTrigger("ScaleBack");
		yield return hideTime;
		base.gameObject.SetActive(value: false);
		m_GiftAnimator.transform.localScale = Vector3.one;
		Singleton<UIManager>.instance.OnClickNoSpin();
	}
}
