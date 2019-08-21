using Archon.SwissArmyLib.Utils;

namespace Archon.SwissArmyLib.Events.Loops
{
	public abstract class CustomUpdateLoopBase : ICustomUpdateLoop
	{
		protected float PreviousRunTimeScaled;

		protected float PreviousRunTimeUnscaled;

		public Event Event
		{
			get;
			protected set;
		}

		public abstract bool IsTimeToRun
		{
			get;
		}

		public float DeltaTime => BetterTime.Time - PreviousRunTimeScaled;

		public float UnscaledDeltaTime => BetterTime.UnscaledTime - PreviousRunTimeUnscaled;

		public virtual void Invoke()
		{
			Event.Invoke();
			PreviousRunTimeScaled = BetterTime.Time;
			PreviousRunTimeUnscaled = BetterTime.UnscaledTime;
		}
	}
}
