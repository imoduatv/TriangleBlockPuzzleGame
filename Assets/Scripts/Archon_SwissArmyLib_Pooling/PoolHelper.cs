using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Archon.SwissArmyLib.Pooling
{
	public static class PoolHelper
	{
		private static readonly Dictionary<UnityEngine.Object, GameObjectPool<UnityEngine.Object>> PrefabToPool;

		private static readonly Dictionary<UnityEngine.Object, UnityEngine.Object> InstanceToPrefab;

		private static readonly List<UnityEngine.Object> DestroyedInstances;

		[CompilerGenerated]
		private static UnityAction<Scene> _003C_003Ef__mg_0024cache0;

		static PoolHelper()
		{
			PrefabToPool = new Dictionary<UnityEngine.Object, GameObjectPool<UnityEngine.Object>>();
			InstanceToPrefab = new Dictionary<UnityEngine.Object, UnityEngine.Object>();
			DestroyedInstances = new List<UnityEngine.Object>();
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		private static void OnSceneUnloaded(Scene unloadedScene)
		{
			foreach (UnityEngine.Object key in InstanceToPrefab.Keys)
			{
				if (!key)
				{
					DestroyedInstances.Add(key);
				}
			}
			for (int i = 0; i < DestroyedInstances.Count; i++)
			{
				InstanceToPrefab.Remove(DestroyedInstances[i]);
			}
			DestroyedInstances.Clear();
		}

		public static T Spawn<T>(T prefab) where T : UnityEngine.Object
		{
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentNullException("prefab");
			}
			GameObjectPool<UnityEngine.Object> pool = GetPool(prefab);
			UnityEngine.Object @object = pool.Spawn();
			InstanceToPrefab[@object] = prefab;
			return @object as T;
		}

		public static T Spawn<T>(T prefab, Transform parent) where T : UnityEngine.Object
		{
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentNullException("prefab");
			}
			GameObjectPool<UnityEngine.Object> pool = GetPool(prefab);
			UnityEngine.Object @object = pool.Spawn(parent);
			InstanceToPrefab[@object] = prefab;
			return @object as T;
		}

		public static T Spawn<T>(T prefab, Vector3 position) where T : UnityEngine.Object
		{
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentNullException("prefab");
			}
			GameObjectPool<UnityEngine.Object> pool = GetPool(prefab);
			UnityEngine.Object @object = pool.Spawn(position);
			InstanceToPrefab[@object] = prefab;
			return @object as T;
		}

		public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : UnityEngine.Object
		{
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentNullException("prefab");
			}
			GameObjectPool<UnityEngine.Object> pool = GetPool(prefab);
			UnityEngine.Object @object = pool.Spawn(position, rotation);
			InstanceToPrefab[@object] = prefab;
			return @object as T;
		}

		public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : UnityEngine.Object
		{
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentNullException("prefab");
			}
			GameObjectPool<UnityEngine.Object> pool = GetPool(prefab);
			UnityEngine.Object @object = pool.Spawn(position, rotation, parent);
			InstanceToPrefab[@object] = prefab;
			return @object as T;
		}

		public static void Despawn(UnityEngine.Object target)
		{
			if (object.ReferenceEquals(target, null))
			{
				throw new ArgumentNullException("target");
			}
			UnityEngine.Object prefab = GetPrefab(target);
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentException("Cannot find prefab for target.");
			}
			GameObjectPool<UnityEngine.Object> pool = GetPool(prefab);
			pool.Despawn(target);
		}

		public static void Despawn(UnityEngine.Object target, float delay, bool unscaledTime = false)
		{
			if (object.ReferenceEquals(target, null))
			{
				throw new ArgumentNullException("target");
			}
			UnityEngine.Object prefab = GetPrefab(target);
			GameObjectPool<UnityEngine.Object> pool = GetPool(prefab);
			pool.Despawn(target, delay, unscaledTime);
		}

		public static int GetFreeCount(UnityEngine.Object prefab)
		{
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentNullException("prefab");
			}
			GameObjectPool<UnityEngine.Object> pool = GetPool(prefab);
			return (pool != null) ? pool.FreeCount : 0;
		}

		public static UnityEngine.Object GetPrefab(UnityEngine.Object instance)
		{
			if (object.ReferenceEquals(instance, null))
			{
				throw new ArgumentNullException("instance");
			}
			InstanceToPrefab.TryGetValue(instance, out UnityEngine.Object value);
			return value;
		}

		public static T GetPrefab<T>(T instance) where T : UnityEngine.Object
		{
			if (object.ReferenceEquals(instance, null))
			{
				throw new ArgumentNullException("instance");
			}
			InstanceToPrefab.TryGetValue(instance, out UnityEngine.Object value);
			return value as T;
		}

		public static GameObjectPool<UnityEngine.Object> GetPool(UnityEngine.Object prefab)
		{
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentNullException("prefab");
			}
			if (!PrefabToPool.TryGetValue(prefab, out GameObjectPool<UnityEngine.Object> value))
			{
				value = new GameObjectPool<UnityEngine.Object>(prefab, multiScene: true);
				PrefabToPool[prefab] = value;
			}
			return value;
		}
	}
	public static class PoolHelper<T> where T : class, new()
	{
		private static readonly Pool<T> Pool = new Pool<T>(() => new T());

		public static int FreeCount => Pool.FreeCount;

		public static T Spawn()
		{
			return Pool.Spawn();
		}

		public static void Despawn(T target)
		{
			Pool.Despawn(target);
		}

		public static void Despawn(T target, float delay, bool unscaledTime = false)
		{
			Pool.Despawn(target, delay, unscaledTime);
		}
	}
}
