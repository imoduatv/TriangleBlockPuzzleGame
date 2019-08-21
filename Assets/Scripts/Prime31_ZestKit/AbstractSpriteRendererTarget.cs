using UnityEngine;

namespace Prime31.ZestKit
{
	public abstract class AbstractSpriteRendererTarget
	{
		protected SpriteRenderer _spriteRenderer;

		public void prepareForUse(SpriteRenderer spriteRenderer)
		{
			_spriteRenderer = spriteRenderer;
		}

		public object getTargetObject()
		{
			return _spriteRenderer;
		}
	}
}
