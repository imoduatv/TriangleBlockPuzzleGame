using ANMiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FB_PostResult : FB_Result
{
	private string _PostId = string.Empty;

	public string PostId => _PostId;

	public FB_PostResult(string RawData, string Error)
		: base(RawData, Error)
	{
		if (_IsSucceeded)
		{
			try
			{
				Dictionary<string, object> dictionary = Json.Deserialize(RawData) as Dictionary<string, object>;
				_PostId = Convert.ToString(dictionary["id"]);
				_IsSucceeded = true;
			}
			catch (Exception ex)
			{
				_IsSucceeded = false;
				UnityEngine.Debug.Log("No Post Id: " + ex.Message);
			}
		}
	}
}
