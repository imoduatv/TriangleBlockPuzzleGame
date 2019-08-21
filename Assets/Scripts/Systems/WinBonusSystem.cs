using Entitas;
using Prime31.ZestKit;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class WinBonusSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		private Vector3 desScale1 = new Vector3(0.1f, 0.1f, 0.1f);

		private Vector3 desScale2 = new Vector3(0.1f, -0.1f, 0.1f);

		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Bonus, Matcher.Transform).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				ShowBonusAnimation(entity);
			}
		}

		private void ShowBonusAnimation(Entity e)
		{
			Transform data = e.transform.data;
			Vector3 one = Vector3.one;
			Vector3 localScale = data.localScale;
			one = ((!(localScale.y < 0f)) ? desScale1 : desScale2);
			float timeDelay = Singleton<GameManager>.instance.GetTimeDelay(e.grid.row, e.grid.col);
			data.ZKlocalScaleTo(one).setContext(e).setDelay(timeDelay * 0.03f)
				.setEaseType(EaseType.BackIn)
				.setCompletionHandler(delegate(ITween<Vector3> tween)
				{
					Entity entity = (Entity)tween.context;
					entity.isDestroy = true;
				})
				.start();
		}
	}
}
