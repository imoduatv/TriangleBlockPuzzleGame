using System.Collections.Generic;
using System.Text;

namespace Entitas
{
	public class GroupObserver
	{
		private readonly HashSet<Entity> _collectedEntities;

		private readonly Group[] _groups;

		private readonly GroupEventType[] _eventTypes;

		private Group.GroupChanged _addEntityCache;

		private string _toStringCache;

		public HashSet<Entity> collectedEntities => _collectedEntities;

		public GroupObserver(Group group, GroupEventType eventType)
			: this(new Group[1]
			{
				group
			}, new GroupEventType[1]
			{
				eventType
			})
		{
		}

		public GroupObserver(Group[] groups, GroupEventType[] eventTypes)
		{
			_groups = groups;
			_collectedEntities = new HashSet<Entity>(EntityEqualityComparer.comparer);
			_eventTypes = eventTypes;
			if (groups.Length != eventTypes.Length)
			{
				throw new GroupObserverException("Unbalanced count with groups (" + groups.Length + ") and event types (" + eventTypes.Length + ").", "Group and event type count must be equal.");
			}
			_addEntityCache = addEntity;
			Activate();
		}

		public void Activate()
		{
			int i = 0;
			for (int num = _groups.Length; i < num; i++)
			{
				Group group = _groups[i];
				switch (_eventTypes[i])
				{
				case GroupEventType.OnEntityAdded:
					group.OnEntityAdded -= _addEntityCache;
					group.OnEntityAdded += _addEntityCache;
					break;
				case GroupEventType.OnEntityRemoved:
					group.OnEntityRemoved -= _addEntityCache;
					group.OnEntityRemoved += _addEntityCache;
					break;
				case GroupEventType.OnEntityAddedOrRemoved:
					group.OnEntityAdded -= _addEntityCache;
					group.OnEntityAdded += _addEntityCache;
					group.OnEntityRemoved -= _addEntityCache;
					group.OnEntityRemoved += _addEntityCache;
					break;
				}
			}
		}

		public void Deactivate()
		{
			int i = 0;
			for (int num = _groups.Length; i < num; i++)
			{
				Group group = _groups[i];
				group.OnEntityAdded -= _addEntityCache;
				group.OnEntityRemoved -= _addEntityCache;
			}
			ClearCollectedEntities();
		}

		public void ClearCollectedEntities()
		{
			foreach (Entity collectedEntity in _collectedEntities)
			{
				collectedEntity.Release(this);
			}
			_collectedEntities.Clear();
		}

		private void addEntity(Group group, Entity entity, int index, IComponent component)
		{
			if (_collectedEntities.Add(entity))
			{
				entity.Retain(this);
			}
		}

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				StringBuilder stringBuilder = new StringBuilder().Append("GroupObserver(");
				int num = _groups.Length - 1;
				int i = 0;
				for (int num2 = _groups.Length; i < num2; i++)
				{
					stringBuilder.Append(_groups[i]);
					if (i < num)
					{
						stringBuilder.Append(", ");
					}
				}
				stringBuilder.Append(")");
				_toStringCache = stringBuilder.ToString();
			}
			return _toStringCache;
		}

		~GroupObserver()
		{
			Deactivate();
		}
	}
}
