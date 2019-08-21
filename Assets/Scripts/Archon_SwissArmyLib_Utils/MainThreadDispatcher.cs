using Archon.SwissArmyLib.Events.Loops;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Archon.SwissArmyLib.Utils
{
	public static class MainThreadDispatcher
	{
		private static readonly Queue<Action> ActionQueue = new Queue<Action>();

		private static Thread _mainThread;

		[CompilerGenerated]
		private static Action _003C_003Ef__mg_0024cache0;

		public static bool IsMainThread
		{
			get
			{
				EnsureInitialized();
				return Thread.CurrentThread == _mainThread;
			}
		}

		public static void Initialize()
		{
			if (_mainThread == null)
			{
				_mainThread = Thread.CurrentThread;
				ManagedUpdate.OnUpdate.AddListener(RunPendingActions);
			}
		}

		public static void Enqueue(Action action)
		{
			EnsureInitialized();
			if (IsMainThread)
			{
				UnityEngine.Debug.LogWarning("Enqueue called from the main thread. Are you sure this is what you meant to do?");
			}
			lock (ActionQueue)
			{
				ActionQueue.Enqueue(action);
			}
		}

		private static void EnsureInitialized()
		{
			if (_mainThread == null)
			{
				UnityEngine.Debug.LogError("Dispatcher has not been initialized yet. Did you forget to call Initialize()?");
				throw new InvalidOperationException("Dispatcher is not initialized.");
			}
		}

		private static void RunPendingActions()
		{
			lock (ActionQueue)
			{
				while (ActionQueue.Count > 0)
				{
					try
					{
						ActionQueue.Dequeue()();
					}
					catch (Exception message)
					{
						UnityEngine.Debug.LogError(message);
					}
				}
			}
		}
	}
}
