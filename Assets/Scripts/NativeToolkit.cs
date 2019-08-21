using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class NativeToolkit : MonoBehaviour
{
	private enum ImageType
	{
		IMAGE,
		SCREENSHOT
	}

	private enum SaveStatus
	{
		NOTSAVED,
		SAVED,
		DENIED,
		TIMEOUT
	}

	private static NativeToolkit instance;

	private static GameObject go;

	private static AndroidJavaClass obj;

	public static NativeToolkit Instance
	{
		get
		{
			if (instance == null)
			{
				go = new GameObject();
				go.name = "NativeToolkit";
				instance = go.AddComponent<NativeToolkit>();
				if (Application.platform == RuntimePlatform.Android)
				{
					obj = new AndroidJavaClass("com.secondfury.nativetoolkit.Main");
				}
			}
			return instance;
		}
	}

	public static event Action<Texture2D> OnScreenshotTaken;

	public static event Action<string> OnScreenshotSaved;

	public static event Action<string> OnImageSaved;

	public static event Action<Texture2D, string> OnImagePicked;

	public static event Action<bool> OnDialogComplete;

	public static event Action<string> OnRateComplete;

	public static event Action<Texture2D, string> OnCameraShotComplete;

	public static event Action<string, string, string> OnContactPicked;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public static void SaveScreenshot(string fileName, string albumName = "MyScreenshots", string fileType = "jpg", Rect screenArea = default(Rect))
	{
		UnityEngine.Debug.Log("Save screenshot to gallery " + fileName);
		if (screenArea == default(Rect))
		{
			screenArea = new Rect(0f, 0f, Screen.width, Screen.height);
		}
		Instance.StartCoroutine(Instance.GrabScreenshot(fileName, albumName, fileType, screenArea));
	}

	private IEnumerator GrabScreenshot(string fileName, string albumName, string fileType, Rect screenArea)
	{
		yield return new WaitForEndOfFrame();
		Texture2D texture = new Texture2D((int)screenArea.width, (int)screenArea.height, TextureFormat.RGB24,  false);
		texture.ReadPixels(screenArea, 0, 0);
		texture.Apply();
		byte[] bytes;
		string fileExt;
		if (fileType == "png")
		{
			bytes = texture.EncodeToPNG();
			fileExt = ".png";
		}
		else
		{
			bytes = texture.EncodeToJPG();
			fileExt = ".jpg";
		}
		if (NativeToolkit.OnScreenshotTaken != null)
		{
			NativeToolkit.OnScreenshotTaken(texture);
		}
		else
		{
			UnityEngine.Object.Destroy(texture);
		}
		string date = DateTime.Now.ToString("hh-mm-ss_dd-MM-yy");
		string screenshotFilename = fileName + "_" + date + fileExt;
		string path = Application.persistentDataPath + "/" + screenshotFilename;
		if (Application.platform == RuntimePlatform.Android)
		{
			string path2 = Path.Combine(albumName, screenshotFilename);
			path = Path.Combine(Application.persistentDataPath, path2);
			string directoryName = Path.GetDirectoryName(path);
			Directory.CreateDirectory(directoryName);
		}
		Instance.StartCoroutine(Instance.Save(bytes, fileName, path, ImageType.SCREENSHOT));
	}

	public static void SaveImage(Texture2D texture, string fileName, string fileType = "jpg")
	{
		UnityEngine.Debug.Log("Save image to gallery " + fileName);
		Instance.Awake();
		byte[] bytes;
		string str;
		if (fileType == "png")
		{
			bytes = texture.EncodeToPNG();
			str = ".png";
		}
		else
		{
			bytes = texture.EncodeToJPG();
			str = ".jpg";
		}
		string path = Application.persistentDataPath + "/" + fileName + str;
		Instance.StartCoroutine(Instance.Save(bytes, fileName, path, ImageType.IMAGE));
	}

	private IEnumerator Save(byte[] bytes, string fileName, string path, ImageType imageType)
	{
		int count = 0;
		SaveStatus saved = SaveStatus.NOTSAVED;
		if (Application.platform == RuntimePlatform.Android)
		{
			File.WriteAllBytes(path, bytes);
			while (saved == SaveStatus.NOTSAVED)
			{
				count++;
				saved = (SaveStatus)((count <= 30) ? obj.CallStatic<int>("addImageToGallery", new object[1]
				{
					path
				}) : 3);
				yield return Instance.StartCoroutine(Instance.Wait(0.5f));
			}
		}
		switch (saved)
		{
		case SaveStatus.DENIED:
			path = "DENIED";
			break;
		case SaveStatus.TIMEOUT:
			path = "TIMEOUT";
			break;
		}
		switch (imageType)
		{
		case ImageType.IMAGE:
			if (NativeToolkit.OnImageSaved != null)
			{
				NativeToolkit.OnImageSaved(path);
			}
			break;
		case ImageType.SCREENSHOT:
			if (NativeToolkit.OnScreenshotSaved != null)
			{
				NativeToolkit.OnScreenshotSaved(path);
			}
			break;
		}
	}

	public static void PickImage()
	{
		Instance.Awake();
		if (Application.platform == RuntimePlatform.Android)
		{
			obj.CallStatic("pickImageFromGallery");
		}
	}

	public void OnPickImage(string path)
	{
		Texture2D arg = LoadImageFromFile(path);
		if (NativeToolkit.OnImagePicked != null)
		{
			NativeToolkit.OnImagePicked(arg, path);
		}
	}

	public static void TakeCameraShot()
	{
		Instance.Awake();
		if (Application.platform == RuntimePlatform.Android)
		{
			obj.CallStatic("takeCameraShot");
		}
	}

	public void OnCameraFinished(string path)
	{
		Texture2D arg = LoadImageFromFile(path);
		if (NativeToolkit.OnCameraShotComplete != null)
		{
			NativeToolkit.OnCameraShotComplete(arg, path);
		}
	}

	public static void PickContact()
	{
		Instance.Awake();
		if (Application.platform == RuntimePlatform.Android)
		{
			obj.CallStatic("pickContact");
		}
	}

	public void OnPickContactFinished(string data)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(data) as Dictionary<string, object>;
		string arg = string.Empty;
		string arg2 = string.Empty;
		string arg3 = string.Empty;
		if (dictionary.ContainsKey("name"))
		{
			arg = dictionary["name"].ToString();
		}
		if (dictionary.ContainsKey("number"))
		{
			arg2 = dictionary["number"].ToString();
		}
		if (dictionary.ContainsKey("email"))
		{
			arg3 = dictionary["email"].ToString();
		}
		if (NativeToolkit.OnContactPicked != null)
		{
			NativeToolkit.OnContactPicked(arg, arg2, arg3);
		}
	}

	public static void SendEmail(string subject, string body, string pathToImageAttachment = "", string to = "", string cc = "", string bcc = "")
	{
		Instance.Awake();
		if (Application.platform == RuntimePlatform.Android)
		{
			obj.CallStatic("sendEmail", to, cc, bcc, subject, body, pathToImageAttachment);
		}
	}

	public static void ShowConfirm(string title, string message, Action<bool> callback = null, string positiveBtnText = "Ok", string negativeBtnText = "Cancel")
	{
		Instance.Awake();
		NativeToolkit.OnDialogComplete = callback;
		if (Application.platform == RuntimePlatform.Android)
		{
			obj.CallStatic("showConfirm", title, message, positiveBtnText, negativeBtnText);
		}
	}

	public static void ShowAlert(string title, string message, Action<bool> callback = null, string btnText = "Ok")
	{
		Instance.Awake();
		NativeToolkit.OnDialogComplete = callback;
		if (Application.platform == RuntimePlatform.Android)
		{
			obj.CallStatic("showAlert", title, message, btnText);
		}
	}

	public void OnDialogPress(string result)
	{
		if (NativeToolkit.OnDialogComplete != null)
		{
			if (result == "Yes")
			{
				NativeToolkit.OnDialogComplete(obj: true);
			}
			else if (result == "No")
			{
				NativeToolkit.OnDialogComplete(obj: false);
			}
		}
	}

	public static void RateApp(string title = "Rate This App", string message = "Please take a moment to rate this App", string positiveBtnText = "Rate Now", string neutralBtnText = "Later", string negativeBtnText = "No, Thanks", string appleId = "", Action<string> callback = null)
	{
		Instance.Awake();
		NativeToolkit.OnRateComplete = callback;
		if (Application.platform == RuntimePlatform.Android)
		{
			obj.CallStatic("rateThisApp", title, message, positiveBtnText, neutralBtnText, negativeBtnText);
		}
	}

	public void OnRatePress(string result)
	{
		if (NativeToolkit.OnRateComplete != null)
		{
			NativeToolkit.OnRateComplete(result);
		}
	}

	public static bool StartLocation()
	{
		if (!Input.location.isEnabledByUser)
		{
			UnityEngine.Debug.Log("Location service disabled");
			return false;
		}
		if (Input.location.status != LocationServiceStatus.Running)
		{
			UnityEngine.Debug.Log("Start location service");
			Input.location.Start(10f, 1f);
		}
		return true;
	}

	public static float GetLongitude()
	{
		if (!Input.location.isEnabledByUser)
		{
			return 0f;
		}
		return Input.location.lastData.longitude;
	}

	public static float GetLatitude()
	{
		if (!Input.location.isEnabledByUser)
		{
			return 0f;
		}
		return Input.location.lastData.latitude;
	}

	public static string GetCountryCode()
	{
		Instance.Awake();
		string result = null;
		if (Application.platform == RuntimePlatform.Android)
		{
			result = obj.CallStatic<string>("getLocale", new object[0]);
		}
		return result;
	}

	public static void ScheduleLocalNotification(string title, string message, int id = 0, int delayInMinutes = 0, string sound = "default_sound", bool vibrate = false, string smallIcon = "ic_notification", string largeIcon = "ic_notification_large")
	{
	}

	public static void ClearLocalNotification(int id)
	{
	}

	public static void ClearAllLocalNotifications()
	{
	}

	public static bool WasLaunchedFromNotification()
	{
		return false;
	}

	public static Texture2D LoadImageFromFile(string path)
	{
		if (path == "Cancelled")
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(128, 128, TextureFormat.RGB24,  false);
		byte[] data = File.ReadAllBytes(path);
		texture2D.LoadImage(data);
		return texture2D;
	}

	private IEnumerator Wait(float delay)
	{
		float pauseTarget = Time.realtimeSinceStartup + delay;
		while (Time.realtimeSinceStartup < pauseTarget)
		{
			yield return null;
		}
	}
}
