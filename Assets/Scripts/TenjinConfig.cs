using System;
using UnityEngine;

[Serializable]
public class TenjinConfig
{
	public string ApiKey;

	[Header("Time out")]
	public int Timeout;

	[Space(10f)]
	[Header("Android")]
	[Header("Gender Key")]
	public string MaleAndroid;

	public string FemaleAndroid;

	[Space(10f)]
	[Header("IOS")]
	public string MaleIOS;

	public string FemaleIOS;
}
