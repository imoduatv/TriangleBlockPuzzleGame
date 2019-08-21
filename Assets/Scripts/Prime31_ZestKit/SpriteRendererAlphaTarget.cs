using UnityEngine;

namespace Prime31.ZestKit
{
	public class SpriteRendererAlphaTarget : AbstractSpriteRendererTarget, ITweenTarget<float>
	{
		public SpriteRendererAlphaTarget(SpriteRenderer spriteRenderer)
		{
			prepareForUse(spriteRenderer);
		}

		public void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || (bool)_spriteRenderer)
			{
				Color color = _spriteRenderer.color;
				color.a = value;
				_spriteRenderer.color = color;
			}
		}

		public float getTweenedValue()
		{
			Color color = _spriteRenderer.color;
			return color.a;
		}
	}
}
