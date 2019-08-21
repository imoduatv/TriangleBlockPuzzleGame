public class AMN_RequestPlayerDataResult : AMN_Result
{
	private string error;

	private GC_Player player;

	public string Error => error;

	public GC_Player Player => player;

	public AMN_RequestPlayerDataResult(bool success)
		: base(success)
	{
	}

	public AMN_RequestPlayerDataResult(string err)
		: base(success: false)
	{
		error = err;
	}

	public AMN_RequestPlayerDataResult(GC_Player pl)
		: base(success: true)
	{
		player = pl;
	}
}
