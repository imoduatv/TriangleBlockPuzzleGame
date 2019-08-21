using System;

namespace Entitas.Unity.VisualDebugging
{
	public class SystemInfo
	{
		[Flags]
		private enum SystemInterfaceFlags
		{
			None = 0x0,
			IInitializeSystem = 0x1,
			IExecuteSystem = 0x2,
			IReactiveSystem = 0x4
		}

		public bool isActive;

		private readonly ISystem _system;

		private readonly SystemInterfaceFlags _interfaceFlags;

		private readonly string _systemName;

		private double _accumulatedExecutionDuration;

		private double _minExecutionDuration;

		private double _maxExecutionDuration;

		private int _durationsCount;

		private const string SYSTEM_SUFFIX = "System";

		public ISystem system => _system;

		public string systemName => _systemName;

		public bool isInitializeSystems => (_interfaceFlags & SystemInterfaceFlags.IInitializeSystem) == SystemInterfaceFlags.IInitializeSystem;

		public bool isExecuteSystems => (_interfaceFlags & SystemInterfaceFlags.IExecuteSystem) == SystemInterfaceFlags.IExecuteSystem;

		public bool isReactiveSystems => (_interfaceFlags & SystemInterfaceFlags.IReactiveSystem) == SystemInterfaceFlags.IReactiveSystem;

		public double accumulatedExecutionDuration => _accumulatedExecutionDuration;

		public double minExecutionDuration => _minExecutionDuration;

		public double maxExecutionDuration => _maxExecutionDuration;

		public double averageExecutionDuration => (_durationsCount != 0) ? (_accumulatedExecutionDuration / (double)_durationsCount) : 0.0;

		public SystemInfo(ISystem system)
		{
			_system = system;
			ReactiveSystem reactiveSystem = system as ReactiveSystem;
			bool flag = reactiveSystem != null;
			Type type;
			if (flag)
			{
				_interfaceFlags = getInterfaceFlags(reactiveSystem.subsystem, flag);
				type = reactiveSystem.subsystem.GetType();
			}
			else
			{
				_interfaceFlags = getInterfaceFlags(system, flag);
				type = system.GetType();
			}
			DebugSystems debugSystems = system as DebugSystems;
			if (debugSystems != null)
			{
				_systemName = debugSystems.name;
			}
			else
			{
				_systemName = ((!type.Name.EndsWith("System", StringComparison.Ordinal)) ? type.Name : type.Name.Substring(0, type.Name.Length - "System".Length));
			}
			isActive = true;
		}

		public void AddExecutionDuration(double executionDuration)
		{
			if (executionDuration < _minExecutionDuration || _minExecutionDuration == 0.0)
			{
				_minExecutionDuration = executionDuration;
			}
			if (executionDuration > _maxExecutionDuration)
			{
				_maxExecutionDuration = executionDuration;
			}
			_accumulatedExecutionDuration += executionDuration;
			_durationsCount++;
		}

		public void ResetDurations()
		{
			_accumulatedExecutionDuration = 0.0;
			_durationsCount = 0;
		}

		private static SystemInterfaceFlags getInterfaceFlags(ISystem system, bool isReactive)
		{
			SystemInterfaceFlags systemInterfaceFlags = SystemInterfaceFlags.None;
			if (system is IInitializeSystem)
			{
				systemInterfaceFlags |= SystemInterfaceFlags.IInitializeSystem;
			}
			if (system is IExecuteSystem)
			{
				systemInterfaceFlags |= SystemInterfaceFlags.IExecuteSystem;
			}
			if (isReactive)
			{
				systemInterfaceFlags |= SystemInterfaceFlags.IReactiveSystem;
			}
			return systemInterfaceFlags;
		}
	}
}
