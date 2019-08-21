public class MNT_BluetoothDeviceTemplate
{
	private string _name = string.Empty;

	private string _address = string.Empty;

	public string Name => _name;

	public string Address => _address;

	public MNT_BluetoothDeviceTemplate(string name, string address)
	{
		_name = name;
		_address = address;
	}
}
