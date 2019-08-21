using UnityEngine;

namespace Prime31.ZestKit
{
	public class RectTransformSizeDeltaTarget : AbstractTweenTarget<RectTransform, Vector2>
	{
		public RectTransformSizeDeltaTarget(RectTransform rectTransform)
		{
			_target = rectTransform;
		}

		public override void setTweenedValue(Vector2 value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				_target.sizeDelta = value;
			}
		}

		public override Vector2 getTweenedValue()
		{
			return _target.sizeDelta;
		}
	}
}
