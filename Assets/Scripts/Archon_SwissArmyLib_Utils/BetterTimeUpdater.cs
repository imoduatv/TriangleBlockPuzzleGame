using JetBrains.Annotations;
using UnityEngine;

namespace Archon.SwissArmyLib.Utils
{
	[AddComponentMenu("")]
	internal class BetterTimeUpdater : MonoBehaviour
	{
		[UsedImplicitly]
		private void Awake()
		{
			BetterTime.Update();
		}

		[UsedImplicitly]
		private void OnEnable()
		{
			BetterTimeUpdater x = ServiceLocator.Resolve<BetterTimeUpdater>();
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
		private void Start()
		{
			BetterTime.Update();
		}

		[UsedImplicitly]
		private void Update()
		{
			BetterTime.Update();
		}

		[UsedImplicitly]
		private void FixedUpdate()
		{
			BetterTime.Update();
		}
	}
}
