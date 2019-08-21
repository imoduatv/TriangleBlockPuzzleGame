using Archon.SwissArmyLib.Utils;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Archon.SwissArmyLib.Coroutines
{
	[AddComponentMenu("")]
	internal class BetterCoroutinesEndOfFrame : MonoBehaviour
	{
		private Coroutine _endOfFrameCoroutine;

		[UsedImplicitly]
		private void OnEnable()
		{
			BetterCoroutinesEndOfFrame x = ServiceLocator.Resolve<BetterCoroutinesEndOfFrame>();
			if (x == null)
			{
				ServiceLocator.RegisterSingleton(this);
				_endOfFrameCoroutine = StartCoroutine(EndOfFrameCoroutine());
			}
			else if (x != this)
			{
				UnityEngine.Object.Destroy(this);
			}
		}

		[UsedImplicitly]
		private void OnDisable()
		{
			StopCoroutine(_endOfFrameCoroutine);
			_endOfFrameCoroutine = null;
		}

		private static IEnumerator EndOfFrameCoroutine()
		{
			while (true)
			{
				yield return BetterCoroutines.WaitForEndOfFrame;
				BetterCoroutines.ProcessEndOfFrame();
			}
		}
	}
}
