namespace Prime31.ZestKit
{
	public class FloatTween : Tween<float>
	{
		public FloatTween()
		{
		}

		public FloatTween(ITweenTarget<float> target, float from, float to, float duration)
		{
			initialize(target, to, duration);
		}

		public static FloatTween create()
		{
			return (!ZestKit.cacheFloatTweens) ? new FloatTween() : QuickCache<FloatTween>.pop();
		}

		public override ITween<float> setIsRelative()
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
			if (_shouldRecycleTween && ZestKit.cacheFloatTweens)
			{
				QuickCache<FloatTween>.push(this);
			}
		}
	}
}
