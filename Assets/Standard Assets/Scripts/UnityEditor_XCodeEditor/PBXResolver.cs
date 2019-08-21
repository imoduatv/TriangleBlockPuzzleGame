using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.XCodeEditor
{
	public class PBXResolver
	{
		private class PBXResolverReverseIndex : Dictionary<string, string>
		{
		}

		private PBXDictionary objects;

		private string rootObject;

		private PBXResolverReverseIndex index;

		public PBXResolver(PBXDictionary pbxData)
		{
			objects = (PBXDictionary)pbxData["objects"];
			index = new PBXResolverReverseIndex();
			rootObject = (string)pbxData["rootObject"];
			BuildReverseIndex();
		}

		private void BuildReverseIndex()
		{
			foreach (KeyValuePair<string, object> @object in objects)
			{
				if (@object.Value is PBXBuildPhase)
				{
					IEnumerator enumerator2 = ((PBXBuildPhase)@object.Value).files.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							string key = (string)enumerator2.Current;
							index[key] = @object.Key;
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
				}
				else if (@object.Value is PBXGroup)
				{
					IEnumerator enumerator3 = ((PBXGroup)@object.Value).children.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							string key2 = (string)enumerator3.Current;
							index[key2] = @object.Key;
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator3 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
			}
		}

		public string ResolveName(string guid)
		{
			if (!objects.ContainsKey(guid))
			{
				UnityEngine.Debug.LogWarning(this + " ResolveName could not resolve " + guid);
				return string.Empty;
			}
			object obj = objects[guid];
			if (obj is PBXBuildFile)
			{
				return ResolveName(((PBXBuildFile)obj).fileRef);
			}
			if (obj is PBXFileReference)
			{
				PBXFileReference pBXFileReference = (PBXFileReference)obj;
				return (pBXFileReference.name == null) ? pBXFileReference.path : pBXFileReference.name;
			}
			if (obj is PBXGroup)
			{
				PBXGroup pBXGroup = (PBXGroup)obj;
				return (pBXGroup.name == null) ? pBXGroup.path : pBXGroup.name;
			}
			if (obj is PBXProject || guid == rootObject)
			{
				return "Project object";
			}
			if (obj is PBXFrameworksBuildPhase)
			{
				return "Frameworks";
			}
			if (obj is PBXResourcesBuildPhase)
			{
				return "Resources";
			}
			if (obj is PBXShellScriptBuildPhase)
			{
				return "ShellScript";
			}
			if (obj is PBXSourcesBuildPhase)
			{
				return "Sources";
			}
			if (obj is PBXCopyFilesBuildPhase)
			{
				return "CopyFiles";
			}
			if (obj is XCConfigurationList)
			{
				XCConfigurationList xCConfigurationList = (XCConfigurationList)obj;
				if (xCConfigurationList.data.ContainsKey("defaultConfigurationName"))
				{
					return (string)xCConfigurationList.data["defaultConfigurationName"];
				}
				return null;
			}
			if (obj is PBXNativeTarget)
			{
				PBXNativeTarget pBXNativeTarget = (PBXNativeTarget)obj;
				if (pBXNativeTarget.data.ContainsKey("name"))
				{
					return (string)pBXNativeTarget.data["name"];
				}
				return null;
			}
			if (obj is XCBuildConfiguration)
			{
				XCBuildConfiguration xCBuildConfiguration = (XCBuildConfiguration)obj;
				if (xCBuildConfiguration.data.ContainsKey("name"))
				{
					return (string)xCBuildConfiguration.data["name"];
				}
			}
			else if (obj is PBXObject)
			{
				PBXObject pBXObject = (PBXObject)obj;
				if (pBXObject.data.ContainsKey("name"))
				{
					UnityEngine.Debug.Log("PBXObject " + (string)pBXObject.data["name"] + " " + guid + " " + ((pBXObject != null) ? pBXObject.ToString() : string.Empty));
				}
				return (string)pBXObject.data["name"];
			}
			UnityEngine.Debug.LogWarning("UNRESOLVED GUID:" + guid);
			return null;
		}

		public string ResolveBuildPhaseNameForFile(string guid)
		{
			if (objects.ContainsKey(guid))
			{
				object obj = objects[guid];
				if (obj is PBXObject)
				{
					PBXObject pBXObject = (PBXObject)obj;
					if (index.ContainsKey(pBXObject.guid))
					{
						string key = index[pBXObject.guid];
						if (objects.ContainsKey(key))
						{
							object obj2 = objects[key];
							if (obj2 is PBXBuildPhase)
							{
								return ResolveName(((PBXBuildPhase)obj2).guid);
							}
						}
					}
				}
			}
			return null;
		}
	}
}
