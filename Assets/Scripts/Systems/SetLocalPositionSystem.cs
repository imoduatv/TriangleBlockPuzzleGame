using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class SetLocalPositionSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.LocalPosition).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				Vector3 localPosition = entity.transform.data.localPosition;
				localPosition.x = entity.localPosition.x;
				localPosition.y = entity.localPosition.y;
				entity.transform.data.localPosition = localPosition;
			}
		}
	}
}
