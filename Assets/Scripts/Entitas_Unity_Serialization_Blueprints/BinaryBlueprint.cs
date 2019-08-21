using Entitas.Serialization.Blueprints;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Entitas.Unity.Serialization.Blueprints
{
	[CreateAssetMenu(menuName = "Entitas/Blueprint", fileName = "Assets/New Blueprint.asset")]
	public class BinaryBlueprint : ScriptableObject
	{
		public byte[] blueprintData;

		private readonly BinaryFormatter _serializer = new BinaryFormatter();

		public Blueprint Deserialize()
		{
			Blueprint blueprint;
			if (blueprintData == null || blueprintData.Length == 0)
			{
				blueprint = new Blueprint(string.Empty, "New Blueprint", new Entity(0, null));
			}
			else
			{
				using (MemoryStream serializationStream = new MemoryStream(blueprintData))
				{
					blueprint = (Blueprint)_serializer.Deserialize(serializationStream);
				}
			}
			base.name = blueprint.name;
			return blueprint;
		}

		public void Serialize(Entity entity)
		{
			Blueprint blueprint = new Blueprint(entity.poolMetaData.poolName, base.name, entity);
			Serialize(blueprint);
		}

		public void Serialize(Blueprint blueprint)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				_serializer.Serialize(memoryStream, blueprint);
				blueprintData = memoryStream.ToArray();
			}
		}
	}
}
