using Facebook.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class FBLogin
{
	private static readonly List<string> readPermissions = new List<string>
	{
		"public_profile",
		"user_friends"
	};

	private static readonly List<string> publishPermissions = new List<string>
	{
		"publish_actions"
	};

	public static bool HavePublishActions
	{
		get
		{
			return (FB.IsLoggedIn && (AccessToken.CurrentAccessToken.Permissions as List<string>).Contains("publish_actions")) ? true : false;
		}
		private set
		{
		}
	}

	public static void PromptForLogin(Action callback = null)
	{
		FB.LogInWithReadPermissions(readPermissions, delegate(ILoginResult result)
		{
			UnityEngine.Debug.Log("LoginCallback");
			if (FB.IsLoggedIn)
			{
				UnityEngine.Debug.Log("Logged in with ID: " + AccessToken.CurrentAccessToken.UserId + "\nGranted Permissions: " + AccessToken.CurrentAccessToken.Permissions.ToCommaSeparateList());
			}
			else
			{
				if (result.Error != null)
				{
					UnityEngine.Debug.LogError(result.Error);
				}
				UnityEngine.Debug.Log("Not Logged In");
			}
			if (callback != null)
			{
				callback();
			}
		});
	}

	public static void PromptForPublish(Action callback = null)
	{
		FB.LogInWithPublishPermissions(publishPermissions, delegate(ILoginResult result)
		{
			UnityEngine.Debug.Log("LoginCallback");
			if (FB.IsLoggedIn)
			{
				UnityEngine.Debug.Log("Logged in with ID: " + AccessToken.CurrentAccessToken.UserId + "\nGranted Permissions: " + AccessToken.CurrentAccessToken.Permissions.ToCommaSeparateList());
			}
			else
			{
				if (result.Error != null)
				{
					UnityEngine.Debug.LogError(result.Error);
				}
				UnityEngine.Debug.Log("Not Logged In");
			}
			if (callback != null)
			{
				callback();
			}
		});
	}
}
