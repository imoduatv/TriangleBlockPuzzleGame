using Components;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class SetScaleSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.Scale).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				Components.Transform transform = entity.transform;
				Vector3 localScale = default(Vector3);
				localScale.x = entity.scale.x;
				localScale.y = entity.scale.y;
				localScale.z = 1f;
				transform.data.localScale = localScale;
			}
		}
	}
}
