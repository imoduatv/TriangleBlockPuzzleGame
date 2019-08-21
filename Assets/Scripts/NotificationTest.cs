using UnityEngine;

public class NotificationTest : MonoBehaviour
{
	private void Awake()
	{
		LocalNotification.ClearNotifications();
	}

	public void OneTime()
	{
		LocalNotification.SendNotification(1, 5000L, "Title", "Long message text", new Color32(byte.MaxValue, 68, 68, byte.MaxValue), true, true, true, "notify_icon_small", string.Empty, null, "default");
	}

	public void OneTimeBigIcon()
	{
		LocalNotification.SendNotification(1, 5000L, "Title", "Long message text with big icon", new Color32(byte.MaxValue, 68, 68, byte.MaxValue), true, true, true, "app_icon", string.Empty, null, "default");
	}

	public void OneTimeWithActions()
	{
		LocalNotification.Action action = new LocalNotification.Action("background", "In Background", this);
		action.Foreground = false;
		LocalNotification.Action action2 = new LocalNotification.Action("foreground", "In Foreground", this);
		LocalNotification.SendNotification(1, 5000L, "Title", "Long message text with actions", new Color32(byte.MaxValue, 68, 68, byte.MaxValue), true, true, true, "notify_icon_small", null, "boing", "default", action, action2);
	}

	public void Repeating()
	{
		LocalNotification.SendRepeatingNotification(1, 5000L, 60000L, "Title", "Long message text", new Color32(byte.MaxValue, 68, 68, byte.MaxValue), true, true, true, "notify_icon_small", string.Empty, null, "default");
	}

	public void Stop()
	{
		LocalNotification.CancelNotification(1);
	}

	public void OnAction(string identifier)
	{
		UnityEngine.Debug.Log("Got action " + identifier);
	}
}
