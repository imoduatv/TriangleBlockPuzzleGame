using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class TransformSpringTween : AbstractTweenable
	{
		private Transform _transform;

		private TransformTargetType _targetType;

		private Vector3 _targetValue;

		private Vector3 _velocity;

		public float dampingRatio = 0.23f;

		public float angularFrequency = 25f;

		public TransformTargetType targetType => _targetType;

		public TransformSpringTween(Transform transform, TransformTargetType targetType, Vector3 targetValue)
		{
			_transform = transform;
			_targetType = targetType;
			setTargetValue(targetValue);
		}

		public void setTargetValue(Vector3 targetValue)
		{
			_velocity = Vector3.zero;
			_targetValue = targetValue;
			if (!_isCurrentlyManagedByZestKit)
			{
				start();
			}
		}

		public void updateDampingRatioWithHalfLife(float lambda)
		{
			dampingRatio = (0f - lambda) / angularFrequency * Mathf.Log(0.5f);
		}

		public override bool tick()
		{
			if (!_isPaused)
			{
				setTweenedValue(Zest.fastSpring(getCurrentValueOfTweenedTargetType(), _targetValue, ref _velocity, dampingRatio, angularFrequency));
			}
			return false;
		}

		private void setTweenedValue(Vector3 value)
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

		private Vector3 getCurrentValueOfTweenedTargetType()
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
				return Vector3.zero;
			}
		}
	}
}
