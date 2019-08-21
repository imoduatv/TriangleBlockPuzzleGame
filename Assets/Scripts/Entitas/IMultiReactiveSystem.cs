namespace Entitas
{
	public interface IMultiReactiveSystem : IReactiveExecuteSystem, ISystem
	{
		TriggerOnEvent[] triggers
		{
			get;
		}
	}
}
