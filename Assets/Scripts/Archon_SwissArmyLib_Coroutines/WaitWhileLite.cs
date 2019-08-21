using Archon.SwissArmyLib.Pooling;
using System;
using System.Collections;
using UnityEngine;

namespace Archon.SwissArmyLib.Coroutines
{
	internal sealed class WaitWhileLite : CustomYieldInstruction, IPoolableYieldInstruction
	{
		private static readonly Pool<WaitWhileLite> Pool = new Pool<WaitWhileLite>(() => new WaitWhileLite());

		private Func<bool> _predicate;

		public override bool keepWaiting => _predicate();

		private WaitWhileLite()
		{
		}

		public static IEnumerator Create(Func<bool> predicate)
		{
			WaitWhileLite waitWhileLite = Pool.Spawn();
			waitWhileLite._predicate = predicate;
			return waitWhileLite;
		}

		public void Despawn()
		{
			_predicate = null;
			Pool.Despawn(this);
		}
	}
}
