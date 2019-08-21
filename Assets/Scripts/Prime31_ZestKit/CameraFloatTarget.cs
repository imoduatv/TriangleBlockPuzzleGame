using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class CameraFloatTarget : AbstractTweenTarget<Camera, float>
	{
		public enum CameraTargetType
		{
			OrthographicSize,
			FieldOfView
		}

		private CameraTargetType _targetType;

		public CameraFloatTarget(Camera camera, CameraTargetType targetType = CameraTargetType.OrthographicSize)
		{
			_target = camera;
			_targetType = targetType;
		}

		public override void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				switch (_targetType)
				{
				case CameraTargetType.OrthographicSize:
					_target.orthographicSize = value;
					break;
				case CameraTargetType.FieldOfView:
					_target.fieldOfView = value;
					break;
				}
			}
		}

		public override float getTweenedValue()
		{
			switch (_targetType)
			{
			case CameraTargetType.OrthographicSize:
				return _target.orthographicSize;
			case CameraTargetType.FieldOfView:
				return _target.fieldOfView;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
