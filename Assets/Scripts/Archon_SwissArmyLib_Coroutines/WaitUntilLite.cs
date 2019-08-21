using Archon.SwissArmyLib.Pooling;
using System;
using System.Collections;
using UnityEngine;

namespace Archon.SwissArmyLib.Coroutines
{
	internal sealed class WaitUntilLite : CustomYieldInstruction, IPoolableYieldInstruction
	{
		private static readonly Pool<WaitUntilLite> Pool = new Pool<WaitUntilLite>(() => new WaitUntilLite());

		private Func<bool> _predicate;

		public override bool keepWaiting => !_predicate();

		private WaitUntilLite()
		{
		}

		public static IEnumerator Create(Func<bool> predicate)
		{
			WaitUntilLite waitUntilLite = Pool.Spawn();
			waitUntilLite._predicate = predicate;
			return waitUntilLite;
		}

		public void Despawn()
		{
			_predicate = null;
			Pool.Despawn(this);
		}
	}
}
