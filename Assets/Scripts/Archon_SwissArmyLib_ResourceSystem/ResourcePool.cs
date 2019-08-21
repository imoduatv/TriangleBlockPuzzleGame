using Archon.SwissArmyLib.Events;
using Archon.SwissArmyLib.Pooling;
using Archon.SwissArmyLib.Utils;
using UnityEngine;

namespace Archon.SwissArmyLib.ResourceSystem
{
	public class ResourcePool : ResourcePool<object, object>
	{
	}
	public class ResourcePool<TSource, TArgs> : ResourcePoolBase
	{
		public static class EventIds
		{
			public const int PreChange = -8000;

			public const int Change = -8001;

			public const int Empty = -8002;

			public const int Full = -8003;

			public const int Renew = -8004;
		}

		public readonly Event<IResourcePreChangeEvent<TSource, TArgs>> OnPreChange = new Event<IResourcePreChangeEvent<TSource, TArgs>>(-8000);

		public readonly Event<IResourceChangeEvent<TSource, TArgs>> OnChange = new Event<IResourceChangeEvent<TSource, TArgs>>(-8001);

		public readonly Event<IResourceEvent<TSource, TArgs>> OnEmpty = new Event<IResourceEvent<TSource, TArgs>>(-8002);

		public readonly Event<IResourceEvent<TSource, TArgs>> OnFull = new Event<IResourceEvent<TSource, TArgs>>(-8003);

		public readonly Event<IResourceEvent<TSource, TArgs>> OnRenew = new Event<IResourceEvent<TSource, TArgs>>(-8004);

		[Tooltip("Current amount of resource in the pool.")]
		[SerializeField]
		private float _current;

		[Tooltip("Max amount of resource that can be in the pool.")]
		[SerializeField]
		private float _max = 100f;

		[Tooltip("Whether the pool should remain empty until it is renewed using Renew().")]
		[SerializeField]
		private bool _emptyTillRenewed = true;

		private bool _isEmpty;

		private bool _isFull;

		private float _timeEmptied;

		public override float Current
		{
			get
			{
				return _current;
			}
			protected set
			{
				_current = Mathf.Clamp(value, 0f, Max);
			}
		}

		public override float Max
		{
			get
			{
				return _max;
			}
			set
			{
				_max = value;
			}
		}

		public override bool EmptyTillRenewed
		{
			get
			{
				return _emptyTillRenewed;
			}
			set
			{
				_emptyTillRenewed = value;
			}
		}

		public override float Percentage => Current / Max;

		public override bool IsEmpty => _isEmpty;

		public override bool IsFull => _isFull;

		public override float TimeSinceEmpty
		{
			get
			{
				if (_isEmpty)
				{
					return 0f;
				}
				return BetterTime.Time - _timeEmptied;
			}
		}

		public virtual TSource DefaultSource => default(TSource);

		public virtual TArgs DefaultArgs => default(TArgs);

		protected virtual void Awake()
		{
			_current = Max;
		}

		public override float Add(float amount, bool forced = false)
		{
			return Change(amount, DefaultSource, DefaultArgs, forced);
		}

		public float Add(float amount, TSource source, bool forced = false)
		{
			return Change(amount, source, DefaultArgs, forced);
		}

		public float Add(float amount, TSource source, TArgs args, bool forced = false)
		{
			return Change(amount, source, args, forced);
		}

		public override float Remove(float amount, bool forced = false)
		{
			return 0f - Change(0f - amount, DefaultSource, DefaultArgs, forced);
		}

		public float Remove(float amount, TSource source, bool forced = false)
		{
			return 0f - Change(0f - amount, source, DefaultArgs, forced);
		}

		public float Remove(float amount, TSource source, TArgs args, bool forced = false)
		{
			return 0f - Change(0f - amount, source, args, forced);
		}

		public override float Empty(bool forced = false)
		{
			return Remove(float.MaxValue, DefaultSource, DefaultArgs, forced);
		}

		public float Empty(TSource source, bool forced = false)
		{
			return Remove(float.MaxValue, source, DefaultArgs, forced);
		}

		public float Empty(TSource source, TArgs args, bool forced = false)
		{
			return Remove(float.MaxValue, source, args, forced);
		}

		public override float Fill(bool forced = false)
		{
			return Fill(float.MaxValue, DefaultSource, DefaultArgs, forced);
		}

		public float Fill(TSource source, bool forced = false)
		{
			return Fill(float.MaxValue, source, DefaultArgs, forced);
		}

		public float Fill(TSource source, TArgs args, bool forced = false)
		{
			return Fill(float.MaxValue, source, args, forced);
		}

		public override float Fill(float toValue, bool forced = false)
		{
			return Change(toValue - Current, DefaultSource, DefaultArgs, forced);
		}

		public float Fill(float toValue, TSource source, bool forced = false)
		{
			return Change(toValue - Current, source, DefaultArgs, forced);
		}

		public float Fill(float toValue, TSource source, TArgs args, bool forced = false)
		{
			return Change(toValue - Current, source, args, forced);
		}

		public override float Renew(bool forced = false)
		{
			return Renew(float.MaxValue, DefaultSource, DefaultArgs, forced);
		}

		public float Renew(TSource source, bool forced = false)
		{
			return Renew(float.MaxValue, source, DefaultArgs, forced);
		}

		public float Renew(TSource source, TArgs args, bool forced = false)
		{
			return Renew(float.MaxValue, source, args, forced);
		}

		public override float Renew(float toValue, bool forced = false)
		{
			return Renew(toValue, DefaultSource, DefaultArgs, forced);
		}

		public float Renew(float toValue, TSource source, bool forced = false)
		{
			return Renew(toValue, source, DefaultArgs, forced);
		}

		public float Renew(float toValue, TSource source, TArgs args, bool forced = false)
		{
			bool emptyTillRenewed = _emptyTillRenewed;
			_emptyTillRenewed = false;
			float result = Fill(toValue, source, args, forced);
			_emptyTillRenewed = emptyTillRenewed;
			ResourceEvent<TSource, TArgs> resourceEvent = PoolHelper<ResourceEvent<TSource, TArgs>>.Spawn();
			resourceEvent.Source = source;
			resourceEvent.Args = args;
			OnRenew.Invoke(resourceEvent);
			PoolHelper<ResourceEvent<TSource, TArgs>>.Despawn(resourceEvent);
			return result;
		}

		protected virtual float Change(float delta, TSource source, TArgs args, bool forced = false)
		{
			if (_isEmpty && _emptyTillRenewed)
			{
				return 0f;
			}
			ResourceEvent<TSource, TArgs> resourceEvent = PoolHelper<ResourceEvent<TSource, TArgs>>.Spawn();
			resourceEvent.OriginalDelta = delta;
			resourceEvent.ModifiedDelta = delta;
			resourceEvent.Source = source;
			resourceEvent.Args = args;
			OnPreChange.Invoke(resourceEvent);
			if (forced)
			{
				resourceEvent.ModifiedDelta = resourceEvent.OriginalDelta;
			}
			if (Mathf.Approximately(resourceEvent.ModifiedDelta, 0f))
			{
				PoolHelper<ResourceEvent<TSource, TArgs>>.Despawn(resourceEvent);
				return 0f;
			}
			float current = _current;
			Current += resourceEvent.ModifiedDelta;
			resourceEvent.AppliedDelta = _current - current;
			bool isEmpty = _isEmpty;
			_isEmpty = (_current < 0.01f);
			bool isFull = _isFull;
			_isFull = (_current > _max - 0.01f);
			OnChange.Invoke(resourceEvent);
			if (_isEmpty && _isEmpty != isEmpty)
			{
				_timeEmptied = BetterTime.Time;
				OnEmpty.Invoke(resourceEvent);
			}
			if (_isFull && _isFull != isFull)
			{
				OnFull.Invoke(resourceEvent);
			}
			float appliedDelta = resourceEvent.AppliedDelta;
			PoolHelper<ResourceEvent<TSource, TArgs>>.Despawn(resourceEvent);
			return appliedDelta;
		}
	}
}
