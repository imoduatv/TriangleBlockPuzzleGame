public class AMN_Result
{
	private bool _isSuccess;

	public bool isSuccess => _isSuccess;

	public bool isFailure => !isSuccess;

	public AMN_Result(bool success)
	{
		_isSuccess = success;
	}
}
