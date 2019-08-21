using UnityEngine;
using UnityEngine.UI;

namespace Prime31.ZestKit
{
	public class TextColorTarget : AbstractTextTarget, ITweenTarget<Color>
	{
		public TextColorTarget(Text text)
		{
			prepareForUse(text);
		}

		public void setTweenedValue(Color value)
		{
			if (!ZestKit.enableBabysitter || (bool)_text)
			{
				_text.color = value;
			}
		}

		public Color getTweenedValue()
		{
			return _text.color;
		}
	}
}
