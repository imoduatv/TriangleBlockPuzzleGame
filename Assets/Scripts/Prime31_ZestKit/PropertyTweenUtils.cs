using System;
using System.Reflection;
using UnityEngine;

namespace Prime31.ZestKit
{
	internal class PropertyTweenUtils
	{
		public static T setterForProperty<T>(object targetObject, string propertyName)
		{
			PropertyInfo property = targetObject.GetType().GetProperty(propertyName);
			if (property == null)
			{
				UnityEngine.Debug.Log("could not find property with name: " + propertyName);
				return default(T);
			}
			return (T)(object)Delegate.CreateDelegate(typeof(T), targetObject, property.GetSetMethod());
		}

		public static T getterForProperty<T>(object targetObject, string propertyName)
		{
			PropertyInfo property = targetObject.GetType().GetProperty(propertyName);
			if (property == null)
			{
				UnityEngine.Debug.Log("could not find property with name: " + propertyName);
				return default(T);
			}
			return (T)(object)Delegate.CreateDelegate(typeof(T), targetObject, property.GetGetMethod());
		}
	}
}
