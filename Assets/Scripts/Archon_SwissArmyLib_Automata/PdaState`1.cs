namespace Archon.SwissArmyLib.Automata
{
	public class PdaState<T> : BaseState<PushdownAutomaton<T>, T>, IPdaState<T>, IState<PushdownAutomaton<T>, T>
	{
		public virtual void Pause()
		{
		}

		public virtual void Resume()
		{
		}
	}
}
