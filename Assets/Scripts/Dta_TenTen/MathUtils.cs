using System;

namespace Dta.TenTen
{
	public class MathUtils
	{
		private static Random rand;

		private static Random GetRand()
		{
			if (rand == null)
			{
				rand = new Random();
			}
			return rand;
		}

		public static int Random(int min, int max)
		{
			return GetRand().Next(min, max);
		}
	}
}
