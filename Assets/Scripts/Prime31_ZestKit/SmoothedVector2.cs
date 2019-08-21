using UnityEngine;

namespace Prime31.ZestKit
{
	public class SmoothedVector2 : SmoothedValue<Vector2>
	{
		public override Vector2 value
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

		public SmoothedVector2(Vector2 currentValue, float duration = 0.3f)
			: base(currentValue, duration)
		{
		}
	}
}
