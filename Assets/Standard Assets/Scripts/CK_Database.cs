using System;
using System.Collections.Generic;
using System.Threading;

public class CK_Database
{
	private static Dictionary<int, CK_Database> _Databases = new Dictionary<int, CK_Database>();

	private int _InternalId;

	public int InternalId => _InternalId;

	public event Action<CK_RecordResult> ActionRecordSaved;

	public event Action<CK_RecordResult> ActionRecordFetchComplete;

	public event Action<CK_RecordDeleteResult> ActionRecordDeleted;

	public event Action<CK_QueryResult> ActionQueryComplete;

	public CK_Database(int internalId)
	{
		this.ActionRecordSaved = delegate
		{
		};
		this.ActionRecordFetchComplete = delegate
		{
		};
		this.ActionRecordDeleted = delegate
		{
		};
		this.ActionQueryComplete = delegate
		{
		};
		//base._002Ector();
		_InternalId = internalId;
		_Databases.Add(_InternalId, this);
	}

	public void SaveRecrod(CK_Record record)
	{
		record.UpdateRecord();
		ISN_CloudKit.SaveRecord(_InternalId, record.Internal_Id);
	}

	public void FetchRecordWithID(CK_RecordID recordId)
	{
		ISN_CloudKit.FetchRecord(_InternalId, recordId.Internal_Id);
	}

	public void DeleteRecordWithID(CK_RecordID recordId)
	{
		ISN_CloudKit.DeleteRecord(_InternalId, recordId.Internal_Id);
	}

	public void PerformQuery(CK_Query query)
	{
		ISN_CloudKit.PerformQuery(_InternalId, query.Predicate, query.RecordType);
	}

	public static CK_Database GetDatabaseByInternalId(int id)
	{
		return _Databases[id];
	}

	public void FireSaveRecordResult(CK_RecordResult result)
	{
		result.SetDatabase(this);
		this.ActionRecordSaved(result);
	}

	public void FireFetchRecordResult(CK_RecordResult result)
	{
		result.SetDatabase(this);
		this.ActionRecordFetchComplete(result);
	}

	public void FireDeleteRecordResult(CK_RecordDeleteResult result)
	{
		result.SetDatabase(this);
		this.ActionRecordDeleted(result);
	}

	public void FireQueryCompleteResult(CK_QueryResult result)
	{
		result.SetDatabase(this);
		this.ActionQueryComplete(result);
	}
}
