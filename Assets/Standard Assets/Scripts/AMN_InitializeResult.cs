public class AMN_InitializeResult : AMN_Result
{
	private string error;

	public string Error => error;

	public AMN_InitializeResult(bool success)
		: base(success)
	{
	}

	public AMN_InitializeResult(string err)
		: base(success: false)
	{
		error = err;
	}
}
