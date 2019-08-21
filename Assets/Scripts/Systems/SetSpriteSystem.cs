using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class SetSpriteSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.Sprite, Matcher.Color).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				GameObject gameObject = entity.transform.data.gameObject;
				SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
				if (spriteRenderer == null)
				{
					spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				}
				if (!entity.hasSpriteRenderer)
				{
					entity.AddSpriteRenderer(spriteRenderer);
				}
				spriteRenderer.sprite = entity.sprite.data;
				spriteRenderer.color = entity.color.data;
				if (entity.hasSortingLayer)
				{
					spriteRenderer.sortingOrder = entity.sortingLayer.data;
				}
				if (entity.hasGrid && entity.grid.material != null)
				{
					spriteRenderer.sharedMaterial = entity.grid.material;
				}
				else
				{
					spriteRenderer.sharedMaterial = Singleton<GameManager>.instance.m_NormalMat;
				}
			}
		}
	}
}
