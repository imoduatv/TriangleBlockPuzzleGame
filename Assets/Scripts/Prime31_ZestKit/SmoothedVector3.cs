using UnityEngine;

namespace Prime31.ZestKit
{
	public class SmoothedVector3 : SmoothedValue<Vector3>
	{
		public override Vector3 value
		{
			get
			{
				if (_currentValue == _toValue)
				{
					return _currentValue;
				}
				float t = Mathf.Clamp(Time.time - _startTime, 0f, _duration);
				_currentValue = Zest.ease(easeType, _fromValue, _toValue, t, _duration);
				return _currentValue;
			}
		}

		public SmoothedVector3(Vector3 currentValue, float duration = 0.3f)
			: base(currentValue, duration)
		{
		}
	}
}
