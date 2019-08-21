using SA.Common.Pattern;
using UnityEngine;

public class UM_ExampleStatusBar : Singleton<UM_ExampleStatusBar>
{
	public string _text;

	private float h = 50f;

	private GUIStyle style;

	public static string text
	{
		get
		{
			return Singleton<UM_ExampleStatusBar>.Instance._text;
		}
		set
		{
			Singleton<UM_ExampleStatusBar>.Instance._text = value;
		}
	}

	private void Awake()
	{
		style = new GUIStyle();
		style.fontSize = 18;
		style.fontStyle = FontStyle.Italic;
		style.alignment = TextAnchor.MiddleRight;
		style.normal.textColor = Color.white;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0f, (float)Screen.height - h, Screen.width - 30, h), _text, style);
	}
}
