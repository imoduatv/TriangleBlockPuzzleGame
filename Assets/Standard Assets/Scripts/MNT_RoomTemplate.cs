public class MNT_RoomTemplate
{
	private int _size;

	private byte[] _data;

	public int size => _size;

	public byte[] data => _data;

	public MNT_RoomTemplate(int roomSize, byte[] roomData)
	{
		_size = roomSize;
		_data = roomData;
	}
}
