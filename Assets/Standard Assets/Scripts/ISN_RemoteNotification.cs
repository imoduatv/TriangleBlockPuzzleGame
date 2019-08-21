public class ISN_RemoteNotification
{
	private string _Body = "{}";

	public string Body => _Body;

	public ISN_RemoteNotification(string body)
	{
		_Body = body;
	}
}
