using UnityEngine;

namespace Prime31.ZestKit
{
	public class RectTransformAnchoredPositionTarget : AbstractTweenTarget<RectTransform, Vector2>
	{
		public RectTransformAnchoredPositionTarget(RectTransform rectTransform)
		{
			_target = rectTransform;
		}

		public override void setTweenedValue(Vector2 value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				_target.anchoredPosition = value;
			}
		}

		public override Vector2 getTweenedValue()
		{
			return _target.anchoredPosition;
		}
	}
}
