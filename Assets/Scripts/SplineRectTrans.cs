using Prime31.ZestKit;
using UnityEngine;

public class SplineRectTrans : Tween<Vector3>, ITweenTarget<Vector3>
{
	private RectTransform _rectTransform;

	private Spline _spline;

	private bool _isRelativeTween;

	public SplineRectTrans(RectTransform rectTrans, Spline spline, float duration)
	{
		_rectTransform = rectTrans;
		_spline = spline;
		_spline.buildPath();
		initialize(this, Vector3.zero, duration);
	}

	public Vector3 getTweenedValue()
	{
		return _rectTransform.anchoredPosition;
	}

	public void setTweenedValue(Vector3 value)
	{
		_rectTransform.anchoredPosition = value;
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

	object ITweenTarget<Vector3>.getTargetObject()
	{
		return getTargetObject();
	}
}
