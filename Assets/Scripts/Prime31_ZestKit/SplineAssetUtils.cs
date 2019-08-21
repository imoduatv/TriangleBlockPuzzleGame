using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Prime31.ZestKit
{
	public static class SplineAssetUtils
	{
		public static List<Vector3> nodeListFromAsset(string pathAssetName)
		{
			string empty = string.Empty;
			if (!pathAssetName.EndsWith(".asset"))
			{
				pathAssetName += ".asset";
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				empty = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", pathAssetName);
				WWW wWW = new WWW(empty);
				while (!wWW.isDone)
				{
				}
				return bytesToVector3List(wWW.bytes);
			}
			empty = Path.Combine(Application.streamingAssetsPath, pathAssetName);
			byte[] bytes = File.ReadAllBytes(empty);
			return bytesToVector3List(bytes);
		}

		public static List<Vector3> bytesToVector3List(byte[] bytes)
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < bytes.Length; i += 12)
			{
				Vector3 item = new Vector3(BitConverter.ToSingle(bytes, i), BitConverter.ToSingle(bytes, i + 4), BitConverter.ToSingle(bytes, i + 8));
				list.Add(item);
			}
			return list;
		}
	}
}
