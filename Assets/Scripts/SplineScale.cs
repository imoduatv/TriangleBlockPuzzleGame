using Prime31.ZestKit;
using UnityEngine;

public class SplineScale : Tween<Vector3>, ITweenTarget<Vector3>
{
	private Transform _transform;

	private Spline _spline;

	private bool _isRelativeTween;

	public SplineScale(Transform trans, Spline spline, float duration)
	{
		_transform = trans;
		_spline = spline;
		_spline.buildPath();
		initialize(this, Vector3.zero, duration);
	}

	public Vector3 getTweenedValue()
	{
		return _transform.localScale;
	}

	public override ITween<Vector3> setIsRelative()
	{
		_isRelativeTween = true;
		return this;
	}

	public void setTweenedValue(Vector3 value)
	{
		_transform.localScale = value;
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

	object ITweenTarget<Vector3>.getTargetObject()
	{
		return getTargetObject();
	}
}
