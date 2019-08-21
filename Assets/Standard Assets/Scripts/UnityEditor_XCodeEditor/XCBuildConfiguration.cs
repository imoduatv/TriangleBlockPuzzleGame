using System;
using System.Collections;
using UnityEngine;

namespace UnityEditor.XCodeEditor
{
	public class XCBuildConfiguration : PBXObject
	{
		protected const string BUILDSETTINGS_KEY = "buildSettings";

		protected const string HEADER_SEARCH_PATHS_KEY = "HEADER_SEARCH_PATHS";

		protected const string LIBRARY_SEARCH_PATHS_KEY = "LIBRARY_SEARCH_PATHS";

		protected const string FRAMEWORK_SEARCH_PATHS_KEY = "FRAMEWORK_SEARCH_PATHS";

		protected const string OTHER_C_FLAGS_KEY = "OTHER_CFLAGS";

		protected const string OTHER_LDFLAGS_KEY = "OTHER_LDFLAGS";

		public PBXSortedDictionary buildSettings
		{
			get
			{
				if (ContainsKey("buildSettings"))
				{
					if (_data["buildSettings"].GetType() == typeof(PBXDictionary))
					{
						PBXSortedDictionary pBXSortedDictionary = new PBXSortedDictionary();
						pBXSortedDictionary.Append((PBXDictionary)_data["buildSettings"]);
						return pBXSortedDictionary;
					}
					return (PBXSortedDictionary)_data["buildSettings"];
				}
				return null;
			}
		}

		public XCBuildConfiguration(string guid, PBXDictionary dictionary)
			: base(guid, dictionary)
		{
		}

		protected bool AddSearchPaths(string path, string key, bool recursive = true)
		{
			PBXList pBXList = new PBXList();
			pBXList.Add(path);
			return AddSearchPaths(pBXList, key, recursive);
		}

		protected bool AddSearchPaths(PBXList paths, string key, bool recursive = true, bool quoted = false)
		{
			bool result = false;
			if (!ContainsKey("buildSettings"))
			{
				Add("buildSettings", new PBXSortedDictionary());
			}
			IEnumerator enumerator = paths.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Current;
					string text2 = text;
					if (!((PBXDictionary)_data["buildSettings"]).ContainsKey(key))
					{
						((PBXDictionary)_data["buildSettings"]).Add(key, new PBXList());
					}
					else if (((PBXDictionary)_data["buildSettings"])[key] is string)
					{
						PBXList pBXList = new PBXList();
						pBXList.Add(((PBXDictionary)_data["buildSettings"])[key]);
						((PBXDictionary)_data["buildSettings"])[key] = pBXList;
					}
					if (text2.Contains(" "))
					{
						quoted = true;
					}
					if (quoted)
					{
						text2 = ((!text2.EndsWith("/**")) ? ("\\\"" + text2 + "\\\"") : ("\\\"" + text2.Replace("/**", "\\\"/**")));
					}
					if (!((PBXList)((PBXDictionary)_data["buildSettings"])[key]).Contains(text2))
					{
						((PBXList)((PBXDictionary)_data["buildSettings"])[key]).Add(text2);
						result = true;
					}
				}
				return result;
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

		public bool AddHeaderSearchPaths(PBXList paths, bool recursive = true)
		{
			return AddSearchPaths(paths, "HEADER_SEARCH_PATHS", recursive);
		}

		public bool AddLibrarySearchPaths(PBXList paths, bool recursive = true)
		{
			UnityEngine.Debug.Log("AddLibrarySearchPaths " + paths);
			return AddSearchPaths(paths, "LIBRARY_SEARCH_PATHS", recursive);
		}

		public bool AddFrameworkSearchPaths(PBXList paths, bool recursive = true)
		{
			return AddSearchPaths(paths, "FRAMEWORK_SEARCH_PATHS", recursive);
		}

		public bool AddOtherCFlags(string flag)
		{
			PBXList pBXList = new PBXList();
			pBXList.Add(flag);
			return AddOtherCFlags(pBXList);
		}

		public bool AddOtherCFlags(PBXList flags)
		{
			bool result = false;
			if (!ContainsKey("buildSettings"))
			{
				Add("buildSettings", new PBXSortedDictionary());
			}
			IEnumerator enumerator = flags.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Current;
					if (!((PBXDictionary)_data["buildSettings"]).ContainsKey("OTHER_CFLAGS"))
					{
						((PBXDictionary)_data["buildSettings"]).Add("OTHER_CFLAGS", new PBXList());
					}
					else if (((PBXDictionary)_data["buildSettings"])["OTHER_CFLAGS"] is string)
					{
						string value = (string)((PBXDictionary)_data["buildSettings"])["OTHER_CFLAGS"];
						((PBXDictionary)_data["buildSettings"])["OTHER_CFLAGS"] = new PBXList();
						((PBXList)((PBXDictionary)_data["buildSettings"])["OTHER_CFLAGS"]).Add(value);
					}
					if (!((PBXList)((PBXDictionary)_data["buildSettings"])["OTHER_CFLAGS"]).Contains(text))
					{
						((PBXList)((PBXDictionary)_data["buildSettings"])["OTHER_CFLAGS"]).Add(text);
						result = true;
					}
				}
				return result;
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

		public bool AddOtherLinkerFlags(string flag)
		{
			PBXList pBXList = new PBXList();
			pBXList.Add(flag);
			return AddOtherLinkerFlags(pBXList);
		}

		public bool AddOtherLinkerFlags(PBXList flags)
		{
			bool result = false;
			if (!ContainsKey("buildSettings"))
			{
				Add("buildSettings", new PBXSortedDictionary());
			}
			IEnumerator enumerator = flags.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Current;
					if (!((PBXDictionary)_data["buildSettings"]).ContainsKey("OTHER_LDFLAGS"))
					{
						((PBXDictionary)_data["buildSettings"]).Add("OTHER_LDFLAGS", new PBXList());
					}
					else if (((PBXDictionary)_data["buildSettings"])["OTHER_LDFLAGS"] is string)
					{
						string value = (string)((PBXDictionary)_data["buildSettings"])["OTHER_LDFLAGS"];
						((PBXDictionary)_data["buildSettings"])["OTHER_LDFLAGS"] = new PBXList();
						if (!string.IsNullOrEmpty(value))
						{
							((PBXList)((PBXDictionary)_data["buildSettings"])["OTHER_LDFLAGS"]).Add(value);
						}
					}
					if (!((PBXList)((PBXDictionary)_data["buildSettings"])["OTHER_LDFLAGS"]).Contains(text))
					{
						((PBXList)((PBXDictionary)_data["buildSettings"])["OTHER_LDFLAGS"]).Add(text);
						result = true;
					}
				}
				return result;
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

		public bool overwriteBuildSetting(string settingName, string settingValue)
		{
			UnityEngine.Debug.Log("overwriteBuildSetting " + settingName + " " + settingValue);
			bool result = false;
			if (!ContainsKey("buildSettings"))
			{
				UnityEngine.Debug.Log("creating key buildSettings");
				Add("buildSettings", new PBXSortedDictionary());
			}
			if (!((PBXDictionary)_data["buildSettings"]).ContainsKey(settingName))
			{
				UnityEngine.Debug.Log("adding key " + settingName);
				((PBXDictionary)_data["buildSettings"]).Add(settingName, new PBXList());
			}
			else if (((PBXDictionary)_data["buildSettings"])[settingName] is string)
			{
				((PBXDictionary)_data["buildSettings"])[settingName] = new PBXList();
			}
			if (!((PBXList)((PBXDictionary)_data["buildSettings"])[settingName]).Contains(settingValue))
			{
				UnityEngine.Debug.Log("setting " + settingName + " to " + settingValue);
				((PBXList)((PBXDictionary)_data["buildSettings"])[settingName]).Add(settingValue);
				result = true;
			}
			return result;
		}
	}
}
