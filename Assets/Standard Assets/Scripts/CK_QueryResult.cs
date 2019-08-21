using SA.Common.Models;
using System.Collections.Generic;

public class CK_QueryResult : Result
{
	private CK_Database _Database;

	private List<CK_Record> _Records = new List<CK_Record>();

	public CK_Database Database => _Database;

	public List<CK_Record> Records => _Records;

	public CK_QueryResult(List<CK_Record> records)
	{
		_Records = records;
	}

	public CK_QueryResult(string errorData)
		: base(new Error(errorData))
	{
	}

	public void SetDatabase(CK_Database database)
	{
		_Database = database;
	}
}
