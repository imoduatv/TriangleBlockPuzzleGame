using UnityEngine;

namespace Prime31.ZestKit
{
	public class SpriteRendererColorTarget : AbstractSpriteRendererTarget, ITweenTarget<Color>
	{
		public SpriteRendererColorTarget(SpriteRenderer spriteRenderer)
		{
			prepareForUse(spriteRenderer);
		}

		public void setTweenedValue(Color value)
		{
			if (!ZestKit.enableBabysitter || (bool)_spriteRenderer)
			{
				_spriteRenderer.color = value;
			}
		}

		public Color getTweenedValue()
		{
			return _spriteRenderer.color;
		}
	}
}
