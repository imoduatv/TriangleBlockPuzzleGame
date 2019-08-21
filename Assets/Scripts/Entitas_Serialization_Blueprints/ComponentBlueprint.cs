using System;
using System.Collections.Generic;

namespace Entitas.Serialization.Blueprints
{
	[Serializable]
	public class ComponentBlueprint
	{
		public int index;

		public string fullTypeName;

		public SerializableMember[] members;

		private Type _type;

		private Dictionary<string, PublicMemberInfo> _componentMembers;

		public ComponentBlueprint()
		{
		}

		public ComponentBlueprint(int index, IComponent component)
		{
			_type = component.GetType();
			_componentMembers = null;
			this.index = index;
			fullTypeName = _type.FullName;
			List<PublicMemberInfo> publicMemberInfos = _type.GetPublicMemberInfos();
			members = new SerializableMember[publicMemberInfos.Count];
			int i = 0;
			for (int count = publicMemberInfos.Count; i < count; i++)
			{
				PublicMemberInfo publicMemberInfo = publicMemberInfos[i];
				members[i] = new SerializableMember(publicMemberInfo.name, publicMemberInfo.GetValue(component));
			}
		}

		public IComponent CreateComponent(Entity entity)
		{
			if (_type == null)
			{
				_type = fullTypeName.ToType();
				if (_type == null)
				{
					throw new ComponentBlueprintException("Type '" + fullTypeName + "' doesn't exist in any assembly!", "Please check the full type name.");
				}
				if (!_type.ImplementsInterface<IComponent>())
				{
					throw new ComponentBlueprintException("Type '" + fullTypeName + "' doesn't implement IComponent!", typeof(ComponentBlueprint).Name + " only supports IComponent.");
				}
			}
			IComponent component = entity.CreateComponent(index, _type);
			if (_componentMembers == null)
			{
				List<PublicMemberInfo> publicMemberInfos = _type.GetPublicMemberInfos();
				_componentMembers = new Dictionary<string, PublicMemberInfo>(publicMemberInfos.Count);
				int i = 0;
				for (int count = publicMemberInfos.Count; i < count; i++)
				{
					PublicMemberInfo publicMemberInfo = publicMemberInfos[i];
					_componentMembers.Add(publicMemberInfo.name, publicMemberInfo);
				}
			}
			int j = 0;
			for (int num = members.Length; j < num; j++)
			{
				SerializableMember serializableMember = members[j];
				if (!_componentMembers.TryGetValue(serializableMember.name, out PublicMemberInfo value))
				{
					throw new ComponentBlueprintException("Could not find member '" + serializableMember.name + "' in type '" + _type.FullName + "'!", "Only non-static public members are supported.");
				}
				value.SetValue(component, serializableMember.value);
			}
			return component;
		}
	}
}
