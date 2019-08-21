using System;
using System.Collections;
using UnityEngine;

namespace Root
{
	public class TimeServerModule : ITimeServer
	{
		public class TimeData
		{
			public string unixtime;
		}

		private const string REQUEST_URL = "http://worldtimeapi.org/api/ip";

		private MonoBehaviour m_Coroutiner;

		private bool m_IsHaveServerResult;

		private DateTime m_Now;

		private IJsonService m_Json;

		public TimeServerModule(MonoBehaviour coroutiner, IJsonService json)
		{
			m_Coroutiner = coroutiner;
			m_Json = json;
		}

		public DateTime GetTimeNow()
		{
			if (m_IsHaveServerResult)
			{
				return m_Now;
			}
			return DateTime.Now;
		}

		public bool IsHaveTimeNow()
		{
			return m_IsHaveServerResult;
		}

		public void RequestTimeNow(Action<bool> callback)
		{
			m_IsHaveServerResult = false;
			m_Coroutiner.StartCoroutine(IERequestTime(callback));
		}

		private IEnumerator IERequestTime(Action<bool> callback)
		{
			WWW www = new WWW("http://worldtimeapi.org/api/ip");
			yield return www;
			if (string.IsNullOrEmpty(www.error))
			{
				UnityEngine.Debug.Log("TimeServer: " + www.text);
				TimeData timeData = m_Json.FromJson<TimeData>(www.text);
				if (timeData != null)
				{
					if (long.TryParse(timeData.unixtime, out long result))
					{
						m_Now = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
						m_Now = m_Now.AddSeconds(result).ToLocalTime();
						m_IsHaveServerResult = true;
						callback(obj: true);
					}
					else
					{
						UnityEngine.Debug.Log("error to long");
						callback(obj: false);
					}
				}
				else
				{
					UnityEngine.Debug.Log("error json");
					callback(obj: false);
				}
			}
			else
			{
				UnityEngine.Debug.Log("Error get time server: " + www.error);
				callback(obj: false);
			}
		}
	}
}
