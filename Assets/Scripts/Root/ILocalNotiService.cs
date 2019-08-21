namespace Root
{
	public interface ILocalNotiService
	{
		int ScheduleNoti(string title, string message, int time);

		void CancelNoti(int notiID);

		void CancalAllNoti();
	}
}
