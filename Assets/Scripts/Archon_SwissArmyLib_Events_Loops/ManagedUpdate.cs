using Archon.SwissArmyLib.Collections;
using Archon.SwissArmyLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Events.Loops
{
	[AddComponentMenu("")]
	public static class ManagedUpdate
	{
		public static class EventIds
		{
			public const int Update = -1000;

			public const int LateUpdate = -1001;

			public const int FixedUpdate = -1002;
		}

		public static readonly Event OnUpdate;

		public static readonly Event OnLateUpdate;

		public static readonly Event OnFixedUpdate;

		private static Dictionary<int, ICustomUpdateLoop> _idToUpdateLoop;

		private static Dictionary<int, UpdateLoop> _idToUnityUpdateLoop;

		private static Dictionary<int, PrioritizedList<ICustomUpdateLoop>> _customUpdateLoops;

		public static float DeltaTime
		{
			get;
			private set;
		}

		public static float UnscaledDeltaTime
		{
			get;
			private set;
		}

		static ManagedUpdate()
		{
			OnUpdate = new Event(-1000);
			OnLateUpdate = new Event(-1001);
			OnFixedUpdate = new Event(-1002);
			if (!ServiceLocator.IsRegistered<ManagedUpdateTicker>())
			{
				ServiceLocator.RegisterSingleton<ManagedUpdateTicker>();
			}
			ServiceLocator.GlobalReset += delegate
			{
				ServiceLocator.RegisterSingleton<ManagedUpdateTicker>();
			};
		}

		public static void AddCustomUpdateLoop(ICustomUpdateLoop updateLoop, UpdateLoop parentLoop = UpdateLoop.Update, int priority = 0)
		{
			if (_idToUpdateLoop == null)
			{
				_idToUpdateLoop = new Dictionary<int, ICustomUpdateLoop>(4);
				_idToUnityUpdateLoop = new Dictionary<int, UpdateLoop>(4);
				_customUpdateLoops = new Dictionary<int, PrioritizedList<ICustomUpdateLoop>>(3);
			}
			if (!_customUpdateLoops.TryGetValue((int)parentLoop, out PrioritizedList<ICustomUpdateLoop> value))
			{
				value = (_customUpdateLoops[(int)parentLoop] = new PrioritizedList<ICustomUpdateLoop>(4));
			}
			int id = updateLoop.Event.Id;
			if (_idToUpdateLoop.ContainsKey(id))
			{
				UnityEngine.Debug.LogErrorFormat("An update loop with ID '{0}' is already registered.", id);
				return;
			}
			_idToUpdateLoop[id] = updateLoop;
			_idToUnityUpdateLoop[id] = parentLoop;
			value.Add(updateLoop, priority);
		}

		public static void RemoveCustomUpdateLoop(ICustomUpdateLoop updateLoop)
		{
			if (_customUpdateLoops != null)
			{
				int id = updateLoop.Event.Id;
				if (_idToUnityUpdateLoop.TryGetValue(id, out UpdateLoop value) && _customUpdateLoops.TryGetValue((int)value, out PrioritizedList<ICustomUpdateLoop> value2))
				{
					value2.Remove(updateLoop);
					_idToUpdateLoop.Remove(id);
					_idToUnityUpdateLoop.Remove(id);
				}
			}
		}

		public static void RemoveCustomUpdateLoop(int eventId)
		{
			ICustomUpdateLoop customUpdateLoop = GetCustomUpdateLoop(eventId);
			if (customUpdateLoop != null)
			{
				RemoveCustomUpdateLoop(customUpdateLoop);
			}
		}

		public static T GetCustomUpdateLoop<T>(int eventId) where T : class, ICustomUpdateLoop
		{
			return GetCustomUpdateLoop(eventId) as T;
		}

		public static ICustomUpdateLoop GetCustomUpdateLoop(int eventId)
		{
			if (_idToUpdateLoop == null)
			{
				return null;
			}
			_idToUpdateLoop.TryGetValue(eventId, out ICustomUpdateLoop value);
			return value;
		}

		public static void AddListener(int eventId, IEventListener listener, int priority = 0)
		{
			GetEventForId(eventId)?.AddListener(listener, priority);
		}

		public static void AddListener(int eventId, Action listener, int priority = 0)
		{
			GetEventForId(eventId)?.AddListener(listener, priority);
		}

		public static void RemoveListener(int eventId, IEventListener listener)
		{
			GetEventForId(eventId)?.RemoveListener(listener);
		}

		public static void RemoveListener(int eventId, Action listener)
		{
			GetEventForId(eventId)?.RemoveListener(listener);
		}

		internal static void Update()
		{
			DeltaTime = BetterTime.DeltaTime;
			UnscaledDeltaTime = BetterTime.UnscaledDeltaTime;
			OnUpdate.Invoke();
			ProcessCustomLoops(UpdateLoop.Update);
		}

		internal static void LateUpdate()
		{
			DeltaTime = BetterTime.DeltaTime;
			UnscaledDeltaTime = BetterTime.UnscaledDeltaTime;
			OnLateUpdate.Invoke();
			ProcessCustomLoops(UpdateLoop.LateUpdate);
		}

		internal static void FixedUpdate()
		{
			DeltaTime = BetterTime.FixedDeltaTime;
			UnscaledDeltaTime = BetterTime.FixedUnscaledDeltaTime;
			OnFixedUpdate.Invoke();
			ProcessCustomLoops(UpdateLoop.FixedUpdate);
		}

		internal static UpdateLoop GetParentLoopForid(int eventId)
		{
			switch (eventId)
			{
			case -1000:
				return UpdateLoop.Update;
			case -1001:
				return UpdateLoop.LateUpdate;
			case -1002:
				return UpdateLoop.FixedUpdate;
			default:
				_idToUnityUpdateLoop.TryGetValue(eventId, out UpdateLoop value);
				return value;
			}
		}

		private static void ProcessCustomLoops(UpdateLoop unityLoop)
		{
			if (_customUpdateLoops == null || !_customUpdateLoops.TryGetValue((int)unityLoop, out PrioritizedList<ICustomUpdateLoop> value))
			{
				return;
			}
			for (int i = 0; i < value.Count; i++)
			{
				PrioritizedItem<ICustomUpdateLoop> prioritizedItem = value[i];
				ICustomUpdateLoop item = prioritizedItem.Item;
				if (item.IsTimeToRun)
				{
					DeltaTime = item.DeltaTime;
					UnscaledDeltaTime = item.UnscaledDeltaTime;
					item.Invoke();
				}
			}
		}

		private static Event GetEventForId(int eventId)
		{
			switch (eventId)
			{
			case -1000:
				return OnUpdate;
			case -1001:
				return OnLateUpdate;
			case -1002:
				return OnFixedUpdate;
			default:
				return GetCustomUpdateLoop(eventId)?.Event;
			}
		}
	}
}
