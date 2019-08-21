using UnityEngine;

namespace Prime31.ZestKit
{
	public class Vector4Tween : Tween<Vector4>
	{
		public Vector4Tween()
		{
		}

		public Vector4Tween(ITweenTarget<Vector4> target, Vector4 from, Vector4 to, float duration)
		{
			initialize(target, to, duration);
		}

		public static Vector4Tween create()
		{
			return (!ZestKit.cacheVector4Tweens) ? new Vector4Tween() : QuickCache<Vector4Tween>.pop();
		}

		public override ITween<Vector4> setIsRelative()
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
			if (_shouldRecycleTween && ZestKit.cacheVector4Tweens)
			{
				QuickCache<Vector4Tween>.push(this);
			}
		}
	}
}
