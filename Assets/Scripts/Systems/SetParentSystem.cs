using Components;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class SetParentSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.Parent).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				Components.Transform transform = entity.transform;
				Parent parent = entity.parent;
				transform.data.SetParent(parent.data);
				transform.data.localScale = new Vector3(1f, 1f, 1f);
				if (entity.hasScale)
				{
					Vector3 localScale = default(Vector3);
					localScale.x = entity.scale.x;
					localScale.y = entity.scale.y;
					localScale.z = 1f;
					transform.data.localScale = localScale;
				}
			}
		}
	}
}
