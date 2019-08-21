using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Entitas.Unity.VisualDebugging
{
	public class DebugSystems : Systems
	{
		public static AvgResetInterval avgResetInterval = AvgResetInterval.Never;

		public bool paused;

		private readonly string _name;

		private readonly List<ISystem> _systems;

		private readonly Transform _container;

		private readonly List<SystemInfo> _initializeSystemInfos;

		private readonly List<SystemInfo> _executeSystemInfos;

		private readonly Stopwatch _stopwatch;

		private double _totalDuration;

		public int totalInitializeSystemsCount
		{
			get
			{
				int num = 0;
				foreach (IInitializeSystem initializeSystem in _initializeSystems)
				{
					DebugSystems debugSystems = initializeSystem as DebugSystems;
					num = ((debugSystems == null) ? (num + 1) : (num + debugSystems.totalInitializeSystemsCount));
				}
				return num;
			}
		}

		public int totalExecuteSystemsCount
		{
			get
			{
				int num = 0;
				foreach (IExecuteSystem executeSystem in _executeSystems)
				{
					DebugSystems debugSystems = executeSystem as DebugSystems;
					num = ((debugSystems == null) ? (num + 1) : (num + debugSystems.totalExecuteSystemsCount));
				}
				return num;
			}
		}

		public int initializeSystemsCount => _initializeSystems.Count;

		public int executeSystemsCount => _executeSystems.Count;

		public int totalSystemsCount => _systems.Count;

		public string name => _name;

		public GameObject container => _container.gameObject;

		public double totalDuration => _totalDuration;

		public SystemInfo[] initializeSystemInfos => _initializeSystemInfos.ToArray();

		public SystemInfo[] executeSystemInfos => _executeSystemInfos.ToArray();

		public DebugSystems(string name = "Systems")
		{
			_name = name;
			_systems = new List<ISystem>();
			_container = new GameObject().transform;
			_container.gameObject.AddComponent<DebugSystemsBehaviour>().Init(this);
			_initializeSystemInfos = new List<SystemInfo>();
			_executeSystemInfos = new List<SystemInfo>();
			_stopwatch = new Stopwatch();
			updateName();
		}

		public override Systems Add(ISystem system)
		{
			_systems.Add(system);
			(system as DebugSystems)?.container.transform.SetParent(_container.transform, worldPositionStays: false);
			SystemInfo systemInfo = new SystemInfo(system);
			if (systemInfo.isInitializeSystems)
			{
				_initializeSystemInfos.Add(systemInfo);
			}
			if (systemInfo.isExecuteSystems || systemInfo.isReactiveSystems)
			{
				_executeSystemInfos.Add(systemInfo);
			}
			return base.Add(system);
		}

		public void ResetDurations()
		{
			foreach (SystemInfo initializeSystemInfo in _initializeSystemInfos)
			{
				initializeSystemInfo.ResetDurations();
			}
			foreach (SystemInfo executeSystemInfo in _executeSystemInfos)
			{
				executeSystemInfo.ResetDurations();
				(executeSystemInfo.system as DebugSystems)?.ResetDurations();
			}
		}

		public override void Initialize()
		{
			_totalDuration = 0.0;
			int i = 0;
			for (int count = _initializeSystems.Count; i < count; i++)
			{
				IInitializeSystem system = _initializeSystems[i];
				SystemInfo systemInfo = _initializeSystemInfos[i];
				if (systemInfo.isActive)
				{
					double num = monitorSystemInitializeDuration(system);
					_totalDuration += num;
					systemInfo.AddExecutionDuration(num);
				}
			}
			updateName();
		}

		public override void Execute()
		{
			if (!paused)
			{
				Step();
			}
		}

		public void Step()
		{
			_totalDuration = 0.0;
			if (Time.frameCount % (int)avgResetInterval == 0)
			{
				ResetDurations();
			}
			int i = 0;
			for (int count = _executeSystems.Count; i < count; i++)
			{
				IExecuteSystem system = _executeSystems[i];
				SystemInfo systemInfo = _executeSystemInfos[i];
				if (systemInfo.isActive)
				{
					double num = monitorSystemExecutionDuration(system);
					_totalDuration += num;
					systemInfo.AddExecutionDuration(num);
				}
			}
			updateName();
		}

		private double monitorSystemInitializeDuration(IInitializeSystem system)
		{
			_stopwatch.Reset();
			_stopwatch.Start();
			system.Initialize();
			_stopwatch.Stop();
			return _stopwatch.Elapsed.TotalMilliseconds;
		}

		private double monitorSystemExecutionDuration(IExecuteSystem system)
		{
			_stopwatch.Reset();
			_stopwatch.Start();
			system.Execute();
			_stopwatch.Stop();
			return _stopwatch.Elapsed.TotalMilliseconds;
		}

		private void updateName()
		{
			if (_container != null)
			{
				_container.name = $"{_name} ({_initializeSystems.Count} init, {_executeSystems.Count} exe, {_totalDuration:0.###} ms)";
			}
		}
	}
}
