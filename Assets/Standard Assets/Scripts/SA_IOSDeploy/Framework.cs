using SA.Common.Util;
using System;
using System.Collections.Generic;

namespace SA.IOSDeploy
{
	[Serializable]
	public class Framework
	{
		public bool IsOpen;

		public iOSFramework Type;

		public bool IsOptional;

		public bool IsEmbeded;

		public string Name => Type.ToString() + ".framework";

		public string TypeString => Type.ToString();

		public Framework(iOSFramework type, bool Embaded = false)
		{
			Type = type;
			IsEmbeded = Embaded;
		}

		public Framework(string frameworkName)
		{
			frameworkName = frameworkName.Replace(".framework", string.Empty);
			Type = General.ParseEnum<iOSFramework>(frameworkName);
		}

		public int[] BaseIndexes()
		{
			string[] array = ISD_FrameworkHandler.BaseFrameworksArray();
			List<int> list = new List<int>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(i);
			}
			return list.ToArray();
		}
	}
}
