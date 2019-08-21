using System.Collections.Generic;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class BezierSplineSolver : AbstractSplineSolver
	{
		private int _curveCount;

		public BezierSplineSolver(List<Vector3> nodes)
		{
			_nodes = nodes;
			_curveCount = (_nodes.Count - 1) / 3;
		}

		protected float quadBezierLength(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint)
		{
			Vector3[] array = new Vector3[2]
			{
				controlPoint - startPoint,
				startPoint - 2f * controlPoint + endPoint
			};
			if (array[1] != Vector3.zero)
			{
				float num = 4f * Vector3.Dot(array[1], array[1]);
				float num2 = 8f * Vector3.Dot(array[0], array[1]);
				float num3 = 4f * Vector3.Dot(array[0], array[0]);
				float num4 = 4f * num3 * num - num2 * num2;
				float num5 = 2f * num + num2;
				float num6 = num + num2 + num3;
				float num7 = 0.25f / num;
				float num8 = num4 / (8f * Mathf.Pow(num, 1.5f));
				return num7 * (num5 * Mathf.Sqrt(num6) - num2 * Mathf.Sqrt(num3)) + num8 * (Mathf.Log(2f * Mathf.Sqrt(num * num6) + num5) - Mathf.Log(2f * Mathf.Sqrt(num * num3) + num2));
			}
			return 2f * array[0].magnitude;
		}

		private Vector3 getPoint(int curveIndex, float t)
		{
			int num = curveIndex * 3;
			Vector3 p = _nodes[num];
			Vector3 p2 = _nodes[num + 1];
			Vector3 p3 = _nodes[num + 2];
			Vector3 p4 = _nodes[num + 3];
			return getPoint(t, p, p2, p3, p4);
		}

		private Vector3 getPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float num = 1f - t;
			float num2 = t * t;
			float num3 = num * num;
			float d = num3 * num;
			float d2 = num2 * t;
			Vector3 a = d * p0;
			a += 3f * num3 * t * p1;
			a += 3f * num * num2 * p2;
			return a + d2 * p3;
		}

		public override void closePath()
		{
			if (_nodes[0] != _nodes[_nodes.Count - 1])
			{
				Vector3 vector = Vector3.Lerp(_nodes[0], _nodes[_nodes.Count - 1], 0.5f);
				Vector3 b = vector - _nodes[0];
				List<Vector3> nodes = _nodes;
				Vector3 value = vector;
				_nodes[_nodes.Count - 1] = value;
				nodes[0] = value;
				List<Vector3> nodes2;
				(nodes2 = _nodes)[1] = nodes2[1] + b;
				int index;
				(nodes2 = _nodes)[index = _nodes.Count - 2] = nodes2[index] - b;
			}
			Vector3 b2 = _nodes[1];
			Vector3 a = _nodes[0];
			Vector3 b3 = a - b2;
			_nodes[_nodes.Count - 2] = a + b3;
		}

		public override Vector3 getPoint(float t)
		{
			if (t > 1f)
			{
				t = 1f - t;
			}
			else if (t < 0f)
			{
				t = 1f + t;
			}
			int num;
			if (t == 1f)
			{
				t = 1f;
				num = _curveCount - 1;
			}
			else
			{
				t *= (float)_curveCount;
				num = (int)t;
				t -= (float)num;
			}
			return getPoint(num, t);
		}

		public override void drawGizmos()
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.red;
			for (int i = 0; i < _nodes.Count; i += 3)
			{
				if (i > 0)
				{
					Gizmos.DrawLine(_nodes[i], _nodes[i - 1]);
				}
				if (i < _nodes.Count - 2)
				{
					Gizmos.DrawLine(_nodes[i], _nodes[i + 1]);
				}
			}
			Gizmos.color = color;
		}
	}
}
