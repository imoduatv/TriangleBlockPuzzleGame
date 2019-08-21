using Dta.TenTen;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class CreateShapeSystem : IReactiveSystem, IInitializeSystem, IReactiveExecuteSystem, ISystem
	{
		private float sin60;

		private float cos60;

		private float sqrt3on2;

		private bool isLongScreen;

		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Transform, Matcher.Index, Matcher.Shape, Matcher.PrefabChild, Matcher.Type, Matcher.ColorID).OnEntityAdded();

		public static GameObject ParentObject
		{
			get;
			private set;
		}

		public void Initialize()
		{
			sin60 = Mathf.Sin(1.04719758f);
			cos60 = Mathf.Cos(1.04719758f);
			sqrt3on2 = Mathf.Sqrt(3f) / 2f;
			ParentObject = new GameObject("Shapes");
			ParentObject.transform.position = Vector3.zero;
			isLongScreen = Singleton<GameManager>.instance.IsLongScreen;
		}

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				entity.transform.data.SetParent(ParentObject.transform);
				if (entity.type.kind == BoardType.Normal)
				{
					CreateShape(entity);
				}
				else if (entity.type.kind == BoardType.Hexagonal)
				{
					CreateShapeHex(entity);
				}
				else if (entity.type.kind == BoardType.Triangle)
				{
					CreateShapeTriangle(entity);
				}
			}
			if (entities.Count > 0)
			{
				Singleton<GameManager>.Instance.OnNewShapeCreated();
			}
		}

		private void SetStartPosition(Entity e, BoxCollider2D box)
		{
			float screenWidth = Singleton<GameManager>.Instance.GetScreenWidth();
			Vector2 vector = Singleton<GameManager>.Instance.shapeCenter.position;
			int num = 6;
			int num2 = 2 * e.index.data + 1;
			float num3 = screenWidth / (float)num;
			if (Singleton<GameManager>.Instance.IsRunTutorial())
			{
				num2 = 3;
			}
			Vector2 a = e.transform.data.position;
			float num4 = (float)num2 * num3 + (vector.x - screenWidth / 2f);
			Vector2 offset = box.offset;
			a.x = num4 - offset.x / 2f - 0.25f;
			float y = vector.y;
			Vector2 size = box.size;
			a.y = y - size.y / 2f;
			Vector2 vector2 = new Vector2(a.x, a.y);
			vector2.x += screenWidth * (3f / (float)num) + 3f;
			if (!e.shape.data.IsNewIndex)
			{
				e.transform.data.position = vector2;
			}
			else
			{
				vector2 = e.transform.data.position;
			}
			Drag component = e.transform.data.gameObject.GetComponent<Drag>();
			component.detector.gameObject.transform.localPosition = box.offset;
			component.entity = e;
			component.StopAllCoroutines();
			component.SetNewGoingIn();
			Vector3 vector3 = Vector3.zero;
			if (isLongScreen)
			{
				vector3 += new Vector3(0f, 0.5f);
			}
			if (e.shape.data.IsBackUpShape)
			{
				component.StartCoroutine(component.IEWaitGoingIn(e.shape.data.BackUpShapePos + vector3, a + (Vector2)vector3, 0.15f));
			}
			else
			{
				component.StartCoroutine(component.IEWaitGoingIn(vector2 + (Vector2)vector3, a + (Vector2)vector3, 0.15f + (float)(num2 - 1) * 0.05f));
			}
			e.ReplaceBoxCollider2D(box);
		}

		private void CreateShape(Entity e)
		{
			Pool pool = Pools.pool;
			Transform data = e.transform.data;
			Shape data2 = e.shape.data;
			int length = data2.map.GetLength(0);
			int length2 = data2.map.GetLength(1);
			Vector2 vector = default(Vector2);
			Vector2 vector2 = default(Vector2);
			vector.x = Singleton<GameManager>.Instance.shapeGridSize.x;
			vector.y = Singleton<GameManager>.Instance.shapeGridSize.y;
			float num = vector.x * (float)length2;
			float num2 = vector.y * (float)length;
			BoxCollider2D component = e.transform.data.gameObject.GetComponent<BoxCollider2D>();
			component.offset = new Vector2(num / 2f, num2 / 2f);
			component.size = new Vector2(num, num2);
			Vector2 gridBoxSize = Singleton<GameManager>.Instance.gridBoxSize;
			gridBoxSize.x *= vector.x;
			gridBoxSize.y *= vector.y;
			if (e.hasChilds)
			{
				List<Entity> data3 = e.childs.data;
				for (int i = 0; i < data3.Count; i++)
				{
					data3[i].isDestroy = true;
				}
				e.RemoveChilds();
			}
			List<Entity> list = new List<Entity>();
			Color color = Singleton<AssetManagers>.Instance.GetColor(e.colorID.data);
			GridCell firstGrid = data2.firstGrid;
			for (int j = 0; j < length; j++)
			{
				for (int k = 0; k < length2; k++)
				{
					if (data2.map[j, k] == 1)
					{
						int num3 = j - firstGrid.x;
						int num4 = k - firstGrid.y;
						vector2.x = (float)num4 * vector.x + vector.x / 2f;
						vector2.y = (float)num3 * vector.y + vector.y / 2f;
						Entity entity = pool.CreateEntity();
						entity.AddPrefab(e.prefabChild.gameObject);
						entity.AddGrid(j, k);
						entity.AddParent(data);
						entity.AddLocalPosition(vector2.x, vector2.y);
						entity.AddBox(gridBoxSize.x, gridBoxSize.y);
						entity.AddSprite(Singleton<AssetManagers>.Instance.normalGrid);
						entity.AddColor(color);
						entity.AddColorID(e.colorID.data);
						if (e.shape.data.firstGrid.x == j && e.shape.data.firstGrid.y == k)
						{
							e.ReplaceFirstGrid(entity);
						}
						list.Add(entity);
					}
				}
			}
			e.AddChilds(list);
			SetStartPosition(e, component);
		}

		private void CreateShapeHex(Entity e)
		{
			Pool pool = Pools.pool;
			Transform data = e.transform.data;
			Shape data2 = e.shape.data;
			int length = data2.map.GetLength(0);
			int length2 = data2.map.GetLength(1);
			Vector2 vector = default(Vector2);
			Vector2 vector2 = default(Vector2);
			vector.x = Singleton<GameManager>.Instance.shapeGridSize.x;
			vector.y = Singleton<GameManager>.Instance.shapeGridSize.y;
			float num = vector.x / 2f;
			float num2 = vector.y / 2f;
			Vector2 gridHexBoxSize = Singleton<GameManager>.Instance.gridHexBoxSize;
			gridHexBoxSize.x *= vector.x;
			gridHexBoxSize.y *= vector.y;
			if (e.hasChilds)
			{
				List<Entity> data3 = e.childs.data;
				for (int i = 0; i < data3.Count; i++)
				{
					data3[i].isDestroy = true;
				}
				e.RemoveChilds();
			}
			List<Entity> list = new List<Entity>();
			float num3 = (0f - vector.x) * cos60;
			float num4 = vector.y * sin60 * Singleton<GameManager>.Instance.hexFactorY;
			bool flag = false;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = num;
			Color color = Singleton<AssetManagers>.Instance.GetColor(e.colorID.data);
			GridCell firstGrid = data2.firstGrid;
			for (int j = 0; j < length; j++)
			{
				for (int k = 0; k < length2; k++)
				{
					if (data2.map[j, k] != 1)
					{
						continue;
					}
					int num10 = j - firstGrid.x;
					int num11 = k - firstGrid.y;
					vector2.x = (float)num11 * vector.x + num;
					vector2.x += (float)num10 * num3;
					vector2.y = (float)num10 * num4 + num2;
					if (!flag)
					{
						flag = true;
						num5 = vector2.x;
						num6 = vector2.x;
						num7 = vector2.y;
						num8 = vector2.y;
					}
					else
					{
						if (vector2.x < num5)
						{
							num5 = vector2.x;
						}
						if (vector2.x > num6)
						{
							num6 = vector2.x;
						}
						if (vector2.y < num7)
						{
							num7 = vector2.y;
						}
						if (vector2.y > num8)
						{
							num8 = vector2.y;
						}
					}
					Entity entity = pool.CreateEntity();
					entity.AddPrefab(e.prefabChild.gameObject);
					entity.AddGrid(j, k);
					entity.AddParent(data);
					entity.AddLocalPosition(vector2.x, vector2.y);
					entity.AddBox(gridHexBoxSize.x, gridHexBoxSize.y);
					entity.AddSprite(Singleton<AssetManagers>.Instance.hexGrid);
					entity.AddColor(color);
					entity.AddColorID(e.colorID.data);
					if (e.shape.data.firstGrid.x == j && e.shape.data.firstGrid.y == k)
					{
						e.ReplaceFirstGrid(entity);
					}
					list.Add(entity);
				}
			}
			e.AddChilds(list);
			BoxCollider2D component = e.transform.data.gameObject.GetComponent<BoxCollider2D>();
			float num12 = num6 - num5 + vector.x;
			float num13 = num8 - num7 + vector.y;
			float num14 = 0f;
			if (num5 < num9)
			{
				num14 = num9 - num5;
			}
			component.offset = new Vector2(num12 / 2f - num14, num13 / 2f);
			component.size = new Vector2(num12, num13);
			SetStartPosition(e, component);
		}

		private void CreateShapeTriangle(Entity e)
		{
			Pool pool = Pools.pool;
			Transform data = e.transform.data;
			Shape data2 = e.shape.data;
			int length = data2.map.GetLength(0);
			int length2 = data2.map.GetLength(1);
			Vector2 a = default(Vector2);
			Vector2 vector = default(Vector2);
			a.x = Singleton<GameManager>.Instance.shapeGridSize.x * 1.6f;
			a.y = Singleton<GameManager>.Instance.shapeGridSize.y * 1.6f;
			Vector2 vector2 = a * 0.5f;
			Vector2 gridTriangleBoxSize = Singleton<GameManager>.Instance.gridTriangleBoxSize;
			gridTriangleBoxSize.x *= a.x;
			gridTriangleBoxSize.y *= a.y;
			BoxCollider2D data3;
			if (data2.IsNewIndex)
			{
				data3 = e.boxCollider2D.data;
				SetStartPosition(e, data3);
				data2.IsNewIndex = false;
				return;
			}
			data2.IsNewIndex = false;
			if (e.hasChilds)
			{
				List<Entity> data4 = e.childs.data;
				for (int i = 0; i < data4.Count; i++)
				{
					data4[i].isDestroy = true;
				}
				e.RemoveChilds();
			}
			List<Entity> list = new List<Entity>();
			float num = a.x * 0.5f;
			float y = a.y;
			bool flag = false;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float x = vector2.x;
			Color color = Singleton<AssetManagers>.Instance.GetColor(e.colorID.data);
			GridCell firstGrid = data2.firstGrid;
			for (int j = 0; j < length; j++)
			{
				for (int k = 0; k < length2; k++)
				{
					if (data2.map[j, k] != 1)
					{
						continue;
					}
					int num6 = j - firstGrid.x;
					int num7 = k - firstGrid.y;
					vector.x = (float)(num6 + num7) * num + vector2.x;
					vector.y = (float)num6 * y + vector2.y;
					if (!flag)
					{
						flag = true;
						num2 = vector.x;
						num3 = vector.x;
						num4 = vector.y;
						num5 = vector.y;
					}
					else
					{
						if (vector.x < num2)
						{
							num2 = vector.x;
						}
						if (vector.x > num3)
						{
							num3 = vector.x;
						}
						if (vector.y < num4)
						{
							num4 = vector.y;
						}
						if (vector.y > num5)
						{
							num5 = vector.y;
						}
					}
					Entity entity = pool.CreateEntity();
					entity.AddPrefab(e.prefabChild.gameObject);
					entity.AddGrid(j, k);
					entity.AddParent(data);
					entity.AddLocalPosition(vector.x, vector.y);
					entity.AddBox(gridTriangleBoxSize.x, gridTriangleBoxSize.y);
					entity.AddColor(color);
					entity.AddColorID(e.colorID.data);
					entity.AddSprite(Singleton<AssetManagers>.instance.GetSprite(e.colorID.data));
					if (k % 2 != 0)
					{
						entity.AddScaleDirect(1f, -1f);
					}
					if (e.shape.data.firstGrid.x == j && e.shape.data.firstGrid.y == k)
					{
						e.ReplaceFirstGrid(entity);
					}
					list.Add(entity);
				}
			}
			e.AddChilds(list);
			data3 = e.transform.data.gameObject.GetComponent<BoxCollider2D>();
			float num8 = num3 - num2 + a.x;
			float num9 = num5 - num4 + a.y;
			float num10 = 0f;
			if (num2 < x)
			{
				num10 = x - num2;
			}
			data3.offset = new Vector2(num8 / 2f + num2 - vector2.x, num9 / 2f);
			data3.size = new Vector2(num8, num9);
			SetStartPosition(e, data3);
		}
	}
}
