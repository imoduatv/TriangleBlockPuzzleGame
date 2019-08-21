using UnityEngine;
using UnityEngine.UI;

namespace Prime31.ZestKit
{
	public class TextAlphaTarget : AbstractTextTarget, ITweenTarget<float>
	{
		public TextAlphaTarget(Text text)
		{
			prepareForUse(text);
		}

		public void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || (bool)_text)
			{
				Color color = _text.color;
				color.a = value;
				_text.color = color;
			}
		}

		public float getTweenedValue()
		{
			Color color = _text.color;
			return color.a;
		}
	}
}
