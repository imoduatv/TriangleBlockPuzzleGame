using System.Collections;
using UnityEngine;

public class UM_SocialExample : MonoBehaviour
{
	private GUIStyle style;

	private GUIStyle style2;

	public Texture2D textureForPost;

	private void Awake()
	{
		InitStyles();
	}

	private void InitStyles()
	{
		style = new GUIStyle();
		style.normal.textColor = Color.white;
		style.fontSize = 16;
		style.fontStyle = FontStyle.BoldAndItalic;
		style.alignment = TextAnchor.UpperLeft;
		style.wordWrap = true;
		style2 = new GUIStyle();
		style2.normal.textColor = Color.white;
		style2.fontSize = 12;
		style2.fontStyle = FontStyle.Italic;
		style2.alignment = TextAnchor.UpperLeft;
		style2.wordWrap = true;
	}

	private void OnGUI()
	{
		float num = 20f;
		float num2 = 10f;
		GUI.Label(new Rect(num2, num, Screen.width, 40f), "Twitter", style);
		num += 25f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Post"))
		{
			UM_ShareUtility.TwitterShare("Titter posting test");
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Post Screehshot"))
		{
			StartCoroutine(PostTwitterScreenshot());
		}
		num += 55f;
		num2 = 10f;
		GUI.Label(new Rect(num2, num, Screen.width, 40f), "Facebook", style);
		num += 25f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Post"))
		{
			UM_ShareUtility.FacebookShare("Facebook posting test");
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Post Screehshot"))
		{
			StartCoroutine(PostFBScreenshot());
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Post Image"))
		{
			UM_ShareUtility.FacebookShare("Hello world", textureForPost);
		}
		num += 55f;
		num2 = 10f;
		GUI.Label(new Rect(num2, num, Screen.width, 40f), "Mail", style);
		num += 25f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Send Mail"))
		{
			UM_ShareUtility.SendMail("My E-mail Subject", "This is my text to share", "mail1@gmail.com, mail2@gmail.com");
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Send Mail with image"))
		{
			UM_ShareUtility.SendMail("My E-mail Subject", "This is my text to share <br> <strong> html text </strong>", "mail1@gmail.com, mail2@gmail.com", textureForPost);
		}
		num += 55f;
		num2 = 10f;
		GUI.Label(new Rect(num2, num, Screen.width, 40f), "Native Sharing", style);
		num += 25f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Text"))
		{
			UM_ShareUtility.ShareMedia("Title", "Some text to share");
		}
		num2 += 170f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Screehshot"))
		{
			StartCoroutine(PostScreenshot());
		}
		num += 55f;
		num2 = 10f;
		GUI.Label(new Rect(num2, num, Screen.width, 40f), "Whatsapp", style);
		num += 25f;
		if (GUI.Button(new Rect(num2, num, 150f, 50f), "Start Share"))
		{
			UM_ShareUtility.WhatsappShare("This is my text to share", textureForPost);
		}
	}

	private IEnumerator PostScreenshot()
	{
		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24,  false);
		tex.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		tex.Apply();
		UM_ShareUtility.ShareMedia("Title", "Some text to share", tex);
		UnityEngine.Object.Destroy(tex);
	}

	private IEnumerator PostTwitterScreenshot()
	{
		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24,  false);
		tex.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		tex.Apply();
		UM_ShareUtility.TwitterShare("My app ScreehShot", tex);
		UnityEngine.Object.Destroy(tex);
	}

	private IEnumerator PostFBScreenshot()
	{
		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24,  false);
		tex.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		tex.Apply();
		UM_ShareUtility.FacebookShare("My app ScreehShot", tex);
		UnityEngine.Object.Destroy(tex);
	}
}
