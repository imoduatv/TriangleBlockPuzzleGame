using UnityEngine;

namespace Prime31.ZestKit
{
	public class QuaternionTween : Tween<Quaternion>
	{
		public QuaternionTween()
		{
		}

		public QuaternionTween(ITweenTarget<Quaternion> target, Quaternion from, Quaternion to, float duration)
		{
			initialize(target, to, duration);
		}

		public static QuaternionTween create()
		{
			return (!ZestKit.cacheQuaternionTweens) ? new QuaternionTween() : QuickCache<QuaternionTween>.pop();
		}

		public override ITween<Quaternion> setIsRelative()
		{
			_isRelative = true;
			_toValue *= _fromValue;
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
			if (_shouldRecycleTween && ZestKit.cacheQuaternionTweens)
			{
				QuickCache<QuaternionTween>.push(this);
			}
		}
	}
}
