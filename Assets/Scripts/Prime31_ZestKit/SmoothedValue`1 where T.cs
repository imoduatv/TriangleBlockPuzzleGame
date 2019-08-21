using UnityEngine;

namespace Prime31.ZestKit
{
	public abstract class SmoothedValue<T> where T : struct
	{
		public EaseType easeType = ZestKit.defaultEaseType;

		protected float _duration;

		protected float _startTime;

		protected T _currentValue;

		protected T _fromValue;

		protected T _toValue;

		public abstract T value
		{
			get;
		}

		public SmoothedValue(T currentValue, float duration = 0.3f)
		{
			_duration = duration;
			_startTime = Time.time;
			_currentValue = currentValue;
			_fromValue = currentValue;
			_toValue = currentValue;
		}

		public void setToValue(T toValue)
		{
			_startTime = Time.time;
			_fromValue = _currentValue;
			_toValue = toValue;
		}

		public void resetFromAndToValues(T fromValue, T toValue)
		{
			_startTime = Time.time;
			_fromValue = fromValue;
			_toValue = toValue;
		}
	}
}
