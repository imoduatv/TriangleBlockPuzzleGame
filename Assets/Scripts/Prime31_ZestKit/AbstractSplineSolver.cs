using System.Collections.Generic;
using UnityEngine;

namespace Prime31.ZestKit
{
	public abstract class AbstractSplineSolver
	{
		protected List<Vector3> _nodes;

		protected float _pathLength;

		protected int totalSubdivisionsPerNodeForLookupTable = 5;

		protected Dictionary<float, float> _segmentTimeForDistance;

		public List<Vector3> nodes => _nodes;

		public float pathLength => _pathLength;

		public virtual void buildPath()
		{
			int num = _nodes.Count * totalSubdivisionsPerNodeForLookupTable;
			_pathLength = 0f;
			float num2 = 1f / (float)num;
			_segmentTimeForDistance = new Dictionary<float, float>(num);
			Vector3 b = getPoint(0f);
			for (int i = 1; i < num + 1; i++)
			{
				float num3 = num2 * (float)i;
				Vector3 point = getPoint(num3);
				_pathLength += Vector3.Distance(point, b);
				b = point;
				_segmentTimeForDistance.Add(num3, _pathLength);
			}
		}

		public abstract void closePath();

		public abstract Vector3 getPoint(float t);

		public virtual Vector3 getPointOnPath(float t)
		{
			float num = _pathLength * t;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float[] array = new float[_segmentTimeForDistance.Keys.Count];
			_segmentTimeForDistance.Keys.CopyTo(array, 0);
			foreach (float num6 in array)
			{
				float num7 = _segmentTimeForDistance[num6];
				if (num7 >= num)
				{
					num4 = num6;
					num5 = num7;
					if (num2 > 0f)
					{
						num3 = _segmentTimeForDistance[num2];
					}
					break;
				}
				num2 = num6;
			}
			float num8 = num4 - num2;
			float num9 = num5 - num3;
			float num10 = num - num3;
			t = num2 + num10 / num9 * num8;
			return getPoint(t);
		}

		public void reverseNodes()
		{
			_nodes.Reverse();
		}

		public virtual void drawGizmos()
		{
		}

		public virtual int getTotalPointsBetweenPoints(float t, float t2)
		{
			int num = 0;
			float num2 = _pathLength * t;
			float num3 = _pathLength * t2;
			float num4 = 0f;
			float[] array = new float[_segmentTimeForDistance.Keys.Count];
			_segmentTimeForDistance.Keys.CopyTo(array, 0);
			foreach (float key in array)
			{
				float num5 = _segmentTimeForDistance[key];
				if (num5 >= num2)
				{
					num4 = num5;
					break;
				}
			}
			float num6 = 0f;
			float num7 = 0f;
			foreach (float num8 in array)
			{
				float num9 = _segmentTimeForDistance[num8];
				if (num9 >= num3)
				{
					if (num6 > 0f)
					{
						num7 = _segmentTimeForDistance[num6];
					}
					break;
				}
				num6 = num8;
			}
			return (int)(num4 + 0.5f) + (int)(num7 + 0.5f);
		}
	}
}
