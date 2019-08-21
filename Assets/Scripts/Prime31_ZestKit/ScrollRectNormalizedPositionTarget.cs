using UnityEngine;
using UnityEngine.UI;

namespace Prime31.ZestKit
{
	public class ScrollRectNormalizedPositionTarget : AbstractTweenTarget<ScrollRect, Vector2>
	{
		public ScrollRectNormalizedPositionTarget(ScrollRect scrollRect)
		{
			_target = scrollRect;
		}

		public override void setTweenedValue(Vector2 value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				_target.normalizedPosition = value;
			}
		}

		public override Vector2 getTweenedValue()
		{
			return _target.normalizedPosition;
		}
	}
}
