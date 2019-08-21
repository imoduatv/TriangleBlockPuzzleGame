using UnityEngine;
using UnityEngine.UI;

public class TextSeasonColor : MonoBehaviour
{
	public Color[] colors;

	private Text text;

	private Text Text()
	{
		if (text == null)
		{
			text = GetComponent<Text>();
		}
		return text;
	}

	public void SetColor(int index)
	{
		if (index < colors.Length)
		{
			Text().color = colors[index];
		}
	}
}
