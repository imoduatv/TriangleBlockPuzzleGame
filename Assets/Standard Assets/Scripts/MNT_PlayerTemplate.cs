using UnityEngine;
public class MNT_PlayerTemplate
{
	private string _id;

	private string _name;

	private byte[] _info;

	private bool _IsServer;

	private string _deviceName;

	private string _macAddress;

	private string _externalIP;

	private int _externalPort;

	private string _guid;

	private string _ipAddress;

	private int _port;

	public string id => _id;

	public string name => _name;

	public byte[] info => _info;

	public bool IsServer => _IsServer;

	public string deviceName => _deviceName;

	public string macAddress => _macAddress;

	public string externalIP => _externalIP;

	public int externalPort => _externalPort;

	public string guid => _guid;

	public string ipAddress => _ipAddress;

	public int port => _port;

	public MNT_PlayerTemplate(string pId, string pName, byte[] pInfo)
	{
		_id = pId;
		_name = pName;
		_info = pInfo;
	}

	public MNT_PlayerTemplate(MNT_PlayerTemplate player)
	{
		_id = player.id;
		_deviceName = player.deviceName;
		_macAddress = player.macAddress;
		_externalIP = player.externalIP;
		_externalPort = player.externalPort;
		_guid = player.guid;
		_ipAddress = player.ipAddress;
		_port = player.port;
	}

    //public MNT_PlayerTemplate(NetworkPlayer player)
    //{
    //    _externalIP = player.externalIP;
    //    _externalPort = player.externalPort;
    //    _guid = player.guid;
    //    _ipAddress = player.ipAddress;
    //    _port = player.port;
    //    _id = player.ipAddress;
    //}

    public MNT_PlayerTemplate(MNT_BluetoothDeviceTemplate device)
	{
		_deviceName = device.Name;
		_macAddress = device.Address;
		_id = device.Address;
	}

	public void SetInfo(string playerName, byte[] PlayerInfo, bool IsServerPlayer = false)
	{
		_name = playerName;
		_info = PlayerInfo;
		_IsServer = IsServerPlayer;
	}
}
