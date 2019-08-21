using UnityEngine.UI;

namespace Prime31.ZestKit
{
	public abstract class AbstractTextTarget
	{
		protected Text _text;

		public void prepareForUse(Text text)
		{
			_text = text;
		}

		public object getTargetObject()
		{
			return _text;
		}
	}
}
