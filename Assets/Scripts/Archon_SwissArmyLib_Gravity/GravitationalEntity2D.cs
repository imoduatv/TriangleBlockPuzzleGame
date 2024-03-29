using JetBrains.Annotations;
using UnityEngine;

namespace Archon.SwissArmyLib.Gravity
{
	[AddComponentMenu("Archon/Gravity/GravitationalEntity2D")]
	[RequireComponent(typeof(Rigidbody2D))]
	public class GravitationalEntity2D : MonoBehaviour
	{
		private Rigidbody2D _rigidbody;

		[UsedImplicitly]
		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
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
