using UnityEngine;

namespace Entitas.Unity.VisualDebugging
{
	[ExecuteInEditMode]
	public class EntityBehaviour : MonoBehaviour
	{
		private Pool _pool;

		private Entity _entity;

		private string _cachedName;

		public Pool pool => _pool;

		public Entity entity => _entity;

		public void Init(Pool pool, Entity entity)
		{
			_pool = pool;
			_entity = entity;
			_entity.OnEntityReleased += onEntityReleased;
			Update();
		}

		private void onEntityReleased(Entity e)
		{
			base.gameObject.DestroyGameObject();
		}

		private void Update()
		{
			if (_entity != null && _cachedName != _entity.ToString())
			{
				base.name = (_cachedName = _entity.ToString());
			}
		}

		private void OnDestroy()
		{
			if (_entity != null)
			{
				_entity.OnEntityReleased -= onEntityReleased;
			}
		}
	}
}
