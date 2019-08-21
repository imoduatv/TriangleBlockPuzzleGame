namespace Archon.SwissArmyLib.Automata
{
	public abstract class FsmState<T> : BaseState<FiniteStateMachine<T>, T>, IFsmState<T>, IState<FiniteStateMachine<T>, T>
	{
	}
}
