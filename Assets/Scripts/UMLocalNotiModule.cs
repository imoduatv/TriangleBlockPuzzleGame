using Root;
using SA.Common.Pattern;

public class UMLocalNotiModule : ILocalNotiService
{
	public void CancalAllNoti()
	{
		SA.Common.Pattern.Singleton<UM_NotificationController>.Instance.CancelAllLocalNotifications();
	}

	public void CancelNoti(int notiID)
	{
		SA.Common.Pattern.Singleton<UM_NotificationController>.Instance.CancelLocalNotification(notiID);
	}

	public int ScheduleNoti(string title, string message, int time)
	{
		return SA.Common.Pattern.Singleton<UM_NotificationController>.Instance.ScheduleLocalNotification(title, message, time);
	}
}
