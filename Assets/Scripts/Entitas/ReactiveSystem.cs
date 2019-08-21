using System.Collections.Generic;

namespace Entitas
{
	public class ReactiveSystem : IExecuteSystem, ISystem
	{
		private readonly IReactiveExecuteSystem _subsystem;

		private readonly GroupObserver _observer;

		private readonly IMatcher _ensureComponents;

		private readonly IMatcher _excludeComponents;

		private readonly bool _clearAfterExecute;

		private readonly List<Entity> _buffer;

		private string _toStringCache;

		public IReactiveExecuteSystem subsystem => _subsystem;

		public ReactiveSystem(Pool pool, IReactiveSystem subSystem)
			: this(subSystem, createGroupObserver(pool, new TriggerOnEvent[1]
			{
				subSystem.trigger
			}))
		{
		}

		public ReactiveSystem(Pool pool, IMultiReactiveSystem subSystem)
			: this(subSystem, createGroupObserver(pool, subSystem.triggers))
		{
		}

		public ReactiveSystem(IGroupObserverSystem subSystem)
			: this(subSystem, subSystem.groupObserver)
		{
		}

		private ReactiveSystem(IReactiveExecuteSystem subSystem, GroupObserver groupObserver)
		{
			_subsystem = subSystem;
			IEnsureComponents ensureComponents = subSystem as IEnsureComponents;
			if (ensureComponents != null)
			{
				_ensureComponents = ensureComponents.ensureComponents;
			}
			IExcludeComponents excludeComponents = subSystem as IExcludeComponents;
			if (excludeComponents != null)
			{
				_excludeComponents = excludeComponents.excludeComponents;
			}
			_clearAfterExecute = (subSystem is IClearReactiveSystem);
			_observer = groupObserver;
			_buffer = new List<Entity>();
		}

		private static GroupObserver createGroupObserver(Pool pool, TriggerOnEvent[] triggers)
		{
			int num = triggers.Length;
			Group[] array = new Group[num];
			GroupEventType[] array2 = new GroupEventType[num];
			for (int i = 0; i < num; i++)
			{
				TriggerOnEvent triggerOnEvent = triggers[i];
				array[i] = pool.GetGroup(triggerOnEvent.trigger);
				array2[i] = triggerOnEvent.eventType;
			}
			return new GroupObserver(array, array2);
		}

		public void Activate()
		{
			_observer.Activate();
		}

		public void Deactivate()
		{
			_observer.Deactivate();
		}

		public void Clear()
		{
			_observer.ClearCollectedEntities();
		}

		public void Execute()
		{
			if (_observer.collectedEntities.Count == 0)
			{
				return;
			}
			if (_ensureComponents != null)
			{
				if (_excludeComponents != null)
				{
					foreach (Entity collectedEntity in _observer.collectedEntities)
					{
						if (_ensureComponents.Matches(collectedEntity) && !_excludeComponents.Matches(collectedEntity))
						{
							_buffer.Add(collectedEntity.Retain(this));
						}
					}
				}
				else
				{
					foreach (Entity collectedEntity2 in _observer.collectedEntities)
					{
						if (_ensureComponents.Matches(collectedEntity2))
						{
							_buffer.Add(collectedEntity2.Retain(this));
						}
					}
				}
			}
			else if (_excludeComponents != null)
			{
				foreach (Entity collectedEntity3 in _observer.collectedEntities)
				{
					if (!_excludeComponents.Matches(collectedEntity3))
					{
						_buffer.Add(collectedEntity3.Retain(this));
					}
				}
			}
			else
			{
				foreach (Entity collectedEntity4 in _observer.collectedEntities)
				{
					_buffer.Add(collectedEntity4.Retain(this));
				}
			}
			_observer.ClearCollectedEntities();
			if (_buffer.Count != 0)
			{
				_subsystem.Execute(_buffer);
				int i = 0;
				for (int count = _buffer.Count; i < count; i++)
				{
					_buffer[i].Release(this);
				}
				_buffer.Clear();
				if (_clearAfterExecute)
				{
					_observer.ClearCollectedEntities();
				}
			}
		}

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				_toStringCache = "ReactiveSystem(" + subsystem + ")";
			}
			return _toStringCache;
		}

		~ReactiveSystem()
		{
			Deactivate();
		}
	}
}
