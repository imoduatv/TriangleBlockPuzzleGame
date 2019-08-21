using Entitas.Serialization.Blueprints;
using System.Collections.Generic;
using UnityEngine;

namespace Entitas.Unity.Serialization.Blueprints
{
	[CreateAssetMenu(menuName = "Entitas/Blueprints", fileName = "Assets/Blueprints.asset")]
	public class Blueprints : ScriptableObject
	{
		public BinaryBlueprint[] blueprints;

		private Dictionary<string, BinaryBlueprint> _binaryBlueprintsMap;

		private Dictionary<string, Blueprint> _blueprintsMap;

		private void OnEnable()
		{
			if (blueprints == null)
			{
				blueprints = new BinaryBlueprint[0];
			}
			_binaryBlueprintsMap = new Dictionary<string, BinaryBlueprint>(blueprints.Length);
			_blueprintsMap = new Dictionary<string, Blueprint>(blueprints.Length);
			int i = 0;
			for (int num = blueprints.Length; i < num; i++)
			{
				BinaryBlueprint binaryBlueprint = blueprints[i];
				if (binaryBlueprint != null)
				{
					_binaryBlueprintsMap.Add(binaryBlueprint.name, binaryBlueprint);
				}
			}
		}

		public Blueprint GetBlueprint(string name)
		{
			if (!_blueprintsMap.TryGetValue(name, out Blueprint value))
			{
				if (!_binaryBlueprintsMap.TryGetValue(name, out BinaryBlueprint value2))
				{
					throw new BlueprintsNotFoundException(name);
				}
				value = value2.Deserialize();
				_blueprintsMap.Add(name, value);
			}
			return value;
		}
	}
}
