using UnityEngine;

public class EscapeController : MonoBehaviour
{
	public delegate void CallBack();

	private bool isInit;

	private static EscapeController _instance;

	private CallBack escapeCallBack;

	public static EscapeController Instance()
	{
		if (_instance == null)
		{
			_instance = UnityEngine.Object.FindObjectOfType<EscapeController>();
			if (_instance == null)
			{
				_instance = new GameObject("EscapeController").AddComponent<EscapeController>();
			}
			Object.DontDestroyOnLoad(_instance.gameObject);
			if (!_instance.isInit)
			{
				_instance.isInit = true;
			}
		}
		return _instance;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.Escape))
		{
			Singleton<SoundManager>.Instance.PlayButton();
			if (escapeCallBack != null && !Application.isMobilePlatform)
			{
				escapeCallBack();
			}
		}
	}

	public void SetCallback(CallBack callback)
	{
		escapeCallBack = callback;
	}
}
