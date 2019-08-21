public class CK_Query
{
	private string _Predicate;

	private string _RecordType;

	public string Predicate => _Predicate;

	public string RecordType => _RecordType;

	public CK_Query(string predicate, string recordType)
	{
		_Predicate = predicate;
		_RecordType = recordType;
	}
}
