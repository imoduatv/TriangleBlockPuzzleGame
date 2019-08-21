using Components;
using Entitas;
using UnityEngine;

namespace Systems
{
	public class BoardInputSystem : IExecuteSystem, ISetPool, ISystem
	{
		private Group _group;

		private Vector2 inputPostion;

		public void SetPool(Pool pool)
		{
			_group = pool.GetGroup(Matcher.AllOf(Matcher.Board));
		}

		public void Execute()
		{
			Entity[] entities = _group.GetEntities();
			foreach (Entity entity in entities)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Board board = entity.board;
					Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
					if (board.x < vector.x && vector.x < board.x + board.width && board.y < vector.y && vector.y < board.y + board.height)
					{
						float num = board.width / (float)board.cols;
						float num2 = board.height / (float)board.rows;
						int num3 = (int)((vector.y - board.y) / num2);
						int num4 = (int)((vector.x - board.x) / num);
					}
				}
			}
		}
	}
}
