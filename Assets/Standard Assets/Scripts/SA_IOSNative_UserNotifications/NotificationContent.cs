using SA.Common.Data;
using System.Collections.Generic;

namespace SA.IOSNative.UserNotifications
{
	public class NotificationContent
	{
		public string Title = string.Empty;

		public string Subtitle = string.Empty;

		public string Body = string.Empty;

		public string Sound = string.Empty;

		public int Badge;

		public string LaunchImageName = string.Empty;

		public Dictionary<string, object> UserInfo = new Dictionary<string, object>();

		public NotificationContent()
		{
		}

		public NotificationContent(Dictionary<string, object> contentDictionary)
		{
			Title = (string)contentDictionary["title"];
			Subtitle = (string)contentDictionary["subtitle"];
			Body = (string)contentDictionary["body"];
			Sound = (string)contentDictionary["sound"];
			LaunchImageName = (string)contentDictionary["launchImageName"];
			Badge = int.Parse(contentDictionary["badge"].ToString());
			UserInfo = (Dictionary<string, object>)Json.Deserialize(contentDictionary["userInfo"].ToString());
		}

		public override string ToString()
		{
			string text = Json.Serialize(UserInfo);
			return "{" + $"\"title\" : \"{Title}\", \"subtitle\" : \"{Subtitle}\", \"body\" : \"{Body}\", \"sound\" : \"{Sound}\", \"badge\" : {Badge}, \"launchImageName\" : \"{LaunchImageName}\", \"userInfo\" : {text}" + "}";
		}
	}
}
