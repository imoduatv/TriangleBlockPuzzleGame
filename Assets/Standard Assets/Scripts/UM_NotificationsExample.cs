using SA.Common.Pattern;
using UnityEngine;

public class UM_NotificationsExample : BaseIOSFeaturePreview
{
	private int LastNotificationId;

	private void OnGUI()
	{
		UpdateToStartPos();
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40f), "Local Notifications API", style);
		StartY += YLableStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Show Notification Popup "))
		{
			Singleton<UM_NotificationController>.Instance.ShowNotificationPoup("Hello", "Notification popup test");
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Schedule Local Notification"))
		{
			LastNotificationId = Singleton<UM_NotificationController>.Instance.ScheduleLocalNotification("Hello Locacl", "Local Notification Example", 5);
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Cansel Last Notification"))
		{
			Singleton<UM_NotificationController>.Instance.CancelLocalNotification(LastNotificationId);
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Cansel All Last Notifications"))
		{
			Singleton<UM_NotificationController>.Instance.CancelAllLocalNotifications();
		}
		StartX = XStartPos;
		StartY += YButtonStep;
		StartY += YLableStep;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40f), "Push Notifications API", style);
		StartY += YLableStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Retrieve Device PushId "))
		{
			UM_NotificationController.OnPushIdLoadResult += OnPushIdLoaded;
			Singleton<UM_NotificationController>.Instance.RetrieveDevicePushId();
		}
	}

	private void OnPushIdLoaded(UM_PushRegistrationResult res)
	{
		if (res.IsSucceeded)
		{
			MNPopup mNPopup = new MNPopup("Succeeded", "Device Id: " + res.deviceId);
			mNPopup.AddAction("Ok", delegate
			{
			});
			mNPopup.Show();
		}
		else
		{
			MNPopup mNPopup2 = new MNPopup("Failed", "No device id");
			mNPopup2.AddAction("Ok", delegate
			{
			});
			mNPopup2.Show();
		}
	}
}
