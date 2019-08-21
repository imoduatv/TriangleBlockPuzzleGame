using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Archon.SwissArmyLib.Utils
{
	public static class ServiceLocator
	{
		private class SceneData
		{
			public readonly Dictionary<Type, Func<object>> Resolvers = new Dictionary<Type, Func<object>>();

			public GameObject GameObject;
		}

		private static readonly Dictionary<Type, Func<object>> GlobalResolvers;

		private static readonly Dictionary<Scene, SceneData> SceneResolvers;

		private static readonly List<Scene> TempSceneList;

		private static GameObject _multiSceneGameObject;

		private static Scene _currentScene;

		private const string MultisceneGameObjectName = "ServiceLocator - Multi-scene";

		[CompilerGenerated]
		private static UnityAction<Scene, Scene> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static UnityAction<Scene> _003C_003Ef__mg_0024cache1;

		public static event Action GlobalReset;

		public static event Action<Scene> SceneReset;

		static ServiceLocator()
		{
			GlobalResolvers = new Dictionary<Type, Func<object>>();
			SceneResolvers = new Dictionary<Scene, SceneData>();
			TempSceneList = new List<Scene>();
			if (Application.isEditor)
			{
				_multiSceneGameObject = GameObject.Find("ServiceLocator - Multi-scene");
			}
			_currentScene = SceneManager.GetActiveScene();
			SceneManager.activeSceneChanged += OnActiveSceneChanged;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		private static void OnActiveSceneChanged(Scene previous, Scene current)
		{
			_currentScene = current;
		}

		private static void OnSceneUnloaded(Scene unloadedScene)
		{
			ResetScene(unloadedScene);
		}

		public static T RegisterSingleton<T>() where T : new()
		{
			return RegisterSingleton<T, T>();
		}

		public static Lazy<T> RegisterLazySingleton<T>() where T : new()
		{
			return RegisterLazySingleton<T, T>();
		}

		public static T RegisterSingletonForScene<T>() where T : new()
		{
			return RegisterSingletonForScene<T, T>(_currentScene);
		}

		public static Lazy<T> RegisterLazySingletonForScene<T>() where T : new()
		{
			return RegisterLazySingletonForScene<T, T>(_currentScene);
		}

		public static T RegisterSingletonForScene<T>(Scene scene) where T : new()
		{
			return RegisterSingletonForScene<T, T>(scene);
		}

		public static Lazy<T> RegisterLazySingletonForScene<T>(Scene scene) where T : new()
		{
			return RegisterLazySingletonForScene<T, T>(scene);
		}

		public static TConcrete RegisterSingleton<TAbstract, TConcrete>() where TConcrete : TAbstract, new()
		{
			TConcrete instance = (TConcrete)CreateInstance<TConcrete>();
			Func<object> value = () => (TConcrete)instance;
			GlobalResolvers[typeof(TAbstract)] = value;
			return (TConcrete)instance;
		}

		public static Lazy<TConcrete> RegisterLazySingleton<TAbstract, TConcrete>() where TConcrete : TAbstract, new()
		{
			Lazy<TConcrete> lazyInstance = (Lazy<TConcrete>)new Lazy<TConcrete>(CreateFactory<TConcrete>());
			Func<object> value = () => ((Lazy<TConcrete>)lazyInstance).Value;
			GlobalResolvers[typeof(TAbstract)] = value;
			return (Lazy<TConcrete>)lazyInstance;
		}

		public static TConcrete RegisterSingletonForScene<TAbstract, TConcrete>() where TConcrete : TAbstract, new()
		{
			return RegisterSingletonForScene<TAbstract, TConcrete>(_currentScene);
		}

		public static Lazy<TConcrete> RegisterLazySingletonForScene<TAbstract, TConcrete>() where TConcrete : TAbstract, new()
		{
			return RegisterLazySingletonForScene<TAbstract, TConcrete>(_currentScene);
		}

		public static TConcrete RegisterSingletonForScene<TAbstract, TConcrete>(Scene scene) where TConcrete : TAbstract, new()
		{
			SceneData orCreateSceneData = GetOrCreateSceneData(scene);
			TConcrete instance = (TConcrete)CreateInstance<TConcrete>(scene);
			Func<object> value = () => (TConcrete)instance;
			orCreateSceneData.Resolvers[typeof(TAbstract)] = value;
			return (TConcrete)instance;
		}

		public static Lazy<TConcrete> RegisterLazySingletonForScene<TAbstract, TConcrete>(Scene scene) where TConcrete : TAbstract, new()
		{
			SceneData orCreateSceneData = GetOrCreateSceneData(scene);
			Lazy<TConcrete> lazyInstance = (Lazy<TConcrete>)new Lazy<TConcrete>(CreateFactory<TConcrete>(scene));
			Func<object> value = () => ((Lazy<TConcrete>)lazyInstance).Value;
			orCreateSceneData.Resolvers[typeof(TAbstract)] = value;
			return (Lazy<TConcrete>)lazyInstance;
		}

		public static void RegisterSingleton<T>(T instance)
		{
			RegisterSingleton<T, T>(instance);
		}

		public static void RegisterSingleton<TAbstract, TConcrete>(TConcrete instance)
		{
			GlobalResolvers[typeof(TAbstract)] = (() => instance);
		}

		public static void RegisterSingletonForScene<T>(T instance)
		{
			RegisterSingletonForScene(instance, _currentScene);
		}

		public static void RegisterSingletonForScene<TAbstract, TConcrete>(TConcrete instance)
		{
			RegisterSingletonForScene<TAbstract, TConcrete>(instance, _currentScene);
		}

		public static void RegisterSingletonForScene<T>(T instance, Scene scene)
		{
			RegisterSingletonForScene<T, T>(instance, scene);
		}

		public static void RegisterSingletonForScene<TAbstract, TConcrete>(TConcrete instance, Scene scene)
		{
			Dictionary<Type, Func<object>> resolvers = GetOrCreateSceneData(scene).Resolvers;
			resolvers[typeof(TAbstract)] = (() => instance);
		}

		public static void RegisterTransient<T>() where T : new()
		{
			RegisterTransient<T, T>();
		}

		public static void RegisterTransientForScene<T>() where T : new()
		{
			RegisterTransientForScene<T, T>(_currentScene);
		}

		public static void RegisterTransientForScene<T>(Scene scene) where T : new()
		{
			RegisterTransientForScene<T, T>(scene);
		}

		public static void RegisterTransient<TAbstract, TConcrete>() where TConcrete : TAbstract, new()
		{
			Func<TConcrete> factory = CreateFactory<TConcrete>();
			RegisterTransient<TAbstract, TConcrete>(factory);
		}

		public static void RegisterTransientForScene<TAbstract, TConcrete>() where TConcrete : TAbstract, new()
		{
			RegisterTransientForScene<TAbstract, TConcrete>(_currentScene);
		}

		public static void RegisterTransientForScene<TAbstract, TConcrete>(Scene scene) where TConcrete : TAbstract, new()
		{
			Func<TConcrete> factory = CreateFactory<TConcrete>(scene);
			RegisterTransientForScene<TAbstract, TConcrete>(factory, scene);
		}

		public static void RegisterTransient<T>(Func<T> factory)
		{
			RegisterTransient<T, T>(factory);
		}

		public static void RegisterTransientForScene<T>(Func<T> factory)
		{
			RegisterTransientForScene<T, T>(factory, _currentScene);
		}

		public static void RegisterTransientForScene<T>(Func<T> factory, Scene scene)
		{
			RegisterTransientForScene<T, T>(factory, scene);
		}

		public static void RegisterTransient<TAbstract, TConcrete>(Func<TConcrete> factory) where TConcrete : TAbstract
		{
			if (object.ReferenceEquals(factory, null))
			{
				throw new ArgumentNullException("factory");
			}
			GlobalResolvers[typeof(TAbstract)] = (() => factory());
		}

		public static void RegisterTransientForScene<TAbstract, TConcrete>(Func<TConcrete> factory) where TConcrete : TAbstract
		{
			RegisterTransientForScene<TAbstract, TConcrete>(factory, _currentScene);
		}

		public static void RegisterTransientForScene<TAbstract, TConcrete>(Func<TConcrete> factory, Scene scene) where TConcrete : TAbstract
		{
			if (object.ReferenceEquals(factory, null))
			{
				throw new ArgumentNullException("factory");
			}
			GetOrCreateSceneData(scene).Resolvers[typeof(TAbstract)] = (() => factory());
		}

		public static T Resolve<T>(bool includeActiveScene = true)
		{
			Type typeFromHandle = typeof(T);
			if (!GlobalResolvers.TryGetValue(typeFromHandle, out Func<object> value) && includeActiveScene)
			{
				return ResolveForScene<T>();
			}
			if (value != null)
			{
				return (T)value();
			}
			return default(T);
		}

		public static T ResolveForScene<T>()
		{
			return ResolveForScene<T>(_currentScene);
		}

		public static T ResolveForScene<T>(Scene scene)
		{
			if (!SceneResolvers.TryGetValue(scene, out SceneData value))
			{
				return default(T);
			}
			value.Resolvers.TryGetValue(typeof(T), out Func<object> value2);
			if (value2 != null)
			{
				return (T)value2();
			}
			return default(T);
		}

		public static bool IsRegistered<T>(bool includeActiveScene = true)
		{
			return GlobalResolvers.ContainsKey(typeof(T)) || (includeActiveScene && IsRegisteredInScene<T>());
		}

		public static bool IsRegisteredInScene<T>()
		{
			return IsRegisteredInScene<T>(_currentScene);
		}

		public static bool IsRegisteredInScene<T>(Scene scene)
		{
			if (!SceneResolvers.TryGetValue(scene, out SceneData value))
			{
				return false;
			}
			return value.Resolvers.ContainsKey(typeof(T));
		}

		public static void Reset()
		{
			ResetScenes();
			ResetGlobal();
		}

		public static void ResetGlobal()
		{
			GlobalResolvers.Clear();
			if (_multiSceneGameObject != null)
			{
				UnityEngine.Object.Destroy(_multiSceneGameObject);
				_multiSceneGameObject = null;
			}
			if (ServiceLocator.GlobalReset != null)
			{
				ServiceLocator.GlobalReset();
			}
		}

		public static void ResetScene()
		{
			ResetScene(_currentScene);
		}

		public static void ResetScene(Scene scene)
		{
			if (SceneResolvers.TryGetValue(scene, out SceneData value))
			{
				if (value.GameObject != null)
				{
					UnityEngine.Object.Destroy(value.GameObject);
				}
				SceneResolvers.Remove(scene);
				if (ServiceLocator.SceneReset != null)
				{
					ServiceLocator.SceneReset(scene);
				}
			}
		}

		public static void ResetScenes()
		{
			TempSceneList.AddRange(SceneResolvers.Keys);
			for (int i = 0; i < TempSceneList.Count; i++)
			{
				Scene scene = TempSceneList[i];
				ResetScene(scene);
			}
			TempSceneList.Clear();
		}

		private static SceneData GetOrCreateSceneData(Scene scene)
		{
			if (!SceneResolvers.TryGetValue(scene, out SceneData value))
			{
				value = (SceneResolvers[scene] = new SceneData());
			}
			return value;
		}

		private static bool IsComponent<T>()
		{
			return typeof(T).IsSubclassOf(typeof(MonoBehaviour));
		}

		private static Func<T> CreateFactory<T>() where T : new()
		{
			bool isComponent = IsComponent<T>();
			return () => CreateInstance<T>(isComponent);
		}

		private static Func<T> CreateFactory<T>(Scene scene) where T : new()
		{
			bool isComponent = IsComponent<T>();
			return () => CreateInstance<T>(scene, isComponent);
		}

		private static T CreateInstance<T>() where T : new()
		{
			return CreateInstance<T>(IsComponent<T>());
		}

		private static T CreateInstance<T>(bool isComponent) where T : new()
		{
			if (!isComponent)
			{
				return new T();
			}
			return CreateComponent<T>();
		}

		private static T CreateInstance<T>(Scene scene) where T : new()
		{
			return CreateInstance<T>(scene, IsComponent<T>());
		}

		private static T CreateInstance<T>(Scene scene, bool isComponent) where T : new()
		{
			if (!isComponent)
			{
				return new T();
			}
			return CreateComponent<T>(scene);
		}

		private static T CreateComponent<T>()
		{
			if (_multiSceneGameObject == null)
			{
				_multiSceneGameObject = new GameObject("ServiceLocator - Multi-scene");
				UnityEngine.Object.DontDestroyOnLoad(_multiSceneGameObject);
			}
			return (T)(object)_multiSceneGameObject.AddComponent(typeof(T));
		}

		private static T CreateComponent<T>(Scene scene)
		{
			SceneData sceneData = SceneResolvers[scene];
			if (sceneData.GameObject == null)
			{
				sceneData.GameObject = new GameObject("ServiceLocator - Scene: " + scene.name);
				SceneManager.MoveGameObjectToScene(sceneData.GameObject, scene);
			}
			return (T)(object)sceneData.GameObject.AddComponent(typeof(T));
		}
	}
}
