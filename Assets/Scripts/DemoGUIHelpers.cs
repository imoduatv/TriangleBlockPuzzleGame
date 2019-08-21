using Prime31.ZestKit;
using System;
using UnityEngine;

public static class DemoGUIHelpers
{
	private static string[] _allEaseTypes;

	private static GUIContent[] comboBoxList;

	private static ComboBox comboBoxControl;

	private static GUIStyle listStyle;

	private static bool _didRetinaIpadCheck;

	private static bool _isRetinaIpad;

	static DemoGUIHelpers()
	{
		listStyle = new GUIStyle();
		_allEaseTypes = Enum.GetNames(typeof(EaseType));
		comboBoxList = new GUIContent[_allEaseTypes.Length];
		for (int i = 0; i < _allEaseTypes.Length; i++)
		{
			comboBoxList[i] = new GUIContent(_allEaseTypes[i]);
		}
		listStyle.normal.textColor = Color.white;
		GUIStyleState onHover = listStyle.onHover;
		Texture2D background = new Texture2D(2, 2);
		listStyle.hover.background = background;
		onHover.background = background;
		RectOffset padding = listStyle.padding;
		int num = 4;
		listStyle.padding.bottom = num;
		num = num;
		listStyle.padding.top = num;
		num = num;
		listStyle.padding.right = num;
		padding.left = num;
		comboBoxControl = new ComboBox(new Rect(Screen.width - 140, 20f, 120f, buttonHeight()), comboBoxList[0], comboBoxList, "button", "box", listStyle);
		comboBoxControl.SelectedItemIndex = 14;
	}

	private static bool isRetinaOrLargeScreen()
	{
		return Screen.width >= 960 || Screen.height >= 960;
	}

	private static bool isRetinaIpad()
	{
		if (!_didRetinaIpadCheck)
		{
			if (Screen.height >= 2048 || Screen.width >= 2048)
			{
				_isRetinaIpad = true;
			}
			_didRetinaIpadCheck = true;
		}
		return _isRetinaIpad;
	}

	private static int buttonHeight()
	{
		if (isRetinaOrLargeScreen())
		{
			if (isRetinaIpad())
			{
				return 140;
			}
			return 70;
		}
		return 30;
	}

	private static int buttonFontSize()
	{
		if (isRetinaOrLargeScreen())
		{
			if (isRetinaIpad())
			{
				return 40;
			}
			return 25;
		}
		return 15;
	}

	public static void easeTypesGUI()
	{
		if (comboBoxControl.Show())
		{
			EaseType easeType = ZestKit.defaultEaseType = (EaseType)Enum.Parse(typeof(EaseType), _allEaseTypes[comboBoxControl.SelectedItemIndex]);
		}
	}

	public static void setupGUIButtons()
	{
		GUI.skin.button.fontSize = buttonFontSize();
		GUI.skin.button.margin = new RectOffset(0, 0, 10, 0);
		GUI.skin.button.stretchWidth = true;
		GUI.skin.button.fixedHeight = buttonHeight();
		GUI.skin.button.wordWrap = false;
		GUI.skin.button.active.textColor = Color.black;
	}

	public static float durationSlider(float duration, float maxDuration = 2f)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label($"Duration: {duration:0.0}", GUILayout.Width(80f));
		GUI.skin.horizontalSlider.margin = new RectOffset(4, 4, 10, 4);
		float result = GUILayout.HorizontalSlider(duration, 0f, maxDuration, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		return result;
	}
}
