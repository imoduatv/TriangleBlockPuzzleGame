public class UM_Error
{
	protected int _Code;

	protected string _Description;

	public int Code => _Code;

	public string Description => _Description;

	public UM_Error(int code, string description)
	{
		_Code = code;
		_Description = description;
	}
}
