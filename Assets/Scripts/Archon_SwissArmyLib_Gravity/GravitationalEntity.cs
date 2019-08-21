using JetBrains.Annotations;
using UnityEngine;

namespace Archon.SwissArmyLib.Gravity
{
	[AddComponentMenu("Archon/Gravity/GravitationalEntity")]
	[RequireComponent(typeof(Rigidbody))]
	public class GravitationalEntity : MonoBehaviour
	{
		private Rigidbody _rigidbody;

		[UsedImplicitly]
		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		[UsedImplicitly]
		private void OnEnable()
		{
			GravitationalSystem.Register(_rigidbody);
		}

		[UsedImplicitly]
		private void OnDisable()
		{
			GravitationalSystem.Unregister(_rigidbody);
		}
	}
}
