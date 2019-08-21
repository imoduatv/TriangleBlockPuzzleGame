using UnityEngine;

namespace Entitas.Unity.VisualDebugging
{
	[ExecuteInEditMode]
	public class PoolObserverBehaviour : MonoBehaviour
	{
		private PoolObserver _poolObserver;

		public PoolObserver poolObserver => _poolObserver;

		public void Init(PoolObserver poolObserver)
		{
			_poolObserver = poolObserver;
			Update();
		}

		private void Update()
		{
			if (_poolObserver == null)
			{
				base.gameObject.DestroyGameObject();
			}
			else if (_poolObserver.entitiesContainer != null)
			{
				_poolObserver.entitiesContainer.name = _poolObserver.ToString();
			}
		}

		private void OnDestroy()
		{
			_poolObserver.Deactivate();
		}
	}
}
