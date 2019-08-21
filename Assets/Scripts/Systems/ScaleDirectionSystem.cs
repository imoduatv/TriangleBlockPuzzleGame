using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class ScaleDirectionSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.ScaleDirect).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				Vector3 localScale = entity.transform.data.localScale;
				localScale.x *= entity.scaleDirect.x;
				localScale.y *= entity.scaleDirect.y;
				entity.transform.data.localScale = localScale;
			}
		}
	}
}
