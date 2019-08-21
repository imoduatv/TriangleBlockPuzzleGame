using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class TransformRotationTarget : AbstractTweenTarget<Transform, Quaternion>
	{
		public enum TransformRotationType
		{
			Rotation,
			LocalRotation
		}

		private TransformRotationType _targetType;

		public TransformRotationTarget(Transform transform, TransformRotationType targetType)
		{
			_target = transform;
			_targetType = targetType;
		}

		public override void setTweenedValue(Quaternion value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				switch (_targetType)
				{
				case TransformRotationType.Rotation:
					_target.rotation = value;
					break;
				case TransformRotationType.LocalRotation:
					_target.localRotation = value;
					break;
				}
			}
		}

		public override Quaternion getTweenedValue()
		{
			switch (_targetType)
			{
			case TransformRotationType.Rotation:
				return _target.rotation;
			case TransformRotationType.LocalRotation:
				return _target.localRotation;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
