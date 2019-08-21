namespace Prime31.ZestKit
{
	public class IntTween : Tween<int>
	{
		public IntTween()
		{
		}

		public IntTween(ITweenTarget<int> target, int to, float duration)
		{
			initialize(target, to, duration);
		}

		public static IntTween create()
		{
			return (!ZestKit.cacheIntTweens) ? new IntTween() : QuickCache<IntTween>.pop();
		}

		public override ITween<int> setIsRelative()
		{
			_isRelative = true;
			_toValue += _fromValue;
			return this;
		}

		protected override void updateValue()
		{
			if (_animationCurve != null)
			{
				_target.setTweenedValue((int)Zest.ease(_animationCurve, _fromValue, _toValue, _elapsedTime, _duration));
			}
			else
			{
				_target.setTweenedValue((int)Zest.ease(_easeType, _fromValue, _toValue, _elapsedTime, _duration));
			}
		}

		public override void recycleSelf()
		{
			base.recycleSelf();
			if (_shouldRecycleTween && ZestKit.cacheIntTweens)
			{
				QuickCache<IntTween>.push(this);
			}
		}
	}
}
