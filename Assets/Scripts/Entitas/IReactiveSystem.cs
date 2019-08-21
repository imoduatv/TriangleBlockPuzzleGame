namespace Entitas
{
	public interface IReactiveSystem : IReactiveExecuteSystem, ISystem
	{
		TriggerOnEvent trigger
		{
			get;
		}
	}
}
