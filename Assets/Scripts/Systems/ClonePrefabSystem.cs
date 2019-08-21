using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class ClonePrefabSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Prefab).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				GameObject gameObject = entity.prefab.gameObject.Spawn();
				if (Application.isMobilePlatform)
				{
					gameObject.name = entity.prefab.gameObject.name;
				}
				gameObject.transform.localScale = entity.prefab.gameObject.transform.localScale;
				entity.AddTransform(gameObject.transform);
			}
		}
	}
}
