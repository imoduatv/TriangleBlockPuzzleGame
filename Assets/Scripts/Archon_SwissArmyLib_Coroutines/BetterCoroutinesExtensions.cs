using Archon.SwissArmyLib.Utils;
using System.Collections;
using UnityEngine;

namespace Archon.SwissArmyLib.Coroutines
{
	public static class BetterCoroutinesExtensions
	{
		public static int StartBetterCoroutine(this Object unityObject, IEnumerator enumerator, UpdateLoop updateLoop = UpdateLoop.Update)
		{
			return BetterCoroutines.Start(enumerator, updateLoop);
		}

		public static int StartBetterCoroutine(this Object unityObject, IEnumerator enumerator, int updateLoopId)
		{
			return BetterCoroutines.Start(enumerator, updateLoopId);
		}

		public static int StartBetterCoroutineLinked(this MonoBehaviour monoBehaviour, IEnumerator enumerator, UpdateLoop updateLoop = UpdateLoop.Update)
		{
			return BetterCoroutines.Start(enumerator, monoBehaviour, updateLoop);
		}

		public static int StartBetterCoroutineLinked(this MonoBehaviour monoBehaviour, IEnumerator enumerator, int updateLoopId)
		{
			return BetterCoroutines.Start(enumerator, monoBehaviour, updateLoopId);
		}

		public static int StartBetterCoroutineLinked(this GameObject gameObject, IEnumerator enumerator, UpdateLoop updateLoop = UpdateLoop.Update)
		{
			return BetterCoroutines.Start(enumerator, gameObject, updateLoop);
		}

		public static int StartBetterCoroutineLinked(this GameObject gameObject, IEnumerator enumerator, int updateLoopId)
		{
			return BetterCoroutines.Start(enumerator, gameObject, updateLoopId);
		}

		public static bool StopBetterCoroutine(this Object unityObject, int coroutineId)
		{
			return BetterCoroutines.Stop(coroutineId);
		}
	}
}
