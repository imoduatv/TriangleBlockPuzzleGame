using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class GridNameSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.Grid, Matcher.Prefab).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				int row = entity.grid.row;
				int col = entity.grid.col;
				if (Application.isMobilePlatform)
				{
					entity.transform.data.gameObject.name = entity.prefab.gameObject.name + "-(" + row + ", " + col + ")";
				}
			}
		}
	}
}
