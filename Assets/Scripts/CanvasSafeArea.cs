using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class CanvasSafeArea : MonoBehaviour
{
	private static bool screenChangeVarsInitialized = false;

	private static ScreenOrientation lastOrientation = ScreenOrientation.Unknown;

	private static Vector2 lastResolution = Vector2.zero;

	private static UnityEvent onOrientationChange = new UnityEvent();

	private static UnityEvent onResolutionChange = new UnityEvent();

	private RectTransform safeAreaTransform;

	[SerializeField]
	private bool m_IsEnable;

	private void Awake()
	{
		if (m_IsEnable)
		{
			safeAreaTransform = (base.transform as RectTransform);
			ApplySafeArea();
		}
	}

	private void OnRectTransformDimensionsChange()
	{
		if (m_IsEnable)
		{
			ApplySafeArea();
			if (!screenChangeVarsInitialized)
			{
				lastOrientation = Screen.orientation;
				lastResolution.x = Screen.width;
				lastResolution.y = Screen.height;
				screenChangeVarsInitialized = true;
			}
			if (Screen.orientation != lastOrientation)
			{
				onOrientationChange.Invoke();
				lastOrientation = Screen.orientation;
				lastResolution.x = Screen.width;
				lastResolution.y = Screen.height;
			}
			else if ((float)Screen.width != lastResolution.x || (float)Screen.height != lastResolution.y)
			{
				onResolutionChange.Invoke();
				lastResolution.x = Screen.width;
				lastResolution.y = Screen.height;
			}
		}
	}

	private void ApplySafeArea()
	{
		if (!(safeAreaTransform == null))
		{
			float num = (float)Screen.height * 1f / (float)Screen.width;
			UnityEngine.Debug.Log("a: " + num);
			UnityEngine.Debug.Log("b: " + 1.882353f);
			Rect rect;
			if (num > 1.882353f)
			{
				int num2 = (int)(0.02f * (float)Screen.height);
				int num3 = (int)(0.01f * (float)Screen.width);
				int num4 = (int)(0.01f * (float)Screen.width);
				int num5 = (int)(0.01f * (float)Screen.width);
				rect = new Rect(num5, num3, Screen.width - num4 - num5, Screen.height - num2 - num3);
				UnityEngine.Debug.Log("1");
			}
			else
			{
				rect = new Rect(0f, 0f, Screen.width, Screen.height);
				UnityEngine.Debug.Log("2");
			}
			Vector2 position = rect.position;
			Vector2 anchorMax = rect.position + rect.size;
			position.x /= Screen.width;
			position.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;
			safeAreaTransform.anchorMin = position;
			safeAreaTransform.anchorMax = anchorMax;
		}
	}
}
