using UnityEngine;

public class ScaleRelateToScreen : MonoBehaviour
{
	public float scaleRelateScreenWidth = 1f;

	public bool isOnlyScaleWidth = true;

	public bool isResetBeforeScale;

	private SpriteRenderer thisRenderer;

	private void Awake()
	{
		Vector3 localScale;
		if (isResetBeforeScale)
		{
			localScale = base.gameObject.transform.localScale;
			if (!isOnlyScaleWidth)
			{
				localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				localScale.x = 1f;
			}
			base.gameObject.transform.localScale = localScale;
		}
		thisRenderer = base.gameObject.GetComponent<SpriteRenderer>();
		Vector3 size = thisRenderer.bounds.size;
		float x = size.x;
		Vector3 size2 = thisRenderer.bounds.size;
		float y = size2.y;
		float num = Camera.main.orthographicSize * 2f;
		float num2 = num * Camera.main.aspect;
		float num3 = num2 * scaleRelateScreenWidth;
		float num4 = num3 / x;
		localScale = base.gameObject.transform.localScale;
		localScale.x = num4;
		if (!isOnlyScaleWidth)
		{
			localScale.y = num4;
			localScale.z = num4;
		}
		base.gameObject.transform.localScale = localScale;
	}
}
