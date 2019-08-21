using System;
using System.Collections.Generic;
using UnityEngine;

namespace Root
{
	public class AnalyticABTestModule : IAnalyticABTestService
	{
		private IDataService m_Data;

		private Dictionary<string, ABTestConfig> m_Configs;

		public AnalyticABTestModule(IDataService dataService)
		{
			m_Data = dataService;
			m_Configs = new Dictionary<string, ABTestConfig>();
		}

		public bool IsActive(string keyNameAB)
		{
			if (m_Configs.ContainsKey(keyNameAB))
			{
				return m_Configs[keyNameAB].IsActive;
			}
			return false;
		}

		public ABTestConfig GetConfig(string keyNameAB)
		{
			if (m_Configs.ContainsKey(keyNameAB))
			{
				return m_Configs[keyNameAB];
			}
			return null;
		}

		public int GetValue(string keyNameAB)
		{
			if (m_Configs.ContainsKey(keyNameAB))
			{
				ABTestConfig aBTestConfig = m_Configs[keyNameAB];
				int @int = m_Data.GetInt(keyNameAB, aBTestConfig.DefaultValue);
				return @int % aBTestConfig.ListEventsAB.Length;
			}
			return -1;
		}

		public void SetUpTest(ABTestConfig[] configs, int versionCode, Func<bool> funcIsNewInstall)
		{
			if (configs == null)
			{
				return;
			}
			bool flag = funcIsNewInstall();
			foreach (ABTestConfig aBTestConfig in configs)
			{
				m_Configs[aBTestConfig.KeyName] = aBTestConfig;
				if (aBTestConfig.IsActive && aBTestConfig.ListEventsAB != null && aBTestConfig.ListEventsAB.Length != 0 && (!aBTestConfig.IsOnlyNewInstall || flag))
				{
					bool flag2 = false;
					string key = "Ver" + aBTestConfig.KeyName;
					if (aBTestConfig.IsOverride && m_Data.HasKey(key) && versionCode > m_Data.GetInt(key, versionCode))
					{
						flag2 = true;
					}
					if (!m_Data.HasKey(aBTestConfig.KeyName))
					{
						flag2 = true;
					}
					if (flag2)
					{
						int randomIndexByRatioList = GetRandomIndexByRatioList(aBTestConfig.ListRandomRatio, aBTestConfig.ListEventsAB.Length);
						randomIndexByRatioList %= aBTestConfig.ListEventsAB.Length;
						m_Data.SetInt(aBTestConfig.KeyName, randomIndexByRatioList);
						m_Data.SetInt(key, versionCode);
						string key2 = "Log" + aBTestConfig.KeyName;
						m_Data.SetBool(key2, value: false);
					}
				}
			}
		}

		private int GetRandomIndexByRatioList(int[] ratio, int itemLength)
		{
			if (ratio == null || ratio.Length == 0)
			{
				return UnityEngine.Random.Range(0, itemLength);
			}
			float num = 0f;
			for (int i = 0; i < ratio.Length; i++)
			{
				num += (float)ratio[i];
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			num = 0f;
			for (int j = 0; j < ratio.Length; j++)
			{
				if (ratio[j] > 0)
				{
					num += (float)ratio[j];
					if (num2 <= num)
					{
						return j;
					}
				}
			}
			return UnityEngine.Random.Range(0, itemLength);
		}

		public void SetLogAnalytic(Action<string> actionLog)
		{
			foreach (ABTestConfig value in m_Configs.Values)
			{
				if (value.IsActive)
				{
					string key = "Log" + value.KeyName;
					if (!m_Data.GetBool(key, defaultValue: false))
					{
						m_Data.SetBool(key, value: true);
						int @int = m_Data.GetInt(value.KeyName, value.DefaultValue);
						@int %= value.ListEventsAB.Length;
						actionLog(value.ListEventsAB[@int]);
					}
				}
			}
		}
	}
}
