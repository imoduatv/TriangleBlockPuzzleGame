namespace Entitas
{
	public interface IGroupObserverSystem : IReactiveExecuteSystem, ISystem
	{
		GroupObserver groupObserver
		{
			get;
		}
	}
}
