using UnityEngine;

namespace Entitas.Unity.VisualDebugging
{
	public class DebugSystemsBehaviour : MonoBehaviour
	{
		private DebugSystems _systems;

		public DebugSystems systems => _systems;

		public void Init(DebugSystems systems)
		{
			_systems = systems;
		}
	}
}
