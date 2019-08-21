using UnityEngine;

namespace Prime31.ZestKit
{
	public class LightColorTarget : AbstractTweenTarget<Light, Color>
	{
		public LightColorTarget(Light light)
		{
			_target = light;
		}

		public override void setTweenedValue(Color value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				_target.color = value;
			}
		}

		public override Color getTweenedValue()
		{
			return _target.color;
		}
	}
}
