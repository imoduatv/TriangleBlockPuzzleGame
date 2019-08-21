public class WP8_Result
{
	protected bool _IsSucceeded = true;

	public bool IsSucceeded => _IsSucceeded;

	public bool IsFailed => !_IsSucceeded;

	public WP8_Result(bool IsResultSucceeded)
	{
		_IsSucceeded = IsResultSucceeded;
	}
}
