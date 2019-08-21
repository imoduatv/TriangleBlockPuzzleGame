using UnityEngine;

namespace Root
{
	public class PlayerPrefsModule : IDataService
	{
		private IJsonService jsonService;

		public PlayerPrefsModule(IJsonService jsonService)
		{
			this.jsonService = jsonService;
		}

		public void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		public void SetBool(string key, bool value)
		{
			int value2 = value ? 1 : 0;
			SetInt(key, value2);
		}

		public void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}

		public void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		public void SetObject(string key, object value)
		{
			string value2 = jsonService.ToJson(value);
			SetString(key, value2);
		}

		public int GetInt(string key, int defaultValue)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		public bool GetBool(string key, bool defaultValue)
		{
			int @int = GetInt(key, -1);
			if (@int == -1)
			{
				return defaultValue;
			}
			return @int != 0;
		}

		public float GetFloat(string key, float defaultValue)
		{
			return PlayerPrefs.GetFloat(key, defaultValue);
		}

		public string GetString(string key, string defaultValue)
		{
			return PlayerPrefs.GetString(key, defaultValue);
		}

		public T GetObject<T>(string key, T defaultValue)
		{
			string @string = GetString(key, string.Empty);
			T val = jsonService.FromJson<T>(@string);
			return (val != null) ? val : defaultValue;
		}

		public void Save()
		{
			PlayerPrefs.Save();
		}

		public bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}
	}
}
