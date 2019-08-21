using UnityEngine;

namespace Prime31.ZestKit
{
	public class Color32Tween : Tween<Color32>
	{
		public Color32Tween()
		{
		}

		public Color32Tween(ITweenTarget<Color32> target, Color32 from, Color32 to, float duration)
		{
			initialize(target, to, duration);
		}

		public static Color32Tween create()
		{
			return (!ZestKit.cacheColor32Tweens) ? new Color32Tween() : QuickCache<Color32Tween>.pop();
		}

		public override ITween<Color32> setIsRelative()
		{
			_isRelative = true;
			_toValue = (Color)_toValue + (Color)_fromValue;
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
			if (_shouldRecycleTween && ZestKit.cacheColor32Tweens)
			{
				QuickCache<Color32Tween>.push(this);
			}
		}
	}
}
