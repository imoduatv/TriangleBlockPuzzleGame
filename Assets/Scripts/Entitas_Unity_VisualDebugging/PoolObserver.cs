using System.Collections.Generic;
using UnityEngine;

namespace Entitas.Unity.VisualDebugging
{
	public class PoolObserver
	{
		private readonly Pool _pool;

		private readonly List<Group> _groups;

		private readonly Transform _entitiesContainer;

		public Pool pool => _pool;

		public Group[] groups => _groups.ToArray();

		public GameObject entitiesContainer => _entitiesContainer.gameObject;

		public PoolObserver(Pool pool)
		{
			_pool = pool;
			_groups = new List<Group>();
			_entitiesContainer = new GameObject().transform;
			_entitiesContainer.gameObject.AddComponent<PoolObserverBehaviour>().Init(this);
			_pool.OnEntityCreated += onEntityCreated;
			_pool.OnGroupCreated += onGroupCreated;
			_pool.OnGroupCleared += onGroupCleared;
		}

		public void Deactivate()
		{
			_pool.OnEntityCreated -= onEntityCreated;
			_pool.OnGroupCreated -= onGroupCreated;
			_pool.OnGroupCleared -= onGroupCleared;
		}

		private void onEntityCreated(Pool pool, Entity entity)
		{
			EntityBehaviour entityBehaviour = new GameObject().AddComponent<EntityBehaviour>();
			entityBehaviour.Init(pool, entity);
			entityBehaviour.transform.SetParent(_entitiesContainer, worldPositionStays: false);
		}

		private void onGroupCreated(Pool pool, Group group)
		{
			_groups.Add(group);
		}

		private void onGroupCleared(Pool pool, Group group)
		{
			_groups.Remove(group);
		}

		public override string ToString()
		{
			string text;
			if (_pool.retainedEntitiesCount != 0)
			{
				text = _pool.metaData.poolName + " (" + _pool.count + " entities, " + _pool.reusableEntitiesCount + " reusable, " + _pool.retainedEntitiesCount + " retained, " + _groups.Count + " groups)";
				_entitiesContainer.name = text;
				return text;
			}
			text = _pool.metaData.poolName + " (" + _pool.count + " entities, " + _pool.reusableEntitiesCount + " reusable, " + _groups.Count + " groups)";
			_entitiesContainer.name = text;
			return text;
		}
	}
}
