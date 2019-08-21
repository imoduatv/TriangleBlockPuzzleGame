using System;
using UnityEngine;

public class GP_RTM_Network_Package
{
	private string _playerId;

	private byte[] _buffer;

	public string participantId => _playerId;

	public byte[] buffer => _buffer;

	public GP_RTM_Network_Package(string player, string recievedData)
	{
		_playerId = player;
		UnityEngine.Debug.Log("GOOGLE_PLAY_RESULT -> OnMatchDataRecieved " + recievedData);
		_buffer = Convert.FromBase64String(recievedData);
	}
}
