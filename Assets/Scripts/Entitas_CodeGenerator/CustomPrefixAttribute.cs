using System;

namespace Entitas.CodeGenerator
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public class CustomPrefixAttribute : Attribute
	{
		public readonly string prefix;

		public CustomPrefixAttribute(string prefix)
		{
			this.prefix = prefix;
		}
	}
}
