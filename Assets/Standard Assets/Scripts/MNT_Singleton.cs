using UnityEngine;

public abstract class MNT_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	private static bool applicationIsQuitting;

	public static T instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				UnityEngine.Debug.Log(typeof(T) + " [Mog.Singleton] is already destroyed. Returning null. Please check HasInstance first before accessing instance in destructor.");
				return (T)null;
			}
			if ((Object)_instance == (Object)null)
			{
				_instance = (UnityEngine.Object.FindObjectOfType(typeof(T)) as T);
				if ((Object)_instance == (Object)null)
				{
					_instance = new GameObject().AddComponent<T>();
					_instance.gameObject.name = _instance.GetType().Name;
					Object.DontDestroyOnLoad(_instance.gameObject);
				}
			}
			return _instance;
		}
	}

	public static bool HasInstance => !IsDestroyed;

	public static bool IsDestroyed
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				return true;
			}
			return false;
		}
	}

	protected virtual void OnDestroy()
	{
		_instance = (T)null;
		applicationIsQuitting = true;
	}

	protected virtual void OnApplicationQuit()
	{
		_instance = (T)null;
		applicationIsQuitting = true;
	}
}
