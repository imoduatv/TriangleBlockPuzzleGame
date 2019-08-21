using UnityEngine;

public static class GameStateManager
{
	public static string Username
	{
		get;
		internal set;
	}

	public static Texture UserTexture
	{
		get;
		internal set;
	}

	internal static void CallUIRedraw()
	{
	}
}
