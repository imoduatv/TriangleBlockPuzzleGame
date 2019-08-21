using System;

namespace Entitas
{
	public static class PoolExtension
	{
		public static Entity[] GetEntities(this Pool pool, IMatcher matcher)
		{
			return pool.GetGroup(matcher).GetEntities();
		}

		public static ISystem CreateSystem<T>(this Pool pool) where T : ISystem, new()
		{
			return CreateSystem(pool, typeof(T));
		}

		public static ISystem CreateSystem(this Pool pool, Type systemType)
		{
			ISystem system = (ISystem)Activator.CreateInstance(systemType);
			return pool.CreateSystem(system);
		}

		public static ISystem CreateSystem(this Pool pool, ISystem system)
		{
			(system as ISetPool)?.SetPool(pool);
			IReactiveSystem reactiveSystem = system as IReactiveSystem;
			if (reactiveSystem != null)
			{
				return new ReactiveSystem(pool, reactiveSystem);
			}
			IMultiReactiveSystem multiReactiveSystem = system as IMultiReactiveSystem;
			if (multiReactiveSystem != null)
			{
				return new ReactiveSystem(pool, multiReactiveSystem);
			}
			return system;
		}

		public static GroupObserver CreateGroupObserver(this Pool[] pools, IMatcher matcher, GroupEventType eventType = GroupEventType.OnEntityAdded)
		{
			Group[] array = new Group[pools.Length];
			GroupEventType[] array2 = new GroupEventType[pools.Length];
			int i = 0;
			for (int num = pools.Length; i < num; i++)
			{
				array[i] = pools[i].GetGroup(matcher);
				array2[i] = eventType;
			}
			return new GroupObserver(array, array2);
		}
	}
}
