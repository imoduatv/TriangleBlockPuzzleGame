using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Entitas.Serialization
{
	public static class TypeSerializationExtension
	{
		private static readonly Dictionary<string, string> _builtInTypesToString = new Dictionary<string, string>
		{
			{
				"System.Boolean",
				"bool"
			},
			{
				"System.Byte",
				"byte"
			},
			{
				"System.SByte",
				"sbyte"
			},
			{
				"System.Char",
				"char"
			},
			{
				"System.Decimal",
				"decimal"
			},
			{
				"System.Double",
				"double"
			},
			{
				"System.Single",
				"float"
			},
			{
				"System.Int32",
				"int"
			},
			{
				"System.UInt32",
				"uint"
			},
			{
				"System.Int64",
				"long"
			},
			{
				"System.UInt64",
				"ulong"
			},
			{
				"System.Object",
				"object"
			},
			{
				"System.Int16",
				"short"
			},
			{
				"System.UInt16",
				"ushort"
			},
			{
				"System.String",
				"string"
			},
			{
				"System.Void",
				"void"
			}
		};

		private static readonly Dictionary<string, string> _builtInTypeStrings = new Dictionary<string, string>
		{
			{
				"bool",
				"System.Boolean"
			},
			{
				"byte",
				"System.Byte"
			},
			{
				"sbyte",
				"System.SByte"
			},
			{
				"char",
				"System.Char"
			},
			{
				"decimal",
				"System.Decimal"
			},
			{
				"double",
				"System.Double"
			},
			{
				"float",
				"System.Single"
			},
			{
				"int",
				"System.Int32"
			},
			{
				"uint",
				"System.UInt32"
			},
			{
				"long",
				"System.Int64"
			},
			{
				"ulong",
				"System.UInt64"
			},
			{
				"object",
				"System.Object"
			},
			{
				"short",
				"System.Int16"
			},
			{
				"ushort",
				"System.UInt16"
			},
			{
				"string",
				"System.String"
			},
			{
				"void",
				"System.Void"
			}
		};

		[CompilerGenerated]
		private static Func<Type, string> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<Type, string> _003C_003Ef__mg_0024cache1;

		public static string ToCompilableString(this Type type)
		{
			if (_builtInTypesToString.ContainsKey(type.FullName))
			{
				return _builtInTypesToString[type.FullName];
			}
			if (type.IsGenericType)
			{
				string str = type.FullName.Split('`')[0];
				string[] value = type.GetGenericArguments().Select(ToCompilableString).ToArray();
				return str + "<" + string.Join(", ", value) + ">";
			}
			if (type.IsArray)
			{
				return type.GetElementType().ToCompilableString() + "[" + new string(',', type.GetArrayRank() - 1) + "]";
			}
			if (type.IsNested)
			{
				return type.FullName.Replace('+', '.');
			}
			return type.FullName;
		}

		public static string ToReadableString(this Type type)
		{
			if (_builtInTypesToString.ContainsKey(type.FullName))
			{
				return _builtInTypesToString[type.FullName];
			}
			if (type.IsGenericType)
			{
				string str = type.FullName.Split('`')[0];
				string[] value = type.GetGenericArguments().Select(ToReadableString).ToArray();
				return str + "<" + string.Join(", ", value) + ">";
			}
			if (type.IsArray)
			{
				return type.GetElementType().ToReadableString() + "[" + new string(',', type.GetArrayRank() - 1) + "]";
			}
			return type.FullName;
		}

		public static Type ToType(this string typeString)
		{
			string text = generateTypeString(typeString);
			Type type = Type.GetType(text);
			if (type != null)
			{
				return type;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				type = assembly.GetType(text);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		private static string generateTypeString(string typeString)
		{
			if (_builtInTypeStrings.ContainsKey(typeString))
			{
				typeString = _builtInTypeStrings[typeString];
			}
			else
			{
				typeString = generateGenericArguments(typeString);
				typeString = generateArray(typeString);
			}
			return typeString;
		}

		private static string generateGenericArguments(string typeString)
		{
			string[] separator = new string[1]
			{
				", "
			};
			typeString = Regex.Replace(typeString, "<(?<arg>.*)>", delegate(Match m)
			{
				string text = generateTypeString(m.Groups["arg"].Value);
				int num = text.Split(separator, StringSplitOptions.None).Length;
				return "`" + num + "[" + text + "]";
			});
			return typeString;
		}

		private static string generateArray(string typeString)
		{
			typeString = Regex.Replace(typeString, "(?<type>[^\\[]*)(?<rank>\\[,*\\])", delegate(Match m)
			{
				string str = generateTypeString(m.Groups["type"].Value);
				string value = m.Groups["rank"].Value;
				return str + value;
			});
			return typeString;
		}
	}
}
