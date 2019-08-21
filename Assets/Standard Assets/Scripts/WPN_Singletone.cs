using UnityEngine;

public class WPN_Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T instance
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				_instance = (UnityEngine.Object.FindObjectOfType(typeof(T)) as T);
				if ((Object)_instance == (Object)null)
				{
					_instance = new GameObject(typeof(T).Name).AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	public static bool HasInstance
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				return false;
			}
			return true;
		}
	}
}
