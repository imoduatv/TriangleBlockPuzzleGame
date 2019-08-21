using System;
using System.Collections.Generic;

namespace Archon.SwissArmyLib.Utils.Extensions
{
	public static class ListExtensions
	{
		private static readonly Random Random = new Random();

		public static void Shuffle<T>(this IList<T> list)
		{
			int num = list.Count;
			while (num > 1)
			{
				num--;
				int index = Random.Next(num + 1);
				T value = list[index];
				list[index] = list[num];
				list[num] = value;
			}
		}
	}
}
