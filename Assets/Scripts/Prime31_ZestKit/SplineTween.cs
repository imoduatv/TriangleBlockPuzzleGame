using UnityEngine;

namespace Prime31.ZestKit
{
	public class SplineTween : Tween<Vector3>, ITweenTarget<Vector3>
	{
		private Transform _transform;

		private Spline _spline;

		private bool _isRelativeTween;

		public SplineTween(Transform transform, Spline spline, float duration)
		{
			_transform = transform;
			_spline = spline;
			_spline.buildPath();
			initialize(this, Vector3.zero, duration);
		}

		public void setTweenedValue(Vector3 value)
		{
			_transform.position = value;
		}

		public Vector3 getTweenedValue()
		{
			return _transform.position;
		}

		public override ITween<Vector3> setIsRelative()
		{
			_isRelativeTween = true;
			return this;
		}

		protected override void updateValue()
		{
			float t = EaseHelper.ease(_easeType, _elapsedTime, _duration);
			Vector3 vector = _spline.getPointOnPath(t);
			if (_isRelativeTween)
			{
				vector += _fromValue;
			}
			setTweenedValue(vector);
		}

		public new object getTargetObject()
		{
			return _transform;
		}
	}
}
