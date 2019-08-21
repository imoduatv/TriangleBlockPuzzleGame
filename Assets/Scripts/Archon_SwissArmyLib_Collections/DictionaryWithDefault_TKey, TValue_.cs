using System.Collections.Generic;

namespace Archon.SwissArmyLib.Collections
{
	public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public TValue DefaultValue
		{
			get;
			set;
		}

		public new TValue this[TKey key]
		{
			get
			{
				TValue value;
				return (!TryGetValue(key, out value)) ? DefaultValue : value;
			}
			set
			{
				base[key] = value;
			}
		}

		public DictionaryWithDefault()
		{
		}

		public DictionaryWithDefault(TValue defaultValue)
		{
			DefaultValue = defaultValue;
		}

		public DictionaryWithDefault(TValue defaultValue, IEqualityComparer<TKey> comparer)
			: base(comparer)
		{
			DefaultValue = defaultValue;
		}
	}
}