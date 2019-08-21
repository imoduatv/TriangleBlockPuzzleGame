using Archon.SwissArmyLib.Utils;

namespace Archon.SwissArmyLib.Events.Loops
{
	public class FrameIntervalUpdateLoop : CustomUpdateLoopBase
	{
		private int _previousUpdateFrame;

		private int _nextUpdateFrame;

		private int _interval;

		public override bool IsTimeToRun => BetterTime.FrameCount >= _nextUpdateFrame;

		public int Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				_interval = value;
				_nextUpdateFrame = _previousUpdateFrame + value;
			}
		}

		public FrameIntervalUpdateLoop(int eventId, int interval)
		{
			base.Event = new Event(eventId);
			Interval = interval;
		}

		public override void Invoke()
		{
			base.Invoke();
			_previousUpdateFrame = BetterTime.FrameCount;
			_nextUpdateFrame = _previousUpdateFrame + Interval;
		}
	}
}
