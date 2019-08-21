using UnityEngine;

namespace Prime31.ZestKit
{
	public class RectTransformAnchoredPosition3DTarget : AbstractTweenTarget<RectTransform, Vector3>
	{
		public RectTransformAnchoredPosition3DTarget(RectTransform rectTransform)
		{
			_target = rectTransform;
		}

		public override void setTweenedValue(Vector3 value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				_target.anchoredPosition3D = value;
			}
		}

		public override Vector3 getTweenedValue()
		{
			return _target.anchoredPosition3D;
		}
	}
}
