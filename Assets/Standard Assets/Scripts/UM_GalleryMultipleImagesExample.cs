using SA.Common.Pattern;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UM_GalleryMultipleImagesExample : MonoBehaviour
{
	public Image[] Images;

	public void LoadImages()
	{
		Singleton<UM_Camera>.Instance.OnImagesPicked += HandleOnImagesPicked;
		Singleton<UM_Camera>.Instance.GetImagesFromGallery();
	}

	private void HandleOnImagesPicked(UM_ImagesPickResult result)
	{
		Singleton<UM_Camera>.Instance.OnImagesPicked -= HandleOnImagesPicked;
		if (result.IsSucceeded && Images != null)
		{
			int num = 0;
			foreach (KeyValuePair<string, Texture2D> image in result.Images)
			{
				if (num < Images.Length)
				{
					Images[num++].sprite = Sprite.Create(image.Value, new Rect(0f, 0f, image.Value.width, image.Value.height), new Vector2(0.5f, 0.5f));
				}
			}
		}
	}
}
