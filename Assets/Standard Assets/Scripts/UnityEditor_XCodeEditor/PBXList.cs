using System;
using System.Collections;

namespace UnityEditor.XCodeEditor
{
	public class PBXList : ArrayList
	{
		public PBXList()
		{
		}

		public PBXList(object firstValue)
		{
			Add(firstValue);
		}

		public static implicit operator bool(PBXList x)
		{
			return x != null && x.Count == 0;
		}

		public string ToCSV()
		{
			string text = string.Empty;
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string str = (string)enumerator.Current;
					text += "\"";
					text += str;
					text += "\", ";
				}
				return text;
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

		public override string ToString()
		{
			return "{" + ToCSV() + "} ";
		}
	}
}
