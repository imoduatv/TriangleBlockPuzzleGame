using UnityEngine;

namespace Prime31.ZestKit
{
	public class Vector2Tween : Tween<Vector2>
	{
		public Vector2Tween()
		{
		}

		public Vector2Tween(ITweenTarget<Vector2> target, Vector2 from, Vector2 to, float duration)
		{
			initialize(target, to, duration);
		}

		public static Vector2Tween create()
		{
			return (!ZestKit.cacheVector2Tweens) ? new Vector2Tween() : QuickCache<Vector2Tween>.pop();
		}

		public override ITween<Vector2> setIsRelative()
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
			if (_shouldRecycleTween && ZestKit.cacheVector2Tweens)
			{
				QuickCache<Vector2Tween>.push(this);
			}
		}
	}
}
