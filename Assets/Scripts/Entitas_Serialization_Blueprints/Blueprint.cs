using System;

namespace Entitas.Serialization.Blueprints
{
	[Serializable]
	public class Blueprint
	{
		public string poolIdentifier;

		public string name;

		public ComponentBlueprint[] components;

		public Blueprint()
		{
		}

		public Blueprint(string poolIdentifier, string name, Entity entity)
		{
			this.poolIdentifier = poolIdentifier;
			this.name = name;
			IComponent[] array = entity.GetComponents();
			int[] componentIndices = entity.GetComponentIndices();
			components = new ComponentBlueprint[array.Length];
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				components[i] = new ComponentBlueprint(componentIndices[i], array[i]);
			}
		}
	}
}
