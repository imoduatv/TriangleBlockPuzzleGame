namespace Archon.SwissArmyLib.Events.Loops
{
	public interface ICustomUpdateLoop
	{
		Event Event
		{
			get;
		}

		bool IsTimeToRun
		{
			get;
		}

		float DeltaTime
		{
			get;
		}

		float UnscaledDeltaTime
		{
			get;
		}

		void Invoke();
	}
}
