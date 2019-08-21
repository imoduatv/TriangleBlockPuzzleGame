using System.Collections.Generic;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class CatmullRomSplineSolver : AbstractSplineSolver
	{
		public CatmullRomSplineSolver(List<Vector3> nodes)
		{
			_nodes = nodes;
		}

		public override void closePath()
		{
			_nodes.RemoveAt(0);
			_nodes.RemoveAt(_nodes.Count - 1);
			if (_nodes[0] != _nodes[_nodes.Count - 1])
			{
				_nodes.Add(_nodes[0]);
			}
			float num = Vector3.Distance(_nodes[0], _nodes[1]);
			float num2 = Vector3.Distance(_nodes[0], _nodes[_nodes.Count - 2]);
			float d = num2 / Vector3.Distance(_nodes[1], _nodes[0]);
			Vector3 item = _nodes[0] + (_nodes[1] - _nodes[0]) * d;
			float d2 = num / Vector3.Distance(_nodes[_nodes.Count - 2], _nodes[0]);
			Vector3 item2 = _nodes[0] + (_nodes[_nodes.Count - 2] - _nodes[0]) * d2;
			_nodes.Insert(0, item2);
			_nodes.Add(item);
		}

		public override Vector3 getPoint(float t)
		{
			int num = _nodes.Count - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 a = _nodes[num2];
			Vector3 a2 = _nodes[num2 + 1];
			Vector3 vector = _nodes[num2 + 2];
			Vector3 b = _nodes[num2 + 3];
			return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
		}

		public override void drawGizmos()
		{
			if (_nodes.Count >= 2)
			{
				Color color = Gizmos.color;
				Gizmos.color = new Color(1f, 1f, 1f, 0.3f);
				Gizmos.DrawLine(_nodes[0], _nodes[1]);
				Gizmos.DrawLine(_nodes[_nodes.Count - 1], _nodes[_nodes.Count - 2]);
				Gizmos.color = color;
			}
		}
	}
}
