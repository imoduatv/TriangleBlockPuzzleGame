using PlistCS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.XCodeEditor
{
	public class XCPlist
	{
		private string plistPath;

		private bool plistModified;

		private const string BundleUrlTypes = "CFBundleURLTypes";

		private const string BundleTypeRole = "CFBundleTypeRole";

		private const string BundleUrlName = "CFBundleURLName";

		private const string BundleUrlSchemes = "CFBundleURLSchemes";

		private const string PlistUrlType = "urltype";

		private const string PlistRole = "role";

		private const string PlistEditor = "Editor";

		private const string PlistName = "name";

		private const string PlistSchemes = "schemes";

		public XCPlist(string plistPath)
		{
			this.plistPath = plistPath;
		}

		public void Process(Hashtable plist)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Plist.readPlist(plistPath);
			IDictionaryEnumerator enumerator = plist.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
					AddPlistItems((string)dictionaryEntry.Key, dictionaryEntry.Value, dictionary);
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
			if (plistModified)
			{
				Plist.writeXml(dictionary, plistPath);
			}
		}

		public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			IDictionaryEnumerator enumerator = table.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
					dictionary.Add((K)dictionaryEntry.Key, (V)dictionaryEntry.Value);
				}
				return dictionary;
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

		public void AddPlistItems(string key, object value, Dictionary<string, object> dict)
		{
			UnityEngine.Debug.Log("AddPlistItems: key=" + key);
			if (key.CompareTo("urltype") == 0)
			{
				processUrlTypes((ArrayList)value, dict);
				return;
			}
			dict[key] = HashtableToDictionary<string, object>((Hashtable)value);
			plistModified = true;
		}

		private void processUrlTypes(ArrayList urltypes, Dictionary<string, object> dict)
		{
			List<object> list = (!dict.ContainsKey("CFBundleURLTypes")) ? new List<object>() : ((List<object>)dict["CFBundleURLTypes"]);
			IEnumerator enumerator = urltypes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Hashtable hashtable = (Hashtable)enumerator.Current;
					string value = (string)hashtable["role"];
					if (string.IsNullOrEmpty(value))
					{
						value = "Editor";
					}
					string text = (string)hashtable["name"];
					ArrayList arrayList = (ArrayList)hashtable["schemes"];
					List<object> list2 = new List<object>();
					IEnumerator enumerator2 = arrayList.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							string item = (string)enumerator2.Current;
							list2.Add(item);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					Dictionary<string, object> dictionary = findUrlTypeByName(list, text);
					if (dictionary == null)
					{
						dictionary = new Dictionary<string, object>();
						dictionary["CFBundleTypeRole"] = value;
						dictionary["CFBundleURLName"] = text;
						dictionary["CFBundleURLSchemes"] = list2;
						list.Add(dictionary);
					}
					else
					{
						dictionary["CFBundleTypeRole"] = value;
						dictionary["CFBundleURLSchemes"] = list2;
					}
					plistModified = true;
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			dict["CFBundleURLTypes"] = list;
		}

		private Dictionary<string, object> findUrlTypeByName(List<object> bundleUrlTypes, string name)
		{
			if (bundleUrlTypes == null || bundleUrlTypes.Count == 0)
			{
				return null;
			}
			foreach (Dictionary<string, object> bundleUrlType in bundleUrlTypes)
			{
				string strA = (string)bundleUrlType["CFBundleURLName"];
				if (string.Compare(strA, name) == 0)
				{
					return bundleUrlType;
				}
			}
			return null;
		}
	}
}
