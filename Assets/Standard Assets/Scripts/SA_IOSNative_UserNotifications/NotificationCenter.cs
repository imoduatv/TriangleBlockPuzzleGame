using SA.Common.Models;
using SA.Common.Pattern;
using SA.IOSNative.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SA.IOSNative.UserNotifications
{
	public static class NotificationCenter
	{
		private static Dictionary<string, Action<Result>> OnCallbackDictionary;

		private static Action<List<NotificationRequest>> OnPendingNotificationsCallback;

		private static Action<Result> RequestPermissionsCallback;

		public static NotificationRequest LastNotificationRequest;

		public static NotificationRequest LaunchNotification => AppController.LaunchNotification;

		public static event Action<NotificationRequest> OnWillPresentNotification;

		static NotificationCenter()
		{
			NotificationCenter.OnWillPresentNotification = delegate
			{
			};
			Singleton<NativeReceiver>.Instance.Init();
			OnCallbackDictionary = new Dictionary<string, Action<Result>>();
		}

		public static void RequestPermissions(Action<Result> callback)
		{
			RequestPermissionsCallback = callback;
		}

		public static void AddNotificationRequest(NotificationRequest request, Action<Result> callback)
		{
			string id = request.Id;
			NotificationContent content = request.Content;
			OnCallbackDictionary[id] = callback;
			string notificationJSONData = "{" + $"\"id\" : \"{id}\", \"content\" : {request.Content.ToString()}, \"trigger\" : {request.Trigger.ToString()}" + "}";
			ScheduleUserNotification(notificationJSONData);
		}

		private static void ScheduleUserNotification(string notificationJSONData)
		{
		}

		public static void CancelAllNotifications()
		{
		}

		public static void CancelUserNotificationById(string nId)
		{
		}

		public static void GetPendingNotificationRequests(Action<List<NotificationRequest>> callback)
		{
			OnPendingNotificationsCallback = callback;
		}

		internal static void RequestPermissionsResponse(string dataString)
		{
			Result obj = (!dataString.Equals("success")) ? new Result(new Error()) : new Result();
			RequestPermissionsCallback(obj);
		}

		internal static void AddNotificationRequestResponse(string dataString)
		{
			string[] array = dataString.Split(new string[1]
			{
				"|%|"
			}, StringSplitOptions.None);
			string key = array[0];
			string text = array[1];
			Result obj = (!text.Equals("success")) ? new Result(new Error(text)) : new Result();
			OnCallbackDictionary[key]?.Invoke(obj);
		}

		internal static void WillPresentNotification(string data)
		{
			NotificationRequest obj = new NotificationRequest(data);
			NotificationCenter.OnWillPresentNotification(obj);
		}

		internal static void PendingNotificationsRequestResponse(string data)
		{
			if (data.Length > 0)
			{
				string[] array = data.Split(new string[1]
				{
					"|%|"
				}, StringSplitOptions.None);
				List<NotificationRequest> list = new List<NotificationRequest>();
				for (int i = 0; i < array.Length && !(array[i] == "endofline"); i++)
				{
					NotificationRequest item = new NotificationRequest(data);
					list.Add(item);
				}
				OnPendingNotificationsCallback(list);
			}
		}

		internal static void SetLastNotifification(string data)
		{
			NotificationRequest notificationRequest = LastNotificationRequest = new NotificationRequest(data);
		}
	}
}
