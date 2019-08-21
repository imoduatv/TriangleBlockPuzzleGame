using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class SetPositionSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.Position).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				Vector3 position = entity.transform.data.position;
				position.x = entity.position.x;
				position.y = entity.position.y;
				entity.transform.data.position = position;
			}
		}
	}
}
