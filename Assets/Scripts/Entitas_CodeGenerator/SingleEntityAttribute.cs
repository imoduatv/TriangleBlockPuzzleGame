using System;

namespace Entitas.CodeGenerator
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public class SingleEntityAttribute : Attribute
	{
	}
}
