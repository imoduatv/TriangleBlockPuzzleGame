using UnityEngine;

public class UM_Preview : BaseIOSFeaturePreview
{
	public static IOSNativePreviewBackButton back;

	private void Awake()
	{
		if (back == null)
		{
			back = IOSNativePreviewBackButton.Create();
		}
	}

	private void OnGUI()
	{
		UpdateToStartPos();
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40f), "Unified  API Examples", style);
		StartY += YLableStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Billing Preview"))
		{
			LoadLevel("UM_BillingPreview");
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Game Service Example"))
		{
			LoadLevel("UM_GameServiceBasics");
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Social API Example"))
		{
			LoadLevel("UM_SocailExample");
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Ad Example"))
		{
			LoadLevel("UM_AdExample");
		}
		StartX = XStartPos;
		StartY += YButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Camera And Gallery"))
		{
			LoadLevel("UM_CameraAndGalleryExample");
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Local And Push Notifications"))
		{
			LoadLevel("UM_NotificationsExample");
		}
	}
}
