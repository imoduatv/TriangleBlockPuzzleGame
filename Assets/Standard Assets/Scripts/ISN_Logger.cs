using SA.Common.Pattern;
using System;
using UnityEngine;

public class ISN_Logger : Singleton<ISN_Logger>
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Create()
	{
	}

	public static void Log(object message, LogType logType = LogType.Log)
	{
		Singleton<ISN_Logger>.Instance.Create();
		if (message != null && !IOSNativeSettings.Instance.DisablePluginLogs && Application.isEditor)
		{
			ISNEditorLog(logType, message);
		}
	}

	private static void ISNEditorLog(LogType logType, object message)
	{
		switch (logType)
		{
		case LogType.Error:
			UnityEngine.Debug.LogError(message);
			break;
		case LogType.Exception:
			UnityEngine.Debug.LogException((Exception)message);
			break;
		case LogType.Warning:
			UnityEngine.Debug.LogWarning(message);
			break;
		default:
			UnityEngine.Debug.Log(message);
			break;
		}
	}
}
