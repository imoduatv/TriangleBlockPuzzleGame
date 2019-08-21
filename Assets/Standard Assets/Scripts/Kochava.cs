using JsonFx.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Kochava : MonoBehaviour
{
	public enum DebugLogLevel
	{
		none,
		error,
		warn,
		info,
		debug,
		trace
	}

	public enum EventType
	{
		Achievement,
		AddToCart,
		AddToWishList,
		CheckoutStart,
		LevelComplete,
		Purchase,
		Rating,
		RegistrationComplete,
		Search,
		TutorialComplete,
		View,
		AdView,
		PushReceived,
		PushOpened,
		ConsentGranted
	}

	public static class Tracker
	{
		public static class Config
		{
			public static void SetAppGuid(string value)
			{
				if (!CheckInitialized())
				{
					_appGUID = value;
					_dictionaryConfig["appGUID"] = value;
					LOG("Config: appGUID = " + _appGUID, 4);
				}
			}

			public static void SetPartnerName(string value)
			{
				if (!CheckInitialized() && value != null && value.Length >= 1)
				{
					_partnerName = value;
					_dictionaryConfig["partnerName"] = value;
					LOG("Config: partnerName = " + value, 4);
				}
			}

			public static void SetLogLevel(DebugLogLevel value)
			{
				if (!CheckInitialized())
				{
					int num = _logLevel = (int)value;
					_dictionaryConfig["logLevel"] = value;
					LOG("Config: debugLogLevel = " + _logLevel.ToString(), 4);
				}
			}

			public static void SetAppLimitAdTracking(bool value)
			{
				if (!CheckInitialized())
				{
					_appAdLimitTracking = value;
					_dictionaryConfig["appLimitAdTracking"] = value;
					LOG("Config: appAdLimitTracking = " + _appAdLimitTracking.ToString(), 4);
				}
			}

			public static void SetRetrieveAttribution(bool value)
			{
				if (!CheckInitialized())
				{
					_requestAttributionCallback = value;
					_dictionaryConfig["retrieveAttribution"] = value;
					LOG("Config: requestAttributionCallback = " + _requestAttributionCallback.ToString(), 4);
				}
			}

			public static void SetCustomKeyValuePair(string key, object value)
			{
				if (!CheckInitialized())
				{
					_dictionaryCustom[key] = value;
					LOG("Config: custom: " + key + " = " + value.ToString(), 4);
				}
			}

			public static void SetIntelligentConsentManagement(bool value)
			{
				if (!CheckInitialized())
				{
					_intelligentConsentManagement = value;
					_dictionaryConfig["consentIntelligentManagement"] = value;
					LOG("Config: consentIntelligentManagement = " + _intelligentConsentManagement.ToString(), 4);
				}
			}

			private static bool CheckInitialized()
			{
				if (instance != null)
				{
					LOG("Config: Kochava Already Initialized.", 4);
					return true;
				}
				return false;
			}
		}

		public static void Initialize()
		{
			if (_dictionaryConfig == null || _dictionaryConfig.Count < 1)
			{
				LOG("CANNOT INITIALIZE KOCHAVA - Tracker.Config not set or no values provided", 1);
			}
			else if (instance == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "KochavaTracker";
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				instance = gameObject.AddComponent<Kochava>();
				instance.kochavaGameObject = gameObject;
				UnityEngine.Debug.Log("Initializing Kochava native Android library...");
				_dictionaryConfig.Add("versionExtension", "Unity 3.2.0");
				int num = 0;
				AndroidJavaObject androidJavaObject = null;
				try
				{
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
					{
						AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
						androidJavaObject = @static.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
					}
					try
					{
						using (new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
						{
							num = 1;
							AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.kochava.base.Tracker$Configuration", androidJavaObject);
							num = 2;
							AndroidJavaObject androidJavaObject3 = null;
							AndroidJavaObject androidJavaObject4 = null;
							AndroidJavaObject androidJavaObject5 = null;
							AndroidJavaObject androidJavaObject6 = null;
							object obj = _appAdLimitTracking;
							object obj2 = _intelligentConsentManagement;
							AndroidJavaObject androidJavaObject7 = null;
							AndroidJavaObject androidJavaObject8 = null;
							object obj3 = _logLevel;
							num = 3;
							foreach (KeyValuePair<string, object> item in _dictionaryConfig)
							{
								if (item.Key == "appGUID")
								{
									androidJavaObject3 = new AndroidJavaObject("java.lang.String", item.Value);
								}
								else if (item.Key == "partnerName")
								{
									androidJavaObject4 = new AndroidJavaObject("java.lang.String", item.Value);
								}
								else if (item.Key == "versionExtension")
								{
									androidJavaObject8 = new AndroidJavaObject("java.lang.String", item.Value);
								}
								else if (item.Key == "controlHost")
								{
									androidJavaObject5 = new AndroidJavaObject("java.lang.String", item.Value);
								}
								else if (item.Key == "retrieveAttribution")
								{
									androidJavaObject6 = new AndroidJavaObject("java.lang.Boolean", item.Value);
									if ((bool)item.Value)
									{
										androidJavaObject2.Call<AndroidJavaObject>("setAttributionListener", new object[1]
										{
											new AndroidAttributionHandler()
										});
									}
								}
							}
							num = 4;
							if (_dictionaryCustom != null && _dictionaryCustom.Count > 0)
							{
								foreach (KeyValuePair<string, object> item2 in _dictionaryCustom)
								{
									androidJavaObject2.Call<AndroidJavaObject>("addCustom", new object[2]
									{
										new AndroidJavaObject("java.lang.String", item2.Key),
										new AndroidJavaObject("java.lang.String", item2.Value.ToString())
									});
								}
							}
							num = 5;
							androidJavaObject2.Call<AndroidJavaObject>("setLogLevel", new object[1]
							{
								obj3
							});
							androidJavaObject2.Call<AndroidJavaObject>("setAppGuid", new object[1]
							{
								androidJavaObject3
							});
							androidJavaObject2.Call<AndroidJavaObject>("setAppLimitAdTracking", new object[1]
							{
								obj
							});
							androidJavaObject2.Call<AndroidJavaObject>("setPartnerName", new object[1]
							{
								androidJavaObject4
							});
							androidJavaObject2.Call<AndroidJavaObject>("setIntelligentConsentManagement", new object[1]
							{
								obj2
							});
							if (_intelligentConsentManagement)
							{
								androidJavaObject2.Call<AndroidJavaObject>("setConsentStatusChangeListener", new object[1]
								{
									new AndroidConsentStatusHandler()
								});
							}
							num = 6;
							androidTracker = new AndroidJavaClass("com.kochava.base.Tracker");
							androidTracker.CallStatic("ext", androidJavaObject8, "2018-05-03T17:07:46Z");
							androidTracker.CallStatic("configure", androidJavaObject2);
							initialized = true;
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.Log("ERROR: Kochava Android initialization failed at step " + num + ": " + ex);
					}
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.Log("ERROR: Kochava context failed: " + arg);
				}
			}
			else
			{
				LOG("ALREADY INITIALIZED KOCHAVA - Kochava can only be initialized once.", 4);
			}
		}

		private static void DeInitialize()
		{
			initialized = false;
			_appGUID = string.Empty;
			_appAdLimitTracking = false;
			_productName = string.Empty;
			_logLevel = 3;
			_partnerName = string.Empty;
			_requestAttributionCallback = false;
			_intelligentConsentManagement = false;
			_dictionaryCustom = new Dictionary<string, object>();
			_dictionaryConfig = new Dictionary<string, object>();
			attributionCallbackDelegate = null;
			consentStatusChangeDelegate = null;
			androidTracker = null;
			if (instance != null && instance.kochavaGameObject != null)
			{
				UnityEngine.Object.Destroy(instance.kochavaGameObject);
				instance.kochavaGameObject = null;
			}
			instance = null;
		}

		public static void SetAttributionHandler(AttributionCallbackDelegate callback)
		{
			attributionCallbackDelegate = callback;
			LOG("Config: attribution handler set", 4);
		}

		public static void SetConsentStatusChangeHandler(ConsentStatusChangeDelegate callback)
		{
			consentStatusChangeDelegate = callback;
			LOG("Config: consent status change handler set", 4);
		}

		public static void SendEvent(string eventName, string eventData)
		{
			if (!CheckTrackerInitialized("SendEvent()"))
			{
				if (eventData == null)
				{
					eventData = string.Empty;
				}
				androidTracker.CallStatic("sendEvent", new AndroidJavaObject("java.lang.String", eventName), new AndroidJavaObject("java.lang.String", eventData));
			}
		}

		public static void SendEvent(string eventName)
		{
			SendEvent(eventName, string.Empty);
		}

		public static void SendEvent(Event kochavaEvent)
		{
			if (!CheckTrackerInitialized("SendEvent(kochavaEvent)"))
			{
				try
				{
					Dictionary<string, object> dictionary = kochavaEvent.getDictionary();
					AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.kochava.base.Tracker$Event", kochavaEvent.getEventName());
					object value = string.Empty;
					object value2 = string.Empty;
					if (dictionary.TryGetValue("receiptDataFromGoogleStore", out value) && dictionary.TryGetValue("receiptDataSignatureFromGoogleStore", out value2))
					{
						androidJavaObject.Call<AndroidJavaObject>("setGooglePlayReceipt", new object[2]
						{
							new AndroidJavaObject("java.lang.String", value),
							new AndroidJavaObject("java.lang.String", value2)
						});
						dictionary.Remove("receiptDataFromGoogleStore");
						dictionary.Remove("receiptDataSignatureFromGoogleStore");
					}
					try
					{
						string text = Json.ToString(dictionary);
						AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("org.json.JSONObject", text);
						androidJavaObject.Call<AndroidJavaObject>("addCustom", new object[1]
						{
							androidJavaObject2
						});
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.Log("ERROR: Kochava failed to add event parameters: " + ex.Message);
					}
					androidTracker.CallStatic("sendEvent", androidJavaObject);
				}
				catch (Exception ex2)
				{
					LOG("SendEvent(Event) failed: " + ex2.Message, 1);
				}
			}
		}

		public static void SendDeepLink(string deepLinkURI, string sourceApplication = "")
		{
			if (!CheckTrackerInitialized("SendDeepLink()"))
			{
				androidTracker.CallStatic("sendEventDeepLink", new AndroidJavaObject("java.lang.String", deepLinkURI));
			}
		}

		public static string GetAttribution()
		{
			if (CheckTrackerInitialized("GetAttribution()"))
			{
				return string.Empty;
			}
			return androidTracker.CallStatic<string>("getAttribution", new object[0]);
		}

		public static string GetDeviceId()
		{
			if (CheckTrackerInitialized("GetDeviceId()"))
			{
				return string.Empty;
			}
			return androidTracker.CallStatic<string>("getDeviceId", new object[0]);
		}

		public static void SetAppLimitAdTracking(bool desiredAppLimitAdTracking)
		{
			if (!CheckTrackerInitialized("SetAppLimitAdTracking()"))
			{
				LOG("Setting LAT: " + desiredAppLimitAdTracking, 4);
				androidTracker.CallStatic("setAppLimitAdTracking", desiredAppLimitAdTracking);
			}
		}

		public static void SetIdentityLink(Dictionary<string, string> identityLinkDictionary)
		{
			try
			{
				if (!CheckTrackerInitialized("SetIdentityLink()"))
				{
					AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.kochava.base.Tracker$IdentityLink");
					foreach (KeyValuePair<string, string> item in identityLinkDictionary)
					{
						androidJavaObject.Call<AndroidJavaObject>("add", new object[2]
						{
							new AndroidJavaObject("java.lang.String", item.Key),
							new AndroidJavaObject("java.lang.String", item.Value)
						});
					}
					androidTracker.CallStatic("setIdentityLink", androidJavaObject);
				}
			}
			catch (Exception ex)
			{
				LOG("SetIdentityLink() failed to serialize identityLinkDictionary: " + ex.Message, 1);
			}
		}

		public static string GetVersion()
		{
			if (CheckTrackerInitialized("getVersion()"))
			{
				return string.Empty;
			}
			return androidTracker.CallStatic<string>("getVersion", new object[0]);
		}

		public static void AddPushToken(byte[] token)
		{
			if (!CheckTrackerInitialized("AddPushToken(byte[])"))
			{
				string token2 = BitConverter.ToString(token).Replace("-", string.Empty);
				AddPushToken(token2);
			}
		}

		public static void AddPushToken(string token)
		{
			if (!CheckTrackerInitialized("AddPushToken(string)"))
			{
				androidTracker.CallStatic("addPushToken", new AndroidJavaObject("java.lang.String", token));
			}
		}

		public static void RemovePushToken(byte[] token)
		{
			if (!CheckTrackerInitialized("RemovePushToken(byte[])"))
			{
				string token2 = BitConverter.ToString(token).Replace("-", string.Empty);
				RemovePushToken(token2);
			}
		}

		public static void RemovePushToken(string token)
		{
			if (!CheckTrackerInitialized("RemovePushToken(string)"))
			{
				androidTracker.CallStatic("removePushToken", new AndroidJavaObject("java.lang.String", token));
			}
		}

		public static bool GetConsentRequired()
		{
			if (CheckTrackerInitialized("GetConsentRequired()"))
			{
				return true;
			}
			return androidTracker.CallStatic<bool>("isConsentRequired", new object[0]);
		}

		public static void SetConsentGranted(bool isGranted)
		{
			if (!CheckTrackerInitialized("SetConsentGranted(bool)"))
			{
				androidTracker.CallStatic("setConsentGranted", isGranted);
			}
		}

		public static bool GetConsentGranted()
		{
			if (CheckTrackerInitialized("GetConsentGranted()"))
			{
				return false;
			}
			return androidTracker.CallStatic<bool>("isConsentGranted", new object[0]);
		}

		public static bool GetConsentShouldPrompt()
		{
			if (CheckTrackerInitialized("GetConsentShouldPrompt()"))
			{
				return false;
			}
			return androidTracker.CallStatic<bool>("isConsentShouldPrompt", new object[0]);
		}

		public static void SetConsentPrompted()
		{
			if (!CheckTrackerInitialized("SetConsentPrompted()"))
			{
				androidTracker.CallStatic("clearConsentShouldPrompt");
			}
		}

		public static string GetConsentPartnersJson()
		{
			if (CheckTrackerInitialized("GetConsentPartnersJson()"))
			{
				return string.Empty;
			}
			return androidTracker.CallStatic<string>("getConsentPartnersJson", new object[0]);
		}

		public static string GetConsentDescription()
		{
			if (CheckTrackerInitialized("GetConsentDescription()"))
			{
				return string.Empty;
			}
			return androidTracker.CallStatic<string>("getConsentDescription", new object[0]);
		}

		public static long GetConsentResponseTime()
		{
			if (CheckTrackerInitialized("GetConsentResponseTime()"))
			{
				return 0L;
			}
			return androidTracker.CallStatic<long>("getConsentResponseTime", new object[0]);
		}

		private static byte[] hexStringToByteArr(string hexString)
		{
			if (hexString == null || hexString.Length < 2 || hexString.Length % 2 != 0)
			{
				return null;
			}
			byte[] array = new byte[hexString.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			}
			return array;
		}

		private static bool CheckTrackerInitialized(string sourceName)
		{
			if (!initialized)
			{
				UnityEngine.Debug.Log("KOCHAVA ERROR: NOT YET INITIALIZED: " + sourceName);
				return true;
			}
			return false;
		}
	}

	public class Event
	{
		private string eventName;

		private Dictionary<string, object> dictionary;

		public string checkoutAsGuest
		{
			set
			{
				dictionary.Add("checkout_as_guest", value);
			}
		}

		public string contentId
		{
			set
			{
				dictionary.Add("content_id", value);
			}
		}

		public string contentType
		{
			set
			{
				dictionary.Add("content_type", value);
			}
		}

		public string currency
		{
			set
			{
				dictionary.Add("currency", value);
			}
		}

		public string dateString
		{
			set
			{
				dictionary.Add("date", value);
			}
		}

		public DateTime date
		{
			set
			{
				dictionary.Add("date", value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
			}
		}

		public string description
		{
			set
			{
				dictionary.Add("description", value);
			}
		}

		public string destination
		{
			set
			{
				dictionary.Add("destination", value);
			}
		}

		public double duration
		{
			set
			{
				dictionary.Add("duration", value);
			}
		}

		public string endDateString
		{
			set
			{
				dictionary.Add("end_date", value);
			}
		}

		public DateTime endDate
		{
			set
			{
				dictionary.Add("end_date", value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
			}
		}

		public string itemAddedFrom
		{
			set
			{
				dictionary.Add("item_added_from", value);
			}
		}

		public string level
		{
			set
			{
				dictionary.Add("level", value);
			}
		}

		public double maxRatingValue
		{
			set
			{
				dictionary.Add("max_rating_value", value);
			}
		}

		public string name
		{
			set
			{
				dictionary.Add("name", value);
			}
		}

		public string orderId
		{
			set
			{
				dictionary.Add("order_id", value);
			}
		}

		public string origin
		{
			set
			{
				dictionary.Add("origin", value);
			}
		}

		public double price
		{
			set
			{
				dictionary.Add("price", value);
			}
		}

		public double quantity
		{
			set
			{
				dictionary.Add("quantity", value);
			}
		}

		public double ratingValue
		{
			set
			{
				dictionary.Add("rating_value", value);
			}
		}

		public string receiptId
		{
			set
			{
				dictionary.Add("receipt_id", value);
			}
		}

		public string referralFrom
		{
			set
			{
				dictionary.Add("referral_from", value);
			}
		}

		public string registrationMethod
		{
			set
			{
				dictionary.Add("registration_method", value);
			}
		}

		public string results
		{
			set
			{
				dictionary.Add("results", value);
			}
		}

		public string score
		{
			set
			{
				dictionary.Add("score", value);
			}
		}

		public string searchTerm
		{
			set
			{
				dictionary.Add("search_term", value);
			}
		}

		public double spatialX
		{
			set
			{
				dictionary.Add("spatial_x", value);
			}
		}

		public double spatialY
		{
			set
			{
				dictionary.Add("spatial_y", value);
			}
		}

		public double spatialZ
		{
			set
			{
				dictionary.Add("spatial_z", value);
			}
		}

		public string startDateString
		{
			set
			{
				dictionary.Add("start_date", value);
			}
		}

		public DateTime startDate
		{
			set
			{
				dictionary.Add("start_date", value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
			}
		}

		public string success
		{
			set
			{
				dictionary.Add("success", value);
			}
		}

		public string userId
		{
			set
			{
				dictionary.Add("user_id", value);
			}
		}

		public string userName
		{
			set
			{
				dictionary.Add("user_name", value);
			}
		}

		public string validated
		{
			set
			{
				dictionary.Add("validated", value);
			}
		}

		public bool background
		{
			set
			{
				dictionary.Add("background", value);
			}
		}

		public string action
		{
			set
			{
				dictionary.Add("action", value);
			}
		}

		public bool completed
		{
			set
			{
				dictionary.Add("completed", value);
			}
		}

		public Dictionary<string, object> payload
		{
			set
			{
				dictionary.Add("payload", value);
			}
		}

		public string adNetworkName
		{
			set
			{
				dictionary.Add("ad_network_name", value);
			}
		}

		public string adMediationName
		{
			set
			{
				dictionary.Add("ad_mediation_name", value);
			}
		}

		public string adDeviceType
		{
			set
			{
				dictionary.Add("device_type", value);
			}
		}

		public string adPlacement
		{
			set
			{
				dictionary.Add("placement", value);
			}
		}

		public string adType
		{
			set
			{
				dictionary.Add("ad_type", value);
			}
		}

		public string adCampaignID
		{
			set
			{
				dictionary.Add("ad_campaign_id", value);
			}
		}

		public string adCampiagnName
		{
			set
			{
				dictionary.Add("ad_campaign_name", value);
			}
		}

		public string adSize
		{
			set
			{
				dictionary.Add("ad_size", value);
			}
		}

		public string adGroupName
		{
			set
			{
				dictionary.Add("ad_group_name", value);
			}
		}

		public string adGroupID
		{
			set
			{
				dictionary.Add("ad_group_id", value);
			}
		}

		public Event(EventType standardEventType)
		{
			dictionary = new Dictionary<string, object>();
			switch (standardEventType)
			{
			case EventType.Achievement:
				eventName = "Achievement";
				break;
			case EventType.AddToCart:
				eventName = "Add To Cart";
				break;
			case EventType.AddToWishList:
				eventName = "Add To Wish List";
				break;
			case EventType.CheckoutStart:
				eventName = "Checkout Start";
				break;
			case EventType.LevelComplete:
				eventName = "Level Complete";
				break;
			case EventType.Purchase:
				eventName = "Purchase";
				break;
			case EventType.Rating:
				eventName = "Rating";
				break;
			case EventType.RegistrationComplete:
				eventName = "Registration Complete";
				break;
			case EventType.Search:
				eventName = "Search";
				break;
			case EventType.TutorialComplete:
				eventName = "Tutorial Complete";
				break;
			case EventType.View:
				eventName = "View";
				break;
			case EventType.AdView:
				eventName = "Ad View";
				break;
			case EventType.PushReceived:
				eventName = "Push Received";
				break;
			case EventType.PushOpened:
				eventName = "Push Opened";
				break;
			case EventType.ConsentGranted:
				eventName = "Consent Granted";
				break;
			}
		}

		public Event(string customEventType)
		{
			dictionary = new Dictionary<string, object>();
			eventName = customEventType;
		}

		public string Serialize()
		{
			return Json.ToString(dictionary);
		}

		public string getEventName()
		{
			return eventName;
		}

		public Dictionary<string, object> getDictionary()
		{
			return dictionary;
		}

		public void SetCustomValue(string key, double value)
		{
			dictionary.Add(key, value);
		}

		public void SetCustomValue(string key, bool value)
		{
			dictionary.Add(key, value);
		}

		public void SetCustomValue(string key, string value)
		{
			dictionary.Add(key, value);
		}

		public void setReceiptFromGooglePlayStore(string receiptDataFromStore, string receiptDataSignatureFromStore)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				dictionary.Add("receiptDataFromGoogleStore", receiptDataFromStore);
				dictionary.Add("receiptDataSignatureFromGoogleStore", receiptDataSignatureFromStore);
			}
			else
			{
				UnityEngine.Debug.Log("WARNING: Event safely ignoring setReceiptFromGooglePlayStore() (only available on Android)");
			}
		}

		public void setReceiptFromAppleAppStoreBase64EncodedString(string value)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				dictionary.Add("appStoreReceiptBase64EncodedString", value);
			}
			else
			{
				UnityEngine.Debug.Log("WARNING: Event safely ignoring setReceiptFromAppleAppStoreBase64EncodedString() (only available on iOS)");
			}
		}
	}

	public delegate void AttributionCallbackDelegate(string response);

	public delegate void ConsentStatusChangeDelegate();

	private class Util
	{
		public static double GetNetworkRetryTime(int failedCount)
		{
			if (failedCount <= 1)
			{
				return 10.0;
			}
			switch (failedCount)
			{
			case 2:
				return 60.0;
			case 3:
				return 360.0;
			default:
				return 3600.0;
			}
		}

		public static string TimeStamp()
		{
			return $"{DateTime.Now:hh:mm:ss.fff}";
		}

		public static double UnixTime()
		{
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (DateTime.UtcNow - d).TotalSeconds;
		}

		public static void SetValue(ref Dictionary<string, object> source, string key, object value, bool doNotReplace = true, bool doNotAllowEmptyString = true)
		{
			if (source != null && value != null)
			{
				try
				{
					if (!source.ContainsKey(key))
					{
						goto IL_002e;
					}
					if (!doNotReplace)
					{
						source.Remove(key);
						goto IL_002e;
					}
					goto end_IL_000d;
					IL_002e:
					if (!doNotAllowEmptyString || !(value is string) || value.ToString().Length >= 1)
					{
						source.Add(key, value);
					}
					end_IL_000d:;
				}
				catch (Exception ex)
				{
					LOG("SetValue() failed for key=" + key + ": " + ex.Message, 1);
				}
			}
		}
	}

	private static class Json
	{
		public static T ToObj<T>(string jsonString)
		{
			return JsonReader.Deserialize<T>(jsonString);
		}

		public static string ToString(object jsonObject)
		{
			return JsonWriter.Serialize(jsonObject);
		}
	}

	private class AndroidAttributionHandler : AndroidJavaProxy
	{
		public AndroidAttributionHandler()
			: base("com.kochava.base.AttributionListener")
		{
		}

		private void onAttributionReceived(string msg)
		{
			LOG("Attribution received from Android native (will pass along to host): " + msg, 4);
			lock (instance.cacheLock)
			{
				instance.cacheAttributionCallback = msg;
			}
		}
	}

	private class AndroidConsentStatusHandler : AndroidJavaProxy
	{
		public AndroidConsentStatusHandler()
			: base("com.kochava.base.ConsentStatusChangeListener")
		{
		}

		private void onConsentStatusChange()
		{
			LOG("Consent Status Change received from Android native (will pass along to host): ", 4);
			lock (instance.cacheLock)
			{
				instance.cacheConsentStatusChangeCallback = true;
			}
		}
	}

	private static Kochava instance = null;

	private GameObject kochavaGameObject;

	private string instanceId = Guid.NewGuid().ToString().Substring(0, 5) + "-";

	private object cacheLock = new object();

	private string cacheAttributionCallback;

	private bool cacheConsentStatusChangeCallback;

	private static bool initialized = false;

	private static string _appGUID = string.Empty;

	private static bool _appAdLimitTracking = false;

	private static string _productName = string.Empty;

	private static int _logLevel;

	private static string _partnerName = string.Empty;

	private static bool _requestAttributionCallback = false;

	private static bool _intelligentConsentManagement = false;

	private static Dictionary<string, object> _dictionaryCustom = new Dictionary<string, object>();

	private static Dictionary<string, object> _dictionaryConfig = new Dictionary<string, object>();

	private static AttributionCallbackDelegate attributionCallbackDelegate;

	private static ConsentStatusChangeDelegate consentStatusChangeDelegate;

	private static AndroidJavaObject androidTracker;

	private const string sdkVersionNumber = "3.2.0";

	private const string sdkBuildDate = "2018-05-03T17:07:46Z";

	private const int LOG_NONE = 0;

	private const int LOG_ERROR = 1;

	private const int LOG_WARN = 2;

	private const int LOG_INFO = 3;

	private const int LOG_DEBUG = 4;

	private const int LOG_TRACE = 5;

	private void Update()
	{
		lock (cacheLock)
		{
			if (cacheAttributionCallback != null)
			{
				if (attributionCallbackDelegate != null)
				{
					attributionCallbackDelegate(cacheAttributionCallback);
				}
				cacheAttributionCallback = null;
			}
			if (cacheConsentStatusChangeCallback)
			{
				if (consentStatusChangeDelegate != null)
				{
					consentStatusChangeDelegate();
				}
				cacheConsentStatusChangeCallback = false;
			}
		}
	}

	public static bool IsInitialized()
	{
		return initialized;
	}

	private static void LOG(string msg, int msglogLevel)
	{
		if (_logLevel >= msglogLevel)
		{
			string str = "[Kochava][" + Util.TimeStamp() + "]";
			switch (msglogLevel)
			{
			case 1:
				str += "[ERROR]";
				break;
			case 2:
				str += "[WARNING]";
				break;
			case 3:
				str += "[I]";
				break;
			case 4:
				str += "[D]";
				break;
			case 5:
				str += "[T]";
				break;
			}
			str = str + " " + msg;
			UnityEngine.Debug.Log(str);
		}
	}
}
