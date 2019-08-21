using Entitas;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Systems
{
	public class DestroyGridSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Grid, Matcher.Destroy).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			Pool pool = Pools.pool;
			foreach (Entity entity in entities)
			{
				if (entity.hasText)
				{
					Text data = entity.text.data;
					data.gameObject.Recycle();
				}
			}
		}
	}
}
