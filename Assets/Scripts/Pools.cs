using Entitas;

public static class Pools
{
	private static Pool[] _allPools;

	private static Pool _pool;

	public static Pool[] allPools
	{
		get
		{
			if (_allPools == null)
			{
				_allPools = new Pool[1]
				{
					pool
				};
			}
			return _allPools;
		}
	}

	public static Pool pool
	{
		get
		{
			if (_pool == null)
			{
				_pool = new Pool(34, 0, new PoolMetaData("Pool", ComponentIds.componentNames, ComponentIds.componentTypes));
			}
			return _pool;
		}
	}
}
