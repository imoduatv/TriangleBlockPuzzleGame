using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Archon.SwissArmyLib.Pooling
{
	[AddComponentMenu("Archon/Poolable Group")]
	public sealed class PoolableGroup : MonoBehaviour, IPoolable, ISerializationCallbackReceiver
	{
		[SerializeField]
		[UsedImplicitly]
		private List<MonoBehaviour> _poolableComponents = new List<MonoBehaviour>();

		void IPoolable.OnSpawned()
		{
			for (int i = 0; i < _poolableComponents.Count; i++)
			{
				((IPoolable)_poolableComponents[i]).OnSpawned();
			}
		}

		void IPoolable.OnDespawned()
		{
			for (int i = 0; i < _poolableComponents.Count; i++)
			{
				((IPoolable)_poolableComponents[i]).OnDespawned();
			}
		}

		public void AddManually<T>(T poolable) where T : MonoBehaviour, IPoolable
		{
			if (object.ReferenceEquals(poolable, null))
			{
				throw new ArgumentNullException("poolable");
			}
			_poolableComponents.Add(poolable);
		}

		public void RemoveManually<T>(T poolable) where T : MonoBehaviour, IPoolable
		{
			if (object.ReferenceEquals(poolable, null))
			{
				throw new ArgumentNullException("poolable");
			}
			_poolableComponents.Remove(poolable);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (!Application.isPlaying)
			{
				_poolableComponents.Clear();
				IEnumerable<MonoBehaviour> collection = GetComponentsInChildren<IPoolable>(includeInactive: true).Cast<MonoBehaviour>();
				_poolableComponents.AddRange(collection);
				_poolableComponents.Remove(this);
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
		}
	}
}
