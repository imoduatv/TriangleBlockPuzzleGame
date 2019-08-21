using System;
using System.Collections.Generic;
using System.Reflection;

namespace Entitas.Serialization
{
	public static class PublicMemberInfoExtension
	{
		public static List<PublicMemberInfo> GetPublicMemberInfos(this Type type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			List<PublicMemberInfo> list = new List<PublicMemberInfo>(fields.Length + properties.Length);
			int i = 0;
			for (int num = fields.Length; i < num; i++)
			{
				list.Add(new PublicMemberInfo(fields[i]));
			}
			int j = 0;
			for (int num2 = properties.Length; j < num2; j++)
			{
				PropertyInfo propertyInfo = properties[j];
				if (propertyInfo.CanRead && propertyInfo.CanWrite)
				{
					list.Add(new PublicMemberInfo(propertyInfo));
				}
			}
			return list;
		}

		public static object PublicMemberClone(this object obj)
		{
			object obj2 = Activator.CreateInstance(obj.GetType());
			obj.CopyPublicMemberValues(obj2);
			return obj2;
		}

		public static T PublicMemberClone<T>(this object obj) where T : new()
		{
			T val = new T();
			obj.CopyPublicMemberValues(val);
			return val;
		}

		public static void CopyPublicMemberValues(this object source, object target)
		{
			List<PublicMemberInfo> publicMemberInfos = source.GetType().GetPublicMemberInfos();
			int i = 0;
			for (int count = publicMemberInfos.Count; i < count; i++)
			{
				PublicMemberInfo publicMemberInfo = publicMemberInfos[i];
				publicMemberInfo.SetValue(target, publicMemberInfo.GetValue(source));
			}
		}
	}
}
