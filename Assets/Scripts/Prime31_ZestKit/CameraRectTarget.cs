using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class CameraRectTarget : AbstractTweenTarget<Camera, Rect>
	{
		public enum CameraTargetType
		{
			Rect,
			PixelRect
		}

		private CameraTargetType _targetType;

		public CameraRectTarget(Camera camera, CameraTargetType targetType = CameraTargetType.Rect)
		{
			_target = camera;
			_targetType = targetType;
		}

		public override void setTweenedValue(Rect value)
		{
			switch (_targetType)
			{
			case CameraTargetType.Rect:
				_target.rect = value;
				break;
			case CameraTargetType.PixelRect:
				_target.pixelRect = value;
				break;
			}
		}

		public override Rect getTweenedValue()
		{
			switch (_targetType)
			{
			case CameraTargetType.Rect:
				return _target.rect;
			case CameraTargetType.PixelRect:
				return _target.pixelRect;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
