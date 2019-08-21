using System.Collections.Generic;

namespace Prime31.ZestKit
{
	public static class QuickCache<T> where T : new()
	{
		private static Stack<T> _objectStack = new Stack<T>(10);

		public static void warmCache(int howMany = 3)
		{
			howMany -= _objectStack.Count;
			if (howMany > 0)
			{
				for (int i = 0; i < howMany; i++)
				{
					_objectStack.Push(new T());
				}
			}
		}

		public static T pop()
		{
			if (_objectStack.Count > 0)
			{
				return _objectStack.Pop();
			}
			return new T();
		}

		public static void push(T obj)
		{
			_objectStack.Push(obj);
		}
	}
}
