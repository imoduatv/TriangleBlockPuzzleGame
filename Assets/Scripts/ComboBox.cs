using UnityEngine;

public class ComboBox
{
	private static bool forceToUnShow;

	private static int useControlID = -1;

	private bool isClickedComboButton;

	private int selectedItemIndex;

	private Rect rect;

	private GUIContent buttonContent;

	private GUIContent[] listContent;

	private string buttonStyle;

	private string boxStyle;

	private GUIStyle listStyle;

	private Vector2 _scrollPos;

	public int SelectedItemIndex
	{
		get
		{
			return selectedItemIndex;
		}
		set
		{
			selectedItemIndex = value;
			buttonContent = listContent[selectedItemIndex];
		}
	}

	public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle)
	{
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		buttonStyle = "button";
		boxStyle = "box";
		this.listStyle = listStyle;
	}

	public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle, string boxStyle, GUIStyle listStyle)
	{
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = buttonStyle;
		this.boxStyle = boxStyle;
		this.listStyle = listStyle;
	}

	public bool Show()
	{
		if (forceToUnShow)
		{
			forceToUnShow = false;
			isClickedComboButton = false;
		}
		bool result = false;
		bool flag = false;
		int controlID = GUIUtility.GetControlID(FocusType.Passive);
		EventType typeForControl = Event.current.GetTypeForControl(controlID);
		if (typeForControl == EventType.MouseUp && isClickedComboButton)
		{
			flag = true;
		}
		if (GUI.Button(this.rect, buttonContent, buttonStyle))
		{
			if (useControlID == -1)
			{
				useControlID = controlID;
				isClickedComboButton = false;
			}
			if (useControlID != controlID)
			{
				forceToUnShow = true;
				useControlID = controlID;
			}
			isClickedComboButton = true;
		}
		if (isClickedComboButton)
		{
			Rect rect = new Rect(this.rect.x, this.rect.y + listStyle.CalcHeight(listContent[0], 1f), this.rect.width, listStyle.CalcHeight(listContent[0], 1f) * (float)listContent.Length);
			_scrollPos = GUI.BeginScrollView(new Rect(this.rect.xMin, this.rect.yMin + this.rect.height, this.rect.width + 18f, (float)Screen.height - 100f), _scrollPos, rect, alwaysShowHorizontal: false, alwaysShowVertical: true);
			GUI.Box(rect, string.Empty, boxStyle);
			int num = GUI.SelectionGrid(rect, selectedItemIndex, listContent, 1, listStyle);
			if (num != selectedItemIndex)
			{
				selectedItemIndex = num;
				buttonContent = listContent[selectedItemIndex];
				result = true;
			}
			GUI.EndScrollView(handleScrollWheel: true);
		}
		if (flag)
		{
			isClickedComboButton = false;
		}
		return result;
	}
}
