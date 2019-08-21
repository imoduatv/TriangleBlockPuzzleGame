using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Entitas.Unity
{
	public class Properties
	{
		private readonly Dictionary<string, string> _dict;

		public string[] keys => _dict.Keys.ToArray();

		public string[] values => _dict.Values.ToArray();

		public int count => _dict.Count;

		public string this[string key]
		{
			get
			{
				return _dict[key];
			}
			set
			{
				_dict[key.Trim()] = value.TrimStart().Replace("\\n", "\n").Replace("\\t", "\t");
			}
		}

		public Properties()
			: this(string.Empty)
		{
		}

		public Properties(string properties)
		{
			properties = convertLineEndings(properties);
			_dict = new Dictionary<string, string>();
			string[] linesWithProperties = getLinesWithProperties(properties);
			addProperties(mergeMultilineValues(linesWithProperties));
			replacePlaceholders();
		}

		public bool HasKey(string key)
		{
			return _dict.ContainsKey(key);
		}

		private static string convertLineEndings(string str)
		{
			return str.Replace("\r\n", "\n");
		}

		private static string[] getLinesWithProperties(string properties)
		{
			char[] separator = new char[1]
			{
				'\n'
			};
			return (from line in properties.Split(separator, StringSplitOptions.RemoveEmptyEntries)
				select line.TrimStart(' ') into line
				where !line.StartsWith("#", StringComparison.Ordinal)
				select line).ToArray();
		}

		private static string[] mergeMultilineValues(string[] lines)
		{
			string currentProperty = string.Empty;
			return lines.Aggregate(new List<string>(), delegate(List<string> acc, string line)
			{
				currentProperty += line;
				if (currentProperty.EndsWith("\\", StringComparison.Ordinal))
				{
					currentProperty = currentProperty.Substring(0, currentProperty.Length - 1);
				}
				else
				{
					acc.Add(currentProperty);
					currentProperty = string.Empty;
				}
				return acc;
			}).ToArray();
		}

		private void addProperties(string[] lines)
		{
			char[] keyValueDelimiter = new char[1]
			{
				'='
			};
			IEnumerable<string[]> enumerable = from line in lines
				select line.Split(keyValueDelimiter, 2);
			foreach (string[] item in enumerable)
			{
				this[item[0]] = item[1];
			}
		}

		private void replacePlaceholders()
		{
			string[] array = _dict.Keys.ToArray();
			foreach (string key in array)
			{
				MatchCollection matchCollection = Regex.Matches(_dict[key], "(?:(?<=\\${).+?(?=}))");
				IEnumerator enumerator = matchCollection.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Match match = (Match)enumerator.Current;
						_dict[key] = _dict[key].Replace("${" + match.Value + "}", _dict[match.Value]);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}

		public override string ToString()
		{
			return _dict.Aggregate(string.Empty, delegate(string properties, KeyValuePair<string, string> kv)
			{
				string text = kv.Value.Replace("\n", "\\n").Replace("\t", "\\t");
				return properties + kv.Key + " = " + text + "\n";
			});
		}
	}
}
