namespace Archon.SwissArmyLib.Automata
{
	public interface IPdaState<T> : IState<PushdownAutomaton<T>, T>
	{
		void Pause();

		void Resume();
	}
}
