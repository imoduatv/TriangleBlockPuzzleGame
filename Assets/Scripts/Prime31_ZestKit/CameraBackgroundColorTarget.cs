using UnityEngine;

namespace Prime31.ZestKit
{
	public class CameraBackgroundColorTarget : AbstractTweenTarget<Camera, Color>
	{
		public CameraBackgroundColorTarget(Camera camera)
		{
			_target = camera;
		}

		public override void setTweenedValue(Color value)
		{
			_target.backgroundColor = value;
		}

		public override Color getTweenedValue()
		{
			return _target.backgroundColor;
		}
	}
}
