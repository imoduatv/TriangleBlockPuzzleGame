using SA.Common.Models;
using SA.Common.Pattern;
using System;
using System.Threading;
using UnityEngine;

public class UM_Camera : Singleton<UM_Camera>
{
	public event Action<UM_ImagePickResult> OnImagePicked;

	public event Action<UM_ImageSaveResult> OnImageSaved;

	public event Action<UM_ImagesPickResult> OnImagesPicked;

	public UM_Camera()
	{
		this.OnImagePicked = delegate
		{
		};
		this.OnImageSaved = delegate
		{
		};
		this.OnImagesPicked = delegate
		{
		};
		//base._002Ector();
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Singleton<AndroidCamera>.Instance.OnImagePicked += OnAndroidImagePicked;
		IOSCamera.OnImagePicked += OnIOSImagePicked;
		Singleton<AndroidCamera>.Instance.OnImageSaved += OnAndroidImageSaved;
		IOSCamera.OnImageSaved += OnIOSImageSaved;
		Singleton<AndroidCamera>.Instance.OnImagesPicked += HandleOnImagesPicked;
	}

	public void SaveImageToGalalry(Texture2D image)
	{
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			Singleton<AndroidCamera>.Instance.SaveImageToGallery(image);
			break;
		case RuntimePlatform.IPhonePlayer:
			Singleton<IOSCamera>.Instance.SaveTextureToCameraRoll(image);
			break;
		}
	}

	public void SaveScreenshotToGallery()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			Singleton<AndroidCamera>.Instance.SaveScreenshotToGallery();
			break;
		case RuntimePlatform.IPhonePlayer:
			Singleton<IOSCamera>.Instance.SaveScreenshotToCameraRoll();
			break;
		}
	}

	public void GetImageFromGallery()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			Singleton<AndroidCamera>.Instance.GetImageFromGallery();
			break;
		case RuntimePlatform.IPhonePlayer:
			Singleton<IOSCamera>.Instance.PickImage(ISN_ImageSource.Library);
			break;
		}
	}

	public void GetImagesFromGallery()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			Singleton<AndroidCamera>.Instance.GetImagesFromGallery();
			break;
		}
	}

	public void GetImageFromCamera()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			Singleton<AndroidCamera>.Instance.GetImageFromCamera();
			break;
		case RuntimePlatform.IPhonePlayer:
			Singleton<IOSCamera>.Instance.PickImage(ISN_ImageSource.Camera);
			break;
		}
	}

	private void HandleOnImagesPicked(AndroidImagesPickResult result)
	{
		this.OnImagesPicked(new UM_ImagesPickResult(result.IsSucceeded, result.Images));
	}

	private void OnAndroidImagePicked(AndroidImagePickResult obj)
	{
		UM_ImagePickResult obj2 = new UM_ImagePickResult(obj.Image);
		this.OnImagePicked(obj2);
	}

	private void OnIOSImagePicked(IOSImagePickResult obj)
	{
		UM_ImagePickResult obj2 = new UM_ImagePickResult(obj.Image);
		this.OnImagePicked(obj2);
	}

	private void OnAndroidImageSaved(GallerySaveResult res)
	{
		UM_ImageSaveResult obj = new UM_ImageSaveResult(res.imagePath, res.IsSucceeded);
		this.OnImageSaved(obj);
	}

	private void OnIOSImageSaved(Result res)
	{
		UM_ImageSaveResult obj = new UM_ImageSaveResult(string.Empty, res.IsSucceeded);
		this.OnImageSaved(obj);
	}
}
