using Entitas;
using System.Collections.Generic;

namespace Systems
{
	public class TestReactiveSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Test).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
			}
		}
	}
}
