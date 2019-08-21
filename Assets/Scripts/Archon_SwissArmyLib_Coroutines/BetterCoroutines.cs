using Archon.SwissArmyLib.Collections;
using Archon.SwissArmyLib.Events;
using Archon.SwissArmyLib.Events.Loops;
using Archon.SwissArmyLib.Pooling;
using Archon.SwissArmyLib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Coroutines
{
	public class BetterCoroutines : IEventListener
	{
		public const object WaitForOneFrame = null;

		public static readonly WaitForEndOfFrame WaitForEndOfFrame;

		private static readonly Pool<LinkedListNode<BetterCoroutine>> SharedNodePool;

		private static readonly Dictionary<int, PooledLinkedList<BetterCoroutine>> UpdateLoopToCoroutines;

		private static readonly PooledLinkedList<BetterCoroutine> CoroutinesWaitingForEndOfFrame;

		private static LinkedListNode<BetterCoroutine> _current;

		private static readonly DictionaryWithDefault<int, BetterCoroutine> IdToCoroutine;

		private static int _nextId;

		private static readonly BetterCoroutines Instance;

		static BetterCoroutines()
		{
			WaitForEndOfFrame = new WaitForEndOfFrame();
			SharedNodePool = new Pool<LinkedListNode<BetterCoroutine>>(() => new LinkedListNode<BetterCoroutine>(null));
			UpdateLoopToCoroutines = new Dictionary<int, PooledLinkedList<BetterCoroutine>>();
			CoroutinesWaitingForEndOfFrame = new PooledLinkedList<BetterCoroutine>(SharedNodePool);
			IdToCoroutine = new DictionaryWithDefault<int, BetterCoroutine>();
			_nextId = 1;
			Instance = new BetterCoroutines();
			ServiceLocator.RegisterSingleton(Instance);
			if (!ServiceLocator.IsRegistered<BetterCoroutinesEndOfFrame>())
			{
				ServiceLocator.RegisterSingleton<BetterCoroutinesEndOfFrame>();
			}
			ServiceLocator.GlobalReset += delegate
			{
				ServiceLocator.RegisterSingleton(Instance);
				ServiceLocator.RegisterSingleton<BetterCoroutinesEndOfFrame>();
			};
		}

		private BetterCoroutines()
		{
		}

		public static int Start(IEnumerator enumerator, UpdateLoop updateLoop = UpdateLoop.Update)
		{
			return Start(enumerator, GetEventId(updateLoop));
		}

		public static int Start(IEnumerator enumerator, int updateLoopId)
		{
			if (object.ReferenceEquals(enumerator, null))
			{
				throw new ArgumentNullException("enumerator");
			}
			BetterCoroutine betterCoroutine = SpawnCoroutine(enumerator, updateLoopId);
			Start(betterCoroutine);
			return betterCoroutine.Id;
		}

		public static int Start(IEnumerator enumerator, GameObject linkedObject, UpdateLoop updateLoop = UpdateLoop.Update)
		{
			return Start(enumerator, linkedObject, GetEventId(updateLoop));
		}

		public static int Start(IEnumerator enumerator, GameObject linkedObject, int updateLoopId)
		{
			if (object.ReferenceEquals(enumerator, null))
			{
				throw new ArgumentNullException("enumerator");
			}
			if (object.ReferenceEquals(linkedObject, null))
			{
				throw new ArgumentNullException("linkedObject");
			}
			BetterCoroutine betterCoroutine = SpawnCoroutine(enumerator, updateLoopId);
			betterCoroutine.LinkedObject = linkedObject;
			betterCoroutine.IsLinkedToObject = true;
			Start(betterCoroutine);
			return betterCoroutine.Id;
		}

		public static int Start(IEnumerator enumerator, MonoBehaviour linkedComponent, UpdateLoop updateLoop = UpdateLoop.Update)
		{
			return Start(enumerator, linkedComponent, GetEventId(updateLoop));
		}

		public static int Start(IEnumerator enumerator, MonoBehaviour linkedComponent, int updateLoopId)
		{
			if (object.ReferenceEquals(enumerator, null))
			{
				throw new ArgumentNullException("enumerator");
			}
			if (object.ReferenceEquals(linkedComponent, null))
			{
				throw new ArgumentNullException("linkedComponent");
			}
			BetterCoroutine betterCoroutine = SpawnCoroutine(enumerator, updateLoopId);
			betterCoroutine.LinkedComponent = linkedComponent;
			betterCoroutine.IsLinkedToComponent = true;
			Start(betterCoroutine);
			return betterCoroutine.Id;
		}

		private static void Start(BetterCoroutine coroutine)
		{
			float time = GetTime(coroutine.UpdateLoopId, unscaled: false);
			float time2 = GetTime(coroutine.UpdateLoopId, unscaled: true);
			IdToCoroutine[coroutine.Id] = coroutine;
			PooledLinkedList<BetterCoroutine> pooledLinkedList = GetList(coroutine.UpdateLoopId);
			if (pooledLinkedList == null)
			{
				pooledLinkedList = (UpdateLoopToCoroutines[coroutine.UpdateLoopId] = new PooledLinkedList<BetterCoroutine>(SharedNodePool));
				ManagedUpdate.AddListener(coroutine.UpdateLoopId, Instance);
			}
			LinkedListNode<BetterCoroutine> current = pooledLinkedList.AddFirst(coroutine);
			LinkedListNode<BetterCoroutine> current2 = _current;
			_current = current;
			if (!UpdateCoroutine(time, time2, coroutine))
			{
				IdToCoroutine.Remove(coroutine.Id);
				pooledLinkedList.Remove(coroutine);
				if (coroutine.Parent != null)
				{
					coroutine.Parent.Child = null;
				}
				PoolHelper<BetterCoroutine>.Despawn(coroutine);
			}
			_current = current2;
		}

		private static void StartChild(IEnumerator enumerator, BetterCoroutine parent)
		{
			BetterCoroutine betterCoroutine = SpawnCoroutine(enumerator, parent.UpdateLoopId);
			betterCoroutine.Parent = parent;
			parent.Child = betterCoroutine;
			Start(betterCoroutine);
		}

		public static bool IsRunning(int id)
		{
			return id > 0 && IdToCoroutine.ContainsKey(id);
		}

		public static void SetPaused(int id, bool paused)
		{
			BetterCoroutine betterCoroutine = IdToCoroutine[id];
			if (betterCoroutine == null)
			{
				throw new ArgumentException("No coroutine is running with the specified ID", "id");
			}
			if (betterCoroutine.IsPaused == paused)
			{
				return;
			}
			betterCoroutine.IsPaused = paused;
			for (BetterCoroutine child = betterCoroutine.Child; child != null; child = child.Child)
			{
				child.IsParentPaused = paused;
				if (child.IsPaused)
				{
					break;
				}
			}
		}

		public static bool IsPaused(int id)
		{
			BetterCoroutine betterCoroutine = IdToCoroutine[id];
			if (betterCoroutine == null)
			{
				throw new ArgumentException("No coroutine is running with the specified ID", "id");
			}
			return betterCoroutine.IsPaused || betterCoroutine.IsParentPaused;
		}

		public static void Pause(int id)
		{
			SetPaused(id, paused: true);
		}

		public static void Unpause(int id)
		{
			SetPaused(id, paused: false);
		}

		public static bool Stop(int id)
		{
			BetterCoroutine betterCoroutine = IdToCoroutine[id];
			if (betterCoroutine != null)
			{
				Stop(betterCoroutine);
				return true;
			}
			return false;
		}

		private static void Stop(BetterCoroutine coroutine)
		{
			if (!coroutine.IsDone)
			{
				if (coroutine.Parent != null)
				{
					coroutine.Parent.Child = null;
				}
				while (coroutine != null)
				{
					coroutine.IsDone = true;
					coroutine = coroutine.Child;
				}
			}
		}

		public static void StopAll()
		{
			foreach (int key in UpdateLoopToCoroutines.Keys)
			{
				StopAll(key);
			}
		}

		public static void StopAll(UpdateLoop updateLoop)
		{
			StopAll(GetEventId(updateLoop));
		}

		public static void StopAll(int updateLoopId)
		{
			PooledLinkedList<BetterCoroutine> list = GetList(updateLoopId);
			if (list == null)
			{
				return;
			}
			bool flag = _current != null && _current.Value.UpdateLoopId == updateLoopId;
			LinkedListNode<BetterCoroutine> next;
			for (LinkedListNode<BetterCoroutine> linkedListNode = list.First; linkedListNode != null; linkedListNode = next)
			{
				next = linkedListNode.Next;
				BetterCoroutine value = linkedListNode.Value;
				Stop(value);
				if (!flag)
				{
					IdToCoroutine.Remove(value.Id);
					PoolHelper<BetterCoroutine>.Despawn(value);
					list.Remove(linkedListNode);
				}
			}
		}

		public static object WaitForSeconds(float seconds, bool unscaled = false)
		{
			WaitForSecondsLite instance = WaitForSecondsLite.Instance;
			instance.Unscaled = unscaled;
			instance.Duration = seconds;
			return instance;
		}

		public static IEnumerator WaitForSecondsRealtime(float seconds)
		{
			return WaitForSecondsRealtimeLite.Create(seconds);
		}

		public static IEnumerator WaitForAsyncOperation(AsyncOperation operation)
		{
			if (object.ReferenceEquals(operation, null))
			{
				throw new ArgumentNullException("operation");
			}
			return Archon.SwissArmyLib.Coroutines.WaitForAsyncOperation.Create(operation);
		}

		public static IEnumerator WaitForWWW(WWW www)
		{
			if (object.ReferenceEquals(www, null))
			{
				throw new ArgumentNullException("www");
			}
			return Archon.SwissArmyLib.Coroutines.WaitForWWW.Create(www);
		}

		public static IEnumerator WaitUntil(Func<bool> predicate)
		{
			if (object.ReferenceEquals(predicate, null))
			{
				throw new ArgumentNullException("predicate");
			}
			return WaitUntilLite.Create(predicate);
		}

		public static IEnumerator WaitWhile(Func<bool> predicate)
		{
			if (object.ReferenceEquals(predicate, null))
			{
				throw new ArgumentNullException("predicate");
			}
			return WaitWhileLite.Create(predicate);
		}

		private static BetterCoroutine SpawnCoroutine(IEnumerator enumerator, int updateLoopId)
		{
			BetterCoroutine betterCoroutine = PoolHelper<BetterCoroutine>.Spawn();
			betterCoroutine.Id = GetNextId();
			betterCoroutine.Enumerator = enumerator;
			betterCoroutine.UpdateLoopId = updateLoopId;
			return betterCoroutine;
		}

		private static int GetNextId()
		{
			if (_nextId < 1)
			{
				_nextId = 1;
			}
			return _nextId++;
		}

		private static PooledLinkedList<BetterCoroutine> GetList(int updateLoopId)
		{
			UpdateLoopToCoroutines.TryGetValue(updateLoopId, out PooledLinkedList<BetterCoroutine> value);
			return value;
		}

		private static void Update(PooledLinkedList<BetterCoroutine> coroutines)
		{
			float time = BetterTime.Time;
			float unscaledTime = BetterTime.UnscaledTime;
			_current = coroutines.First;
			while (_current != null)
			{
				BetterCoroutine value = _current.Value;
				if (!UpdateCoroutine(time, unscaledTime, value))
				{
					LinkedListNode<BetterCoroutine> next = _current.Next;
					Stop(value);
					coroutines.Remove(_current);
					IdToCoroutine.Remove(value.Id);
					PoolHelper<BetterCoroutine>.Despawn(value);
					_current = next;
				}
				else
				{
					_current = _current.Next;
				}
			}
		}

		private static bool UpdateCoroutine(float scaledTime, float unscaledTime, BetterCoroutine coroutine)
		{
			if (coroutine.IsDone)
			{
				return false;
			}
			if (coroutine.IsLinkedToObject && (!coroutine.LinkedObject || !coroutine.LinkedObject.activeInHierarchy))
			{
				return false;
			}
			if (coroutine.IsLinkedToComponent && (!coroutine.LinkedComponent || !coroutine.LinkedComponent.isActiveAndEnabled))
			{
				return false;
			}
			if (coroutine.IsPaused || coroutine.IsParentPaused)
			{
				return true;
			}
			if (coroutine.Child != null)
			{
				return true;
			}
			if (coroutine.WaitingForEndOfFrame)
			{
				return true;
			}
			float num = (!coroutine.WaitTimeIsUnscaled) ? scaledTime : unscaledTime;
			if (coroutine.WaitTillTime > num)
			{
				return true;
			}
			IEnumerator enumerator = coroutine.Enumerator;
			try
			{
				if (!enumerator.MoveNext())
				{
					return false;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
				return false;
			}
			object current = enumerator.Current;
			if (current == null)
			{
				return true;
			}
			int? num2 = current as int?;
			if (num2.HasValue)
			{
				BetterCoroutine betterCoroutine = IdToCoroutine[num2.Value];
				if (betterCoroutine != null)
				{
					coroutine.Child = betterCoroutine;
					betterCoroutine.Parent = coroutine;
				}
				return true;
			}
			IEnumerator enumerator2 = current as IEnumerator;
			if (enumerator2 != null)
			{
				StartChild(enumerator2, coroutine);
				return true;
			}
			WaitForSecondsLite waitForSecondsLite = current as WaitForSecondsLite;
			if (waitForSecondsLite != null)
			{
				num = ((!waitForSecondsLite.Unscaled) ? scaledTime : unscaledTime);
				coroutine.WaitTimeIsUnscaled = waitForSecondsLite.Unscaled;
				coroutine.WaitTillTime = num + waitForSecondsLite.Duration;
				return true;
			}
			if (current is WaitForEndOfFrame)
			{
				coroutine.WaitingForEndOfFrame = true;
				CoroutinesWaitingForEndOfFrame.AddFirst(coroutine);
				return true;
			}
			WWW wWW = current as WWW;
			if (wWW != null)
			{
				if (!wWW.isDone)
				{
					StartChild(Archon.SwissArmyLib.Coroutines.WaitForWWW.Create(wWW), coroutine);
				}
				return true;
			}
			AsyncOperation asyncOperation = current as AsyncOperation;
			if (asyncOperation != null)
			{
				if (!asyncOperation.isDone)
				{
					StartChild(Archon.SwissArmyLib.Coroutines.WaitForAsyncOperation.Create(asyncOperation), coroutine);
				}
				return true;
			}
			if (current is WaitForSeconds)
			{
				UnityEngine.Debug.LogError("UnityEngine.WaitForSeconds is not supported in BetterCoroutines. Please use BetterCoroutines.WaitForSeconds() instead.");
			}
			return true;
		}

		void IEventListener.OnEvent(int eventId)
		{
			if (UpdateLoopToCoroutines.TryGetValue(eventId, out PooledLinkedList<BetterCoroutine> value))
			{
				Update(value);
			}
		}

		private static int GetEventId(UpdateLoop updateLoop)
		{
			switch (updateLoop)
			{
			case UpdateLoop.Update:
				return -1000;
			case UpdateLoop.LateUpdate:
				return -1001;
			case UpdateLoop.FixedUpdate:
				return -1002;
			default:
				throw new ArgumentOutOfRangeException("updateLoop", updateLoop, null);
			}
		}

		private static float GetTime(int updateLoopId, bool unscaled)
		{
			if (updateLoopId == -1002 || ManagedUpdate.GetParentLoopForid(updateLoopId) == UpdateLoop.FixedUpdate)
			{
				return (!unscaled) ? BetterTime.FixedTime : BetterTime.FixedUnscaledTime;
			}
			return (!unscaled) ? BetterTime.Time : BetterTime.UnscaledTime;
		}

		internal static void ProcessEndOfFrame()
		{
			LinkedListNode<BetterCoroutine> next;
			for (LinkedListNode<BetterCoroutine> linkedListNode = CoroutinesWaitingForEndOfFrame.First; linkedListNode != null; linkedListNode = next)
			{
				next = linkedListNode.Next;
				BetterCoroutine value = linkedListNode.Value;
				if (value.WaitingForEndOfFrame)
				{
					value.WaitingForEndOfFrame = false;
					float time = GetTime(value.UpdateLoopId, unscaled: false);
					float time2 = GetTime(value.UpdateLoopId, unscaled: true);
					if (!UpdateCoroutine(time, time2, value))
					{
						Stop(value);
					}
				}
				CoroutinesWaitingForEndOfFrame.Remove(linkedListNode);
			}
		}
	}
}
