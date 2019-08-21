using UnityEngine;
using UnityEngine.UI;

public class NativeToolkitExample : MonoBehaviour
{
	public Text console;

	public Texture2D texture;

	private string imagePath = string.Empty;

	private void Start()
	{
		Text text = console;
		text.text = text.text + "\nLocation enabled: " + NativeToolkit.StartLocation();
		Text text2 = console;
		text2.text = text2.text + "\nDevice country: " + NativeToolkit.GetCountryCode();
		Text text3 = console;
		text3.text = text3.text + "\nLaunched from notification: " + NativeToolkit.WasLaunchedFromNotification();
	}

	private void OnEnable()
	{
		NativeToolkit.OnScreenshotSaved += ScreenshotSaved;
		NativeToolkit.OnImageSaved += ImageSaved;
		NativeToolkit.OnImagePicked += ImagePicked;
		NativeToolkit.OnCameraShotComplete += CameraShotComplete;
		NativeToolkit.OnContactPicked += ContactPicked;
	}

	private void OnDisable()
	{
		NativeToolkit.OnScreenshotSaved -= ScreenshotSaved;
		NativeToolkit.OnImageSaved -= ImageSaved;
		NativeToolkit.OnImagePicked -= ImagePicked;
		NativeToolkit.OnCameraShotComplete -= CameraShotComplete;
		NativeToolkit.OnContactPicked -= ContactPicked;
	}

	public void OnSaveScreenshotPress()
	{
		NativeToolkit.SaveScreenshot("MyScreenshot", "MyScreenshotFolder", "jpeg");
	}

	public void OnSaveImagePress()
	{
		NativeToolkit.SaveImage(texture, "MyImage", "png");
	}

	public void OnPickImagePress()
	{
		NativeToolkit.PickImage();
	}

	public void OnEmailSharePress()
	{
		NativeToolkit.SendEmail("Hello there", "<html><body><b>This is an email sent from my App!</b></body></html>", imagePath, string.Empty, string.Empty, string.Empty);
	}

	public void OnCameraPress()
	{
		NativeToolkit.TakeCameraShot();
	}

	public void OnPickContactPress()
	{
		NativeToolkit.PickContact();
	}

	public void OnShowAlertPress()
	{
		NativeToolkit.ShowAlert("Native Toolkit", "This is an alert dialog!", DialogFinished);
	}

	public void OnShowDialogPress()
	{
		NativeToolkit.ShowConfirm("Native Toolkit", "This is a confirm dialog!", DialogFinished);
	}

	public void OnLocalNotificationPress()
	{
		string message = "This is a local notification! This is a super long one to show how long we can make a notification. On Android this will appear as an extended notification.";
		NativeToolkit.ScheduleLocalNotification("Hello there", message, 1, 0, "sound_notification", vibrate: true);
	}

	public void OnClearNotificationsPress()
	{
		NativeToolkit.ClearAllLocalNotifications();
	}

	public void OnRateAppPress()
	{
		NativeToolkit.RateApp("Rate This App", "Please take a moment to rate this App", "Rate Now", "Later", "No, Thanks", "343200656", AppRated);
	}

	private void ScreenshotSaved(string path)
	{
		Text text = console;
		text.text = text.text + "\nScreenshot saved to: " + path;
	}

	private void ImageSaved(string path)
	{
		Text text = console;
		string text2 = text.text;
		text.text = text2 + "\n" + texture.name + " saved to: " + path;
	}

	private void ImagePicked(Texture2D img, string path)
	{
		imagePath = path;
		Text text = console;
		text.text = text.text + "\nImage picked at: " + imagePath;
		UnityEngine.Object.Destroy(img);
	}

	private void CameraShotComplete(Texture2D img, string path)
	{
		imagePath = path;
		Text text = console;
		text.text = text.text + "\nCamera shot saved to: " + imagePath;
		UnityEngine.Object.Destroy(img);
	}

	private void DialogFinished(bool result)
	{
		Text text = console;
		text.text = text.text + "\nDialog returned: " + result;
	}

	private void AppRated(string result)
	{
		Text text = console;
		text.text = text.text + "\nRate this app result: " + result;
	}

	private void ContactPicked(string name, string number, string email)
	{
		Text text = console;
		string text2 = text.text;
		text.text = text2 + "\nContact Details:\nName:" + name + ", number:" + number + ", email:" + email;
	}
}
