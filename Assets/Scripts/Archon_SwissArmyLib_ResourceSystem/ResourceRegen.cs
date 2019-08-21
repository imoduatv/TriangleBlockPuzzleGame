using Archon.SwissArmyLib.Events;
using Archon.SwissArmyLib.Utils;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Archon.SwissArmyLib.ResourceSystem
{
	public class ResourceRegen : ResourceRegen<object, object>
	{
		[Tooltip("The target resource pool that should regen.")]
		[SerializeField]
		private ResourcePool _target;

		protected override void Awake()
		{
			base.Target = _target;
			base.Awake();
		}
	}
	public class ResourceRegen<TSource, TArgs> : MonoBehaviour, IEventListener<IResourceChangeEvent<TSource, TArgs>>
	{
		[Tooltip("Time in seconds that regen should be paused when the target loses resource.")]
		[SerializeField]
		private float _downTimeOnResourceLoss;

		[Tooltip("Amount of resource that should be gained per second.")]
		[SerializeField]
		private float _constantAmountPerSecond;

		[Tooltip("Amount of resource that should be gained every interval.")]
		[SerializeField]
		private float _amountPerInterval;

		[Tooltip("How often in seconds that resource should be gained.")]
		[SerializeField]
		private float _interval;

		private ResourcePool<TSource, TArgs> _target;

		private float _lastInterval;

		private float _lastLossTime;

		public float Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				_interval = value;
			}
		}

		public float AmountPerInterval
		{
			get
			{
				return _amountPerInterval;
			}
			set
			{
				_amountPerInterval = value;
			}
		}

		public float ConstantAmountPerSecond
		{
			get
			{
				return _constantAmountPerSecond;
			}
			set
			{
				_constantAmountPerSecond = value;
			}
		}

		public float DownTimeOnResourceLoss
		{
			get
			{
				return _downTimeOnResourceLoss;
			}
			set
			{
				_downTimeOnResourceLoss = value;
			}
		}

		public ResourcePool<TSource, TArgs> Target
		{
			get
			{
				return _target;
			}
			set
			{
				if (_target != null && base.enabled)
				{
					_target.OnChange.RemoveListener(this);
				}
				_target = value;
				if (_target != null && base.enabled)
				{
					_target.OnChange.AddListener(this);
				}
			}
		}

		[UsedImplicitly]
		protected virtual void Awake()
		{
			if (_target == null)
			{
				_target = GetComponent<ResourcePool<TSource, TArgs>>();
			}
		}

		[UsedImplicitly]
		protected void OnEnable()
		{
			if (_target != null)
			{
				_target.OnChange.AddListener(this);
			}
		}

		[UsedImplicitly]
		protected void OnDisable()
		{
			if (_target != null)
			{
				_target.OnChange.RemoveListener(this);
			}
		}

		[UsedImplicitly]
		protected void Update()
		{
			if (_target == null)
			{
				return;
			}
			float time = BetterTime.Time;
			if (!(time < _lastLossTime + DownTimeOnResourceLoss))
			{
				if (Math.Abs(ConstantAmountPerSecond) > 0.001f)
				{
					_target.Add(ConstantAmountPerSecond * BetterTime.DeltaTime);
				}
				if (Interval > 0f && time > _lastInterval + Interval)
				{
					_target.Add(AmountPerInterval);
					_lastInterval = time;
				}
			}
		}

		void IEventListener<IResourceChangeEvent<TSource, TArgs>>.OnEvent(int eventId, IResourceChangeEvent<TSource, TArgs> args)
		{
			if (args.AppliedDelta < 0f)
			{
				_lastLossTime = BetterTime.Time;
			}
		}
	}
}
