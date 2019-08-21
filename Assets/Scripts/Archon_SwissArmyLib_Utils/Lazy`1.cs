using System;
using System.Runtime.CompilerServices;

namespace Archon.SwissArmyLib.Utils
{
	public class Lazy<T>
	{
		private T _value;

		private readonly Func<T> _valueFactory;

		private bool _isValueCreated;

		[CompilerGenerated]
		private static Func<T> _003C_003Ef__mg_0024cache0;

		public T Value
		{
			get
			{
				if (!_isValueCreated)
				{
					Initialize();
				}
				return _value;
			}
		}

		public bool IsValueCreated => _isValueCreated;

		public Lazy()
			: this((Func<T>)Activator.CreateInstance<T>)
		{
		}

		public Lazy(Func<T> valueFactory)
		{
			if (object.ReferenceEquals(valueFactory, null))
			{
				throw new ArgumentNullException("valueFactory");
			}
			_valueFactory = valueFactory;
		}

		private void Initialize()
		{
			_value = _valueFactory();
			_isValueCreated = true;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static explicit operator T(Lazy<T> lazy)
		{
			return lazy.Value;
		}
	}
}
