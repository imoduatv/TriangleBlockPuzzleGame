using Entitas;
using System.Collections.Generic;

namespace Systems
{
	public class DestroySystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Destroy).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			Pool pool = Pools.pool;
			foreach (Entity entity in entities)
			{
				if (entity.hasTransform && entity.hasPrefab)
				{
					entity.transform.data.gameObject.name = entity.prefab.gameObject.name;
					entity.transform.data.localScale = entity.prefab.gameObject.transform.localScale;
					entity.transform.data.gameObject.Recycle();
				}
				else if (entity.hasTransform)
				{
					entity.transform.data.gameObject.Recycle();
				}
				if (entity.hasChilds)
				{
					List<Entity> data = entity.childs.data;
					for (int i = 0; i < data.Count; i++)
					{
						data[i].isDestroy = true;
					}
					entity.RemoveChilds();
				}
				pool.DestroyEntity(entity);
			}
		}
	}
}
