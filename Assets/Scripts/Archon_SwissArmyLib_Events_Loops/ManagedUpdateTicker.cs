using Archon.SwissArmyLib.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Archon.SwissArmyLib.Events.Loops
{
	[AddComponentMenu("")]
	internal sealed class ManagedUpdateTicker : MonoBehaviour
	{
		[UsedImplicitly]
		private void OnEnable()
		{
			ManagedUpdateTicker x = ServiceLocator.Resolve<ManagedUpdateTicker>();
			if (x == null)
			{
				ServiceLocator.RegisterSingleton(this);
			}
			else if (x != this)
			{
				UnityEngine.Object.Destroy(this);
			}
		}

		[UsedImplicitly]
		private void Update()
		{
			ManagedUpdate.Update();
		}

		[UsedImplicitly]
		private void LateUpdate()
		{
			ManagedUpdate.LateUpdate();
		}

		[UsedImplicitly]
		private void FixedUpdate()
		{
			ManagedUpdate.FixedUpdate();
		}
	}
}
