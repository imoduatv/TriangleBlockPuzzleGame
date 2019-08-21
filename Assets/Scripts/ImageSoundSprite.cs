using UnityEngine;
using UnityEngine.UI;

public class ImageSoundSprite : MonoBehaviour
{
	public Sprite soundOn;

	public Sprite soundOff;

	private Image image;

	private Image Image()
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}
		return image;
	}

	public void SetSoundOn()
	{
		Image().sprite = soundOn;
	}

	public void SetSoundOff()
	{
		Image().sprite = soundOff;
	}
}
