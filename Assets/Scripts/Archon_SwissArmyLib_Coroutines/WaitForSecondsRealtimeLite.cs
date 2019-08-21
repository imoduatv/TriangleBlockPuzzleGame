using Archon.SwissArmyLib.Pooling;
using System.Collections;
using UnityEngine;

namespace Archon.SwissArmyLib.Coroutines
{
	internal sealed class WaitForSecondsRealtimeLite : CustomYieldInstruction, IPoolableYieldInstruction
	{
		private static readonly Pool<WaitForSecondsRealtimeLite> Pool = new Pool<WaitForSecondsRealtimeLite>(() => new WaitForSecondsRealtimeLite());

		private float _expirationTime;

		public override bool keepWaiting => Time.realtimeSinceStartup < _expirationTime;

		private WaitForSecondsRealtimeLite()
		{
		}

		public static IEnumerator Create(float seconds)
		{
			WaitForSecondsRealtimeLite waitForSecondsRealtimeLite = Pool.Spawn();
			waitForSecondsRealtimeLite._expirationTime = Time.realtimeSinceStartup + seconds;
			return waitForSecondsRealtimeLite;
		}

		public void Despawn()
		{
			_expirationTime = 0f;
			Pool.Despawn(this);
		}
	}
}
