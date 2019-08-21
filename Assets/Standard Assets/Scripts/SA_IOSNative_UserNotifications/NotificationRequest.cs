using SA.Common.Data;
using System.Collections.Generic;

namespace SA.IOSNative.UserNotifications
{
	public class NotificationRequest
	{
		private string _Id = string.Empty;

		private NotificationContent _Content;

		private NotificationTrigger _Trigger;

		public string Id => _Id;

		public NotificationContent Content => _Content;

		public NotificationTrigger Trigger => _Trigger;

		public NotificationRequest()
		{
		}

		public NotificationRequest(string id, NotificationContent content, NotificationTrigger trigger)
		{
			_Id = id;
			_Content = content;
			_Trigger = trigger;
		}

		public NotificationRequest(string data)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(data);
			_Id = (string)dictionary["id"];
			Dictionary<string, object> contentDictionary = (Dictionary<string, object>)dictionary["content"];
			Dictionary<string, object> triggerDictionary = (Dictionary<string, object>)dictionary["trigger"];
			_Content = new NotificationContent(contentDictionary);
			_Trigger = NotificationTrigger.triggerFromDictionary(triggerDictionary);
		}
	}
}
