using UnityEngine;
using UnityEngine.UI;

public class ImageSeasonSprite : MonoBehaviour
{
	public Sprite[] sprites;

	private Image image;

	private Image Image()
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}
		return image;
	}

	public void SetSprite(int index)
	{
		if (index < sprites.Length)
		{
			Image().sprite = sprites[index];
		}
	}
}
