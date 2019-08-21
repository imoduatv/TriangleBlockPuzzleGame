using System;

namespace Entitas
{
	public class EntitasException : Exception
	{
		public EntitasException(string message, string hint)
			: base((hint == null) ? message : (message + "\n" + hint))
		{
		}
	}
}
