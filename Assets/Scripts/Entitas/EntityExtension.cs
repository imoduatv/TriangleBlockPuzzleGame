namespace Entitas
{
	public static class EntityExtension
	{
		public const string COMPONENT_SUFFIX = "Component";

		public static string AddComponentSuffix(this string componentName)
		{
			return (!componentName.EndsWith("Component")) ? (componentName + "Component") : componentName;
		}

		public static string RemoveComponentSuffix(this string componentName)
		{
			return (!componentName.EndsWith("Component")) ? componentName : componentName.Substring(0, componentName.Length - "Component".Length);
		}
	}
}
