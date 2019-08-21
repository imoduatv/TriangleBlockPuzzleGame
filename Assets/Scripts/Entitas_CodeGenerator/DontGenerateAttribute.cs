using System;

namespace Entitas.CodeGenerator
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public class DontGenerateAttribute : Attribute
	{
		public readonly bool generateIndex;

		public DontGenerateAttribute(bool generateIndex = true)
		{
			this.generateIndex = generateIndex;
		}
	}
}
