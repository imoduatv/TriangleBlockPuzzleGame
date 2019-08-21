using SA.Common.Pattern;
using SA.Common.Util;
using System;
using System.Text;

public class ISN_LocalNotification
{
	private int _Id;

	private DateTime _Date;

	private string _Message = string.Empty;

	private bool _UseSound = true;

	private int _Badges;

	private string _Data = string.Empty;

	private string _SoundName = string.Empty;

	private const string DATA_SPLITTER = "|||";

	public int Id => _Id;

	public DateTime Date => _Date;

	public bool IsFired
	{
		get
		{
			if (DateTime.Now.Ticks > Date.Ticks)
			{
				return true;
			}
			return false;
		}
	}

	public string Message => _Message;

	public bool UseSound => _UseSound;

	public int Badges => _Badges;

	public string Data => _Data;

	public string SoundName => _SoundName;

	public string SerializedString => Convert.ToBase64String(Encoding.UTF8.GetBytes(Id.ToString() + "|||" + UseSound.ToString() + "|||" + Badges.ToString() + "|||" + Data + "|||" + SoundName + "|||" + Date.Ticks.ToString()));

	public ISN_LocalNotification(DateTime time, string message, bool useSound = true)
	{
		_Id = IdFactory.NextId;
		_Date = time;
		_Message = message;
		_UseSound = useSound;
	}

	public ISN_LocalNotification(string serializaedNotificationData)
	{
		try
		{
			string[] array = serializaedNotificationData.Split(new string[1]
			{
				"|||"
			}, StringSplitOptions.None);
			_Id = Convert.ToInt32(array[0]);
			_UseSound = Convert.ToBoolean(array[1]);
			_Badges = Convert.ToInt32(array[2]);
			_Data = array[3];
			_SoundName = array[4];
			_Date = new DateTime(Convert.ToInt64(array[5]));
		}
		catch (Exception ex)
		{
			ISN_Logger.Log("Failed to deserialize the ISN_LocalNotification object");
			ISN_Logger.Log(ex.Message);
		}
	}

	public void SetId(int id)
	{
		_Id = id;
	}

	public void SetData(string data)
	{
		_Data = data;
	}

	public void SetSoundName(string soundName)
	{
		_SoundName = soundName;
	}

	public void SetBadgesNumber(int badges)
	{
		_Badges = badges;
	}

	public void Schedule()
	{
		Singleton<ISN_LocalNotificationsController>.Instance.ScheduleNotification(this);
	}
}
