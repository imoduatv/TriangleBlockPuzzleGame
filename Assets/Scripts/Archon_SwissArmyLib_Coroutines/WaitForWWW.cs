using Archon.SwissArmyLib.Pooling;
using System.Collections;
using UnityEngine;

namespace Archon.SwissArmyLib.Coroutines
{
	internal sealed class WaitForWWW : CustomYieldInstruction, IPoolableYieldInstruction
	{
		private static readonly Pool<WaitForWWW> Pool = new Pool<WaitForWWW>(() => new WaitForWWW());

		private WWW _wwwObject;

		public override bool keepWaiting => !_wwwObject.isDone;

		private WaitForWWW()
		{
		}

		public static IEnumerator Create(WWW wwwObject)
		{
			WaitForWWW waitForWWW = Pool.Spawn();
			waitForWWW._wwwObject = wwwObject;
			return waitForWWW;
		}

		public void Despawn()
		{
			_wwwObject = null;
			Pool.Despawn(this);
		}
	}
}
