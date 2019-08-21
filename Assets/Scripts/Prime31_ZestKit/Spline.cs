using System.Collections.Generic;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class Spline
	{
		private bool _isReversed;

		private AbstractSplineSolver _solver;

		public int currentSegment
		{
			get;
			private set;
		}

		public bool isClosed
		{
			get;
			private set;
		}

		public SplineType splineType
		{
			get;
			private set;
		}

		public List<Vector3> nodes => _solver.nodes;

		public float pathLength => _solver.pathLength;

		public Spline(List<Vector3> nodes, bool useBezierIfPossible = false, bool useStraightLines = false)
		{
			if (useStraightLines || nodes.Count == 2)
			{
				splineType = SplineType.StraightLine;
				_solver = new StraightLineSplineSolver(nodes);
			}
			else if (nodes.Count == 3)
			{
				splineType = SplineType.QuadraticBezier;
				_solver = new QuadraticBezierSplineSolver(nodes);
			}
			else if (nodes.Count == 4)
			{
				splineType = SplineType.CubicBezier;
				_solver = new CubicBezierSplineSolver(nodes);
			}
			else if (useBezierIfPossible)
			{
				splineType = SplineType.Bezier;
				_solver = new BezierSplineSolver(nodes);
			}
			else
			{
				splineType = SplineType.CatmullRom;
				_solver = new CatmullRomSplineSolver(nodes);
			}
		}

		public Spline(string pathAssetName, bool useBezierIfPossible = false, bool useStraightLines = false)
			: this(SplineAssetUtils.nodeListFromAsset(pathAssetName), useBezierIfPossible, useStraightLines)
		{
		}

		public Spline(Vector3[] nodes, bool useBezierIfPossible = false, bool useStraightLines = false)
			: this(new List<Vector3>(nodes), useBezierIfPossible, useStraightLines)
		{
		}

		public static Spline generateArc(Vector3 start, Vector3 end, float curvature)
		{
			return generateArc(start, end, curvature, Vector3.Cross(start, end));
		}

		public static Spline generateArc(Vector3 start, Vector3 end, float curvature, Vector3 curvatureAxis)
		{
			curvatureAxis.Normalize();
			return generateArc(start, end, curvature, curvatureAxis, curvatureAxis);
		}

		public static Spline generateArc(Vector3 start, Vector3 end, float curvature, Vector3 startCurvatureAxis, Vector3 endCurvatureAxis)
		{
			startCurvatureAxis.Normalize();
			endCurvatureAxis.Normalize();
			List<Vector3> list = new List<Vector3>();
			list.Add(start);
			list.Add(start + startCurvatureAxis * curvature);
			list.Add(end + endCurvatureAxis * curvature);
			list.Add(end);
			List<Vector3> nodes = list;
			return new Spline(nodes);
		}

		public Vector3 getLastNode()
		{
			return _solver.nodes[_solver.nodes.Count];
		}

		public void buildPath()
		{
			_solver.buildPath();
		}

		private Vector3 getPoint(float t)
		{
			return _solver.getPoint(t);
		}

		public Vector3 getPointOnPath(float t)
		{
			if (t < 0f || t > 1f)
			{
				t = ((!isClosed) ? Mathf.Clamp01(t) : ((!(t < 0f)) ? (t - 1f) : (t + 1f)));
			}
			return _solver.getPointOnPath(t);
		}

		public void closePath()
		{
			if (!isClosed)
			{
				isClosed = true;
				_solver.closePath();
			}
		}

		public void reverseNodes()
		{
			if (!_isReversed)
			{
				_solver.reverseNodes();
				_isReversed = true;
			}
		}

		public void unreverseNodes()
		{
			if (_isReversed)
			{
				_solver.reverseNodes();
				_isReversed = false;
			}
		}

		public void drawGizmos(float resolution, bool isInEditMode)
		{
			if (_solver.nodes.Count != 0)
			{
				if (isInEditMode)
				{
					_solver.drawGizmos();
				}
				Vector3 to = _solver.getPoint(0f);
				resolution *= (float)_solver.nodes.Count;
				for (int i = 1; (float)i <= resolution; i++)
				{
					float t = (float)i / resolution;
					Vector3 point = _solver.getPoint(t);
					Gizmos.DrawLine(point, to);
					to = point;
				}
			}
		}

		public static void drawGizmos(Vector3[] nodes, float resolution = 50f, bool isInEditMode = false)
		{
			Spline spline = new Spline(new List<Vector3>(nodes));
			spline.drawGizmos(resolution, isInEditMode);
		}

		public int getTotalPointsBetweenPoints(float t, float t2)
		{
			return _solver.getTotalPointsBetweenPoints(t, t2);
		}
	}
}
