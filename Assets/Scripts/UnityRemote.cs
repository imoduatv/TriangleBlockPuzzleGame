using UnityEngine;

public class UnityRemote : Singleton<UnityRemote>
{
	[Header("Ads Type")]
	[SerializeField]
	private string AndroidKey;

	[SerializeField]
	private string IosKey;

	[Space(10f)]
	[Header("Inmobi init")]
	[SerializeField]
	private string AndroidInmobiInit;

	[SerializeField]
	private string IosInmobiInit;

	[Space(10f)]
	[SerializeField]
	private string AndroidEventActive;

	[SerializeField]
	private string IosEventActive;

	private int m_AndroidAdType = 1;

	private int m_IosAdType = 1;

	private bool m_InmobiInit;

	private bool m_IsOpenEvent;

	private void Start()
	{
		RemoteSettings.Updated += HandleUpdated;
		RemoteSettings.ForceUpdate();
	}

	private void HandleUpdated()
	{
		string empty = string.Empty;
		int num;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			m_IosAdType = RemoteSettings.GetInt(IosKey);
			num = m_IosAdType;
			UnityEngine.Debug.Log("Fetch AdType:" + m_IosAdType);
			m_InmobiInit = RemoteSettings.GetBool(IosInmobiInit, defaultValue: false);
			m_IsOpenEvent = RemoteSettings.GetBool(IosEventActive, defaultValue: false);
		}
		else
		{
			m_AndroidAdType = RemoteSettings.GetInt(AndroidKey);
			num = m_AndroidAdType;
			UnityEngine.Debug.Log("Fetch AdType:" + m_AndroidAdType);
			m_InmobiInit = RemoteSettings.GetBool(AndroidInmobiInit, defaultValue: false);
			m_IsOpenEvent = RemoteSettings.GetBool(AndroidEventActive, defaultValue: false);
		}
		if (num == 5 || num == 6)
		{
			Singleton<ServicesManager>.instance.LoadAdAppotaxEcpm();
		}
		if (num == 7 || num == 8)
		{
			Singleton<ServicesManager>.instance.LoadAdAppmaticEcpm();
		}
		UnityEngine.Debug.Log("event active:" + m_IsOpenEvent);
		Singleton<UIManager>.instance.OpenEvent(m_IsOpenEvent);
	}

	public int GetAdType()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return m_IosAdType;
		}
		return m_AndroidAdType;
	}

	public bool GetIsInitInmobi()
	{
		return m_InmobiInit;
	}

	public bool GetIsOpenEvent()
	{
		return m_IsOpenEvent;
	}
}
