using UnityEngine;
using UnityEngine.UI;

public class ImageSeasonColor : MonoBehaviour
{
	public Color[] colors;

	private Image image;

	private Image Image()
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}
		return image;
	}

	public void SetColor(int index)
	{
		if (index < colors.Length)
		{
			Image().color = colors[index];
		}
	}
}
