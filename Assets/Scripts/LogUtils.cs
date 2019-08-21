using UnityEngine;

public class LogUtils
{
	public static void Log(object obj)
	{
		UnityEngine.Debug.Log(obj);
	}

	public static void Log(object obj, Object context)
	{
		UnityEngine.Debug.Log(obj, context);
	}
}
