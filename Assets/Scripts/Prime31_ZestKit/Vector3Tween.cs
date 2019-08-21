using UnityEngine;

namespace Prime31.ZestKit
{
	public class Vector3Tween : Tween<Vector3>
	{
		public Vector3Tween()
		{
		}

		public Vector3Tween(ITweenTarget<Vector3> target, Vector3 from, Vector3 to, float duration)
		{
			initialize(target, to, duration);
		}

		public static Vector3Tween create()
		{
			return (!ZestKit.cacheVector3Tweens) ? new Vector3Tween() : QuickCache<Vector3Tween>.pop();
		}

		public override ITween<Vector3> setIsRelative()
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
			if (_shouldRecycleTween && ZestKit.cacheVector3Tweens)
			{
				QuickCache<Vector3Tween>.push(this);
			}
		}
	}
}
