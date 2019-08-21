using Archon.SwissArmyLib.Pooling;
using System.Collections;
using UnityEngine;

namespace Archon.SwissArmyLib.Coroutines
{
	internal sealed class WaitForAsyncOperation : CustomYieldInstruction, IPoolableYieldInstruction
	{
		private static readonly Pool<WaitForAsyncOperation> Pool = new Pool<WaitForAsyncOperation>(() => new WaitForAsyncOperation());

		private AsyncOperation _operation;

		public override bool keepWaiting => !_operation.isDone;

		private WaitForAsyncOperation()
		{
		}

		public static IEnumerator Create(AsyncOperation operation)
		{
			WaitForAsyncOperation waitForAsyncOperation = Pool.Spawn();
			waitForAsyncOperation._operation = operation;
			return waitForAsyncOperation;
		}

		public void Despawn()
		{
			_operation = null;
			Pool.Despawn(this);
		}
	}
}
