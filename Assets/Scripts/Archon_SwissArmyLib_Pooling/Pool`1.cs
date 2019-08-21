using Archon.SwissArmyLib.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Pooling
{
	public class Pool<T> : IPool<T>, TellMeWhen.ITimerCallback where T : class
	{
		protected readonly List<T> Free = new List<T>();

		private readonly Func<T> _factory;

		private int _nextTimerId;

		private readonly Dictionary<T, int> _instanceToTimerId = new Dictionary<T, int>();

		public int FreeCount => Free.Count;

		public Pool(Func<T> create)
		{
			if (object.ReferenceEquals(create, null))
			{
				throw new ArgumentNullException("create");
			}
			_factory = create;
		}

		public void Prewarm(int targetCount)
		{
			if (Free.Capacity < targetCount)
			{
				Free.Capacity = targetCount;
			}
			for (int i = 0; i < targetCount; i++)
			{
				if (Free.Count >= targetCount)
				{
					break;
				}
				Free.Add(_factory());
			}
		}

		public T Spawn()
		{
			T val = SpawnInternal();
			OnSpawned(val);
			return val;
		}

		protected virtual T SpawnInternal()
		{
			T result;
			if (Free.Count > 0)
			{
				result = Free[Free.Count - 1];
				Free.RemoveAt(Free.Count - 1);
			}
			else
			{
				result = _factory();
			}
			return result;
		}

		public virtual void Despawn(T target)
		{
			if (object.ReferenceEquals(target, null))
			{
				throw new ArgumentNullException("target");
			}
			if (Application.isEditor && Free.Contains(target))
			{
				throw new ArgumentException("Target is already despawned!", "target");
			}
			_instanceToTimerId.Remove(target);
			OnDespawned(target);
			Free.Add(target);
		}

		protected virtual void OnSpawned(T target)
		{
			(target as IPoolable)?.OnSpawned();
		}

		protected virtual void OnDespawned(T target)
		{
			(target as IPoolable)?.OnDespawned();
		}

		public void Despawn(T target, float delay, bool unscaledTime = false)
		{
			if (object.ReferenceEquals(target, null))
			{
				throw new ArgumentNullException("target");
			}
			int num = _nextTimerId++;
			_instanceToTimerId[target] = num;
			if (unscaledTime)
			{
				TellMeWhen.SecondsUnscaled(delay, this, num, target);
			}
			else
			{
				TellMeWhen.Seconds(delay, this, num, target);
			}
		}

		public void CancelDespawn(T target)
		{
			if (object.ReferenceEquals(target, null))
			{
				throw new ArgumentNullException("target");
			}
			_instanceToTimerId.Remove(target);
		}

		void TellMeWhen.ITimerCallback.OnTimesUp(int id, object args)
		{
			T val = args as T;
			int value;
			if (val != null && _instanceToTimerId.TryGetValue(val, out value) && value == id)
			{
				Despawn(val);
			}
		}
	}
}
