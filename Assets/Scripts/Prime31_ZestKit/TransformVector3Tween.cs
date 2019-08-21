using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class TransformVector3Tween : Vector3Tween, ITweenTarget<Vector3>
	{
		private Transform _transform;

		private TransformTargetType _targetType;

		public void setTweenedValue(Vector3 value)
		{
			if (!ZestKit.enableBabysitter || (bool)_transform)
			{
				switch (_targetType)
				{
				case TransformTargetType.Position:
					_transform.position = value;
					break;
				case TransformTargetType.LocalPosition:
					_transform.localPosition = value;
					break;
				case TransformTargetType.LocalScale:
					_transform.localScale = value;
					break;
				case TransformTargetType.EulerAngles:
					_transform.eulerAngles = value;
					break;
				case TransformTargetType.LocalEulerAngles:
					_transform.localEulerAngles = value;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public Vector3 getTweenedValue()
		{
			switch (_targetType)
			{
			case TransformTargetType.Position:
				return _transform.position;
			case TransformTargetType.LocalPosition:
				return _transform.localPosition;
			case TransformTargetType.LocalScale:
				return _transform.localScale;
			case TransformTargetType.EulerAngles:
				return _transform.eulerAngles;
			case TransformTargetType.LocalEulerAngles:
				return _transform.localEulerAngles;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public new object getTargetObject()
		{
			return _transform;
		}

		public void setTargetAndType(Transform transform, TransformTargetType targetType)
		{
			_transform = transform;
			_targetType = targetType;
		}

		protected override void updateValue()
		{
			if ((_targetType == TransformTargetType.EulerAngles || _targetType == TransformTargetType.LocalEulerAngles) && !_isRelative)
			{
				if (_animationCurve != null)
				{
					setTweenedValue(Zest.easeAngle(_animationCurve, _fromValue, _toValue, _elapsedTime, _duration));
				}
				else
				{
					setTweenedValue(Zest.easeAngle(_easeType, _fromValue, _toValue, _elapsedTime, _duration));
				}
			}
			else if (_animationCurve != null)
			{
				setTweenedValue(Zest.ease(_animationCurve, _fromValue, _toValue, _elapsedTime, _duration));
			}
			else
			{
				setTweenedValue(Zest.ease(_easeType, _fromValue, _toValue, _elapsedTime, _duration));
			}
		}

		public override void recycleSelf()
		{
			if (_shouldRecycleTween)
			{
				_target = null;
				_nextTween = null;
				_transform = null;
				QuickCache<TransformVector3Tween>.push(this);
			}
		}
	}
}
