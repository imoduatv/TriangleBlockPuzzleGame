using SA.Common.Models;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WSAMultimediaManager
{
	[CompilerGenerated]
	private static Action<Texture2D> _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static Action<Texture2D> _003C_003Ef__mg_0024cache1;

	public static void PickImageFromGallery()
	{
	}

	public static void SaveScreenshot()
	{
		ScreenshotMaker screenshotMaker = ScreenshotMaker.Create();
		screenshotMaker.OnScreenshotReady = (Action<Texture2D>)Delegate.Combine(screenshotMaker.OnScreenshotReady, new Action<Texture2D>(ScreenshotReady));
		ScreenshotMaker.Create().GetScreenshot();
	}

	private static void ScreenshotReady(Texture2D screenshot)
	{
		UnityEngine.Debug.Log("[ScreenshotReady]");
		ScreenshotMaker screenshotMaker = ScreenshotMaker.Create();
		screenshotMaker.OnScreenshotReady = (Action<Texture2D>)Delegate.Remove(screenshotMaker.OnScreenshotReady, new Action<Texture2D>(ScreenshotReady));
	}
}
