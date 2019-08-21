using System;
using System.Collections;
using System.IO;
using UnityEngine;
using XUPorterJSON;

namespace UnityEditor.XCodeEditor
{
	public class XCMod
	{
		private Hashtable _datastore = new Hashtable();

		private ArrayList _libs;

		public string name
		{
			get;
			private set;
		}

		public string path
		{
			get;
			private set;
		}

		public string group
		{
			get
			{
				if (_datastore != null && _datastore.Contains("group"))
				{
					return (string)_datastore["group"];
				}
				return string.Empty;
			}
		}

		public ArrayList patches => (ArrayList)_datastore["patches"];

		public ArrayList libs
		{
			get
			{
				if (_libs == null)
				{
					_libs = new ArrayList(((ArrayList)_datastore["libs"]).Count);
					IEnumerator enumerator = ((ArrayList)_datastore["libs"]).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string text = (string)enumerator.Current;
							UnityEngine.Debug.Log("Adding to Libs: " + text);
							_libs.Add(new XCModFile(text));
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
				return _libs;
			}
		}

		public ArrayList frameworks => (ArrayList)_datastore["frameworks"];

		public ArrayList headerpaths => (ArrayList)_datastore["headerpaths"];

		public ArrayList files => (ArrayList)_datastore["files"];

		public ArrayList folders => (ArrayList)_datastore["folders"];

		public ArrayList excludes => (ArrayList)_datastore["excludes"];

		public ArrayList compiler_flags => (ArrayList)_datastore["compiler_flags"];

		public ArrayList linker_flags => (ArrayList)_datastore["linker_flags"];

		public ArrayList embed_binaries => (ArrayList)_datastore["embed_binaries"];

		public Hashtable plist => (Hashtable)_datastore["plist"];

		public XCMod(string filename)
		{
			FileInfo fileInfo = new FileInfo(filename);
			if (!fileInfo.Exists)
			{
				UnityEngine.Debug.LogWarning("File does not exist.");
			}
			name = Path.GetFileNameWithoutExtension(filename);
			path = Path.GetDirectoryName(filename);
			string text = fileInfo.OpenText().ReadToEnd();
			UnityEngine.Debug.Log(text);
			_datastore = (Hashtable)MiniJSON.jsonDecode(text);
			if (_datastore == null || _datastore.Count == 0)
			{
				UnityEngine.Debug.Log(text);
				throw new UnityException("Parse error in file " + Path.GetFileName(filename) + "! Check for typos such as unbalanced quotation marks, etc.");
			}
		}
	}
}
