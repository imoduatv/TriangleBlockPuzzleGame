using Archon.SwissArmyLib.Utils;

namespace Archon.SwissArmyLib.Events.Loops
{
	public class TimeIntervalUpdateLoop : CustomUpdateLoopBase
	{
		private float _nextUpdateTime;

		private float _interval;

		public override bool IsTimeToRun
		{
			get
			{
				float num = (!UsingScaledTime) ? BetterTime.UnscaledTime : BetterTime.Time;
				return num >= _nextUpdateTime;
			}
		}

		public float Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				_interval = value;
				float num = (!UsingScaledTime) ? PreviousRunTimeUnscaled : PreviousRunTimeScaled;
				_nextUpdateTime = num + value;
			}
		}

		public bool UsingScaledTime
		{
			get;
			private set;
		}

		public TimeIntervalUpdateLoop(int eventId, float interval, bool usingScaledTime = true)
		{
			base.Event = new Event(eventId);
			Interval = interval;
			UsingScaledTime = usingScaledTime;
		}

		public override void Invoke()
		{
			base.Invoke();
			float num = (!UsingScaledTime) ? BetterTime.UnscaledTime : BetterTime.Time;
			_nextUpdateTime = num + Interval;
		}
	}
}
