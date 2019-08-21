using Archon.SwissArmyLib.Events;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.ResourceSystem
{
	public class Shield : Shield<object, object>
	{
		[Tooltip("The target resource pool that should be protected.")]
		[SerializeField]
		private ResourcePool _protectedTarget;

		protected override void Awake()
		{
			base.ProtectedTarget = _protectedTarget;
			base.Awake();
		}
	}
	public class Shield<TSource, TArgs> : ResourcePool<TSource, TArgs>, IEventListener<IResourcePreChangeEvent<TSource, TArgs>>, IEventListener<IResourceEvent<TSource, TArgs>>
	{
		private static readonly List<ResourcePool<TSource, TArgs>> GetComponentResults = new List<ResourcePool<TSource, TArgs>>();

		[Tooltip("Flat amount of removed resource that should be absorbed.")]
		[SerializeField]
		private float _absorptionFlat;

		[Tooltip("Fraction of removed resource that should be absorbed by the shield.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _absorptionScaling = 0.5f;

		[Tooltip("Whether the shield should get drained when the target is empty.")]
		[SerializeField]
		private bool _emptiesWithTarget = true;

		[Tooltip("Whether the shield should renew when the target does.")]
		[SerializeField]
		private bool _renewsWithTarget = true;

		private ResourcePool<TSource, TArgs> _protectedTarget;

		public ResourcePool<TSource, TArgs> ProtectedTarget
		{
			get
			{
				return _protectedTarget;
			}
			set
			{
				if (_protectedTarget != null && base.enabled)
				{
					_protectedTarget.OnPreChange.RemoveListener(this);
					_protectedTarget.OnEmpty.RemoveListener(this);
				}
				_protectedTarget = value;
				if (_protectedTarget != null && base.enabled)
				{
					_protectedTarget.OnPreChange.AddListener(this);
					_protectedTarget.OnEmpty.AddListener(this);
				}
			}
		}

		public float AbsorptionFlat
		{
			get
			{
				return _absorptionFlat;
			}
			set
			{
				_absorptionFlat = value;
			}
		}

		public float AbsorptionScaling
		{
			get
			{
				return _absorptionScaling;
			}
			set
			{
				_absorptionScaling = value;
			}
		}

		public bool EmptiesWithTarget
		{
			get
			{
				return _emptiesWithTarget;
			}
			set
			{
				_emptiesWithTarget = value;
			}
		}

		public bool RenewsWithTarget
		{
			get
			{
				return _renewsWithTarget;
			}
			set
			{
				_renewsWithTarget = value;
			}
		}

		[UsedImplicitly]
		protected override void Awake()
		{
			base.Awake();
			if (_protectedTarget == null)
			{
				_protectedTarget = GetDefaultTarget();
			}
		}

		[UsedImplicitly]
		protected void OnEnable()
		{
			if (_protectedTarget != null)
			{
				_protectedTarget.OnPreChange.AddListener(this);
				_protectedTarget.OnEmpty.AddListener(this);
				_protectedTarget.OnRenew.AddListener(this);
			}
		}

		[UsedImplicitly]
		protected void OnDisable()
		{
			if (_protectedTarget != null)
			{
				_protectedTarget.OnPreChange.RemoveListener(this);
				_protectedTarget.OnEmpty.RemoveListener(this);
				_protectedTarget.OnRenew.RemoveListener(this);
			}
		}

		protected override float Change(float delta, TSource source = default(TSource), TArgs args = default(TArgs), bool forced = false)
		{
			if (_emptiesWithTarget && ProtectedTarget.IsEmpty && !forced)
			{
				return 0f;
			}
			return base.Change(delta, source, args, forced);
		}

		public void OnEvent(int eventId, IResourcePreChangeEvent<TSource, TArgs> args)
		{
			if (args.ModifiedDelta < 0f && !IsEmpty)
			{
				float absorptionFlat = AbsorptionFlat;
				absorptionFlat += (0f - args.ModifiedDelta) * AbsorptionScaling;
				absorptionFlat = Mathf.Clamp(absorptionFlat, 0f, Mathf.Min(0f - args.ModifiedDelta, Current));
				args.ModifiedDelta += absorptionFlat;
				Remove(absorptionFlat, args.Source, args.Args);
			}
		}

		public void OnEvent(int eventId, IResourceEvent<TSource, TArgs> args)
		{
			switch (eventId)
			{
			case -8002:
				if (_emptiesWithTarget)
				{
					Empty(args.Source, args.Args, forced: true);
				}
				break;
			case -8004:
				if (_renewsWithTarget)
				{
					Renew(args.Source, args.Args, forced: true);
				}
				break;
			}
		}

		private ResourcePool<TSource, TArgs> GetDefaultTarget()
		{
			GetComponentsInChildren(GetComponentResults);
			for (int i = 0; i < GetComponentResults.Count; i++)
			{
				ResourcePool<TSource, TArgs> resourcePool = GetComponentResults[i];
				if (resourcePool != this)
				{
					return resourcePool;
				}
			}
			return null;
		}
	}
}
