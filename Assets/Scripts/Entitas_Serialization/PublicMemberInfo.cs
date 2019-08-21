using System;
using System.Reflection;

namespace Entitas.Serialization
{
	public class PublicMemberInfo
	{
		private readonly FieldInfo _fieldInfo;

		private readonly PropertyInfo _propertyInfo;

		private readonly Type _type;

		private readonly string _name;

		public Type type => _type;

		public string name => _name;

		public PublicMemberInfo(FieldInfo info)
		{
			_fieldInfo = info;
			_propertyInfo = null;
			_type = _fieldInfo.FieldType;
			_name = _fieldInfo.Name;
		}

		public PublicMemberInfo(PropertyInfo info)
		{
			_fieldInfo = null;
			_propertyInfo = info;
			_type = _propertyInfo.PropertyType;
			_name = _propertyInfo.Name;
		}

		public PublicMemberInfo(Type type, string name)
		{
			_type = type;
			_name = name;
		}

		public object GetValue(object obj)
		{
			return (_fieldInfo == null) ? _propertyInfo.GetValue(obj, null) : _fieldInfo.GetValue(obj);
		}

		public void SetValue(object obj, object value)
		{
			if (_fieldInfo != null)
			{
				_fieldInfo.SetValue(obj, value);
			}
			else
			{
				_propertyInfo.SetValue(obj, value, null);
			}
		}
	}
}
