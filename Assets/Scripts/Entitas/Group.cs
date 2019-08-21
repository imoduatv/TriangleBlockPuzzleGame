using System;
using System.Collections.Generic;
using System.Threading;

namespace Entitas
{
	public class Group
	{
		public delegate void GroupChanged(Group group, Entity entity, int index, IComponent component);

		public delegate void GroupUpdated(Group group, Entity entity, int index, IComponent previousComponent, IComponent newComponent);

		private readonly IMatcher _matcher;

		private readonly HashSet<Entity> _entities = new HashSet<Entity>(EntityEqualityComparer.comparer);

		private Entity[] _entitiesCache;

		private Entity _singleEntityCache;

		private string _toStringCache;

		public int count => _entities.Count;

		public IMatcher matcher => _matcher;

		public event GroupChanged OnEntityAdded;

		public event GroupChanged OnEntityRemoved;

		public event GroupUpdated OnEntityUpdated;

		public Group(IMatcher matcher)
		{
			_matcher = matcher;
		}

		public void HandleEntitySilently(Entity entity)
		{
			if (_matcher.Matches(entity))
			{
				addEntitySilently(entity);
			}
			else
			{
				removeEntitySilently(entity);
			}
		}

		public void HandleEntity(Entity entity, int index, IComponent component)
		{
			if (_matcher.Matches(entity))
			{
				addEntity(entity, index, component);
			}
			else
			{
				removeEntity(entity, index, component);
			}
		}

		internal GroupChanged handleEntity(Entity entity)
		{
			return (!_matcher.Matches(entity)) ? removeEntity(entity) : addEntity(entity);
		}

		public void UpdateEntity(Entity entity, int index, IComponent previousComponent, IComponent newComponent)
		{
			if (_entities.Contains(entity))
			{
				if (this.OnEntityRemoved != null)
				{
					this.OnEntityRemoved(this, entity, index, previousComponent);
				}
				if (this.OnEntityAdded != null)
				{
					this.OnEntityAdded(this, entity, index, newComponent);
				}
				if (this.OnEntityUpdated != null)
				{
					this.OnEntityUpdated(this, entity, index, previousComponent, newComponent);
				}
			}
		}

		public void RemoveAllEventHandlers()
		{
			this.OnEntityAdded = null;
			this.OnEntityRemoved = null;
			this.OnEntityUpdated = null;
		}

		private bool addEntitySilently(Entity entity)
		{
			bool flag = _entities.Add(entity);
			if (flag)
			{
				_entitiesCache = null;
				_singleEntityCache = null;
				entity.Retain(this);
			}
			return flag;
		}

		private void addEntity(Entity entity, int index, IComponent component)
		{
			if (addEntitySilently(entity) && this.OnEntityAdded != null)
			{
				this.OnEntityAdded(this, entity, index, component);
			}
		}

		private GroupChanged addEntity(Entity entity)
		{
			return (!addEntitySilently(entity)) ? null : this.OnEntityAdded;
		}

		private bool removeEntitySilently(Entity entity)
		{
			bool flag = _entities.Remove(entity);
			if (flag)
			{
				_entitiesCache = null;
				_singleEntityCache = null;
				entity.Release(this);
			}
			return flag;
		}

		private void removeEntity(Entity entity, int index, IComponent component)
		{
			if (_entities.Remove(entity))
			{
				_entitiesCache = null;
				_singleEntityCache = null;
				if (this.OnEntityRemoved != null)
				{
					this.OnEntityRemoved(this, entity, index, component);
				}
				entity.Release(this);
			}
		}

		private GroupChanged removeEntity(Entity entity)
		{
			return (!removeEntitySilently(entity)) ? null : this.OnEntityRemoved;
		}

		public bool ContainsEntity(Entity entity)
		{
			return _entities.Contains(entity);
		}

		public Entity[] GetEntities()
		{
			if (_entitiesCache == null)
			{
				_entitiesCache = new Entity[_entities.Count];
				_entities.CopyTo(_entitiesCache);
			}
			return _entitiesCache;
		}

		public Entity GetSingleEntity()
		{
			if (_singleEntityCache == null)
			{
				switch (_entities.Count)
				{
				case 1:
					break;
				case 0:
					return null;
				default:
					throw new GroupSingleEntityException(this);
				}
				using (HashSet<Entity>.Enumerator enumerator = _entities.GetEnumerator())
				{
					enumerator.MoveNext();
					_singleEntityCache = enumerator.Current;
				}
			}
			return _singleEntityCache;
		}

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				_toStringCache = "Group(" + _matcher + ")";
			}
			return _toStringCache;
		}
	}
}
