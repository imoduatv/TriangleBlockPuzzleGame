using SA.Common.Pattern;
using System;
using System.Threading;

namespace SA.IOSNative.UIKit
{
	public static class DateTimePicker
	{
		private static event Action<DateTime> OnPickerClosed;

		private static event Action<DateTime> OnPickerDateChanged;

		static DateTimePicker()
		{
			Singleton<NativeReceiver>.Instance.Init();
		}

		public static void Show(DateTimePickerMode mode, Action<DateTime> callback)
		{
			DateTimePicker.OnPickerClosed = callback;
		}

		public static void Show(DateTimePickerMode mode, DateTime dateTime, Action<DateTime> callback)
		{
			DateTimePicker.OnPickerClosed = callback;
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			double totalSeconds = (dateTime - d).TotalSeconds;
		}

		internal static void DateChangedEvent(string time)
		{
			DateTime obj = DateTime.Parse(time);
			DateTimePicker.OnPickerDateChanged(obj);
		}

		internal static void PickerClosed(string time)
		{
			DateTime obj = DateTime.Parse(time);
			DateTimePicker.OnPickerClosed(obj);
		}
	}
}
