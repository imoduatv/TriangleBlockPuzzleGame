using System;
using System.Collections.Generic;
using System.Threading;

namespace Entitas
{
	public class Pool
	{
		public delegate void PoolChanged(Pool pool, Entity entity);

		public delegate void GroupChanged(Pool pool, Group group);

		protected readonly HashSet<Entity> _entities = new HashSet<Entity>(EntityEqualityComparer.comparer);

		protected readonly Dictionary<IMatcher, Group> _groups = new Dictionary<IMatcher, Group>();

		protected readonly List<Group>[] _groupsForIndex;

		private readonly Stack<Entity> _reusableEntities = new Stack<Entity>();

		private readonly HashSet<Entity> _retainedEntities = new HashSet<Entity>();

		private readonly int _totalComponents;

		private readonly Stack<IComponent>[] _componentPools;

		private int _creationIndex;

		private readonly PoolMetaData _metaData;

		private Entity[] _entitiesCache;

		private Entity.EntityChanged _cachedUpdateGroupsComponentAddedOrRemoved;

		private Entity.ComponentReplaced _cachedUpdateGroupsComponentReplaced;

		private Entity.EntityReleased _cachedOnEntityReleased;

		public int totalComponents => _totalComponents;

		public Stack<IComponent>[] componentPools => _componentPools;

		public PoolMetaData metaData => _metaData;

		public int count => _entities.Count;

		public int reusableEntitiesCount => _reusableEntities.Count;

		public int retainedEntitiesCount => _retainedEntities.Count;

		public event PoolChanged OnEntityCreated;

		public event PoolChanged OnEntityWillBeDestroyed;

		public event PoolChanged OnEntityDestroyed;

		public event GroupChanged OnGroupCreated;

		public event GroupChanged OnGroupCleared;

		public Pool(int totalComponents)
			: this(totalComponents, 0, null)
		{
		}

		public Pool(int totalComponents, int startCreationIndex, PoolMetaData metaData)
		{
			_totalComponents = totalComponents;
			_componentPools = new Stack<IComponent>[totalComponents];
			_creationIndex = startCreationIndex;
			if (metaData != null)
			{
				_metaData = metaData;
				if (metaData.componentNames.Length != totalComponents)
				{
					throw new PoolMetaDataException(this, metaData);
				}
			}
			else
			{
				string[] array = new string[totalComponents];
				int i = 0;
				for (int num = array.Length; i < num; i++)
				{
					array[i] = "Index " + i;
				}
				_metaData = new PoolMetaData("Unnamed Pool", array, null);
			}
			_groupsForIndex = new List<Group>[totalComponents];
			_cachedUpdateGroupsComponentAddedOrRemoved = updateGroupsComponentAddedOrRemoved;
			_cachedUpdateGroupsComponentReplaced = updateGroupsComponentReplaced;
			_cachedOnEntityReleased = onEntityReleased;
		}

		public virtual Entity CreateEntity()
		{
			Entity entity = (_reusableEntities.Count <= 0) ? new Entity(_totalComponents, _componentPools, _metaData) : _reusableEntities.Pop();
			entity._isEnabled = true;
			entity._creationIndex = _creationIndex++;
			entity.Retain(this);
			_entities.Add(entity);
			_entitiesCache = null;
			entity.OnComponentAdded += _cachedUpdateGroupsComponentAddedOrRemoved;
			entity.OnComponentRemoved += _cachedUpdateGroupsComponentAddedOrRemoved;
			entity.OnComponentReplaced += _cachedUpdateGroupsComponentReplaced;
			entity.OnEntityReleased += _cachedOnEntityReleased;
			if (this.OnEntityCreated != null)
			{
				this.OnEntityCreated(this, entity);
			}
			return entity;
		}

		public virtual void DestroyEntity(Entity entity)
		{
			if (!_entities.Remove(entity))
			{
				throw new PoolDoesNotContainEntityException("'" + this + "' cannot destroy " + entity + "!", "Did you call pool.DestroyEntity() on a wrong pool?");
			}
			_entitiesCache = null;
			if (this.OnEntityWillBeDestroyed != null)
			{
				this.OnEntityWillBeDestroyed(this, entity);
			}
			entity.destroy();
			if (this.OnEntityDestroyed != null)
			{
				this.OnEntityDestroyed(this, entity);
			}
			if (entity.retainCount == 1)
			{
				entity.OnEntityReleased -= _cachedOnEntityReleased;
				_reusableEntities.Push(entity);
				entity.Release(this);
				entity.removeAllOnEntityReleasedHandlers();
			}
			else
			{
				_retainedEntities.Add(entity);
				entity.Release(this);
			}
		}

		public virtual void DestroyAllEntities()
		{
			Entity[] entities = GetEntities();
			int i = 0;
			for (int num = entities.Length; i < num; i++)
			{
				DestroyEntity(entities[i]);
			}
			_entities.Clear();
			if (_retainedEntities.Count != 0)
			{
				throw new PoolStillHasRetainedEntitiesException(this);
			}
		}

		public virtual bool HasEntity(Entity entity)
		{
			return _entities.Contains(entity);
		}

		public virtual Entity[] GetEntities()
		{
			if (_entitiesCache == null)
			{
				_entitiesCache = new Entity[_entities.Count];
				_entities.CopyTo(_entitiesCache);
			}
			return _entitiesCache;
		}

		public virtual Group GetGroup(IMatcher matcher)
		{
			if (!_groups.TryGetValue(matcher, out Group value))
			{
				value = new Group(matcher);
				Entity[] entities = GetEntities();
				int i = 0;
				for (int num = entities.Length; i < num; i++)
				{
					value.HandleEntitySilently(entities[i]);
				}
				_groups.Add(matcher, value);
				int j = 0;
				for (int num2 = matcher.indices.Length; j < num2; j++)
				{
					int num3 = matcher.indices[j];
					if (_groupsForIndex[num3] == null)
					{
						_groupsForIndex[num3] = new List<Group>();
					}
					_groupsForIndex[num3].Add(value);
				}
				if (this.OnGroupCreated != null)
				{
					this.OnGroupCreated(this, value);
				}
			}
			return value;
		}

		public void ClearGroups()
		{
			foreach (Group value in _groups.Values)
			{
				value.RemoveAllEventHandlers();
				int i = 0;
				for (int num = value.GetEntities().Length; i < num; i++)
				{
					value.GetEntities()[i].Release(value);
				}
				if (this.OnGroupCleared != null)
				{
					this.OnGroupCleared(this, value);
				}
			}
			_groups.Clear();
			int j = 0;
			for (int num2 = _groupsForIndex.Length; j < num2; j++)
			{
				_groupsForIndex[j] = null;
			}
		}

		public void ResetCreationIndex()
		{
			_creationIndex = 0;
		}

		public void ClearComponentPool(int index)
		{
			_componentPools[index]?.Clear();
		}

		public void ClearComponentPools()
		{
			int i = 0;
			for (int num = _componentPools.Length; i < num; i++)
			{
				ClearComponentPool(i);
			}
		}

		public void Reset()
		{
			ClearGroups();
			DestroyAllEntities();
			ResetCreationIndex();
		}

		public override string ToString()
		{
			return _metaData.poolName;
		}

		protected void updateGroupsComponentAddedOrRemoved(Entity entity, int index, IComponent component)
		{
			List<Group> list = _groupsForIndex[index];
			if (list != null)
			{
				List<Group.GroupChanged> list2 = new List<Group.GroupChanged>(list.Count);
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					list2.Add(list[i].handleEntity(entity));
				}
				int j = 0;
				for (int count2 = list2.Count; j < count2; j++)
				{
					list2[j]?.Invoke(list[j], entity, index, component);
				}
			}
		}

		protected void updateGroupsComponentReplaced(Entity entity, int index, IComponent previousComponent, IComponent newComponent)
		{
			List<Group> list = _groupsForIndex[index];
			if (list != null)
			{
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					list[i].UpdateEntity(entity, index, previousComponent, newComponent);
				}
			}
		}

		protected void onEntityReleased(Entity entity)
		{
			if (entity._isEnabled)
			{
				throw new EntityIsNotDestroyedException("Cannot release " + entity + "!");
			}
			entity.removeAllOnEntityReleasedHandlers();
			_retainedEntities.Remove(entity);
			_reusableEntities.Push(entity);
		}
	}
}
