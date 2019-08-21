using Facebook.Unity;
using System;
using UnityEngine;

public static class FBGraph
{
	private static MonoBehaviour coroutiner;

	public static void GetPlayerInfo(string appScopeId, Action<IGraphResult> callback)
	{
		string query = "/" + appScopeId + "?fields=id,first_name,name";
		FB.API(query, HttpMethod.GET, delegate(IGraphResult result)
		{
			UnityEngine.Debug.Log(result.RawResult);
			callback(result);
		});
	}

	public static void GetPlayerAvatar(string appScopeId, Action<IGraphResult> callback)
	{
		string query = "/" + appScopeId + "?fields=id,picture.width(240).height(240)";
		FB.API(query, HttpMethod.GET, delegate(IGraphResult result)
		{
			UnityEngine.Debug.Log(result.RawResult);
			callback(result);
		});
	}

	public static void GetScores(Action<IGraphResult> callback)
	{
		FB.API("/app/scores?fields=score,user.limit(20)", HttpMethod.GET, delegate(IGraphResult result)
		{
			callback(result);
		});
	}

	public static void GetFriends(Action<IGraphResult> callback)
	{
		string query = "/me/friends?fields=id";
		FB.API(query, HttpMethod.GET, delegate(IGraphResult result)
		{
			callback(result);
		});
	}
}
