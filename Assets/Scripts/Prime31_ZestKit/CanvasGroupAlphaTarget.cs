using UnityEngine;

namespace Prime31.ZestKit
{
	public class CanvasGroupAlphaTarget : AbstractTweenTarget<CanvasGroup, float>
	{
		public CanvasGroupAlphaTarget(CanvasGroup canvasGroup)
		{
			_target = canvasGroup;
		}

		public override void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				_target.alpha = value;
			}
		}

		public override float getTweenedValue()
		{
			return _target.alpha;
		}
	}
}
