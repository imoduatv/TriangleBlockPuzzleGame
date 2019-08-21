using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class PropertyTarget<T> : ITweenTarget<T> where T : struct
	{
		protected object _target;

		protected Action<T> _setter;

		protected Func<T> _getter;

		public PropertyTarget(object target, string propertyName)
		{
			_target = target;
			_setter = PropertyTweenUtils.setterForProperty<Action<T>>(target, propertyName);
			_getter = PropertyTweenUtils.getterForProperty<Func<T>>(target, propertyName);
			if (_setter == null)
			{
				UnityEngine.Debug.LogError("either the property (" + propertyName + ") setter or getter could not be found on the object " + target);
			}
		}

		public void setTweenedValue(T value)
		{
			_setter(value);
		}

		public T getTweenedValue()
		{
			return _getter();
		}

		public object getTargetObject()
		{
			return _target;
		}
	}
}
