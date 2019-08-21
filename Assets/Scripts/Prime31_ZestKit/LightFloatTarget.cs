using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class LightFloatTarget : AbstractTweenTarget<Light, float>
	{
		public enum LightTargetType
		{
			Intensity,
			Range,
			SpotAngle
		}

		private LightTargetType _targetType;

		public LightFloatTarget(Light light, LightTargetType targetType = LightTargetType.Intensity)
		{
			_target = light;
			_targetType = targetType;
		}

		public override void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				switch (_targetType)
				{
				case LightTargetType.Intensity:
					_target.intensity = value;
					break;
				case LightTargetType.Range:
					_target.range = value;
					break;
				case LightTargetType.SpotAngle:
					_target.spotAngle = value;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public override float getTweenedValue()
		{
			switch (_targetType)
			{
			case LightTargetType.Intensity:
				return _target.intensity;
			case LightTargetType.Range:
				return _target.range;
			case LightTargetType.SpotAngle:
				return _target.spotAngle;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
