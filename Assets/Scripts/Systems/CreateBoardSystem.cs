using Dta.TenTen;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class CreateBoardSystem : IReactiveSystem, IInitializeSystem, IReactiveExecuteSystem, ISystem
	{
		private float sin60;

		private float cos60;

		private float sqrt3on2;

		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Board, Matcher.BoardEntities, Matcher.PrefabChild, Matcher.Type).OnEntityAdded();

		public void Initialize()
		{
			sin60 = Mathf.Sin(1.04719758f);
			cos60 = Mathf.Cos(1.04719758f);
			sqrt3on2 = Mathf.Sqrt(3f) / 2f;
		}

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				if (entity.type.kind == BoardType.Normal)
				{
					CreateBoard(entity);
				}
				else if (entity.type.kind == BoardType.Hexagonal)
				{
					CreateBoardHex(entity);
				}
				else if (entity.type.kind == BoardType.Triangle)
				{
					CreateBoardTriangle(entity);
				}
			}
		}

		private void CreateBoard(Entity e)
		{
			Pool pool = Pools.pool;
			int rows = e.board.rows;
			int cols = e.board.cols;
			Vector2 a = default(Vector2);
			Vector2 vector = default(Vector2);
			a.x = e.board.width / (float)cols;
			a.y = e.board.height / (float)rows;
			Vector2 gridBoxSize = Singleton<GameManager>.Instance.gridBoxSize;
			gridBoxSize.x *= a.x;
			gridBoxSize.y *= a.y;
			Singleton<GameManager>.Instance.shapeGridSize = a * 0.5f;
			Singleton<GameManager>.Instance.shapeGridBigSize = Singleton<GameManager>.Instance.shapeGridSize * 2f;
			int sortingOrder = e.prefabChild.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					vector.x = e.board.x + (float)j * a.x + a.x / 2f;
					vector.y = e.board.y + (float)i * a.y + a.y / 2f;
					Entity entity = pool.CreateEntity();
					entity.AddPrefab(e.prefabChild.gameObject);
					entity.AddGrid(i, j);
					entity.AddParent(e.board.tranform);
					entity.AddPosition(vector.x, vector.y);
					entity.AddBox(gridBoxSize.x, gridBoxSize.y);
					entity.AddSprite(Singleton<AssetManagers>.Instance.normalGrid);
					entity.AddSortingLayer(sortingOrder);
					entity.AddColor(Singleton<AssetManagers>.Instance.tileEmpty);
					entity.isEmpty = true;
					e.boardEntities.board[i, j] = entity;
				}
			}
		}

		private void CreateBoardHex(Entity e)
		{
			Pool pool = Pools.pool;
			int rows = e.board.rows;
			int cols = e.board.cols;
			int num = rows / 2;
			int num2 = cols / 2;
			Vector3 position = e.board.tranform.position;
			Vector2 a = default(Vector2);
			Vector2 vector = default(Vector2);
			a.x = e.board.width / (float)cols;
			a.y = a.x / sqrt3on2;
			Vector2 gridHexBoxSize = Singleton<GameManager>.Instance.gridHexBoxSize;
			gridHexBoxSize.x *= a.x;
			gridHexBoxSize.y *= a.y;
			Singleton<GameManager>.Instance.shapeGridSize = a * 0.5f;
			Singleton<GameManager>.Instance.shapeGridBigSize = Singleton<GameManager>.Instance.shapeGridSize * 2f;
			int sortingOrder = e.prefabChild.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
			float num3 = (0f - a.x) * cos60;
			float num4 = a.y * sin60 * Singleton<GameManager>.Instance.hexFactorY;
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num2; j <= num2; j++)
				{
					if (i * j >= 0 || (Mathf.Abs(i) + Mathf.Abs(j) <= num && Mathf.Abs(i) + Mathf.Abs(j) <= num2))
					{
						vector.x = position.x + (float)j * a.x;
						vector.x += (float)i * num3;
						vector.y = position.y + (float)i * num4;
						Entity entity = pool.CreateEntity();
						entity.AddPrefab(e.prefabChild.gameObject);
						entity.AddGrid(i + num, j + num2);
						entity.AddParent(e.board.tranform);
						entity.AddPosition(vector.x, vector.y);
						entity.AddBox(gridHexBoxSize.x, gridHexBoxSize.y);
						entity.AddSprite(Singleton<AssetManagers>.Instance.hexGrid);
						entity.AddSortingLayer(sortingOrder);
						entity.AddColor(Singleton<AssetManagers>.Instance.tileEmpty);
						entity.isEmpty = true;
						e.boardEntities.board[i + num, j + num2] = entity;
					}
				}
			}
		}

		private void CreateBoardTriangle(Entity e)
		{
			Pool pool = Pools.pool;
			int rows = e.board.rows;
			int cols = e.board.cols;
			Vector3 position = e.board.tranform.position;
			Vector2 vector = default(Vector2);
			Vector2 vector2 = default(Vector2);
			int num = rows / 2;
			int num2 = cols / 2;
			vector.x = 2f * e.board.width / (float)cols;
			vector.y = vector.x * sin60;
			Vector2 vector3 = default(Vector2);
			vector3.x = vector.x * Singleton<GameManager>.Instance.gridTriangleBoxSize.x;
			vector3.y = vector.y * Singleton<GameManager>.Instance.gridTriangleBoxSize.y;
			Singleton<GameManager>.Instance.shapeGridSize = vector * 0.5f;
			Singleton<GameManager>.Instance.shapeGridBigSize = vector;
			int sortingOrder = e.prefabChild.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
			float num3 = vector.x * 0.5f;
			float y = vector.y;
			Vector2 vector4 = vector * 0.5f;
			int num4 = -num;
			int num5 = num - 1;
			int num6 = -num2;
			int num7 = num2 - 1;
			for (int i = num4; i <= num5; i++)
			{
				for (int j = num6; j <= num7; j++)
				{
					if ((i * j > 0 && ((i > 0 && 2 * i + j <= num7 - 1) || (i < 0 && 2 * Mathf.Abs(i) + Mathf.Abs(j) <= num2 + 1))) || (i * j <= 0 && i + j < num7))
					{
						vector2.x = position.x + (float)(i + j) * num3 + vector4.x;
						vector2.y = position.y + (float)i * y + vector4.y;
						Entity entity = pool.CreateEntity();
						entity.AddPrefab(e.prefabChild.gameObject);
						entity.AddGrid(i + num, j + num2);
						entity.grid.material = Singleton<GameManager>.instance.m_NormalMat;
						entity.AddParent(e.board.tranform);
						entity.AddPosition(vector2.x, vector2.y);
						entity.AddBox(vector3.x, vector3.y);
						entity.AddSprite(Singleton<AssetManagers>.Instance.triGrid);
						entity.AddSortingLayer(sortingOrder);
						entity.AddColor(Singleton<AssetManagers>.Instance.tileEmpty);
						entity.isEmpty = true;
						if (j % 2 != 0)
						{
							entity.AddScaleDirect(1f, -1f);
						}
						e.boardEntities.board[i + num, j + num2] = entity;
					}
				}
			}
			Singleton<GameManager>.instance.SyncTableToBoard(isNeedCheckClear: false);
		}
	}
}
