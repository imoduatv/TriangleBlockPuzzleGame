using System;

namespace Entitas.CodeGenerator
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = true)]
	public class PoolAttribute : Attribute
	{
		public readonly string poolName;

		public PoolAttribute(string poolName = "")
		{
			this.poolName = poolName;
		}
	}
}
