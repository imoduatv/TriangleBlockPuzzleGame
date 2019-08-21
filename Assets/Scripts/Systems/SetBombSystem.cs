using Dta.TenTen;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
	public class SetBombSystem : IReactiveSystem, IReactiveExecuteSystem, ISystem
	{
		public TriggerOnEvent trigger => Matcher.AllOf(Matcher.Bomb, Matcher.Text, Matcher.Transform).OnEntityAdded();

		public void Execute(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				Vector3 position = entity.transform.data.position;
				if (Singleton<GameManager>.Instance.gameType == BoardType.Triangle)
				{
					float num = entity.box.height * 0.2f;
					if (entity.grid.col % 2 == 0)
					{
						position.y -= num;
					}
					else
					{
						position.y += num;
					}
				}
				Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);
				RectTransform component = entity.text.data.gameObject.GetComponent<RectTransform>();
				RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)component.parent, screenPoint, Camera.main, out Vector2 localPoint);
				component.anchoredPosition = localPoint;
				component.localScale = new Vector3(1f, 1f, 1f);
				entity.text.data.gameObject.name = "bomb " + entity.bomb.time;
				entity.text.data.text = string.Empty + entity.bomb.time;
			}
		}
	}
}
