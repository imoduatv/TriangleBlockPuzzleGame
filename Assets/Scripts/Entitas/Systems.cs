using System;
using System.Collections.Generic;

namespace Entitas
{
	public class Systems : IInitializeSystem, IExecuteSystem, ISystem
	{
		protected readonly List<IInitializeSystem> _initializeSystems;

		protected readonly List<IExecuteSystem> _executeSystems;

		public Systems()
		{
			_initializeSystems = new List<IInitializeSystem>();
			_executeSystems = new List<IExecuteSystem>();
		}

		public virtual Systems Add<T>()
		{
			return Add(typeof(T));
		}

		public virtual Systems Add(Type systemType)
		{
			return Add((ISystem)Activator.CreateInstance(systemType));
		}

		public virtual Systems Add(ISystem system)
		{
			ReactiveSystem reactiveSystem = system as ReactiveSystem;
			IInitializeSystem initializeSystem = (reactiveSystem == null) ? (system as IInitializeSystem) : (reactiveSystem.subsystem as IInitializeSystem);
			if (initializeSystem != null)
			{
				_initializeSystems.Add(initializeSystem);
			}
			IExecuteSystem executeSystem = system as IExecuteSystem;
			if (executeSystem != null)
			{
				_executeSystems.Add(executeSystem);
			}
			return this;
		}

		public virtual void Initialize()
		{
			int i = 0;
			for (int count = _initializeSystems.Count; i < count; i++)
			{
				_initializeSystems[i].Initialize();
			}
		}

		public virtual void Execute()
		{
			int i = 0;
			for (int count = _executeSystems.Count; i < count; i++)
			{
				_executeSystems[i].Execute();
			}
		}

		public virtual void ActivateReactiveSystems()
		{
			int i = 0;
			for (int count = _executeSystems.Count; i < count; i++)
			{
				(_executeSystems[i] as ReactiveSystem)?.Activate();
				(_executeSystems[i] as Systems)?.ActivateReactiveSystems();
			}
		}

		public virtual void DeactivateReactiveSystems()
		{
			int i = 0;
			for (int count = _executeSystems.Count; i < count; i++)
			{
				(_executeSystems[i] as ReactiveSystem)?.Deactivate();
				(_executeSystems[i] as Systems)?.DeactivateReactiveSystems();
			}
		}

		public virtual void ClearReactiveSystems()
		{
			int i = 0;
			for (int count = _executeSystems.Count; i < count; i++)
			{
				(_executeSystems[i] as ReactiveSystem)?.Clear();
				(_executeSystems[i] as Systems)?.ClearReactiveSystems();
			}
		}
	}
}
