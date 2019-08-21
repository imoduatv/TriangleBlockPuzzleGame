using System;
using UnityEngine;
using UnityEngine.UI;

namespace Prime31.ZestKit
{
	public class ImageFloatTarget : AbstractTweenTarget<Image, float>
	{
		public enum ImageTargetType
		{
			Alpha,
			FillAmount
		}

		private ImageTargetType _targetType;

		public ImageFloatTarget(Image image, ImageTargetType targetType = ImageTargetType.Alpha)
		{
			_target = image;
			_targetType = targetType;
		}

		public override void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				switch (_targetType)
				{
				case ImageTargetType.Alpha:
				{
					Color color = _target.color;
					color.a = value;
					_target.color = color;
					break;
				}
				case ImageTargetType.FillAmount:
					_target.fillAmount = value;
					break;
				}
			}
		}

		public override float getTweenedValue()
		{
			switch (_targetType)
			{
			case ImageTargetType.Alpha:
			{
				Color color = _target.color;
				return color.a;
			}
			case ImageTargetType.FillAmount:
				return _target.fillAmount;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void setTweenedValue(Color value)
		{
			_target.color = value;
		}
	}
}
