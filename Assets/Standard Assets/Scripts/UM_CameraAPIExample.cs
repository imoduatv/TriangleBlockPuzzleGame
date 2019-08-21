using SA.Common.Pattern;
using UnityEngine;

public class UM_CameraAPIExample : BaseIOSFeaturePreview
{
	public Texture2D hello_texture;

	public Texture2D darawTexgture;

	private void OnGUI()
	{
		UpdateToStartPos();
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40f), "Camera And Gallery", style);
		StartY += YLableStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Save Screenshot To Camera Roll"))
		{
			Singleton<UM_Camera>.Instance.OnImageSaved += OnImageSaved;
			Singleton<UM_Camera>.Instance.SaveScreenshotToGallery();
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Save Texture To Camera Roll"))
		{
			Singleton<UM_Camera>.Instance.OnImageSaved += OnImageSaved;
			Singleton<UM_Camera>.Instance.SaveImageToGalalry(hello_texture);
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Image From Album"))
		{
			Singleton<UM_Camera>.Instance.OnImagePicked += OnImage;
			Singleton<UM_Camera>.Instance.GetImageFromGallery();
		}
		StartX += XButtonStep;
		if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Image From Camera"))
		{
			Singleton<UM_Camera>.Instance.OnImagePicked += OnImage;
			Singleton<UM_Camera>.Instance.GetImageFromCamera();
		}
		StartX = XStartPos;
		StartY += YButtonStep;
		StartY += YLableStep;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40f), "PickedImage", style);
		StartY += YLableStep;
		if (darawTexgture != null)
		{
			GUI.DrawTexture(new Rect(StartX, StartY, buttonWidth, buttonWidth), darawTexgture);
		}
	}

	private void OnImageSaved(UM_ImageSaveResult result)
	{
		Singleton<UM_Camera>.Instance.OnImageSaved -= OnImageSaved;
		if (result.IsSucceeded)
		{
			MNPopup mNPopup = new MNPopup("Image Saved", result.imagePath);
			mNPopup.AddAction("Ok", delegate
			{
			});
			mNPopup.Show();
		}
		else
		{
			MNPopup mNPopup2 = new MNPopup("Failed", "Image Save Failed");
			mNPopup2.AddAction("Ok", delegate
			{
			});
			mNPopup2.Show();
		}
	}

	private void OnImage(UM_ImagePickResult result)
	{
		Singleton<UM_Camera>.Instance.OnImageSaved -= OnImageSaved;
		if (result.IsSucceeded)
		{
			darawTexgture = result.image;
		}
		Singleton<UM_Camera>.Instance.OnImagePicked -= OnImage;
	}
}
