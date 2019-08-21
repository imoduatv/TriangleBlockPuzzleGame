using UnityEngine;

namespace Prime31.ZestKit
{
	public class ColorTween : Tween<Color>
	{
		public ColorTween()
		{
		}

		public ColorTween(ITweenTarget<Color> target, Color from, Color to, float duration)
		{
			initialize(target, to, duration);
		}

		public static ColorTween create()
		{
			return (!ZestKit.cacheColorTweens) ? new ColorTween() : QuickCache<ColorTween>.pop();
		}

		public override ITween<Color> setIsRelative()
		{
			_isRelative = true;
			_toValue += _fromValue;
			return this;
		}

		protected override void updateValue()
		{
			if (_animationCurve != null)
			{
				_target.setTweenedValue(Zest.ease(_animationCurve, _fromValue, _toValue, _elapsedTime, _duration));
			}
			else
			{
				_target.setTweenedValue(Zest.ease(_easeType, _fromValue, _toValue, _elapsedTime, _duration));
			}
		}

		public override void recycleSelf()
		{
			base.recycleSelf();
			if (_shouldRecycleTween && ZestKit.cacheColorTweens)
			{
				QuickCache<ColorTween>.push(this);
			}
		}
	}
}
