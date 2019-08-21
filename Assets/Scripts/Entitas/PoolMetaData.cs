using System;

namespace Entitas
{
	public class PoolMetaData
	{
		private readonly string _poolName;

		private readonly string[] _componentNames;

		private readonly Type[] _componentTypes;

		public string poolName => _poolName;

		public string[] componentNames => _componentNames;

		public Type[] componentTypes => _componentTypes;

		public PoolMetaData(string poolName, string[] componentNames, Type[] componentTypes)
		{
			_poolName = poolName;
			_componentNames = componentNames;
			_componentTypes = componentTypes;
		}
	}
}
