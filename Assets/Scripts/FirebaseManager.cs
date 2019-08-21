using Dta.TenTen.Triangle;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseManager : Singleton<FirebaseManager>
{
	[SerializeField]
	private FirebaseRemoteData m_Data;

	private bool m_IsFirebaseInitDone;

	private int[] m_Difficulty;

	private float m_TimeFullAd;

	private const string FIREBASE_SAVE_KEY = "Firebase_Remote_Save";

	private void Start()
	{
		FirebaseRemoteData @object = ServicesManager.DataSecure().GetObject("Firebase_Remote_Save", m_Data);
		if (@object != m_Data)
		{
			m_Data.Difficulty_Android = @object.Difficulty_Android;
			m_Data.TimeFullAd_Android = @object.TimeFullAd_Android;
			m_Data.Difficulty_IOS = @object.Difficulty_IOS;
			m_Data.TimeFullAd_IOS = @object.TimeFullAd_IOS;
		}
		FirebaseConfig timeFullAd;
		FirebaseConfig difficulty;
		if (Application.platform == RuntimePlatform.Android)
		{
			timeFullAd = m_Data.TimeFullAd_Android;
			difficulty = m_Data.Difficulty_Android;
			m_TimeFullAd = m_Data.TimeFullAd_Android.DefaultValue;
			m_Difficulty = m_Data.Difficulty_Android.DefaultValue;
		}
		else
		{
			timeFullAd = m_Data.TimeFullAd_IOS;
			difficulty = m_Data.Difficulty_IOS;
			m_TimeFullAd = m_Data.TimeFullAd_IOS.DefaultValue;
			m_Difficulty = m_Data.Difficulty_IOS.DefaultValue;
		}
		ShapeTypeUtil.SetRemoteDifficulty(m_Difficulty);
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(delegate(Task<DependencyStatus> task)
		{
			DependencyStatus result = task.Result;
			if (result == DependencyStatus.Available)
			{
				FirebaseAnalytics.SetAnalyticsCollectionEnabled(enabled: true);
				m_IsFirebaseInitDone = true;
				FirebaseRemoteConfig.FetchAsync(new TimeSpan(0L)).ContinueWith(delegate
				{
					FirebaseRemoteConfig.ActivateFetched();
					m_TimeFullAd = (float)FirebaseRemoteConfig.GetValue(timeFullAd.ConfigName).DoubleValue;
					UnityEngine.Debug.Log("TimeFullAd:" + m_TimeFullAd);
					string stringValue = FirebaseRemoteConfig.GetValue(difficulty.ConfigName).StringValue;
					UnityEngine.Debug.Log("Difficulty Str:" + stringValue);
					m_Difficulty = ServicesManager.Json().FromJson<int[]>(stringValue);
					ShapeTypeUtil.SetRemoteDifficulty(m_Difficulty);
					if (Application.platform == RuntimePlatform.Android)
					{
						m_Data.TimeFullAd_Android.DefaultValue = m_TimeFullAd;
						m_Data.Difficulty_Android.DefaultValue = m_Difficulty;
					}
					else
					{
						m_Data.TimeFullAd_IOS.DefaultValue = m_TimeFullAd;
						m_Data.Difficulty_IOS.DefaultValue = m_Difficulty;
					}
					ServicesManager.DataSecure().SetObject("Firebase_Remote_Save", m_Data);
					ServicesManager.DataSecure().Save();
				});
			}
			else
			{
				UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {result}");
			}
		});
	}

	public int[] GetDifficulty()
	{
		return m_Difficulty;
	}

	public float GetTimeFullAd()
	{
		return m_TimeFullAd;
	}

	public bool IsFirebaseInitDone()
	{
		return m_IsFirebaseInitDone;
	}
}
