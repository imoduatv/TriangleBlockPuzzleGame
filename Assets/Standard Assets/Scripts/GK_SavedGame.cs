using SA.Common.Models;
using SA.Common.Pattern;
using System;
using System.Threading;
using UnityEngine;

public class GK_SavedGame
{
	private string _Id;

	private string _Name;

	private string _DeviceName;

	private DateTime _ModificationDate;

	private string _OriginalDateString;

	private byte[] _Data;

	private bool _IsDataLoaded;

	public string Id => _Id;

	public string Name => _Name;

	public string DeviceName => _DeviceName;

	public DateTime ModificationDate => _ModificationDate;

	public string OriginalDateString => _OriginalDateString;

	public byte[] Data => _Data;

	public bool IsDataLoaded => _IsDataLoaded;

	public event Action<GK_SaveDataLoaded> ActionDataLoaded;

	public GK_SavedGame(string id, string name, string device, string dateString)
	{
		this.ActionDataLoaded = delegate
		{
		};
		//base._002Ector();
		_Id = id;
		_Name = name;
		_DeviceName = device;
		_OriginalDateString = dateString;
		_ModificationDate = DateTime.Now;
		try
		{
			_ModificationDate = DateTime.Parse(dateString);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("GK_SavedGame Date parsing issue, cnonsider to parce date byyourself using  _OriginalDateString property");
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	public void LoadData()
	{
		Singleton<ISN_GameSaves>.Instance.LoadSaveData(this);
	}

	public void GenerateDataLoadEvent(string base64Data)
	{
		_Data = Convert.FromBase64String(base64Data);
		_IsDataLoaded = true;
		GK_SaveDataLoaded obj = new GK_SaveDataLoaded(this);
		this.ActionDataLoaded(obj);
	}

	public void GenerateDataLoadFailedEvent(string erorrData)
	{
		Error error = new Error(erorrData);
		GK_SaveDataLoaded obj = new GK_SaveDataLoaded(error);
		this.ActionDataLoaded(obj);
	}
}
