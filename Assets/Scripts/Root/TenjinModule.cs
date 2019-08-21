using System.Collections.Generic;
using UnityEngine;

namespace Root
{
	public class TenjinModule
	{
		private BaseTenjin m_Instance;

		private TenjinConfig m_Config;

		private IDataService m_DataService;

		private const string GENDER_KEY = "Gender";

		private const string INIT = "init_tenjin";

		private bool m_IsHaveGender;

		private int m_Gender;

		public void Init(TenjinConfig config, IDataService dataService)
		{
			m_Config = config;
			m_DataService = dataService;
			m_Instance = Tenjin.getInstance(m_Config.ApiKey);
			m_IsHaveGender = dataService.HasKey("Gender");
			if (m_IsHaveGender)
			{
				m_Gender = dataService.GetInt("Gender", 0);
			}
		}

		public void Connect()
		{
			if (!m_DataService.HasKey("init_tenjin"))
			{
				m_Instance.Connect();
				m_Instance.GetDeeplink(DeferredDeeplinkCallback);
				m_DataService.SetBool("init_tenjin", value: true);
			}
		}

		private void DeferredDeeplinkCallback(Dictionary<string, string> data)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (data.ContainsKey("clicked_tenjin_link"))
			{
				flag = (data["clicked_tenjin_link"] == "true");
				UnityEngine.Debug.Log("===> DeferredDeeplinkCallback ---> clicked_tenjin_link: " + data["clicked_tenjin_link"]);
			}
			if (data.ContainsKey("is_first_session"))
			{
				flag2 = (data["is_first_session"] == "true");
				UnityEngine.Debug.Log("===> DeferredDeeplinkCallback ---> is_first_session: " + data["is_first_session"]);
			}
			if (data.ContainsKey("ad_network"))
			{
			}
			if (data.ContainsKey("campaign_id"))
			{
				flag3 = true;
				UnityEngine.Debug.Log("===> DeferredDeeplinkCallback ---> campaignId: " + data["campaign_id"]);
			}
			if (data.ContainsKey("advertising_id"))
			{
			}
			if (data.ContainsKey("deferred_deeplink_url"))
			{
				UnityEngine.Debug.Log("===> DeferredDeeplinkCallback ---> deferredDeeplink: " + data["deferred_deeplink_url"]);
			}
			if (flag && flag2 && !string.IsNullOrEmpty(data["deferred_deeplink_url"]) && flag3)
			{
				if (data["campaign_id"] == m_Config.MaleAndroid || data["campaign_id"] == m_Config.MaleIOS)
				{
					m_IsHaveGender = true;
					m_Gender = 0;
					m_DataService.SetInt("Gender", m_Gender);
				}
				else if (data["campaign_id"] == m_Config.FemaleAndroid || data["campaign_id"] == m_Config.FemaleIOS)
				{
					m_IsHaveGender = true;
					m_Gender = 1;
					m_DataService.SetInt("Gender", m_Gender);
				}
			}
		}

		public bool IsHaveGender()
		{
			return m_IsHaveGender;
		}

		public int GetGender()
		{
			return m_Gender;
		}
	}
}
