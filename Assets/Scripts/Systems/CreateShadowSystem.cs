using Dta.TenTen;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class CreateShadowSystem : IReactiveSystem, IInitializeSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.ShapeShadow, Matcher.Transform, Matcher.PrefabChild).OnEntityAdded();

		public static GameObject ParentObject
		{
			get;
			private set;
		}

		public void Initialize()
		{
			ParentObject = new GameObject("Shadows");
			ParentObject.transform.position = Vector3.zero;
		}

		public void Execute(List<Entity> entities)
		{
			Pool pool = Pools.pool;
			foreach (Entity entity3 in entities)
			{
				Entity shape = entity3.shapeShadow.shape;
				entity3.transform.data.SetParent(ParentObject.transform);
				if (shape.hasChilds)
				{
					List<Entity> data = shape.childs.data;
					List<Entity> list = new List<Entity>();
					Vector2 vector = default(Vector2);
					vector.x = Singleton<GameManager>.Instance.shapeGridSize.x;
					vector.y = Singleton<GameManager>.Instance.shapeGridSize.y;
					GameObject gameObject = entity3.prefabChild.gameObject;
					Vector2 shadowNormalDelta = Singleton<GameManager>.Instance.shadowNormalDelta;
					shadowNormalDelta = ((shape.type.kind != BoardType.Hexagonal) ? shadowNormalDelta : Singleton<GameManager>.Instance.shadowHexDelta);
					shadowNormalDelta = ((shape.type.kind != BoardType.Triangle) ? shadowNormalDelta : Singleton<GameManager>.Instance.shadowTriDelta);
					for (int i = 0; i < data.Count; i++)
					{
						Entity entity = data[i];
						if (entity.hasColor)
						{
							Entity entity2 = pool.CreateEntity();
							Color data2 = entity.color.data;
							data2.a = 0.6f;
							entity2.AddPrefab(gameObject);
							entity2.AddGrid(entity.grid.row, entity.grid.col);
							entity2.AddParent(entity3.transform.data);
							entity2.AddLocalPosition(entity.localPosition.x + shadowNormalDelta.x, entity.localPosition.y + shadowNormalDelta.y);
							entity2.AddBox(entity.box.width * 1.25f, entity.box.height * 1.25f);
							entity2.AddSprite(entity.sprite.data);
							entity2.AddColor(data2);
							entity2.AddColorID(entity.colorID.data);
							entity2.grid.material = Singleton<GameManager>.Instance.m_NormalMat;
							if (entity.hasScaleDirect)
							{
								entity2.AddScaleDirect(entity.scaleDirect.x, entity.scaleDirect.y);
							}
							list.Add(entity2);
						}
					}
					entity3.AddChilds(list);
					entity3.transform.data.gameObject.SetActive(value: false);
				}
			}
		}
	}
}
