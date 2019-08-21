using Components;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class SpriteFitBoxSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.SpriteRenderer, Matcher.Box).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				UnityEngine.SpriteRenderer data = entity.spriteRenderer.data;
				UnityEngine.Transform data2 = entity.transform.data;
				Box box = entity.box;
				float width = box.width;
				Vector3 size = data.sprite.bounds.size;
				float x = size.x;
				Vector3 lossyScale = data2.lossyScale;
				float x2 = width / (x * lossyScale.x);
				float height = box.height;
				Vector3 size2 = data.sprite.bounds.size;
				float y = size2.y;
				Vector3 lossyScale2 = data2.lossyScale;
				float y2 = height / (y * lossyScale2.y);
				data2.localScale = new Vector3(x2, y2, 1f);
			}
		}
	}
}
