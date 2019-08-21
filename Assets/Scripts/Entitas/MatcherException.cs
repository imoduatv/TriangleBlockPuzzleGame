using System;

namespace Entitas
{
	public class MatcherException : Exception
	{
		public MatcherException(IMatcher matcher)
			: base("matcher.indices.Length must be 1 but was " + matcher.indices.Length)
		{
		}
	}
}
