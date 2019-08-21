using UnityEngine;
using UnityEngine.UI;

namespace Prime31.ZestKit
{
	public class ImageColorTarget : AbstractTweenTarget<Image, Color>
	{
		public ImageColorTarget(Image image)
		{
			_target = image;
		}

		public override void setTweenedValue(Color value)
		{
			_target.color = value;
		}

		public override Color getTweenedValue()
		{
			return _target.color;
		}
	}
}
