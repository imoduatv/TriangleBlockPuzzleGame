using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archon.SwissArmyLib.Pooling
{
	public class GameObjectPool<T> : Pool<T>, IDisposable where T : UnityEngine.Object
	{
		private readonly Transform _root;

		private readonly bool _multiScene;

		public T Prefab
		{
			get;
			private set;
		}

		public GameObjectPool(T prefab, bool multiScene)
			: this(prefab.name, (Func<T>)(() => UnityEngine.Object.Instantiate(prefab)), multiScene)
		{
			if (object.ReferenceEquals(prefab, null))
			{
				throw new ArgumentNullException("prefab");
			}
			Prefab = prefab;
		}

		public GameObjectPool(string name, Func<T> create, bool multiScene)
			: base(create)
		{
			GameObject gameObject = new GameObject($"'{name}' Pool");
			_root = gameObject.transform;
			_multiScene = multiScene;
			if (multiScene)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		~GameObjectPool()
		{
			if ((bool)_root)
			{
				UnityEngine.Object.Destroy(_root.gameObject);
			}
		}

		public void Dispose()
		{
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			if ((bool)_root)
			{
				UnityEngine.Object.Destroy(_root.gameObject);
			}
			Free.Clear();
		}

		private void OnSceneUnloaded(Scene unloadedScene)
		{
			for (int num = Free.Count - 1; num >= 0; num--)
			{
				if (!(UnityEngine.Object)Free[num])
				{
					Free.RemoveAt(num);
				}
			}
		}

		protected override T SpawnInternal()
		{
			T val = base.SpawnInternal();
			GameObject gameObject = GetGameObject(val);
			gameObject.transform.SetParent(null, worldPositionStays: false);
			if (_multiScene)
			{
				SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
			}
			return val;
		}

		public T Spawn(Transform parent)
		{
			T val = SpawnInternal();
			GameObject gameObject = GetGameObject(val);
			Transform transform = gameObject.transform;
			transform.SetParent(parent, worldPositionStays: false);
			OnSpawned(val);
			return val;
		}

		public T Spawn(Vector3 position)
		{
			T val = SpawnInternal();
			GameObject gameObject = GetGameObject(val);
			Transform transform = gameObject.transform;
			transform.SetPositionAndRotation(position, Quaternion.identity);
			OnSpawned(val);
			return val;
		}

		public T Spawn(Vector3 position, Quaternion rotation)
		{
			T val = SpawnInternal();
			GameObject gameObject = GetGameObject(val);
			Transform transform = gameObject.transform;
			transform.SetPositionAndRotation(position, rotation);
			OnSpawned(val);
			return val;
		}

		public T Spawn(Vector3 position, Quaternion rotation, Transform parent)
		{
			T val = SpawnInternal();
			GameObject gameObject = GetGameObject(val);
			Transform transform = gameObject.transform;
			transform.SetParent(parent, worldPositionStays: false);
			transform.SetPositionAndRotation(position, rotation);
			OnSpawned(val);
			return val;
		}

		public override void Despawn(T target)
		{
			if (object.ReferenceEquals(target, null))
			{
				throw new ArgumentNullException("target");
			}
			try
			{
				base.Despawn(target);
			}
			catch (ArgumentException)
			{
				throw new ArgumentException($"Target '{target.name}' is already despawned!", "target");
			}
			GameObject gameObject = GetGameObject(target);
			gameObject.SetActive(value: false);
			Transform transform = gameObject.transform;
			transform.SetParent(_root, worldPositionStays: false);
		}

		protected override void OnSpawned(T target)
		{
			GameObject gameObject = GetGameObject(target);
			gameObject.SetActive(value: true);
			base.OnSpawned(target);
		}

		private static GameObject GetGameObject(T obj)
		{
			if (object.ReferenceEquals(obj, null))
			{
				throw new ArgumentNullException("obj");
			}
			Component component = obj as Component;
			if (component != null)
			{
				return component.gameObject;
			}
			return obj as GameObject;
		}
	}
}
