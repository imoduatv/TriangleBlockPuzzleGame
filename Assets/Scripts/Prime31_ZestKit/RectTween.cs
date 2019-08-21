using UnityEngine;

namespace Prime31.ZestKit
{
	public class RectTween : Tween<Rect>
	{
		public RectTween()
		{
		}

		public RectTween(ITweenTarget<Rect> target, Rect from, Rect to, float duration)
		{
			initialize(target, to, duration);
		}

		public static RectTween create()
		{
			return (!ZestKit.cacheRectTweens) ? new RectTween() : QuickCache<RectTween>.pop();
		}

		public override ITween<Rect> setIsRelative()
		{
			_isRelative = true;
			_toValue = new Rect(_toValue.x + _fromValue.x, _toValue.y + _fromValue.y, _toValue.width + _fromValue.width, _toValue.height + _fromValue.height);
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
			if (_shouldRecycleTween && ZestKit.cacheRectTweens)
			{
				QuickCache<RectTween>.push(this);
			}
		}
	}
}
