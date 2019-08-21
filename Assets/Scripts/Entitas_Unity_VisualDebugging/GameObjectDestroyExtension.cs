using UnityEngine;

namespace Entitas.Unity.VisualDebugging
{
	public static class GameObjectDestroyExtension
	{
		public static void DestroyGameObject(this GameObject gameObject)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}
}
